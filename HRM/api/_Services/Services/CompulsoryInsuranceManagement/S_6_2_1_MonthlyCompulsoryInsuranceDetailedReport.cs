using System.Drawing;
using API.Data;
using API._Services.Interfaces;
using API._Services.Interfaces.CompulsoryInsuranceManagement;
using API.Helper.Constant;
using API.Helper.Utilities;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.CompulsoryInsuranceManagement
{
    public class S_6_2_1_MonthlyCompulsoryInsuranceDetailedReport : BaseServices, I_6_2_1_MonthlyCompulsoryInsuranceDetailedReport
    {
        private readonly I_Common _common;
        public S_6_2_1_MonthlyCompulsoryInsuranceDetailedReport(DBContext dbContext,I_Common common) : base(dbContext)
        {
            _common = common;
        }

        #region GetDataSource 
        public async Task<List<D_6_2_1_MonthlyCompulsoryInsuranceDetailedReportSource>> GetDataSource(D_6_2_1_MonthlyCompulsoryInsuranceDetailedReportParam param)
        {
            DateTime yearMonth = param.Year_Month.ToDateTime();
            List<D_6_2_1_MonthlyCompulsoryInsuranceDetailedReportSource> source = new();
            var tblEmp_Personal = _repositoryAccessor.HRMS_Emp_Personal.FindAll(x => x.Factory == param.Factory && param.Permission_Group.Contains(x.Permission_Group), true);

            // kind On Job 
            if (param.Kind == "O")
            {
                var predicate_sal_monthly = PredicateBuilder.New<HRMS_Sal_Monthly>(x =>
                                            x.Factory == param.Factory &&
                                            x.Sal_Month == yearMonth &&
                                            param.Permission_Group.Contains(x.Permission_Group));

                var predicate_sal_monthly_detail = PredicateBuilder.New<HRMS_Sal_Monthly_Detail>(x =>
                                                    x.Factory == param.Factory &&
                                                    x.Sal_Month == yearMonth &&
                                                    x.Type_Seq == "57" &&
                                                    x.Amount > 0);

                if (param.Insurance_Type is "V01" or "V02" or "V03")
                    predicate_sal_monthly_detail.And(x => x.Item == param.Insurance_Type);

                if (!string.IsNullOrWhiteSpace(param.Department))
                    predicate_sal_monthly.And(x => x.Department == param.Department);

                source = await _repositoryAccessor.HRMS_Sal_Monthly.FindAll(predicate_sal_monthly, true)
                .Join(_repositoryAccessor.HRMS_Sal_Monthly_Detail.FindAll(predicate_sal_monthly_detail, true),
                    a => new { a.Factory, a.Sal_Month, a.Employee_ID },
                    b => new { b.Factory, b.Sal_Month, b.Employee_ID },
                    (a, b) => new { a, b })
                .Join(tblEmp_Personal, x => x.a.USER_GUID, y => y.USER_GUID, (x, y) => new { x.a, x.b.Amount, y.Local_Full_Name })
                .Select(x => new D_6_2_1_MonthlyCompulsoryInsuranceDetailedReportSource
                {
                    USER_GUID = x.a.USER_GUID,
                    Local_Full_Name = x.Local_Full_Name,
                    Factory = x.a.Factory,
                    Sal_Month = x.a.Sal_Month,
                    Employee_ID = x.a.Employee_ID,
                    Department = x.a.Department,
                    Permission_Group = x.a.Permission_Group,
                    Salary_Type = x.a.Salary_Type,
                    Employee_Amt = param.Insurance_Type == "V04" ? 0 : x.Amount
                }).Distinct().ToListAsync();
            }
            else
            {
                var predicate_sal_resign_monthly = PredicateBuilder.New<HRMS_Sal_Resign_Monthly>(x =>
                                                    x.Factory == param.Factory &&
                                                    x.Sal_Month == yearMonth &&
                                                    param.Permission_Group.Contains(x.Permission_Group));
                var predicate_sal_resign_monthly_detail = PredicateBuilder.New<HRMS_Sal_Resign_Monthly_Detail>(x =>
                                                            x.Factory == param.Factory &&
                                                            x.Sal_Month == yearMonth &&
                                                            x.Type_Seq == "57" &&
                                                            x.Amount > 0);
                if (!string.IsNullOrWhiteSpace(param.Department))
                    predicate_sal_resign_monthly.And(x => x.Department == param.Department);

                if (param.Insurance_Type is "V01" or "V02" or "V03")
                    predicate_sal_resign_monthly_detail.And(x => x.Item == param.Insurance_Type);

                source = await _repositoryAccessor.HRMS_Sal_Resign_Monthly.FindAll(predicate_sal_resign_monthly, true)
                .Join(_repositoryAccessor.HRMS_Sal_Resign_Monthly_Detail.FindAll(predicate_sal_resign_monthly_detail, true),
                    a => new { a.Factory, a.Sal_Month, a.Employee_ID },
                    b => new { b.Factory, b.Sal_Month, b.Employee_ID },
                    (a, b) => new { a, b })
                .Join(tblEmp_Personal, x => x.a.USER_GUID, y => y.USER_GUID, (x, y) => new { x.a, x.b.Amount, y.Local_Full_Name })
                .Select(x => new D_6_2_1_MonthlyCompulsoryInsuranceDetailedReportSource
                {
                    USER_GUID = x.a.USER_GUID,
                    Local_Full_Name = x.Local_Full_Name,
                    Factory = x.a.Factory,
                    Sal_Month = x.a.Sal_Month,
                    Employee_ID = x.a.Employee_ID,
                    Department = x.a.Department,
                    Currency = x.a.Currency,
                    Permission_Group = x.a.Permission_Group,
                    Salary_Type = x.a.Salary_Type,
                    Employee_Amt = param.Insurance_Type == "V04" ? 0 : x.Amount
                }).Distinct().ToListAsync();
            }
            return source;

        }
        #endregion

        #region DownloadFileExcel
        public async Task<OperationResult> DownloadFileExcel(D_6_2_1_MonthlyCompulsoryInsuranceDetailedReportParam param, string userName)
        {
            // required field: Factory, Year_Month, Permisson_Group, Insurance_Type, Kind 
            if (string.IsNullOrWhiteSpace(param.Factory) || string.IsNullOrWhiteSpace(param.Year_Month) || !param.Permission_Group.Any() || string.IsNullOrWhiteSpace(param.Insurance_Type) || string.IsNullOrWhiteSpace(param.Kind))
                return new OperationResult(false, "Invalid input");

            List<D_6_2_1_MonthlyCompulsoryInsuranceDetailedReportSource> source = await GetDataSource(param);
            if (!source.Any())
                return new OperationResult(false, "No Data");
            TotalResult totalResult = new();
            int count = source.Count;

            DateTime yearMonth = param.Year_Month.ToDateTime();
            // Query_Department_Report
            var query_Department_Report = await _repositoryAccessor.HRMS_Org_Department
                .FindAll(x => x.Factory == param.Factory && (string.IsNullOrEmpty(param.Department) || x.Department_Code == param.Department), true)
                .GroupJoin(
                    _repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == param.Language.ToLower() && x.Factory == param.Factory && (string.IsNullOrEmpty(param.Department) || x.Department_Code == param.Department), true),
                    x => new { x.Department_Code },
                    y => new { y.Department_Code },
                    (x, y) => new { HOD = x, HODL = y }
                )
                .SelectMany(x => x.HODL.DefaultIfEmpty(), (x, y) => new { x.HOD, HBCL = y })
               .Select(x => new D_6_2_1_Department_Report
               {
                   Factory = x.HOD.Factory,
                   Department_Code = x.HOD.Department_Code,
                   Department_Name = x.HBCL != null ? x.HBCL.Name : x.HOD.Department_Name
               }).ToListAsync();

            var tblSal_MasterBackup_Detail = _repositoryAccessor.HRMS_Sal_MasterBackup_Detail
                              .FindAll(x => x.Factory == param.Factory
                                         && x.Sal_Month == yearMonth);
            foreach (var row in source)
            {
                // 3. 保險比率值與變數
                Dictionary<string, VariableCombine> Insurance_57 = await Query_Ins_Rate_Variable_Combine("57", "Insurance", "EmployerRate", "EmployeeRate", "Amt", param.Factory, yearMonth, row.Permission_Group);
                decimal V01_EmployerRate_57 = Insurance_57.GetValueOrDefault("V01_EmployerRate_57")?.Value as decimal? ?? 0m;
                decimal V02_EmployerRate_57 = Insurance_57.GetValueOrDefault("V02_EmployerRate_57")?.Value as decimal? ?? 0m;
                decimal V03_EmployerRate_57 = Insurance_57.GetValueOrDefault("V03_EmployerRate_57")?.Value as decimal? ?? 0m;
                decimal V04_EmployerRate_57 = Insurance_57.GetValueOrDefault("V04_EmployerRate_57")?.Value as decimal? ?? 0m;

                // 4.Basic_Amt
                // Query_WageStandard_Sum default Kind = B
                var tblSal_Item_Settings = _repositoryAccessor.HRMS_Sal_Item_Settings
                       .FindAll(x => x.Factory == param.Factory
                                  && x.Permission_Group == row.Permission_Group && x.Salary_Type == row.Salary_Type).AsEnumerable();

                DateTime max_Effective_Month = tblSal_Item_Settings
                    .Where(x => x.Effective_Month <= yearMonth)
                    .Select(x => x.Effective_Month)
                    .DefaultIfEmpty(new DateTime())
                    .Max();

                var Salary_Code_List = tblSal_Item_Settings
                    .Where(x => x.Insurance == "Y"
                             && x.Effective_Month == max_Effective_Month)
                    .Select(x => x.Salary_Item);

                int basic_Amt = tblSal_MasterBackup_Detail
                        .Where(x => x.Employee_ID == row.Employee_ID
                                && Salary_Code_List.Contains(x.Salary_Item))?
                        .Sum(x => x.Amount) ?? 0;
                row.Basic_Amt = basic_Amt;
                // End Query_WageStandard_Sum
                // 5. Employer_Amt
                switch (param.Insurance_Type)
                {
                    case "V01":
                        row.Employer_Amt = Math.Round(basic_Amt * V01_EmployerRate_57, 0);
                        break;
                    case "V02":
                        row.Employer_Amt = Math.Round(basic_Amt * V02_EmployerRate_57, 0);
                        break;
                    case "V03":
                        row.Employer_Amt = Math.Round(basic_Amt * V03_EmployerRate_57, 0);
                        break;
                    case "V04":
                        row.Employer_Amt = Math.Round(basic_Amt * V04_EmployerRate_57, 0);
                        break;
                    default:
                        break;
                }
                // 6. Department
                row.Department = query_Department_Report.FirstOrDefault(x => x.Factory == row.Factory && x.Department_Code == row.Department)?.Department_Code;
                // 7. Department_Name
                row.Department_Name = query_Department_Report.FirstOrDefault(x => x.Factory == row.Factory && x.Department_Code == row.Department)?.Department_Name;
            }
            foreach (var item in source)
            {
                totalResult.Basic_Amt += item.Basic_Amt;
                totalResult.Employer_Amt += item.Employer_Amt;
                totalResult.Employee_Amt += item.Employee_Amt;
                totalResult.Total_Amt += item.Total_Amt;
            }
            List<Cell> cells = new()
            {
                new Cell("B2", param.Factory),
                new Cell("B3", param.Insurance_Type_Full),
                new Cell("D2", yearMonth.ToString("yyyy/MM")),
                new Cell("D3", param.Kind == "O" ? "在職On Job" : "離職 Resigned"),
                new Cell("F2", param.Department),
                new Cell("F3", userName),
                new Cell("H2", string.Join(", ", param.Permission_Group_Name)),
                new Cell("H3", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")),
            };

            int index = 7;
            for (int i = 0; i < count; i++)
            {
                cells.Add(new Cell("A" + index, source[i].Department));
                cells.Add(new Cell("B" + index, source[i].Department_Name));
                cells.Add(new Cell("C" + index, source[i].Employee_ID));
                cells.Add(new Cell("D" + index, source[i].Local_Full_Name));
                cells.Add(new Cell("E" + index, source[i].Basic_Amt));
                cells.Add(new Cell("F" + index, source[i].Employer_Amt));
                cells.Add(new Cell("G" + index, source[i].Employee_Amt));
                cells.Add(new Cell("H" + index, source[i].Total_Amt));
                index += 1;
            }
            // Apply color to the last row
            Aspose.Cells.Style style = new Aspose.Cells.CellsFactory().CreateStyle();
            style.Pattern = Aspose.Cells.BackgroundType.Solid;
            style.Font.Size = 12;
            style.Font.Name = "Calibri";
            style.ForegroundColor = Color.FromArgb(221, 235, 247);
            style.IsTextWrapped = true;
            // Add the total values to the last row
            cells.Add(new Cell("A" + index, "總計:\nTotal:", style));
            cells.Add(new Cell("B" + index, string.Empty, style));
            cells.Add(new Cell("C" + index, string.Empty, style));
            cells.Add(new Cell("D" + index, string.Empty, style));
            cells.Add(new Cell("E" + index, totalResult.Basic_Amt, style));
            cells.Add(new Cell("F" + index, totalResult.Employer_Amt, style));
            cells.Add(new Cell("G" + index, totalResult.Employee_Amt, style));
            cells.Add(new Cell("H" + index, totalResult.Total_Amt, style));

            ExcelResult excelResult = ExcelUtility.DownloadExcel(
              cells,
              "Resources\\Template\\CompulsoryInsuranceManagement\\6_2_1_MonthlyCompulsoryInsuranceDetailedReport\\Download.xlsx"
            );
            var dataResult = new
            {
                excelResult.Result,
                source.Count
            };
            return new OperationResult(excelResult.IsSuccess, excelResult.Error, dataResult);
        }
        #endregion

        #region GetCountRecords
        public async Task<int> GetCountRecords(D_6_2_1_MonthlyCompulsoryInsuranceDetailedReportParam param)
        {
            var source = await GetDataSource(param);
            return source.Count;
        }
        #endregion

        #region DropdownList search main
        public async Task<List<KeyValuePair<string, string>>> GetListFactory(string language, List<string> roleList)
        {
            var predHBC = PredicateBuilder.New<HRMS_Basic_Code>(x => x.Type_Seq == BasicCodeTypeConstant.Factory);

            var factorys = await Queryt_Factory_AddList(roleList);
            predHBC.And(x => factorys.Contains(x.Code));

            var data = await _repositoryAccessor.HRMS_Basic_Code.FindAll(predHBC, true)
                        .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                           x => new { x.Type_Seq, x.Code },
                           y => new { y.Type_Seq, y.Code },
                           (x, y) => new { HBC = x, HBCL = y }
                        ).SelectMany(x => x.HBCL.DefaultIfEmpty(),
                            (x, y) => new { x.HBC, HBCL = y }
                        ).Select(x => new KeyValuePair<string, string>(
                            x.HBC.Code.Trim(),
                            x.HBC.Code.Trim() + " - " + (x.HBCL != null ? x.HBCL.Code_Name.Trim() : x.HBC.Code_Name.Trim())
                        )).Distinct().ToListAsync();
            return data;
        }

        public async Task<List<KeyValuePair<string, string>>> GetListInsuranceType(string language)
        {
            var data = await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == BasicCodeTypeConstant.InsuranceType, true)
                        .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                           x => new { x.Type_Seq, x.Code },
                           y => new { y.Type_Seq, y.Code },
                           (x, y) => new { HBC = x, HBCL = y }
                        ).SelectMany(x => x.HBCL.DefaultIfEmpty(),
                            (x, y) => new { x.HBC, HBCL = y }
                        ).Select(x => new KeyValuePair<string, string>(
                            x.HBC.Code.Trim(),
                            x.HBC.Code.Trim() + " - " + (x.HBCL != null ? x.HBCL.Code_Name.Trim() : x.HBC.Code_Name.Trim())
                        )).Distinct().ToListAsync();
            return data;
        }

        public async Task<List<KeyValuePair<string, string>>> GetListPermissionGroupByFactory(string factory, string language)
        {
            var permissionGroupByFactory = await Query_Permission_Group_List(factory);
            var permissionGroup = await GetDataBasicCode(BasicCodeTypeConstant.PermissionGroup, language);
            return permissionGroup.IntersectBy(permissionGroupByFactory, x => x.Key).ToList();
        }

        public async Task<List<KeyValuePair<string, string>>> GetDepartments(string factory, string language)
        {
            return await _common.GetListDepartment(language, factory);
        }
        #endregion
    }
}