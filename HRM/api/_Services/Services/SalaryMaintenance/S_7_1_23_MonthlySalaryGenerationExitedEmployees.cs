using System.Globalization;
using System.Text.RegularExpressions;
using API.Data;
using API._Services.Interfaces.SalaryMaintenance;
using API.DTOs.SalaryMaintenance;
using API.Helper.Constant;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;
namespace API._Services.Services.SalaryMaintenance
{
    public partial class S_7_1_23_MonthlySalaryGenerationExitedEmployees : BaseServices, I_7_1_23_MonthlySalaryGenerationExitedEmployees
    {
        [GeneratedRegex("[^A-Z]+")]
        private static partial Regex MyRegex();
        private static readonly SemaphoreSlim semaphore = new(1, 1);
        public S_7_1_23_MonthlySalaryGenerationExitedEmployees(DBContext dbContext) : base(dbContext)
        {
        }
        public async Task<List<KeyValuePair<string, string>>> GetDropDownList(MonthlySalaryGenerationExitedEmployees_Param param, List<string> roleList)
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

        public Task<OperationResult> CheckCloseStatus(MonthlySalaryGenerationExitedEmployees_Param param)
        {
            // Salary lock status check
            var result = Query_SalaryClose_Status("N", param.Factory, param.Year_Month_Str, param.Permission_Group, param.Employee_Id);
            return result;
        }

