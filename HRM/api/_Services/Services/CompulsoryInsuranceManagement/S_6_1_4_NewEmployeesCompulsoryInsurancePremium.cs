using System.Globalization;
using API.Data;
using API._Services.Interfaces.CompulsoryInsuranceManagement;
using API.DTOs.CompulsoryInsuranceManagement;
using API.Helper.Constant;
using API.Helper.Utilities;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.CompulsoryInsuranceManagement
{
    public partial class S_6_1_4_NewEmployeesCompulsoryInsurancePremium : BaseServices, I_6_1_4_NewEmployeesCompulsoryInsurancePremium
    {
        private static readonly SemaphoreSlim semaphore = new(1, 1);

        public S_6_1_4_NewEmployeesCompulsoryInsurancePremium(DBContext dbContext) : base(dbContext) { }

        public async Task<List<KeyValuePair<string, string>>> GetDropDownList(NewEmployeesCompulsoryInsurancePremium_Param param, List<string> roleList)
        {
            var HBC = await _repositoryAccessor.HRMS_Basic_Code.FindAll().ToListAsync();
            var HBCL = _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == param.Lang.ToLower()).ToList();
            var result = new List<KeyValuePair<string, string>>();
            var data = HBC.GroupJoin(HBCL,
                x => new { x.Type_Seq, x.Code },
                y => new { y.Type_Seq, y.Code },
                (x, y) => new { hbc = x, hbcl = y })
                .SelectMany(x => x.hbcl.DefaultIfEmpty(),
                (x, y) => new { x.hbc, hbcl = y });
            var authFactories = await Queryt_Factory_AddList(roleList);
            result.AddRange(data.Where(x => x.hbc.Type_Seq == BasicCodeTypeConstant.Factory && authFactories.Contains(x.hbc.Code)).Select(x => new KeyValuePair<string, string>("FA", $"{x.hbc.Code}-{(x.hbcl != null ? x.hbcl.Code_Name : x.hbc.Code_Name)}")).Distinct().ToList());
            if (!string.IsNullOrWhiteSpace(param.Factory))
            {
                var authPermission = await Query_Permission_Group_List(param.Factory);
                result.AddRange(data.Where(x => x.hbc.Type_Seq == BasicCodeTypeConstant.PermissionGroup && authPermission.Contains(x.hbc.Code)).Select(x => new KeyValuePair<string, string>("PE", $"{x.hbc.Code}-{(x.hbcl != null ? x.hbcl.Code_Name : x.hbc.Code_Name)}")).Distinct().ToList());
            }
            return result;
        }
        public async Task<OperationResult> Process(NewEmployeesCompulsoryInsurancePremium_Param param, string userName)
        {
            if (!DateTime.TryParseExact(param.Year_Month_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime Year_Month) ||
                !param.Paid_Salary_Days.isIntRange(1, 31))
                return new OperationResult(false, "InvalidInput");
            param.Year_Month_Date = Year_Month;
            param.Start_Date = Year_Month;
            param.End_Date = Year_Month.AddMonths(1).AddDays(-1);
            OperationResult result = param.Function_Type switch
            {
                "execute" => await Execute(param, userName),
                "search" => await Search(param, userName),
                "excel" => await Excel(param, userName),
                _ => new OperationResult(false, "InvalidFunc")
            };
            return result;
        }
        private async Task<OperationResult> Execute(NewEmployeesCompulsoryInsurancePremium_Param param, string userName)
        {
            await semaphore.WaitAsync();
            await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                var HEP = _repositoryAccessor.HRMS_Emp_Personal.FindAll(x =>
                    x.Factory == param.Factory &&
                    param.Permission_Group.Contains(x.Permission_Group) &&
                    (!x.Resign_Date.HasValue || (x.Resign_Date.HasValue && x.Resign_Date.Value.Date > param.End_Date.Date))
                );
                var HIEM = _repositoryAccessor.HRMS_Ins_Emp_Maintain.FindAll(x =>
                    x.Factory == param.Factory &&
                    x.Insurance_Type == "V01"
                );
                var TEMP_GUID_1 = HEP.Join(HIEM.Where(x => x.Insurance_Start.Date >= param.Start_Date.Date && x.Insurance_Start.Date <= param.End_Date.Date),
                    x => x.Employee_ID,
                    y => y.Employee_ID,
                    (x, y) => x.USER_GUID);
                var TEMP_GUID_2 = HEP.Where(x => !HIEM.Select(y => y.USER_GUID).Contains(x.USER_GUID)).Select(x => x.USER_GUID);

                var TEMP_GUID = await TEMP_GUID_1
                    .Union(TEMP_GUID_2)
                    .Distinct().ToListAsync();
                var delDatas = await _repositoryAccessor.HRMS_Sal_AddDedItem_Monthly.FindAll(x =>
                    x.Factory == param.Factory &&
                    x.Sal_Month.Date == param.Year_Month_Date.Date &&
                    x.AddDed_Type == "B" &&
                    x.AddDed_Item == "B49" &&
                    TEMP_GUID.Contains(x.USER_GUID)
                ).ToListAsync();
                if (delDatas.Count > 0)
                {
                    var delResult = await CRUD_Data(new NewEmployeesCompulsoryInsurancePremium_CRUD("Del_Multi_HRMS_Sal_AddDedItem_Monthly", new NewEmployeesCompulsoryInsurancePremium_General(delDatas)));
                    if (!delResult.IsSuccess)
                    {
                        await _repositoryAccessor.RollbackAsync();
                        return new OperationResult(false, delResult.Error);
                    }
                }
                var res = await Calculation(param, userName);
                var insDatas = res.Data_Insert.FindAll(x => x.Amount > 0);
                if (insDatas.Count > 0)
                {
                    var insResult = await CRUD_Data(new NewEmployeesCompulsoryInsurancePremium_CRUD("Ins_Multi_HRMS_Sal_AddDedItem_Monthly", new NewEmployeesCompulsoryInsurancePremium_General(insDatas)));
                    if (!insResult.IsSuccess)
                    {
                        await _repositoryAccessor.RollbackAsync();
                        return new OperationResult(false, insResult.Error);
                    }
                }
                await _repositoryAccessor.CommitAsync();
                return new OperationResult(true, insDatas.Count);
            }
            catch (Exception)
            {
                await _repositoryAccessor.RollbackAsync();
                return new OperationResult(false);
            }
            finally
            {
                semaphore.Release();
            }
        }
        private async Task<OperationResult> Search(NewEmployeesCompulsoryInsurancePremium_Param param, string userName)
        {
            var res = await Calculation(param, userName);
            return new OperationResult(true, res.Data_Excel.Count);
        }
        private async Task<OperationResult> Excel(NewEmployeesCompulsoryInsurancePremium_Param param, string userName)
        {
            var res = await Calculation(param, userName);
            if (res.Data_Excel.Count > 0)
            {
                var HBC = _repositoryAccessor.HRMS_Basic_Code.FindAll(x =>
                                x.Type_Seq == BasicCodeTypeConstant.PermissionGroup &&
                                param.Permission_Group.Contains(x.Code)
                            );
                var HBCL = _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == param.Lang.ToLower());
                var Permission_Group = await HBC
                    .GroupJoin(HBCL,
                        x => new { x.Type_Seq, x.Code },
                        y => new { y.Type_Seq, y.Code },
                        (x, y) => new { hbc = x, hbcl = y })
                    .SelectMany(x => x.hbcl.DefaultIfEmpty(),
                        (x, y) => new { x.hbc, hbcl = y })
                    .Select(x => $"{x.hbc.Code}-{(x.hbcl != null ? x.hbcl.Code_Name : x.hbc.Code_Name)}").ToListAsync();
                List<Table> tables = new()
                {
                    new Table("result", res.Data_Excel)
                };
                List<Cell> cells = new()
                {
                    new Cell("B2", param.Factory),
                    new Cell("D2", param.Year_Month_Date.ToString("yyyy/MM")),
                    new Cell("F2", param.Paid_Salary_Days),
                    new Cell("H2", string.Join(" / ", Permission_Group)),
                    new Cell("B3", userName),
                    new Cell("D3", res.Data_Excel[0].Print_Date.ToString("yyyy/MM/dd HH:mm:ss")),
                };
                ExcelResult excelResult = ExcelUtility.DownloadExcel(
                    tables,
                    cells,
                    "Resources\\Template\\CompulsoryInsuranceManagement\\6_1_4_NewEmployeesCompulsoryInsurancePremium\\Download.xlsx",
                    new ConfigDownload(false)
                );
                return new OperationResult(excelResult.IsSuccess, excelResult.Error, new { res.Data_Excel.Count, excelResult.Result });
            }
            return new OperationResult(true, new { res.Data_Excel.Count });
        }
        private async Task<NewEmployeesCompulsoryInsurancePremium_Calculation> Calculation(NewEmployeesCompulsoryInsurancePremium_Param param, string userName)
        {
            decimal Salary_Days = decimal.TryParse(param.Paid_Salary_Days, out decimal _salary_Days) ? _salary_Days : 0m;
            DateTime now = DateTime.Now;
            var HIEM = _repositoryAccessor.HRMS_Ins_Emp_Maintain.FindAll(x =>
                x.Factory == param.Factory &&
                x.Insurance_Type == "V01"
            );
            var HAC = _repositoryAccessor.HRMS_Att_Calendar.FindAll(x =>
                x.Factory == param.Factory &&
                x.Att_Date.Date >= param.Start_Date.Date &&
                x.Type_Code == "C05"
            );
            var HALM = _repositoryAccessor.HRMS_Att_Leave_Maintain.FindAll(x =>
                x.Factory == param.Factory &&
                x.Leave_Date.Date >= param.Start_Date.Date && x.Leave_Date.Date <= param.End_Date.Date
            );
            var HAM = _repositoryAccessor.HRMS_Att_Monthly.FindAll(x =>
                x.Factory == param.Factory &&
                x.Att_Month.Date == param.Year_Month_Date.Date
            );
            var HSM = _repositoryAccessor.HRMS_Sal_Master.FindAll(x => x.Factory == param.Factory);
            var depLang = await Query_Department_List(param.Factory, param.Lang);

            var HEPs = await _repositoryAccessor.HRMS_Emp_Personal.FindAll(x =>
                x.Factory == param.Factory &&
                param.Permission_Group.Contains(x.Permission_Group) &&
                (!x.Resign_Date.HasValue || (x.Resign_Date.HasValue && x.Resign_Date.Value.Date > param.End_Date.Date)) &&
                x.Onboard_Date.Date <= param.End_Date.Date &&
                !HIEM.Select(y => y.USER_GUID).Contains(x.USER_GUID)
                // x.USER_GUID == "30a7ebc2-99f6-44d9-bfa3-31a52f305aac"
            ).ToListAsync();
            NewEmployeesCompulsoryInsurancePremium_Calculation result = new()
            {
                Data_Excel = new List<NewEmployeesCompulsoryInsurancePremium_Excel>(),
                Data_Insert = new List<HRMS_Sal_AddDedItem_Monthly>()
            };
            foreach (var Emp_Personal in HEPs)
            {
                int Before_Onboard = 0;
                int Calendar_days = 0;

                if (Emp_Personal.Onboard_Date.Date <= param.Start_Date.Date)
                    Before_Onboard = 0;
                else
                {
                    Calendar_days = (int)(Emp_Personal.Onboard_Date.Date - param.Start_Date.Date).TotalDays;
                    int Holiday = HAC.Where(x => x.Att_Date.Date <= Emp_Personal.Onboard_Date.Date).Count();
                    Before_Onboard = Calendar_days - Holiday;
                }
                var Leave_Days = HALM.Where(x =>
                    x.Employee_ID == Emp_Personal.Employee_ID &&
                    new List<string>() { "A0", "B0", "C0", "J4" }.Contains(x.Leave_code))?
                .Sum(x => x.Days) ?? 0m;
                var Out_Day = Before_Onboard + Leave_Days;
                decimal Actual_Days = 0m;
                if (Out_Day >= 14)
                {
                    Actual_Days = HAM.Where(x => x.Employee_ID == Emp_Personal.Employee_ID)?.Sum(x => x.Actual_Days) ?? 0m;
                    var Leave_Cnt = HALM.Where(x =>
                        x.Employee_ID == Emp_Personal.Employee_ID &&
                        new List<string>() { "I0", "I1", "D0", "K0" }.Contains(x.Leave_code))?
                    .Sum(x => x.Days) ?? 0m;
                    Actual_Days += Leave_Cnt;
                }
                else
                {
                    Actual_Days = Salary_Days;
                }

                Dictionary<string, VariableCombine> Insurance_57 = await Query_Ins_Rate_Variable_Combine(
                    "57", "Insurance", "EmployerRate", "EmployeeRate", "Amt",
                    param.Factory, param.Year_Month_Date, Emp_Personal.Permission_Group);

                decimal V01_Amt_57 = Insurance_57.GetValueOrDefault("V01_Amt_57")?.Value as decimal? ?? 0m;
                decimal V02_Amt_57 = Insurance_57.GetValueOrDefault("V02_Amt_57")?.Value as decimal? ?? 0m;
                decimal V03_Amt_57 = Insurance_57.GetValueOrDefault("V03_Amt_57")?.Value as decimal? ?? 0m;

                decimal V01_EmployerRate_57 = Insurance_57.GetValueOrDefault("V01_EmployerRate_57")?.Value as decimal? ?? 0m;
                decimal V02_EmployerRate_57 = Insurance_57.GetValueOrDefault("V02_EmployerRate_57")?.Value as decimal? ?? 0m;
                decimal V03_EmployerRate_57 = Insurance_57.GetValueOrDefault("V03_EmployerRate_57")?.Value as decimal? ?? 0m;

                decimal Basic_Amt = 0m;
                var Sal_Master = HSM.FirstOrDefault(x => x.Employee_ID == Emp_Personal.Employee_ID);
                if (Sal_Master != null)
                    Basic_Amt = await Query_WageStandard_Sum("M", param.Factory, param.Year_Month_Date, Emp_Personal.Employee_ID, Emp_Personal.Permission_Group, Sal_Master.Salary_Type);

                V01_Amt_57 = Basic_Amt * V01_EmployerRate_57 / Salary_Days * Actual_Days;
                V02_Amt_57 = Basic_Amt * V02_EmployerRate_57 / Salary_Days * Actual_Days;
                V03_Amt_57 = Basic_Amt * V03_EmployerRate_57 / Salary_Days * Actual_Days;

                decimal Add_Amt = Math.Round(V01_Amt_57 + V02_Amt_57 + V03_Amt_57, 0);
                result.Data_Excel.Add(new NewEmployeesCompulsoryInsurancePremium_Excel()
                {
                    Department = Emp_Personal.Department,
                    Department_Name = depLang.FirstOrDefault(x => x.Department_Code == Emp_Personal.Department)?.Department_Name ?? "",
                    Employee_ID = Emp_Personal.Employee_ID,
                    Local_Full_Name = Emp_Personal.Local_Full_Name,
                    Insured_Salary = Basic_Amt,
                    Paid_Salary_Days = Salary_Days,
                    Actual_Paid_Days = Actual_Days,
                    Unemployment_Insurance = Math.Round(V03_Amt_57,0),
                    Social_Insurance = Math.Round(V01_Amt_57,0),
                    Health_Insurance = Math.Round(V02_Amt_57,0),
                    Amount = (int)Add_Amt,
                    Onboard_Date = Emp_Personal.Onboard_Date,
                    Print_Date = now
                });
                if (Sal_Master != null)
                    result.Data_Insert.Add(new HRMS_Sal_AddDedItem_Monthly()
                    {
                        USER_GUID = Emp_Personal.USER_GUID,
                        Factory = param.Factory,
                        Sal_Month = param.Year_Month_Date,
                        Employee_ID = Emp_Personal.Employee_ID,
                        AddDed_Type = "B",
                        AddDed_Item = "B49",
                        Currency = Sal_Master.Currency,
                        Amount = (int)Add_Amt,
                        Update_By = userName,
                        Update_Time = now
                    });
            }
            return result;
        }
        private async Task<NewEmployeesCompulsoryInsurancePremium_CRUD> CRUD_Data(NewEmployeesCompulsoryInsurancePremium_CRUD initial)
        {
            try
            {
                switch (initial.Function)
                {
                    case "Ins_Multi_HRMS_Sal_AddDedItem_Monthly":
                        _repositoryAccessor.HRMS_Sal_AddDedItem_Monthly.AddMultiple(initial.Data.HRMS_Sal_AddDedItem_Monthly_List);
                        initial.IsSuccess = await _repositoryAccessor.Save();
                        break;
                    case "Del_Multi_HRMS_Sal_AddDedItem_Monthly":
                        _repositoryAccessor.HRMS_Sal_AddDedItem_Monthly.RemoveMultiple(initial.Data.HRMS_Sal_AddDedItem_Monthly_List);
                        initial.IsSuccess = await _repositoryAccessor.Save();
                        break;
                    default:
                        initial.IsSuccess = false;
                        break;
                }
                return initial;
            }
            catch (Exception)
            {
                initial.IsSuccess = false;
                return initial;
            }
            finally
            {
                if (!initial.IsSuccess)
                    initial.Error = initial.Function switch
                    {
                        "Ins_Multi_HRMS_Sal_AddDedItem_Monthly" => "InsFailHSAM",
                        "Del_Multi_HRMS_Sal_AddDedItem_Monthly" => "DelFailHSAM",
                        _ => "ExecuteFail"
                    };
            }
        }
    }
}