        public async Task<OperationResult> Execute(MonthlySalaryGenerationExitedEmployees_Param param, string userName)
        {
            OperationResult result = param.Tab_Type switch
            {
                "salaryGeneration" => await SalaryGeneration(param, userName),
                "dataLock" => await DataLock(param, userName),
                _ => new OperationResult(false, "InvalidTab")
            };
            return result;
        }
        private async Task<OperationResult> SalaryGeneration(MonthlySalaryGenerationExitedEmployees_Param param, string userName)
        {
            await semaphore.WaitAsync();
            string method = nameof(SalaryGeneration);
            string _msg = "";
            string _err = "";
            await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                if (!DateTime.TryParseExact(param.Year_Month_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime InputYearMonth))
                {
                    await _repositoryAccessor.RollbackAsync();
                    return new OperationResult(false, "InvalidInput");
                }
                DateTime now = DateTime.Now;

                // 2.1 Retrieve employee master file
                _msg = GetJSON(method, "2.1");
                var Month_First_day = InputYearMonth;
                var Month_Last_day = InputYearMonth.AddMonths(1).AddDays(-1);
                int cnt = 0;

                var HARM = _repositoryAccessor.HRMS_Att_Resign_Monthly.FindAll(x =>
                    x.Factory == param.Factory &&
                    x.Att_Month.Date == InputYearMonth.Date
                );
                var HARMD = _repositoryAccessor.HRMS_Att_Resign_Monthly_Detail.FindAll(x =>
                   x.Factory == param.Factory &&
                   x.Att_Month.Date == InputYearMonth.Date
                );
                var HAPM = _repositoryAccessor.HRMS_Att_Probation_Monthly.FindAll(x =>
                    x.Factory == param.Factory &&
                    x.Att_Month.Date == InputYearMonth.Date
                );

                var preHSRM = PredicateBuilder.New<HRMS_Sal_Resign_Monthly>(x =>
                   x.Factory == param.Factory &&
                    x.Sal_Month.Date == InputYearMonth.Date);
                if (!string.IsNullOrWhiteSpace(param.Employee_Id))
                    preHSRM.And(x => x.Employee_ID == param.Employee_Id);
                var HSRM = _repositoryAccessor.HRMS_Sal_Resign_Monthly.FindAll(preHSRM);

                var HSMB = _repositoryAccessor.HRMS_Sal_MasterBackup.FindAll(x =>
                    x.Factory == param.Factory &&
                    x.Sal_Month.Date == InputYearMonth.AddMonths(-1).Date
                );
                var HSAM = _repositoryAccessor.HRMS_Sal_AddDedItem_Monthly.FindAll(x =>
                    x.Factory == param.Factory &&
                    x.Sal_Month.Date == InputYearMonth.Date
                );
                var HSP = _repositoryAccessor.HRMS_Sal_Parameter.FindAll(x => x.Factory == param.Factory).ToList();
                var codeList = HSP.Where(x => x.Seq == "5").Select(x => x.Code).ToList();
                var HALM = _repositoryAccessor.HRMS_Att_Leave_Maintain
                    .FindAll(x => x.Factory == param.Factory);
                var HSM = _repositoryAccessor.HRMS_Sal_Master
                    .FindAll(x => x.Factory == param.Factory);
                var HSMD = _repositoryAccessor.HRMS_Sal_Master_Detail
                    .FindAll(x => x.Factory == param.Factory);
                var HSBA = _repositoryAccessor.HRMS_Sal_Bank_Account
                    .FindAll(x => x.Factory == param.Factory);
                var HIEM = _repositoryAccessor.HRMS_Ins_Emp_Maintain
                    .FindAll(x => x.Factory == param.Factory &&
                                  x.Insurance_Type == "V01" &&
                                  (x.Insurance_Start.Date <= Month_First_day.Date ||
                                  (x.Insurance_Start.Date >= Month_First_day.Date && x.Insurance_Start.Date <= Month_Last_day.Date)) &&
                                  x.Insurance_End == null);

                // A
                var HECM = _repositoryAccessor.HRMS_Emp_Contract_Management
                    .FindAll(x => x.Contract_Start.Date > Month_First_day.Date);
                // B
                var HECT = _repositoryAccessor.HRMS_Emp_Contract_Type
                    .FindAll(x => !x.Probationary_Period);

                var HECM_HECT = HECM.Join(HECT,
                    x => new { x.Factory, x.Contract_Type },
                    y => new { y.Factory, y.Contract_Type },
                    (x, y) => new { x.Employee_ID, x.Contract_Start });

                var preHARM = PredicateBuilder.New<HRMS_Att_Resign_Monthly>(true);
                if (!string.IsNullOrWhiteSpace(param.Employee_Id))
                    preHSRM.And(x => x.Employee_ID == param.Employee_Id);
                var HARMs = HARM.Where(preHARM).Select(y => y.Employee_ID);
                var HSRMs = HSRM.Select(y => y.Employee_ID);

                var preHEP = PredicateBuilder.New<HRMS_Emp_Personal>(x =>
                   x.Factory == param.Factory &&
                   x.Onboard_Date.Date <= Month_Last_day.Date &&
                   param.Permission_Group.Contains(x.Permission_Group) &&
                   HARMs.Contains(x.Employee_ID) &&
                   !HSRMs.Contains(x.Employee_ID));

                if (!string.IsNullOrWhiteSpace(param.Employee_Id))
                    preHEP.And(x => x.Employee_ID.ToLower() == param.Employee_Id.ToLower());

                var cr_pt1 = await _repositoryAccessor.HRMS_Emp_Personal.FindAll(preHEP).ToListAsync();

                var LeaveList = HSP.Where(x => x.Seq == "6").Select(x => x.Code).ToList();

                List<HRMS_Sal_Resign_Monthly> addHSRMs = new();
                List<HRMS_Sal_Resign_Monthly_Detail> addHSRMDs = new();
                List<HRMS_Sal_Probation_Monthly_Detail> addDataHSPMD = new();
                List<HRMS_Sal_Probation_Monthly> addDataHSPM = new();

                var HSPMD = _repositoryAccessor.HRMS_Sal_Probation_MasterBackup_Detail
                    .FindAll(x => x.Factory == param.Factory
                        && x.Sal_Month == InputYearMonth);

                var HSPM = _repositoryAccessor.HRMS_Sal_Probation_MasterBackup
                    .FindAll(x => x.Factory == param.Factory
                        && x.Sal_Month == InputYearMonth);

                foreach (var Emp_Personal_Values in cr_pt1)
                {
                    bool Probation_flag = false;
                    HRMS_Sal_MasterBackup Sal_MasterBackup_Values = new();
                    HRMS_Att_Probation_Monthly Att_Probation_Monthly_Values = new();

                    // 2.2 Get leave change file information
                    _msg = GetJSON(method, "2.2");
                    var tempHALM = HALM.Where(x =>
                        x.Employee_ID == Emp_Personal_Values.Employee_ID &&
                        x.Leave_Date.Date >= Month_First_day &&
                        x.Leave_Date.Date <= Month_Last_day
                    );

                    var Leave_Values = HARMD
                        .Where(x => x.Employee_ID == Emp_Personal_Values.Employee_ID &&
                                    LeaveList.Contains(x.Leave_Code))?
                        .Sum(x => x.Days) ?? 0m;
                    var Att_Leave_Maintain_J5 = tempHALM
                        .Where(x => x.Leave_code == "J5")?
                        .Sum(x => x.Days) ?? 0m;

                    // 2.3 Check data
                    _msg = GetJSON(method, "2.3");
                    var Sal_Master_Values = HSM.FirstOrDefault(x => x.Employee_ID == Emp_Personal_Values.Employee_ID);
                    var Sal_Master_Detail_Values = HSMD.FirstOrDefault(x => x.Employee_ID == Emp_Personal_Values.Employee_ID);
                    var Att_Resign_Monthly_Values = HARM.FirstOrDefault(x => x.Employee_ID == Emp_Personal_Values.Employee_ID);
                    var Att_Resign_Monthly_Detail_Values = HARMD.FirstOrDefault(x => x.Employee_ID == Emp_Personal_Values.Employee_ID);

                    if (Sal_Master_Values == null || Sal_Master_Detail_Values == null ||
                        Att_Resign_Monthly_Values == null || Att_Resign_Monthly_Detail_Values == null)
                        continue;

                    // 2.4 Notes on probationary period conversion
                    _msg = GetJSON(method, "2.4");
                    var contracts = HECM_HECT.Where(x => x.Employee_ID == Emp_Personal_Values.Employee_ID);

                    Dictionary<string, VariableCombine> Pkamt_45 = new();
                    Dictionary<string, VariableCombine> Famt_45 = new();
                    Dictionary<string, VariableCombine> PLeave_40 = new();
                    Dictionary<string, VariableCombine> POvertime_42 = new();
                    Dictionary<string, VariableCombine> FOvertime_42 = new();
                    var Probation_MasterBackup_Detail = HSPMD.Where(x => x.Employee_ID == Emp_Personal_Values.Employee_ID).ToList();
                    var Probation_MasterBackup = HSPM.FirstOrDefault(x => x.Employee_ID == Emp_Personal_Values.Employee_ID);
                    if (contracts.Any())
                    {
                        var StartDate = contracts.Max(x => x.Contract_Start);

                        var YEAR_MONTH_StartDate = new DateTime(StartDate.Year, StartDate.Month, 1);
                        if (YEAR_MONTH_StartDate == InputYearMonth && StartDate > Month_First_day)
                        {
                            Probation_flag = true;
                            Att_Probation_Monthly_Values = HAPM.FirstOrDefault(x => x.Employee_ID == Emp_Personal_Values.Employee_ID);

                            if (!Probation_MasterBackup_Detail.Any())
                            {
                                _err += string.Format(
                                    "尚未產生試用期備份 HRMS_Sal_Probation_MasterBackup_Detail:\n{0}, {1:yyyy/MM}, {2}\n",
                                    param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID);
                                continue;
                            }

                            if (Probation_MasterBackup == null)
                            {
                                _err += string.Format(
                                    "尚未產生試用期備份 HRMS_Sal_Probation_MasterBackup:\n{0}, {1:yyyy/MM}, {2}\n",
                                    param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID);
                                continue;
                            }

                            Pkamt_45 = await Query_Sal_Detail_Variable_Combine("P", "45", "Pkvalue", "Pkamt", "Pamt",
                                param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID);

                            Famt_45 = await Query_Sal_Detail_Variable_Combine("M", "45", "Fvalue", "FMainamt", "Famt",
                                param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID);

                            PLeave_40 = await Query_Att_Monthly_Detail_Variable_Combine(
                                TableSourceType.HRMS_Att_Probation_Monthly_Detail, "40", "PLeave", "PLeaveDays", "PAmt",
                                param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "1");

                            POvertime_42 = await Query_Att_Monthly_Detail_Variable_Combine(
                                TableSourceType.HRMS_Att_Probation_Monthly_Detail, "42", "POvertime", "POvertimeHours", "PAmt",
                                param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "2");

                            FOvertime_42 = await Query_Att_Monthly_Detail_Variable_Combine(
                                TableSourceType.HRMS_Att_Resign_Monthly_Detail, "42", "FOvertime", "FOvertimeHours", "FAmt",
                                param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "2");
                        }
                    }
                    // 2.5 Declare monthly salary details-main file variables
                    _msg = GetJSON(method, "2.5");
                    Dictionary<string, VariableCombine> Amt_45 = await Query_Sal_Detail_Variable_Combine(
                        "M", "45", "Mvalue", "Mainamt", "Amt",
                        param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID);
                    Dictionary<string, VariableCombine> Leave_40 = await Query_Att_Monthly_Detail_Variable_Combine(
                        TableSourceType.HRMS_Att_Resign_Monthly_Detail, "40", "Leave", "LeaveDays", "Amt",
                        param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "1");
                    Dictionary<string, VariableCombine> Overtime_42 = await Query_Att_Monthly_Detail_Variable_Combine(
                       TableSourceType.HRMS_Att_Resign_Monthly_Detail, "42", "Overtime", "OvertimeHours", "Amt",
                       param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "2");

                    // 2.6 The actual number of working days is 0
                    _msg = GetJSON(method, "2.6");
                    if (Att_Resign_Monthly_Values.Actual_Days == 0 && Leave_Values == 0)
                        continue;

                    // 2.7 Declare monthly salary master file variables && 2.8 Bank transfer
                    _msg = GetJSON(method, "2.7");
                    HRMS_Sal_Resign_Monthly Sal_Resign_Monthly = new()
                    {
                        USER_GUID = Emp_Personal_Values.USER_GUID,
                        Division = Emp_Personal_Values.Division,
                        Factory = Emp_Personal_Values.Factory,
                        Sal_Month = InputYearMonth,
                        Employee_ID = Emp_Personal_Values.Employee_ID,
                        Department = Att_Resign_Monthly_Values.Department,
                        Currency = Sal_Master_Values.Currency,
                        Permission_Group = Sal_Master_Values.Permission_Group,
                        Salary_Type = Sal_Master_Values.Salary_Type,
                        Lock = "N",
                        Tax = 0,
                        Update_By = userName,
                        Update_Time = now
                    };

                    // 2.7.1
                    HRMS_Sal_Probation_Monthly PSal_Resign_Monthly = null;
                    if (Probation_MasterBackup != null)
                    {
                        PSal_Resign_Monthly = new()
                        {
                            USER_GUID = Emp_Personal_Values.USER_GUID,
                            Division = Emp_Personal_Values.Division,
                            Factory = Emp_Personal_Values.Factory,
                            Sal_Month = InputYearMonth,
                            Employee_ID = Emp_Personal_Values.Employee_ID,
                            Probation = "Y",
                            Department = Att_Resign_Monthly_Values.Department,
                            Currency = Probation_MasterBackup.Currency,
                            Permission_Group = Probation_MasterBackup.Permission_Group,
                            Salary_Type = Probation_MasterBackup.Salary_Type,
                            Lock = "N",
                            Tax = 0,
                            Update_By = userName,
                            Update_Time = now
                        };
                    }

                    // 2.7.2
                    HRMS_Sal_Probation_Monthly FSal_Resign_Monthly = new()
                    {
                        USER_GUID = Emp_Personal_Values.USER_GUID,
                        Division = Emp_Personal_Values.Division,
                        Factory = Emp_Personal_Values.Factory,
                        Sal_Month = InputYearMonth,
                        Employee_ID = Emp_Personal_Values.Employee_ID,
                        Probation = "N",
                        Department = Att_Resign_Monthly_Values.Department,
                        Currency = Sal_Master_Values.Currency,
                        Permission_Group = Sal_Master_Values.Permission_Group,
                        Salary_Type = Sal_Master_Values.Salary_Type,
                        Lock = "N",
                        Tax = 0,
                        Update_By = userName,
                        Update_Time = now
                    };

                    // 2.8 Bank transfer
                    _msg = GetJSON(method, "2.8");
                    var isBankAccount = HSBA.Any(x => x.Employee_ID == Emp_Personal_Values.Employee_ID);
                    Sal_Resign_Monthly.BankTransfer = isBankAccount ? "Y" : "N";
                    if (PSal_Resign_Monthly != null)
                        PSal_Resign_Monthly.BankTransfer = isBankAccount ? "Y" : "N";
                    FSal_Resign_Monthly.BankTransfer = isBankAccount ? "Y" : "N";

                    // 2.9 Packing paid leave
                    _msg = GetJSON(method, "2.9");
                    decimal D0_LeaveDays_40 = Leave_40.GetValueOrDefault("D0_LeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal H0_LeaveDays_40 = Leave_40.GetValueOrDefault("H0_LeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal I0_LeaveDays_40 = Leave_40.GetValueOrDefault("I0_LeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal I1_LeaveDays_40 = Leave_40.GetValueOrDefault("I1_LeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal K0_LeaveDays_40 = Leave_40.GetValueOrDefault("K0_LeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal E0_LeaveDays_40 = Leave_40.GetValueOrDefault("E0_LeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal F0_LeaveDays_40 = Leave_40.GetValueOrDefault("F0_LeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal G1_LeaveDays_40 = Leave_40.GetValueOrDefault("G1_LeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal J2_LeaveDays_40 = Leave_40.GetValueOrDefault("J2_LeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal J1_LeaveDays_40 = Leave_40.GetValueOrDefault("J1_LeaveDays_40")?.Value as decimal? ?? 0m;

                    var Paid_days = Att_Resign_Monthly_Values.Actual_Days +
                        D0_LeaveDays_40 + H0_LeaveDays_40 + I0_LeaveDays_40 + I1_LeaveDays_40 +
                        K0_LeaveDays_40 + E0_LeaveDays_40 + F0_LeaveDays_40 + G1_LeaveDays_40 +
                        J2_LeaveDays_40 + J1_LeaveDays_40;

                    decimal D0_PLeaveDays_40 = Leave_40.GetValueOrDefault("D0_PLeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal H0_PLeaveDays_40 = Leave_40.GetValueOrDefault("H0_PLeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal I0_PLeaveDays_40 = Leave_40.GetValueOrDefault("I0_PLeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal I1_PLeaveDays_40 = Leave_40.GetValueOrDefault("I1_PLeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal K0_PLeaveDays_40 = Leave_40.GetValueOrDefault("K0_PLeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal E0_PLeaveDays_40 = Leave_40.GetValueOrDefault("E0_PLeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal F0_PLeaveDays_40 = Leave_40.GetValueOrDefault("F0_PLeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal G1_PLeaveDays_40 = Leave_40.GetValueOrDefault("G1_PLeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal J2_PLeaveDays_40 = Leave_40.GetValueOrDefault("J2_PLeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal J1_PLeaveDays_40 = Leave_40.GetValueOrDefault("J1_PLeaveDays_40")?.Value as decimal? ?? 0m;

                    var Paid_days_P = Att_Probation_Monthly_Values.Actual_Days +
                        D0_PLeaveDays_40 + H0_PLeaveDays_40 + I0_PLeaveDays_40 + I1_PLeaveDays_40 +
                        K0_PLeaveDays_40 + E0_PLeaveDays_40 + F0_PLeaveDays_40 + G1_PLeaveDays_40 +
                        J2_PLeaveDays_40 + J1_PLeaveDays_40;

                    // 2.10	Preparatory work END	
                    // 2.11	Round to the nearest integer
                    // 2.12 Probationary period to full-time employee 
                    _msg = GetJSON(method, "2.12");
                    decimal A01_Pamt_45 = Pkamt_45.GetValueOrDefault("A01_Pamt_45")?.Value as decimal? ?? 0m;
                    decimal A02_Pamt_45 = Pkamt_45.GetValueOrDefault("A02_Pamt_45")?.Value as decimal? ?? 0m;
                    decimal B01_Pamt_45 = Pkamt_45.GetValueOrDefault("B01_Pamt_45")?.Value as decimal? ?? 0m;
                    decimal B02_Pamt_45 = Pkamt_45.GetValueOrDefault("B02_Pamt_45")?.Value as decimal? ?? 0m;
                    decimal B04_Pamt_45 = Pkamt_45.GetValueOrDefault("B04_Pamt_45")?.Value as decimal? ?? 0m;
                    decimal B05_Pamt_45 = Pkamt_45.GetValueOrDefault("B05_Pamt_45")?.Value as decimal? ?? 0m;
                    decimal B06_Pamt_45 = Pkamt_45.GetValueOrDefault("B06_Pamt_45")?.Value as decimal? ?? 0m;
                    decimal B09_Pamt_45 = Pkamt_45.GetValueOrDefault("B09_Pamt_45")?.Value as decimal? ?? 0m;
                    decimal B10_Pamt_45 = Pkamt_45.GetValueOrDefault("B10_Pamt_45")?.Value as decimal? ?? 0m;

                    decimal AV2_Pamt_45 = Pkamt_45.GetValueOrDefault("AV2_Pamt_45")?.Value as decimal? ?? 0m;
                    decimal C02_Pamt_45 = Pkamt_45.GetValueOrDefault("C02_Pamt_45")?.Value as decimal? ?? 0m;
                    decimal D01_Pamt_45 = Pkamt_45.GetValueOrDefault("D01_Pamt_45")?.Value as decimal? ?? 0m;

                    decimal A01_Pkamt_45 = Pkamt_45.GetValueOrDefault("A01_Pkamt_45")?.Value as decimal? ?? 0m;
                    decimal A02_Pkamt_45 = Pkamt_45.GetValueOrDefault("A02_Pkamt_45")?.Value as decimal? ?? 0m;
                    decimal B01_Pkamt_45 = Pkamt_45.GetValueOrDefault("B01_Pkamt_45")?.Value as decimal? ?? 0m;
                    decimal B02_Pkamt_45 = Pkamt_45.GetValueOrDefault("B02_Pkamt_45")?.Value as decimal? ?? 0m;
                    decimal B04_Pkamt_45 = Pkamt_45.GetValueOrDefault("B04_Pkamt_45")?.Value as decimal? ?? 0m;
                    decimal B05_Pkamt_45 = Pkamt_45.GetValueOrDefault("B05_Pkamt_45")?.Value as decimal? ?? 0m;
                    decimal B06_Pkamt_45 = Pkamt_45.GetValueOrDefault("B06_Pkamt_45")?.Value as decimal? ?? 0m;
                    decimal B09_Pkamt_45 = Pkamt_45.GetValueOrDefault("B09_Pkamt_45")?.Value as decimal? ?? 0m;
                    decimal B10_Pkamt_45 = Pkamt_45.GetValueOrDefault("B10_Pkamt_45")?.Value as decimal? ?? 0m;

                    decimal A01_Famt_45 = Famt_45.GetValueOrDefault("A01_Famt_45")?.Value as decimal? ?? 0m;
                    decimal A02_Famt_45 = Famt_45.GetValueOrDefault("A02_Famt_45")?.Value as decimal? ?? 0m;
                    decimal B01_Famt_45 = Famt_45.GetValueOrDefault("B01_Famt_45")?.Value as decimal? ?? 0m;
                    decimal B02_Famt_45 = Famt_45.GetValueOrDefault("B02_Famt_45")?.Value as decimal? ?? 0m;
                    decimal B04_Famt_45 = Famt_45.GetValueOrDefault("B04_Famt_45")?.Value as decimal? ?? 0m;
                    decimal B05_Famt_45 = Famt_45.GetValueOrDefault("B05_Famt_45")?.Value as decimal? ?? 0m;
                    decimal B06_Famt_45 = Famt_45.GetValueOrDefault("B06_Famt_45")?.Value as decimal? ?? 0m;
                    decimal B07_Famt_45 = Famt_45.GetValueOrDefault("B07_Famt_45")?.Value as decimal? ?? 0m;
                    decimal B09_Famt_45 = Famt_45.GetValueOrDefault("B09_Famt_45")?.Value as decimal? ?? 0m;
                    decimal B10_Famt_45 = Famt_45.GetValueOrDefault("B10_Famt_45")?.Value as decimal? ?? 0m;

                    decimal A01_Amt_45 = Amt_45.GetValueOrDefault("A01_Amt_45")?.Value as decimal? ?? 0m;
                    decimal A02_Amt_45 = Amt_45.GetValueOrDefault("A02_Amt_45")?.Value as decimal? ?? 0m;
                    decimal B01_Amt_45 = Amt_45.GetValueOrDefault("B01_Amt_45")?.Value as decimal? ?? 0m;
                    decimal B02_Amt_45 = Amt_45.GetValueOrDefault("B02_Amt_45")?.Value as decimal? ?? 0m;
                    decimal B04_Amt_45 = Amt_45.GetValueOrDefault("B04_Amt_45")?.Value as decimal? ?? 0m;
                    decimal B05_Amt_45 = Amt_45.GetValueOrDefault("B05_Amt_45")?.Value as decimal? ?? 0m;
                    decimal B06_Amt_45 = Amt_45.GetValueOrDefault("B06_Amt_45")?.Value as decimal? ?? 0m;
                    decimal B07_Amt_45 = Amt_45.GetValueOrDefault("B07_Amt_45")?.Value as decimal? ?? 0m;
                    decimal B09_Amt_45 = Amt_45.GetValueOrDefault("B09_Amt_45")?.Value as decimal? ?? 0m;
                    decimal B10_Amt_45 = Amt_45.GetValueOrDefault("B10_Amt_45")?.Value as decimal? ?? 0m;
                    decimal C02_Amt_45 = Amt_45.GetValueOrDefault("C02_Amt_45")?.Value as decimal? ?? 0m;
                    decimal C01_Amt_45 = Amt_45.GetValueOrDefault("C01_Amt_45")?.Value as decimal? ?? 0m;
                    decimal AV2_Amt_45 = Amt_45.GetValueOrDefault("AV2_Amt_45")?.Value as decimal? ?? 0m;

                    decimal A01_Mainamt_45 = Amt_45.GetValueOrDefault("A01_Mainamt_45")?.Value as decimal? ?? 0m;
                    decimal A02_Mainamt_45 = Amt_45.GetValueOrDefault("A02_Mainamt_45")?.Value as decimal? ?? 0m;
                    decimal B01_Mainamt_45 = Amt_45.GetValueOrDefault("B01_Mainamt_45")?.Value as decimal? ?? 0m;
                    decimal B02_Mainamt_45 = Amt_45.GetValueOrDefault("B02_Mainamt_45")?.Value as decimal? ?? 0m;
                    decimal B04_Mainamt_45 = Amt_45.GetValueOrDefault("B04_Mainamt_45")?.Value as decimal? ?? 0m;
                    decimal B05_Mainamt_45 = Amt_45.GetValueOrDefault("B05_Mainamt_45")?.Value as decimal? ?? 0m;
                    decimal B06_Mainamt_45 = Amt_45.GetValueOrDefault("B06_Mainamt_45")?.Value as decimal? ?? 0m;
                    decimal B07_Mainamt_45 = Amt_45.GetValueOrDefault("B07_Mainamt_45")?.Value as decimal? ?? 0m;
                    decimal B09_Mainamt_45 = Amt_45.GetValueOrDefault("B09_Mainamt_45")?.Value as decimal? ?? 0m;
                    decimal B10_Mainamt_45 = Amt_45.GetValueOrDefault("B10_Mainamt_45")?.Value as decimal? ?? 0m;
                    decimal C02_Mainamt_45 = Amt_45.GetValueOrDefault("C02_Mainamt_45")?.Value as decimal? ?? 0m;
                    decimal C01_Mainamt_45 = Amt_45.GetValueOrDefault("C01_Mainamt_45")?.Value as decimal? ?? 0m;
                    decimal AV2_Mainamt_45 = Amt_45.GetValueOrDefault("AV2_Mainamt_45")?.Value as decimal? ?? 0m;

                    decimal W0_LeaveDays_40 = Leave_40.GetValueOrDefault("W0_LeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal J5_LeaveDays_40 = Leave_40.GetValueOrDefault("J5_LeaveDays_40")?.Value as decimal? ?? 0m;

                    if (Probation_flag)
                    {
                        //2.12.1  Trial period salary
                        // 2.12.1.1  A01 Basic salary
                        _msg = GetJSON(method, "2.12.1.1");
                        A01_Pamt_45 = Math.Round(A01_Pkamt_45 / param.Salary_Days * Paid_days_P, 0);

                        // 2.12.1.2  A02 Years of Service Basic Salary
                        _msg = GetJSON(method, "2.12.1.2");
                        A02_Pamt_45 = Math.Round(A02_Pkamt_45 / param.Salary_Days * Paid_days_P, 0);

                        // 2.12.1.3  B01 Supervisor Allowance
                        _msg = GetJSON(method, "2.12.1.3");
                        B01_Pamt_45 = Math.Round(B01_Pkamt_45 / param.Salary_Days * Paid_days_P, 0);

                        // 2.12.1.4  B02 Expertise Allowance
                        _msg = GetJSON(method, "2.12.1.4");
                        B02_Pamt_45 = Math.Round(B02_Pkamt_45 / param.Salary_Days * Paid_days_P, 0);

                        // 2.12.1.5  B04 Technical Allowance
                        _msg = GetJSON(method, "2.12.1.5");
                        B04_Pamt_45 = Math.Round(B04_Pkamt_45 / param.Salary_Days * Paid_days_P, 0);

                        // 2.12.1.6  B05 Special Technical Allowance
                        _msg = GetJSON(method, "2.12.1.6");
                        B05_Pamt_45 = Math.Round(B05_Pkamt_45 / param.Salary_Days * Paid_days_P, 0);

                        // 2.12.1.7  B06 Special Allowance
                        _msg = GetJSON(method, "2.12.1.7");
                        B06_Pamt_45 = Math.Round(B06_Pkamt_45 / param.Salary_Days * Paid_days_P, 0);

                        // 2.12.1.8  B09 Production process management allowance
                        _msg = GetJSON(method, "2.12.1.8");
                        B09_Pamt_45 = Math.Round(B09_Pkamt_45 / param.Salary_Days * Paid_days_P, 0);

                        // 2.12.1.9  B10 Lean Production Promotion Subsidy
                        _msg = GetJSON(method, "2.12.1.9");
                        B10_Pamt_45 = Math.Round(B10_Pkamt_45 / param.Salary_Days * Paid_days_P, 0);

                        // 2.12.2  Salary after signing	
                        // 2.12.2.1  A01 Basic salary
                        _msg = GetJSON(method, "2.12.2.1");
                        A01_Famt_45 = Math.Round(A01_Mainamt_45 / param.Salary_Days * (Paid_days - Paid_days_P), 0);
                        A01_Amt_45 = A01_Famt_45 + A01_Pamt_45;

                        // 2.12.2.2  A02 Years of Service Basic Salary
                        _msg = GetJSON(method, "2.12.2.2");
                        A02_Famt_45 = Math.Round(A02_Mainamt_45 / param.Salary_Days * (Paid_days - Paid_days_P), 0);
                        A02_Amt_45 = A02_Famt_45 + A02_Pamt_45;

                        // 2.12.2.3  B01 Supervisor Allowance
                        _msg = GetJSON(method, "2.12.2.3");
                        B01_Famt_45 = Math.Round(B01_Mainamt_45 / param.Salary_Days * (Paid_days - Paid_days_P), 0);
                        B01_Amt_45 = B01_Famt_45 + B01_Pamt_45;

                        // 2.12.2.4  B02 Expertise Allowance
                        _msg = GetJSON(method, "2.12.2.4");
                        B02_Famt_45 = Math.Round(B02_Mainamt_45 / param.Salary_Days * (Paid_days - Paid_days_P), 0);
                        B02_Amt_45 = B02_Famt_45 + B02_Pamt_45;

                        // 2.12.2.5  B04 Technical Allowance
                        _msg = GetJSON(method, "2.12.2.5");
                        B04_Famt_45 = Math.Round(B04_Mainamt_45 / param.Salary_Days * (Paid_days - Paid_days_P), 0);
                        B04_Amt_45 = B04_Famt_45 + B04_Pamt_45;

                        // 2.12.2.6  B05 Special Technical Allowance
                        _msg = GetJSON(method, "2.12.2.6");
                        B05_Famt_45 = Math.Round(B05_Mainamt_45 / param.Salary_Days * (Paid_days - Paid_days_P), 0);
                        B05_Amt_45 = B05_Famt_45 + B05_Pamt_45;

                        // 2.12.2.7  B06 Special Allowance
                        _msg = GetJSON(method, "2.12.2.7");
                        B06_Famt_45 = Math.Round(B06_Mainamt_45 / param.Salary_Days * (Paid_days - Paid_days_P), 0);
                        B06_Amt_45 = B06_Famt_45 + B06_Pamt_45;

                        // 2.12.2.8  B07 Work allowance
                        _msg = GetJSON(method, "2.12.2.8");
                        B07_Famt_45 = Math.Round(B07_Mainamt_45 / param.Salary_Days * Paid_days , 0);
                        B07_Amt_45 = B07_Famt_45;

                        // 2.12.2.9  B09 Production process management allowance
                        _msg = GetJSON(method, "2.12.2.9");
                        B09_Famt_45 = Math.Round(B09_Mainamt_45 / param.Salary_Days * (Paid_days - Paid_days_P), 0);
                        B09_Amt_45 = B09_Famt_45 + B09_Pamt_45;

                        // 2.12.2.10  B10 Lean Production Promotion Subsidy
                        _msg = GetJSON(method, "2.12.2.10");
                        B10_Famt_45 = Math.Round(B10_Mainamt_45 / param.Salary_Days * (Paid_days - Paid_days_P), 0);
                        B10_Amt_45 = B10_Famt_45 + B10_Pamt_45;

                        // 2.12.2.11  C02 Living allowance
                        _msg = GetJSON(method, "2.12.2.11");
                        C02_Amt_45 = Math.Round(C02_Mainamt_45 / param.Salary_Days * (Paid_days + W0_LeaveDays_40 * 2), 0);

                        // 2.12.2.12  C01 Overtime Pay
                        _msg = GetJSON(method, "2.12.2.12");
                        C01_Amt_45 = Math.Round(C01_Mainamt_45 / param.Salary_Days * Paid_days, 0);

                        // 2.12.2.13  AV2 stops work to negotiate salary
                        _msg = GetJSON(method, "2.12.2.13");
                        AV2_Amt_45 = Math.Round(AV2_Mainamt_45 / param.Salary_Days * J5_LeaveDays_40, 0);

                    }
                    else
                    {
                        // 2.13	Resignation	
                        // 2.13.1  A01 Basic salary
                        _msg = GetJSON(method, "2.13.1");
                        A01_Amt_45 = Math.Round(A01_Mainamt_45 / param.Salary_Days * Paid_days, 0);

                        // 2.13.2  A02 Years of Service Basic Salary
                        _msg = GetJSON(method, "2.13.2");
                        A02_Amt_45 = Math.Round(A02_Mainamt_45 / param.Salary_Days * Paid_days, 0);

                        // 2.13.3  B01 Supervisor Allowance
                        _msg = GetJSON(method, "2.13.3");
                        B01_Amt_45 = Math.Round(B01_Mainamt_45 / param.Salary_Days * Paid_days, 0);

                        // 2.13.4  B02 Expertise Allowance
                        _msg = GetJSON(method, "2.13.4");
                        B02_Amt_45 = Math.Round(B02_Mainamt_45 / param.Salary_Days * Paid_days, 0);

                        // 2.13.5  B04 Technical Allowance
                        _msg = GetJSON(method, "2.13.5");
                        B04_Amt_45 = Math.Round(B04_Mainamt_45 / param.Salary_Days * Paid_days, 0);

                        // 2.13.6  B05 Special Technical Allowance
                        _msg = GetJSON(method, "2.13.6");
                        B05_Amt_45 = Math.Round(B05_Mainamt_45 / param.Salary_Days * Paid_days, 0);

                        // 2.13.7  B06 Special Allowance
                        _msg = GetJSON(method, "2.13.7");
                        B06_Amt_45 = Math.Round(B06_Mainamt_45 / param.Salary_Days * Paid_days, 0);

                        // 2.13.8  B07 Work allowance
                        _msg = GetJSON(method, "2.13.8");
                        B07_Amt_45 = Math.Round(B07_Mainamt_45 / param.Salary_Days * Paid_days, 0);

                        // 2.13.9  B09 Production process management allowance
                        _msg = GetJSON(method, "2.13.9");
                        B09_Amt_45 = Math.Round(B09_Mainamt_45 / param.Salary_Days * Paid_days, 0);

                        // 2.13.10  B10 Lean Production Promotion Subsidy
                        _msg = GetJSON(method, "2.13.10");
                        B10_Amt_45 = Math.Round(B10_Mainamt_45 / param.Salary_Days * Paid_days, 0);

                        // 2.13.11  C02 Living allowance
                        _msg = GetJSON(method, "2.13.11");
                        C02_Amt_45 = Math.Round(C02_Mainamt_45 / param.Salary_Days * (Paid_days + W0_LeaveDays_40), 0);

                        // 2.13.12  C01 Overtime Pay
                        _msg = GetJSON(method, "2.13.12");
                        C01_Amt_45 = Math.Round(C01_Mainamt_45 / param.Salary_Days * Paid_days, 0);

                        // 2.13.13  AV2 stops work to negotiate salary
                        _msg = GetJSON(method, "2.13.13");
                        AV2_Amt_45 = Math.Round(AV2_Mainamt_45 / param.Salary_Days * J5_LeaveDays_40, 0);
                    }
                    // 2.14 Overtime & Insurance Salary Basic Amount
                    _msg = GetJSON(method, "2.14");
                    var Basic_Amt = await Query_WageStandard_Sum("M", param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, Sal_Master_Values.Permission_Group, Sal_Master_Values.Salary_Type);

                    decimal A01_PAmt_42 = POvertime_42.GetValueOrDefault("A01_PAmt_42")?.Value as decimal? ?? 0m;
                    decimal A02_PAmt_42 = POvertime_42.GetValueOrDefault("A02_PAmt_42")?.Value as decimal? ?? 0m;
                    decimal B01_PAmt_42 = POvertime_42.GetValueOrDefault("B01_PAmt_42")?.Value as decimal? ?? 0m;
                    decimal C01_PAmt_42 = POvertime_42.GetValueOrDefault("C01_PAmt_42")?.Value as decimal? ?? 0m;

                    decimal A01_FAmt_42 = FOvertime_42.GetValueOrDefault("A01_FAmt_42")?.Value as decimal? ?? 0m;
                    decimal A02_FAmt_42 = FOvertime_42.GetValueOrDefault("A02_FAmt_42")?.Value as decimal? ?? 0m;
                    decimal B01_FAmt_42 = FOvertime_42.GetValueOrDefault("B01_FAmt_42")?.Value as decimal? ?? 0m;
                    decimal C01_FAmt_42 = FOvertime_42.GetValueOrDefault("C01_FAmt_42")?.Value as decimal? ?? 0m;

                    decimal A01_POvertimeHours_42 = POvertime_42.GetValueOrDefault("A01_POvertimeHours_42")?.Value as decimal? ?? 0m;
                    decimal A02_POvertimeHours_42 = POvertime_42.GetValueOrDefault("A02_POvertimeHours_42")?.Value as decimal? ?? 0m;
                    decimal A04_POvertimeHours_42 = POvertime_42.GetValueOrDefault("A04_POvertimeHours_42")?.Value as decimal? ?? 0m;
                    decimal B01_POvertimeHours_42 = POvertime_42.GetValueOrDefault("B01_POvertimeHours_42")?.Value as decimal? ?? 0m;
                    decimal B04_POvertimeHours_42 = POvertime_42.GetValueOrDefault("B04_POvertimeHours_42")?.Value as decimal? ?? 0m;
                    decimal C01_POvertimeHours_42 = POvertime_42.GetValueOrDefault("C01_POvertimeHours_42")?.Value as decimal? ?? 0m;

                    decimal A01_Amt_42 = Overtime_42.GetValueOrDefault("A01_Amt_42")?.Value as decimal? ?? 0m;
                    decimal A02_Amt_42 = Overtime_42.GetValueOrDefault("A02_Amt_42")?.Value as decimal? ?? 0m;
                    decimal A03_Amt_42 = Overtime_42.GetValueOrDefault("A03_Amt_42")?.Value as decimal? ?? 0m;
                    decimal B01_Amt_42 = Overtime_42.GetValueOrDefault("B01_Amt_42")?.Value as decimal? ?? 0m;
                    decimal B03_Amt_42 = Overtime_42.GetValueOrDefault("B03_Amt_42")?.Value as decimal? ?? 0m;
                    decimal C01_Amt_42 = Overtime_42.GetValueOrDefault("C01_Amt_42")?.Value as decimal? ?? 0m;

                    decimal A01_OvertimeHours_42 = Overtime_42.GetValueOrDefault("A01_OvertimeHours_42")?.Value as decimal? ?? 0m;
                    decimal A02_OvertimeHours_42 = Overtime_42.GetValueOrDefault("A02_OvertimeHours_42")?.Value as decimal? ?? 0m;
                    decimal A03_OvertimeHours_42 = Overtime_42.GetValueOrDefault("A03_OvertimeHours_42")?.Value as decimal? ?? 0m;
                    decimal A04_OvertimeHours_42 = Overtime_42.GetValueOrDefault("A04_OvertimeHours_42")?.Value as decimal? ?? 0m;
                    decimal B01_OvertimeHours_42 = Overtime_42.GetValueOrDefault("B01_OvertimeHours_42")?.Value as decimal? ?? 0m;
                    decimal B04_OvertimeHours_42 = Overtime_42.GetValueOrDefault("B04_OvertimeHours_42")?.Value as decimal? ?? 0m;
                    decimal C01_OvertimeHours_42 = Overtime_42.GetValueOrDefault("C01_OvertimeHours_42")?.Value as decimal? ?? 0m;

                    // 2.14.1 Overtime pay calculation
                    _msg = GetJSON(method, "2.14.1");
                    decimal Oneday_hour = 8;

                    HRMS_Sal_Parameter Sal_Parameter = new();
                    if (Probation_flag)
                    {
                        var Basic_PKAmt = await Query_WageStandard_Sum("P", param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, Probation_MasterBackup.Permission_Group, Probation_MasterBackup.Salary_Type);

                        // 2.14.1.1	A01 Overtime pay 1.5 
                        _msg = GetJSON(method, "2.14.1.1");
                        Sal_Parameter = HSP.FirstOrDefault(x => x.Seq == "1" && x.Code == "A01");
                        A01_PAmt_42 = Math.Round(Basic_PKAmt / param.Salary_Days / Oneday_hour * Sal_Parameter.Code_Amt * (A01_POvertimeHours_42 + A04_POvertimeHours_42), 0);

                        // 2.14.1.2	A02 Normal night shift hours
                        _msg = GetJSON(method, "2.14.1.2");
                        Sal_Parameter = HSP.FirstOrDefault(x => x.Seq == "1" && x.Code == "A02");
                        A02_PAmt_42 = Math.Round(Basic_PKAmt / param.Salary_Days / Oneday_hour * Sal_Parameter.Code_Amt * A02_POvertimeHours_42, 0);

                        // 2.14.1.3	B01 Holiday overtime pay
                        _msg = GetJSON(method, "2.14.1.3");
                        Sal_Parameter = HSP.FirstOrDefault(x => x.Seq == "1" && x.Code == "B01");
                        B01_PAmt_42 = Math.Round(Basic_PKAmt / param.Salary_Days / Oneday_hour * Sal_Parameter.Code_Amt * (B01_POvertimeHours_42 + B04_POvertimeHours_42), 0);

                        // 2.14.1.4	C01 National Holiday Overtime 3.0
                        _msg = GetJSON(method, "2.14.1.4");
                        Sal_Parameter = HSP.FirstOrDefault(x => x.Seq == "1" && x.Code == "C01");
                        C01_PAmt_42 = Math.Round(Basic_PKAmt / param.Salary_Days / Oneday_hour * Sal_Parameter.Code_Amt * C01_POvertimeHours_42, 0);

                        // 2.14.1.5	A01 Overtime pay 1.5
                        _msg = GetJSON(method, "2.14.1.5");
                        Sal_Parameter = HSP.FirstOrDefault(x => x.Seq == "1" && x.Code == "A01");
                        A01_FAmt_42 = Math.Round(Basic_Amt / param.Salary_Days / Oneday_hour * Sal_Parameter.Code_Amt * (A01_OvertimeHours_42 + A04_OvertimeHours_42 - A01_POvertimeHours_42 - A04_POvertimeHours_42), 0);
                        A01_Amt_42 = A01_FAmt_42 + A01_PAmt_42;

                        // 2.14.1.6	A02 Normal night shift hours
                        _msg = GetJSON(method, "2.14.1.6");
                        Sal_Parameter = HSP.FirstOrDefault(x => x.Seq == "1" && x.Code == "A02");
                        A02_FAmt_42 = Math.Round(Basic_Amt / param.Salary_Days / Oneday_hour * Sal_Parameter.Code_Amt * (A02_OvertimeHours_42 - A02_POvertimeHours_42), 0);
                        A02_Amt_42 = A02_FAmt_42 + A02_PAmt_42;

                        // 2.14.1.7	B01 Holiday overtime pay
                        _msg = GetJSON(method, "2.14.1.7");
                        Sal_Parameter = HSP.FirstOrDefault(x => x.Seq == "1" && x.Code == "B01");
                        B01_FAmt_42 = Math.Round(Basic_Amt / param.Salary_Days / Oneday_hour * Sal_Parameter.Code_Amt * (B01_OvertimeHours_42 + B04_OvertimeHours_42 - B01_POvertimeHours_42 - B04_POvertimeHours_42), 0);
                        B01_Amt_42 = B01_FAmt_42 + B01_PAmt_42;

                        // 2.14.1.8	C01 National Holiday Overtime 3.0
                        _msg = GetJSON(method, "2.14.1.8");
                        Sal_Parameter = HSP.FirstOrDefault(x => x.Seq == "1" && x.Code == "C01");
                        C01_FAmt_42 = Math.Round(Basic_Amt / param.Salary_Days / Oneday_hour * Sal_Parameter.Code_Amt * (C01_OvertimeHours_42 - C01_POvertimeHours_42), 0);
                        C01_Amt_42 = C01_FAmt_42 + C01_PAmt_42;
                    }
                    else
                    {
                        // 2.14.2	Resignation
                        // 2.14.2.1 A01 Regular overtime pay 1.5
                        _msg = GetJSON(method, "2.14.2.1");
                        Sal_Parameter = HSP.FirstOrDefault(x => x.Seq == "1" && x.Code == "A01");
                        A01_Amt_42 = Math.Round(Basic_Amt / param.Salary_Days / Oneday_hour * Sal_Parameter.Code_Amt * (A01_OvertimeHours_42 + A04_OvertimeHours_42), 0);

                        // 2.14.2.2	A02 Normal night shift hours
                        _msg = GetJSON(method, "2.14.2.2");
                        Sal_Parameter = HSP.FirstOrDefault(x => x.Seq == "1" && x.Code == "A02");
                        A02_Amt_42 = Math.Round(Basic_Amt / param.Salary_Days / Oneday_hour * Sal_Parameter.Code_Amt * A02_OvertimeHours_42, 0);

                        // 2.14.2.3	B01 Holiday overtime pay
                        _msg = GetJSON(method, "2.14.2.3");
                        Sal_Parameter = HSP.FirstOrDefault(x => x.Seq == "1" && x.Code == "B01");
                        B01_Amt_42 = Math.Round(Basic_Amt / param.Salary_Days / Oneday_hour * Sal_Parameter.Code_Amt * (B01_OvertimeHours_42 + B04_OvertimeHours_42), 0);

                        // 2.14.2.4	C01 National Holiday Overtime 3.0
                        _msg = GetJSON(method, "2.14.2.4");
                        Sal_Parameter = HSP.FirstOrDefault(x => x.Seq == "1" && x.Code == "C01");
                        C01_Amt_42 = Math.Round(Basic_Amt / param.Salary_Days / Oneday_hour * Sal_Parameter.Code_Amt * C01_OvertimeHours_42, 0);
                    }

                    // 2.14.2.5 A03 Overtime hours during normal night shifts
                    _msg = GetJSON(method, "2.14.2.5");
                    if (A01_OvertimeHours_42 > 0)
                    {
                        Sal_Parameter = HSP.FirstOrDefault(x => x.Seq == "1" && x.Code == "A03");
                        A03_Amt_42 = Math.Round(Basic_Amt / param.Salary_Days / Oneday_hour * Sal_Parameter.Code_Amt * A03_OvertimeHours_42, 0);
                    }
                    else
                    {
                        Sal_Parameter = HSP.FirstOrDefault(x => x.Seq == "1" && x.Code == "A03-1");
                        A03_Amt_42 = Math.Round(Basic_Amt / param.Salary_Days / Oneday_hour * Sal_Parameter.Code_Amt * A03_OvertimeHours_42, 0);
                    }

                    // 2.14.2.6	B03 Holiday night shift overtime hours
                    _msg = GetJSON(method, "2.14.2.6");
                    Sal_Parameter = HSP.FirstOrDefault(x => x.Seq == "1" && x.Code == "B03");
                    B03_Amt_42 = Math.Round(Basic_Amt / param.Salary_Days / Oneday_hour * Sal_Parameter.Code_Amt, 0);

                    // 2.14.2.7	Adjustment of class 00
                    _msg = GetJSON(method, "2.14.2.7");
                    if (Emp_Personal_Values.Work_Shift_Type == "00")
                        A01_Amt_42 = Math.Round(C01_Amt_45, 0);

                    // 2.15	Additional items	
                    // 2.15.1	B54 Day shift meal allowance
                    _msg = GetJSON(method, "2.15.1");
                    decimal Amt = 0;
                    Amt = await Query_HRMS_Sal_AddDedItem_Values(param.Factory, InputYearMonth, Sal_Master_Values.Permission_Group, Sal_Master_Values.Salary_Type, "B", "B54");
                    decimal B54_Amt_49 = Math.Round(Amt * (Att_Resign_Monthly_Values.DayShift_Food ?? 0), 0);

                    // 2.15.2	B55 Nighttime Fee
                    _msg = GetJSON(method, "2.15.2");
                    Amt = await Query_HRMS_Sal_AddDedItem_Values(param.Factory, InputYearMonth, Sal_Master_Values.Permission_Group, Sal_Master_Values.Salary_Type, "B", "B55");
                    decimal B55_Amt_49 = Math.Round(Amt * Att_Resign_Monthly_Values.Night_Eat_Times, 0);

                    // 2.15.3	B56 Overtime meal allowance
                    _msg = GetJSON(method, "2.15.3");
                    Amt = await Query_HRMS_Sal_AddDedItem_Values(param.Factory, InputYearMonth, Sal_Master_Values.Permission_Group, Sal_Master_Values.Salary_Type, "B", "B56");
                    decimal B56_Amt_49 = Math.Round(Amt * Att_Resign_Monthly_Values.Food_Expenses, 0);

                    // 2.15.4	B57 Night shift meal allowance
                    _msg = GetJSON(method, "2.15.4");
                    Amt = await Query_HRMS_Sal_AddDedItem_Values(param.Factory, InputYearMonth, Sal_Master_Values.Permission_Group, Sal_Master_Values.Salary_Type, "B", "B57");
                    decimal B57_Amt_49 = Math.Round(Amt * (Att_Resign_Monthly_Values.NightShift_Food ?? 0), 0);

                    // 2.16	Deductible insurance items
                    // 2.16.1 Social insurance V01_Amt_57, medical insurance V02_Amt_57, unemployment insurance V03_Amt_57
                    _msg = GetJSON(method, "2.16.1");
                    Dictionary<string, VariableCombine> Insurance_57 = await Query_Ins_Rate_Variable_Combine(
                        "57", "Insurance", "EmployerRate", "EmployeeRate", "Amt",
                        param.Factory, InputYearMonth, Sal_Master_Values.Permission_Group);

                    decimal V01_Amt_57 = Insurance_57.GetValueOrDefault("V01_Amt_57")?.Value as decimal? ?? 0m;
                    decimal V02_Amt_57 = Insurance_57.GetValueOrDefault("V02_Amt_57")?.Value as decimal? ?? 0m;
                    decimal V03_Amt_57 = Insurance_57.GetValueOrDefault("V03_Amt_57")?.Value as decimal? ?? 0m;

                    if (Att_Leave_Maintain_J5 > 13)
                        Basic_Amt = 4420000;
                    if (!HIEM.Any(x => x.Employee_ID == Emp_Personal_Values.Employee_ID))
                    {
                        V01_Amt_57 = 0;
                        V02_Amt_57 = 0;
                        V03_Amt_57 = 0;
                    }
                    else
                    {
                        decimal V01_EmployeeRate_57 = Insurance_57.GetValueOrDefault("V01_EmployeeRate_57")?.Value as decimal? ?? 0m;
                        decimal V02_EmployeeRate_57 = Insurance_57.GetValueOrDefault("V02_EmployeeRate_57")?.Value as decimal? ?? 0m;
                        decimal V03_EmployeeRate_57 = Insurance_57.GetValueOrDefault("V03_EmployeeRate_57")?.Value as decimal? ?? 0m;
                        var Code_Amt = HSP.FirstOrDefault(x => x.Seq == "4" && x.Code == "V01")?.Code_Amt ?? 0;
                        if (Basic_Amt < Code_Amt)
                            V01_Amt_57 = Basic_Amt * V01_EmployeeRate_57;
                        else
                            V01_Amt_57 = Sal_Parameter.Code_Amt * V01_EmployeeRate_57;
                        V02_Amt_57 = Basic_Amt * V02_EmployeeRate_57;
                        V03_Amt_57 = Basic_Amt * V03_EmployeeRate_57;
                    }
                    decimal G0_LeaveDays_40 = Leave_40.GetValueOrDefault("G0_LeaveDays_40")?.Value as decimal? ?? 0m;

                    if (G0_LeaveDays_40 > 0)
                        V01_Amt_57 = 0;

                    DateTime dateset = new(InputYearMonth.Year, InputYearMonth.Month, 15);  //(EX input 2025/01 =>2025/01/15)
                    if (Emp_Personal_Values.Onboard_Date.Date > dateset.Date)
                    {
                        V01_Amt_57 = 0;
                        V02_Amt_57 = 0;
                        V03_Amt_57 = 0;
                    }

                    // 2.17 Add salary details to temporary archive
                    List<MonthlySalaryGenerationExitedEmployees_Temp> temp = new();
                    List<MonthlySalaryGenerationExitedEmployees_PTemp> pTemp = new();

                    // 2.17.1 Added 45 salary verification item code, amount
                    _msg = GetJSON(method, "2.17.1");
                    string A01_Mvalue_45 = Amt_45.GetValueOrDefault("A01_Mvalue_45")?.Value as string ?? "";
                    string A02_Mvalue_45 = Amt_45.GetValueOrDefault("A02_Mvalue_45")?.Value as string ?? "";
                    string B01_Mvalue_45 = Amt_45.GetValueOrDefault("B01_Mvalue_45")?.Value as string ?? "";
                    string B02_Mvalue_45 = Amt_45.GetValueOrDefault("B02_Mvalue_45")?.Value as string ?? "";
                    string B04_Mvalue_45 = Amt_45.GetValueOrDefault("B04_Mvalue_45")?.Value as string ?? "";
                    string B05_Mvalue_45 = Amt_45.GetValueOrDefault("B05_Mvalue_45")?.Value as string ?? "";
                    string B06_Mvalue_45 = Amt_45.GetValueOrDefault("B06_Mvalue_45")?.Value as string ?? "";
                    string B07_Mvalue_45 = Amt_45.GetValueOrDefault("B07_Mvalue_45")?.Value as string ?? "";
                    string B09_Mvalue_45 = Amt_45.GetValueOrDefault("B09_Mvalue_45")?.Value as string ?? "";
                    string B10_Mvalue_45 = Amt_45.GetValueOrDefault("B10_Mvalue_45")?.Value as string ?? "";
                    string C02_Mvalue_45 = Amt_45.GetValueOrDefault("C02_Mvalue_45")?.Value as string ?? "";
                    string AV2_Mvalue_45 = Amt_45.GetValueOrDefault("AV2_Mvalue_45")?.Value as string ?? "";
                    temp.Ins_Temp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "45", "A", A01_Mvalue_45, (int)A01_Amt_45, Sal_Resign_Monthly.Currency, Sal_Resign_Monthly.Department);
                    temp.Ins_Temp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "45", "A", A02_Mvalue_45, (int)A02_Amt_45, Sal_Resign_Monthly.Currency, Sal_Resign_Monthly.Department);
                    temp.Ins_Temp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "45", "A", B01_Mvalue_45, (int)B01_Amt_45, Sal_Resign_Monthly.Currency, Sal_Resign_Monthly.Department);
                    temp.Ins_Temp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "45", "A", B02_Mvalue_45, (int)B02_Amt_45, Sal_Resign_Monthly.Currency, Sal_Resign_Monthly.Department);
                    temp.Ins_Temp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "45", "A", B04_Mvalue_45, (int)B04_Amt_45, Sal_Resign_Monthly.Currency, Sal_Resign_Monthly.Department);
                    temp.Ins_Temp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "45", "A", B05_Mvalue_45, (int)B05_Amt_45, Sal_Resign_Monthly.Currency, Sal_Resign_Monthly.Department);
                    temp.Ins_Temp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "45", "A", B06_Mvalue_45, (int)B06_Amt_45, Sal_Resign_Monthly.Currency, Sal_Resign_Monthly.Department);
                    temp.Ins_Temp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "45", "A", B07_Mvalue_45, (int)B07_Amt_45, Sal_Resign_Monthly.Currency, Sal_Resign_Monthly.Department);
                    temp.Ins_Temp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "45", "A", B09_Mvalue_45, (int)B09_Amt_45, Sal_Resign_Monthly.Currency, Sal_Resign_Monthly.Department);
                    temp.Ins_Temp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "45", "A", B10_Mvalue_45, (int)B10_Amt_45, Sal_Resign_Monthly.Currency, Sal_Resign_Monthly.Department);
                    temp.Ins_Temp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "45", "A", C02_Mvalue_45, (int)C02_Amt_45, Sal_Resign_Monthly.Currency, Sal_Resign_Monthly.Department);
                    temp.Ins_Temp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "45", "A", AV2_Mvalue_45, (int)AV2_Amt_45, Sal_Resign_Monthly.Currency, Sal_Resign_Monthly.Department);

                    // 2.17.1.1 新增試用期&轉正薪資明細暫存檔新增45核薪項目代碼、金額
                    _msg = GetJSON(method, "2.17.1.1");
                    // --試用期
                    string A01_Pkvalue_45 = Pkamt_45.GetValueOrDefault("A01_Pkvalue_45")?.Value as string ?? "";
                    string A02_Pkvalue_45 = Pkamt_45.GetValueOrDefault("A02_Pkvalue_45")?.Value as string ?? "";
                    string B01_Pkvalue_45 = Pkamt_45.GetValueOrDefault("B01_Pkvalue_45")?.Value as string ?? "";
                    string B02_Pkvalue_45 = Pkamt_45.GetValueOrDefault("B02_Pkvalue_45")?.Value as string ?? "";
                    string B04_Pkvalue_45 = Pkamt_45.GetValueOrDefault("B04_Pkvalue_45")?.Value as string ?? "";
                    string B05_Pkvalue_45 = Pkamt_45.GetValueOrDefault("B05_Pkvalue_45")?.Value as string ?? "";
                    string B06_Pkvalue_45 = Pkamt_45.GetValueOrDefault("B06_Pkvalue_45")?.Value as string ?? "";
                    string B09_Pkvalue_45 = Pkamt_45.GetValueOrDefault("B09_Pkvalue_45")?.Value as string ?? "";
                    string B10_Pkvalue_45 = Pkamt_45.GetValueOrDefault("B10_Pkvalue_45")?.Value as string ?? "";
                    string AV2_Pkvalue_45 = Pkamt_45.GetValueOrDefault("AV2_Pkvalue_45")?.Value as string ?? "";
                    string C02_Pkvalue_45 = Pkamt_45.GetValueOrDefault("C02_Pkvalue_45")?.Value as string ?? "";
                    string D01_Pkvalue_45 = Pkamt_45.GetValueOrDefault("D01_Pkvalue_45")?.Value as string ?? "";

                    pTemp.Ins_PTemp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "Y", "45", "A", A01_Pkvalue_45, (int)A01_Pamt_45);
                    pTemp.Ins_PTemp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "Y", "45", "A", A02_Pkvalue_45, (int)A02_Pamt_45);
                    pTemp.Ins_PTemp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "Y", "45", "A", B01_Pkvalue_45, (int)B01_Pamt_45);
                    pTemp.Ins_PTemp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "Y", "45", "A", B02_Pkvalue_45, (int)B02_Pamt_45);
                    pTemp.Ins_PTemp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "Y", "45", "A", B04_Pkvalue_45, (int)B04_Pamt_45);
                    pTemp.Ins_PTemp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "Y", "45", "A", B05_Pkvalue_45, (int)B05_Pamt_45);
                    pTemp.Ins_PTemp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "Y", "45", "A", B06_Pkvalue_45, (int)B06_Pamt_45);
                    pTemp.Ins_PTemp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "Y", "45", "A", B09_Pkvalue_45, (int)B09_Pamt_45);
                    pTemp.Ins_PTemp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "Y", "45", "A", B10_Pkvalue_45, (int)B10_Pamt_45);
                    pTemp.Ins_PTemp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "Y", "45", "A", AV2_Pkvalue_45, (int)AV2_Pamt_45);
                    pTemp.Ins_PTemp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "Y", "45", "A", C02_Pkvalue_45, (int)C02_Pamt_45);
                    pTemp.Ins_PTemp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "Y", "45", "A", D01_Pkvalue_45, (int)D01_Pamt_45);

                    // --轉正
                    pTemp.Ins_PTemp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "N", "45", "A", A01_Mvalue_45, (int)A01_Famt_45);
                    pTemp.Ins_PTemp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "N", "45", "A", A02_Mvalue_45, (int)A02_Famt_45);
                    pTemp.Ins_PTemp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "N", "45", "A", B01_Mvalue_45, (int)B01_Famt_45);
                    pTemp.Ins_PTemp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "N", "45", "A", B02_Mvalue_45, (int)B02_Famt_45);
                    pTemp.Ins_PTemp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "N", "45", "A", B04_Mvalue_45, (int)B04_Famt_45);
                    pTemp.Ins_PTemp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "N", "45", "A", B05_Mvalue_45, (int)B05_Famt_45);
                    pTemp.Ins_PTemp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "N", "45", "A", B06_Mvalue_45, (int)B06_Famt_45);
                    pTemp.Ins_PTemp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "N", "45", "A", B09_Mvalue_45, (int)B09_Famt_45);
                    pTemp.Ins_PTemp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "N", "45", "A", B10_Mvalue_45, (int)B10_Famt_45);

                    // 2.17.2 Added 42 overtime subsidy code, amount
                    _msg = GetJSON(method, "2.17.2");
                    string A01_Overtime_42 = Overtime_42.GetValueOrDefault("A01_Overtime_42")?.Value as string ?? "";
                    string A02_Overtime_42 = Overtime_42.GetValueOrDefault("A02_Overtime_42")?.Value as string ?? "";
                    string B01_Overtime_42 = Overtime_42.GetValueOrDefault("B01_Overtime_42")?.Value as string ?? "";
                    string C01_Overtime_42 = Overtime_42.GetValueOrDefault("C01_Overtime_42")?.Value as string ?? "";
                    string A03_Overtime_42 = Overtime_42.GetValueOrDefault("A03_Overtime_42")?.Value as string ?? "";
                    temp.Ins_Temp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "42", "A", A01_Overtime_42, (int)A01_Amt_42, Sal_Resign_Monthly.Currency, Sal_Resign_Monthly.Department);
                    temp.Ins_Temp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "42", "A", A02_Overtime_42, (int)A02_Amt_42, Sal_Resign_Monthly.Currency, Sal_Resign_Monthly.Department);
                    temp.Ins_Temp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "42", "A", B01_Overtime_42, (int)B01_Amt_42, Sal_Resign_Monthly.Currency, Sal_Resign_Monthly.Department);
                    temp.Ins_Temp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "42", "A", C01_Overtime_42, (int)C01_Amt_42, Sal_Resign_Monthly.Currency, Sal_Resign_Monthly.Department);
                    temp.Ins_Temp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "42", "A", A03_Overtime_42, (int)A03_Amt_42, Sal_Resign_Monthly.Currency, Sal_Resign_Monthly.Department);

                    // 2.17.2.1
                    _msg = GetJSON(method, "2.17.2.1");
                    // --試用期
                    string A01_POvertime_42 = POvertime_42.GetValueOrDefault("A01_POvertime_42")?.Value as string ?? "";
                    string A02_POvertime_42 = POvertime_42.GetValueOrDefault("A02_POvertime_42")?.Value as string ?? "";
                    string B01_POvertime_42 = POvertime_42.GetValueOrDefault("B01_POvertime_42")?.Value as string ?? "";
                    string C01_POvertime_42 = POvertime_42.GetValueOrDefault("C01_POvertime_42")?.Value as string ?? "";
                    pTemp.Ins_PTemp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "Y", "42", "A", A01_POvertime_42, (int)A01_PAmt_42);
                    pTemp.Ins_PTemp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "Y", "42", "A", A02_POvertime_42, (int)A02_PAmt_42);
                    pTemp.Ins_PTemp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "Y", "42", "A", B01_POvertime_42, (int)B01_PAmt_42);
                    pTemp.Ins_PTemp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "Y", "42", "A", C01_POvertime_42, (int)C01_PAmt_42);
                    // --轉正
                    pTemp.Ins_PTemp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "N", "42", "A", A01_Overtime_42, (int)A01_FAmt_42);
                    pTemp.Ins_PTemp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "N", "42", "A", A02_Overtime_42, (int)A02_FAmt_42);
                    pTemp.Ins_PTemp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "N", "42", "A", B01_Overtime_42, (int)B01_FAmt_42);
                    pTemp.Ins_PTemp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "N", "42", "A", C01_Overtime_42, (int)C01_FAmt_42);

                    // 2.17.3 Add 49 additional item codes, amount
                    _msg = GetJSON(method, "2.17.3");
                    temp.Ins_Temp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "49", "B", "B54", (int)B54_Amt_49, Sal_Resign_Monthly.Currency, Sal_Resign_Monthly.Department);
                    temp.Ins_Temp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "49", "B", "B55", (int)B55_Amt_49, Sal_Resign_Monthly.Currency, Sal_Resign_Monthly.Department);
                    temp.Ins_Temp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "49", "B", "B56", (int)B56_Amt_49, Sal_Resign_Monthly.Currency, Sal_Resign_Monthly.Department);
                    temp.Ins_Temp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "49", "B", "B57", (int)B57_Amt_49, Sal_Resign_Monthly.Currency, Sal_Resign_Monthly.Department);

                    // 2.17.4 Added 57 insurance code, amount
                    _msg = GetJSON(method, "2.17.4");
                    string V01_Insurance_57 = Insurance_57.GetValueOrDefault("V01_Insurance_57")?.Value as string ?? "";
                    string V02_Insurance_57 = Insurance_57.GetValueOrDefault("V02_Insurance_57")?.Value as string ?? "";
                    string V03_Insurance_57 = Insurance_57.GetValueOrDefault("V03_Insurance_57")?.Value as string ?? "";
                    temp.Ins_Temp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "57", "D", V01_Insurance_57, (int)V01_Amt_57, Sal_Resign_Monthly.Currency, Sal_Resign_Monthly.Department);
                    temp.Ins_Temp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "57", "D", V02_Insurance_57, (int)V02_Amt_57, Sal_Resign_Monthly.Currency, Sal_Resign_Monthly.Department);
                    temp.Ins_Temp(Emp_Personal_Values.USER_GUID, Emp_Personal_Values.Division, param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, "57", "D", V03_Insurance_57, (int)V03_Amt_57, Sal_Resign_Monthly.Currency, Sal_Resign_Monthly.Department);

                    // 2.17.5 Other deductions, recorded in monthly salary details
                    _msg = GetJSON(method, "2.17.5");
                    await HSAM.Where(x => x.Employee_ID == Emp_Personal_Values.Employee_ID && !codeList.Contains(x.AddDed_Item)).ForEachAsync(x =>
                    {
                        temp.Ins_Temp(x.USER_GUID, Emp_Personal_Values.Division, x.Factory, x.Sal_Month, x.Employee_ID, "49", x.AddDed_Type, x.AddDed_Item, x.Amount, Sal_Resign_Monthly.Currency, Sal_Resign_Monthly.Department);
                    });

                    // 2.18 Personal income tax
                    _msg = GetJSON(method, "2.18");
                    var Sal_Tax_VN_result = await Sal_Tax_VN(param.Factory, InputYearMonth, Emp_Personal_Values.Employee_ID, A01_OvertimeHours_42, temp, Sal_Resign_Monthly, userName, now);
                    if (!Sal_Tax_VN_result.IsSuccess)
                    {
                        await _repositoryAccessor.RollbackAsync();
                        return new OperationResult(false, Sal_Tax_VN_result.Error);
                    }
                    Sal_Resign_Monthly.Tax = (int)Sal_Tax_VN_result.Data;

                    // 2.19 Add monthly salary details file
                    _msg = GetJSON(method, "2.19");
                    var addHSRMD = temp.Where(x =>
                        x.Factory == param.Factory &&
                        x.Sal_Month.Date == InputYearMonth.Date &&
                        x.Employee_ID == Emp_Personal_Values.Employee_ID &&
                        x.Item != null && x.Item != ""
                    ).Select(x => new HRMS_Sal_Resign_Monthly_Detail
                    {
                        USER_GUID = x.USER_GUID,
                        Division = x.Division,
                        Factory = x.Factory,
                        Sal_Month = x.Sal_Month,
                        Employee_ID = x.Employee_ID,
                        Type_Seq = x.Type_Seq,
                        AddDed_Type = x.AddDed_Type,
                        Item = x.Item,
                        Amount = x.Amount,
                        Update_By = userName,
                        Update_Time = now
                    }).AsQueryable().AsNoTracking();

                    if (Probation_flag)
                    {
                        // 2.20.1
                        var probation_Monthly_Detail = pTemp.Where(x =>
                           x.Factory == param.Factory &&
                           x.Sal_Month.Date == InputYearMonth.Date &&
                           x.Employee_ID == Emp_Personal_Values.Employee_ID &&
                           !string.IsNullOrEmpty(x.Item)
                        ).Select(x => new HRMS_Sal_Probation_Monthly_Detail()
                        {
                            USER_GUID = x.USER_GUID,
                            Division = x.Division,
                            Factory = x.Factory,
                            Sal_Month = InputYearMonth,
                            Employee_ID = x.Employee_ID,
                            Probation = x.Probation,
                            Type_Seq = x.Type_Seq,
                            AddDed_Type = x.AddDed_Type,
                            Item = x.Item,
                            Amount = x.Amount,
                            Update_By = userName,
                            Update_Time = DateTime.Now
                        });

                        addDataHSPMD.AddRange(probation_Monthly_Detail);

                        // 2.20.2
                        if (PSal_Resign_Monthly != null)
                            addDataHSPM.Add(PSal_Resign_Monthly);
                        addDataHSPM.Add(FSal_Resign_Monthly);
                    }

                    addHSRMs.Add(Sal_Resign_Monthly);
                    addHSRMDs.AddRange(addHSRMD);
                    cnt++;
                }

                if (addHSRMs.Any())
                    _repositoryAccessor.HRMS_Sal_Resign_Monthly.AddMultiple(addHSRMs);
                if (addHSRMDs.Any())
                    _repositoryAccessor.HRMS_Sal_Resign_Monthly_Detail.AddMultiple(addHSRMDs);
                if (addDataHSPM.Any())
                    _repositoryAccessor.HRMS_Sal_Probation_Monthly.AddMultiple(addDataHSPM);
                if (addDataHSPMD.Any())
                    _repositoryAccessor.HRMS_Sal_Probation_Monthly_Detail.AddMultiple(addDataHSPMD);

                await _repositoryAccessor.Save();
                await _repositoryAccessor.CommitAsync();
                return new OperationResult(true, new { Count = cnt, Error = _err });
            }
            catch (Exception)
            {
                await _repositoryAccessor.RollbackAsync();
                return new OperationResult(false, _msg);
            }
            finally
            {
                semaphore.Release();
            }

        }
        private async Task<OperationResult> DataLock(MonthlySalaryGenerationExitedEmployees_Param param, string userName)
        {
            await semaphore.WaitAsync();
            string method = nameof(DataLock);
            string _msg = "";
            int cnt = 0;
            await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                if (!DateTime.TryParseExact(param.Year_Month_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime InputYearMonth))
                {
                    await _repositoryAccessor.RollbackAsync();
                    return new OperationResult(false, "InvalidInput");
                }
                DateTime now = DateTime.Now;
                var HSC = _repositoryAccessor.HRMS_Sal_Close.FindAll(x =>
                    x.Factory == param.Factory &&
                    x.Sal_Month.Date == InputYearMonth.Date
                );
                var HSRM = _repositoryAccessor.HRMS_Sal_Resign_Monthly.FindAll(x =>
                    x.Factory == param.Factory &&
                    x.Sal_Month.Date == InputYearMonth.Date
                );
                if (param.salary_Lock == "Y")
                {
                    // 1.1 Salary lock = Y, Check the employee number that has not generated a closed account file in the current month
                    _msg = GetJSON(method, "1.1");
                    var std_cur = await HSRM.Where(x =>
                        param.Permission_Group.Contains(x.Permission_Group) &&
                        !HSC.Where(y => param.Permission_Group.Contains(y.Permission_Group)).Select(y => y.Employee_ID).Contains(x.Employee_ID)
                    ).ToListAsync();

                    // 1.2 Salary lock = Y, Generate closing file
                    _msg = GetJSON(method, "1.2");
                    foreach (var std_values in std_cur)
                    {
                        if (string.IsNullOrWhiteSpace(std_values.Employee_ID))
                            continue;
                        HRMS_Sal_Close Sal_Close_Values = new()
                        {
                            USER_GUID = std_values.USER_GUID,
                            Division = std_values.Division,
                            Factory = std_values.Factory,
                            Sal_Month = std_values.Sal_Month,
                            Employee_ID = std_values.Employee_ID,
                            Permission_Group = std_values.Permission_Group,
                            Close_Status = "N",
                            Update_By = userName,
                            Update_Time = now
                        };
                        var insResult = await CRUD_Data(new MonthlySalaryGenerationExitedEmployees_CRUD("Ins_HRMS_Sal_Close", "1.2", new MonthlySalaryGenerationExitedEmployees_General(Sal_Close_Values)));
                        if (!insResult.IsSuccess)
                        {
                            await _repositoryAccessor.RollbackAsync();
                            return new OperationResult(false, insResult.Error);
                        }
                        var updateHSRM = await HSRM.FirstOrDefaultAsync(x => x.Employee_ID == std_values.Employee_ID);
                        if (updateHSRM != null)
                        {
                            updateHSRM.Lock = "Y";
                            var updResult = await CRUD_Data(new MonthlySalaryGenerationExitedEmployees_CRUD("Upd_HRMS_Sal_Resign_Monthly", "1.2", new MonthlySalaryGenerationExitedEmployees_General(updateHSRM)));
                            if (!updResult.IsSuccess)
                            {
                                await _repositoryAccessor.RollbackAsync();
                                return new OperationResult(false, updResult.Error);
                            }
                        }
                        cnt++;
                    }
                }
                else
                {
                    // 2.1 Salary lock = N, Check whether the salary has been paid, otherwise it cannot be cancelled
                    _msg = GetJSON(method, "2.1");
                    var ct = await HSC.CountAsync(x =>
                        x.Close_Status == "Y" &&
                        HSRM.Where(y => param.Permission_Group.Contains(y.Permission_Group)).Select(y => y.Employee_ID).Contains(x.Employee_ID)
                    );
                    if (ct > 0)
                    {
                        await _repositoryAccessor.RollbackAsync();
                        return new OperationResult(false, "SalaryPaid");
                    }

                    // 2.2 Salary lock = N, Execute the cancellation operation
                    _msg = GetJSON(method, "2.2");
                    var deleteHSC = await HSC
                        .Where(x => HSRM.Where(y => param.Permission_Group.Contains(y.Permission_Group)).Select(y => y.Employee_ID).Contains(x.Employee_ID))
                        .ToListAsync();
                    if (deleteHSC.Any())
                    {
                        var delResult = await CRUD_Data(new MonthlySalaryGenerationExitedEmployees_CRUD("Del_Multi_HRMS_Sal_Close", "2.2", new MonthlySalaryGenerationExitedEmployees_General(deleteHSC)));
                        if (!delResult.IsSuccess)
                        {
                            await _repositoryAccessor.RollbackAsync();
                            return new OperationResult(false, delResult.Error);
                        }
                    }

                    var updateHSRMs = await HSRM
                        .Where(x => param.Permission_Group.Contains(x.Permission_Group))
                        .ToListAsync();
                    if (updateHSRMs.Any())
                    {
                        foreach (var item in updateHSRMs)
                        {
                            item.Lock = "N";
                            cnt++;
                        }
                        var updResult = await CRUD_Data(new MonthlySalaryGenerationExitedEmployees_CRUD("Upd_Multi_HRMS_Sal_Resign_Monthly", "2.2", new MonthlySalaryGenerationExitedEmployees_General(updateHSRMs)));
                        if (!updResult.IsSuccess)
                        {
                            await _repositoryAccessor.RollbackAsync();
                            return new OperationResult(false, updResult.Error);
                        }
                    }
                }

                await _repositoryAccessor.CommitAsync();
                return new OperationResult(true, cnt);
            }
            catch (Exception)
            {
                await _repositoryAccessor.RollbackAsync();
                return new OperationResult(false, _msg);
            }
            finally
            {
                semaphore.Release();
            }
        }
        private async Task<MonthlySalaryGenerationExitedEmployees_CRUD> CRUD_Data(MonthlySalaryGenerationExitedEmployees_CRUD initial)
        {
            try
            {
                switch (initial.Function)
                {
                    case "Ins_HRMS_Sal_Close":
                        _repositoryAccessor.HRMS_Sal_Close.Add(initial.Data.HRMS_Sal_Close);
                        initial.IsSuccess = await _repositoryAccessor.Save();
                        break;
                    case "Ins_HRMS_Sal_Tax":
                        _repositoryAccessor.HRMS_Sal_Tax.Add(initial.Data.HRMS_Sal_Tax);
                        initial.IsSuccess = await _repositoryAccessor.Save();
                        break;
                    case "Del_Multi_HRMS_Sal_Close":
                        _repositoryAccessor.HRMS_Sal_Close.RemoveMultiple(initial.Data.HRMS_Sal_Close_List);
                        initial.IsSuccess = await _repositoryAccessor.Save();
                        break;
                    case "Upd_HRMS_Sal_Resign_Monthly":
                        _repositoryAccessor.HRMS_Sal_Resign_Monthly.Update(initial.Data.HRMS_Sal_Resign_Monthly);
                        initial.IsSuccess = await _repositoryAccessor.Save();
                        break;
                    case "Upd_Multi_HRMS_Sal_Resign_Monthly":
                        _repositoryAccessor.HRMS_Sal_Resign_Monthly.UpdateMultiple(initial.Data.HRMS_Sal_Resign_Monthly_List);
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
                    initial.Error = initial.Step.Replace(".", string.Empty) + (initial.Function switch
                    {
                        "Ins_HRMS_Sal_Close" => "InsFailHSC",
                        "Ins_HRMS_Sal_Tax" => "InsFailHST",
                        "Del_Multi_HRMS_Sal_Close" => "DelMultiFailHSC",
                        "Upd_HRMS_Sal_Resign_Monthly" => "UpdFailHSRM",
                        "Upd_Multi_HRMS_Sal_Resign_Monthly" => "UpdFailHSRM",
                        _ => "ExecuteFail"
                    });
            }
        }
        private async Task<OperationResult> Sal_Tax_VN(string Factory, DateTime Year_Month, string Employee_ID, decimal A01_OvertimeHours_42, List<MonthlySalaryGenerationExitedEmployees_Temp> Temp, HRMS_Sal_Resign_Monthly Sal_Monthly, string userName, DateTime now)
        {
            // 1
            // 1. Take the total of positive items
            // 2. Take out the amount of each deduction
            // 3. Taxable salary

            // 1.1  Take the total of positive items
            int A1 = 0;
            A1 = Temp.Where(x =>
               x.Factory == Factory &&
               x.Sal_Month == Year_Month.Date &&
               new List<string>() { "42", "45", "49" }.Contains(x.Type_Seq) &&
               x.Employee_ID == Employee_ID &&
               new List<string>() { "A", "B" }.Contains(x.AddDed_Type)
           )?.Sum(x => x.Amount) ?? 0;

            // 1.2 Take out the amount of each deduction
            // 1.2.1 Deductions
            int D1 = 0;
            D1 = Temp.Query_Sal_Temp(Sal_Monthly.Factory, Sal_Monthly.Sal_Month, Sal_Monthly.Employee_ID, "49", "B", "B14")
               + Temp.Query_Sal_Temp(Sal_Monthly.Factory, Sal_Monthly.Sal_Month, Sal_Monthly.Employee_ID, "49", "B", "B44")
               + Temp.Query_Sal_Temp(Sal_Monthly.Factory, Sal_Monthly.Sal_Month, Sal_Monthly.Employee_ID, "49", "B", "B50");
            int D8 = 0;
            D8 = Temp.Query_Sal_Temp(Sal_Monthly.Factory, Sal_Monthly.Sal_Month, Sal_Monthly.Employee_ID, "49", "B", "B49");
            int D10 = 0;
            D10 = Temp.Query_Sal_Temp(Sal_Monthly.Factory, Sal_Monthly.Sal_Month, Sal_Monthly.Employee_ID, "49", "A", "A06") * 1 / 3;

            // 1.2.2 Overtime allowance
            int D3 = 0;
            D3 = Temp.Query_Sal_Temp(Sal_Monthly.Factory, Sal_Monthly.Sal_Month, Sal_Monthly.Employee_ID, "42", "A", "A02");
            int D9 = 0;
            D9 = Temp.Query_Sal_Temp(Sal_Monthly.Factory, Sal_Monthly.Sal_Month, Sal_Monthly.Employee_ID, "42", "A", "A01") * 1 / 3;
            int D11 = 0;
            D11 = Temp.Query_Sal_Temp(Sal_Monthly.Factory, Sal_Monthly.Sal_Month, Sal_Monthly.Employee_ID, "42", "A", "B01");
            int D12 = 0;
            D12 = Temp.Query_Sal_Temp(Sal_Monthly.Factory, Sal_Monthly.Sal_Month, Sal_Monthly.Employee_ID, "42", "A", "C01");
            int D14 = 0;
            if (A01_OvertimeHours_42 <= 0) D14 = Temp.Query_Sal_Temp(Sal_Monthly.Factory, Sal_Monthly.Sal_Month, Sal_Monthly.Employee_ID, "42", "A", "A03") * 100 / 210;
            int D15 = 0;
            if (A01_OvertimeHours_42 > 0) D15 = Temp.Query_Sal_Temp(Sal_Monthly.Factory, Sal_Monthly.Sal_Month, Sal_Monthly.Employee_ID, "42", "A", "A03") * 110 / 210;
            int D16 = 0;
            D16 = Temp.Query_Sal_Temp(Sal_Monthly.Factory, Sal_Monthly.Sal_Month, Sal_Monthly.Employee_ID, "42", "A", "C01");

            // 1.2.4 Meal expenses
            int D4 = 0;
            D4 = Temp.Query_Sal_Temp(Sal_Monthly.Factory, Sal_Monthly.Sal_Month, Sal_Monthly.Employee_ID, "49", "B", "B54")
               + Temp.Query_Sal_Temp(Sal_Monthly.Factory, Sal_Monthly.Sal_Month, Sal_Monthly.Employee_ID, "49", "B", "B55")
               + Temp.Query_Sal_Temp(Sal_Monthly.Factory, Sal_Monthly.Sal_Month, Sal_Monthly.Employee_ID, "49", "B", "B56")
               + Temp.Query_Sal_Temp(Sal_Monthly.Factory, Sal_Monthly.Sal_Month, Sal_Monthly.Employee_ID, "49", "B", "B57");

            // 1.2.3 Insurance
            int D5 = 0;
            D5 = Temp.Query_Sal_Temp(Sal_Monthly.Factory, Sal_Monthly.Sal_Month, Sal_Monthly.Employee_ID, "57", "D", "V01");
            int D6 = 0;
            D6 = Temp.Query_Sal_Temp(Sal_Monthly.Factory, Sal_Monthly.Sal_Month, Sal_Monthly.Employee_ID, "57", "D", "V02");
            int D7 = 0;
            D7 = Temp.Query_Sal_Temp(Sal_Monthly.Factory, Sal_Monthly.Sal_Month, Sal_Monthly.Employee_ID, "57", "D", "V03");

            // 1.3  Taxable salary
            int WageTax = 0;
            int SumDeduction = D1 + D3 + D4 + D5 + D6 + D7 + D8 + D9 + D10 + D11 + D12 + D14 + D15 + D16;
            WageTax = A1 - SumDeduction;

            // 2
            // 1. Calculate the required deduction information
            // 2. Calculate income tax

            // 2.1 Calculate the required deduction information
            var HSTN = await _repositoryAccessor.HRMS_Sal_Tax_Number.FindAll(x =>
               x.Factory == Sal_Monthly.Factory &&
               x.Year.Year == Sal_Monthly.Sal_Month.Year
            ).ToListAsync();
            var HSTF = await _repositoryAccessor.HRMS_Sal_TaxFree.FindAll(x =>
               x.Factory == Sal_Monthly.Factory &&
               x.Salary_Type == Sal_Monthly.Salary_Type
            ).ToListAsync();
            var HSTB = await _repositoryAccessor.HRMS_Sal_Taxbracket.FindAll(x =>
               x.Nation == "VN" &&
               x.Tax_Code == "ZZ"
            ).ToListAsync();

            // 2.1.1 Number of dependents
            int Dependents = 0;
            Dependents = HSTN.FirstOrDefault(x => x.Employee_ID == Employee_ID)?.Dependents ?? 0;

            // 2.1.2 Duty Free Allowance
            int TaxFree = 0;
            TaxFree = (int)(HSTF.FirstOrDefault(x =>
                x.Type == "A" &&
                x.Effective_Month.Date == HSTF.Where(y => y.Type == "A" && y.Effective_Month.Date <= Sal_Monthly.Sal_Month.Date).Max(y => y.Effective_Month)
            )?.Amount ?? 0);

            // 2.1.3 Dependent Deduction
            int DependentTaxFree = 0;
            DependentTaxFree = (int)(HSTF.FirstOrDefault(x =>
                x.Type == "K" &&
                x.Effective_Month.Date == HSTF.Where(y => y.Type == "K" && y.Effective_Month.Date <= Sal_Monthly.Sal_Month.Date).Max(y => y.Effective_Month)
            )?.Amount ?? 0);

            // 2.2	Calculate income tax	
            int Tax = 0;
            int TotalAmount = 0;
            var VALUE = new HRMS_Sal_Taxbracket();

            if (WageTax >= TaxFree)
            {
                TotalAmount = WageTax - TaxFree - (DependentTaxFree * Dependents);
                if (TotalAmount <= 0)
                    Tax = 0;

                // 2.2.1  Income tax brackets
                var dataHSTB = HSTB.Where(y => y.Effective_Month.Date <= Sal_Monthly.Sal_Month.Date).ToList();
                if (dataHSTB.Any())
                {
                    var maxEffectiveMonth = dataHSTB.Max(y => y.Effective_Month);
                    VALUE = HSTB.FirstOrDefault(x =>
                    x.Income_Start < TotalAmount &&
                    x.Income_End >= TotalAmount &&
                    x.Effective_Month.Date == maxEffectiveMonth
                );
                }

                // 2.2.2  Calculate
                Tax = (int)Math.Round(TotalAmount * ((VALUE?.Rate ?? 0) / 100) - (VALUE?.Deduction ?? 0), 0);
            }

            // 2.3  Write income tax records
            var temp = Temp.FirstOrDefault(x => x.Factory == Factory && x.Sal_Month == Year_Month && x.Employee_ID == Employee_ID);
            HRMS_Sal_Tax addTax = new();
            if (temp != null && !string.IsNullOrWhiteSpace(temp.USER_GUID))
            {
                addTax = new()
                {
                    USER_GUID = temp.USER_GUID,
                    Factory = Factory,
                    Sal_Month = Year_Month,
                    Employee_ID = temp.Employee_ID,
                    Department = temp.Department,
                    Currency = temp.Currency,
                    Num_Dependents = (short)Dependents,
                    Add_Total = A1,
                    Ded_Total = SumDeduction,
                    Salary_Amt = WageTax,
                    Rate = VALUE?.Rate ?? 0,
                    Update_By = userName,
                    Update_Time = now
                };

                var insResult = await CRUD_Data(new MonthlySalaryGenerationExitedEmployees_CRUD("Ins_HRMS_Sal_Tax", "SalTaxVN", new MonthlySalaryGenerationExitedEmployees_General(addTax)));
                if (!insResult.IsSuccess)
                    return new OperationResult(false, insResult.Error);
            }
            return new OperationResult(true, Tax);
        }
        private async Task<OperationResult> Query_SalaryClose_Status(string Kind, string Factory, string Sal_Month, List<string> Permission_Group_List, string Employee_ID)
        {
            if (!DateTime.TryParseExact(Sal_Month, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime sal_Month))
                return new OperationResult(false, "InvalidInput");
            var Before_Month = sal_Month.AddMonths(-1);
            var HSC = _repositoryAccessor.HRMS_Sal_Close.FindAll(x => x.Factory == Factory && x.Sal_Month.Date == Before_Month.Date);
            if (!await HSC.AnyAsync())
                return new OperationResult(false, "SalaryCloseNotFound");

            var preHSM = PredicateBuilder.New<HRMS_Sal_Monthly>(x =>
                    x.Factory == Factory &&
                    x.Sal_Month.Date == sal_Month.Date &&
                    Permission_Group_List.Contains(x.Permission_Group));
            var preHSRM = PredicateBuilder.New<HRMS_Sal_Resign_Monthly>(x =>
                    x.Factory == Factory &&
                    x.Sal_Month.Date == sal_Month.Date &&
                    Permission_Group_List.Contains(x.Permission_Group));
            if (!string.IsNullOrWhiteSpace(Employee_ID))
            {
                preHSM.And(x => x.Employee_ID.ToLower() == Employee_ID.ToLower());
                preHSRM.And(x => x.Employee_ID.ToLower() == Employee_ID.ToLower());
            }

            if (Kind == "Y")
            {
                var HSM = _repositoryAccessor.HRMS_Sal_Monthly.FindAll(preHSM);
                if (await HSM.AnyAsync(x => x.Lock == "Y"))
                    return new OperationResult(false, "SalaryMonthlyLocked");
                if (await HSM.AnyAsync())
                    return new OperationResult(true, "ContinueExecuting");
            }
            else
            {
                var HSRM = _repositoryAccessor.HRMS_Sal_Resign_Monthly.FindAll(preHSRM);
                if (await HSRM.AnyAsync())
                    return new OperationResult(true, "ContinueExecuting");
            }
            return new OperationResult(true);
        }
        private static string GetJSON(string method, string step) => $"{MyRegex().Replace(method, "")}{step.Replace(".", string.Empty)}";
    }
}
