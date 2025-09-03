using System.Drawing;
using System.Globalization;
using API._Services.Interfaces.SalaryReport;
using API.Data;
using API.DTOs.SalaryReport;
using API.Helper.Constant;
using API.Models;
using Aspose.Cells;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.SalaryReport
{
    public class S_7_2_14_TaxPayingEmployeeMonthlyNightShiftExtraPayAndOvertimePay : BaseServices, I_7_2_14_TaxPayingEmployeeMonthlyNightShiftExtraPayAndOvertimePay
    {
        public S_7_2_14_TaxPayingEmployeeMonthlyNightShiftExtraPayAndOvertimePay(DBContext dbContext) : base(dbContext) { }
        private static readonly string rootPath = Directory.GetCurrentDirectory();
        private async Task<OperationResult> GetData(NightShiftExtraAndOvertimePayParam param)
        {
            if (string.IsNullOrWhiteSpace(param.Factory)
                || !param.Permission_Group.Any()
                || string.IsNullOrWhiteSpace(param.Year_Month)
                || !DateTime.TryParseExact(param.Year_Month, "yyyy/MM", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime yearMonth))
                return new OperationResult(false, "SalaryReport.MonthlySalaryAdditionsDeductionsSummaryReport.InvalidInput");

            var pred = PredicateBuilder.New<HRMS_Sal_Monthly>(x => x.Factory == param.Factory
                && x.Sal_Month.Date == yearMonth.Date
                && x.Tax > 0
                && param.Permission_Group.Contains(x.Permission_Group));

            var listEmployeeID = await _repositoryAccessor.HRMS_Emp_Personal
                .FindAll(x => x.Factory == param.Factory
                            && param.Permission_Group.Contains(x.Permission_Group), true)
                .Select(x => x.Employee_ID)
                .ToListAsync();

            if (!string.IsNullOrWhiteSpace(param.Department))
                pred.And(x => x.Department == param.Department);

            if (!string.IsNullOrWhiteSpace(param.EmployeeID))
                pred.And(x => x.Employee_ID == param.EmployeeID);

            pred.And(x => listEmployeeID.Contains(x.Employee_ID));

            var wk_sql = await _repositoryAccessor.HRMS_Sal_Monthly.FindAll(pred).ToListAsync();

            var result = new List<NightShiftExtraAndOvertimePayReport>();
            var listDepartments = await GetDepartmentName(param.Factory, param.Language);

            foreach (var item in wk_sql)
            {
                var emp = await _repositoryAccessor.HRMS_Emp_Personal
                    .FirstOrDefaultAsync(x => x.Factory == item.Factory && x.Employee_ID == item.Employee_ID);

                var taxNumber = await _repositoryAccessor.HRMS_Sal_Tax_Number
                    .FindAll(x => x.Factory == item.Factory
                            && x.Employee_ID == item.Employee_ID
                            && x.Year <= item.Sal_Month, true)
                    .OrderByDescending(x => x.Year)
                    .FirstOrDefaultAsync();

                var wageStandard = await Query_WageStandard_Sum("B",
                    item.Factory, item.Sal_Month, item.Employee_ID,
                    item.Permission_Group, item.Salary_Type);

                var A06_AMT = await _repositoryAccessor.HRMS_Sal_AddDedItem_Monthly
                    .FindAll(x => x.Factory == item.Factory
                            && x.Sal_Month == item.Sal_Month
                            && x.Employee_ID == item.Employee_ID
                            && x.AddDed_Type == "A"
                            && x.AddDed_Item == "A06", true)
                    .Select(x => x.Amount)
                    .FirstOrDefaultAsync();

                var overtime50_AMT = await Query_Single_Sal_Monthly_Detail("Y", item.Factory, item.Sal_Month, item.Employee_ID, "42", "A", "A01");

                var ho_AMT = await Query_Single_Sal_Monthly_Detail("Y", item.Factory, item.Sal_Month, item.Employee_ID, "42", "A", "C01");

                var total1 = await Query_Single_Sal_Monthly_Detail("Y", item.Factory, item.Sal_Month, item.Employee_ID, "42", "A", "A03");
                var total2 = await Query_Single_Sal_Monthly_Detail("Y", item.Factory, item.Sal_Month, item.Employee_ID, "57", "D", "V01");
                var total3 = await Query_Single_Sal_Monthly_Detail("Y", item.Factory, item.Sal_Month, item.Employee_ID, "57", "D", "V02");
                var total4 = await Query_Single_Sal_Monthly_Detail("Y", item.Factory, item.Sal_Month, item.Employee_ID, "57", "D", "V03");

                var overtimeHours = await Query_Att_Monthly_Detail("Y", item.Factory, item.Sal_Month, item.Employee_ID, "2");
                var overtimeAndNightShiftAllowance = await Query_Sal_Monthly_Detail("Y", item.Factory, item.Sal_Month, item.Employee_ID,
                                                            "42", "A", item.Permission_Group, item.Salary_Type, "2");
                decimal nhno_AMT = 0;
                var days = await Query_Att_Monthly_Detail_Item(item.Factory, item.Sal_Month, item.Employee_ID, "2", "A01");
                if (days <= 0)
                    nhno_AMT = total1 * 100 / 210;
                else if (days > 0)
                    nhno_AMT = total1 * 110 / 210;

                decimal ins_AMT = total2 + total3 + total4;

                result.Add(new NightShiftExtraAndOvertimePayReport
                {
                    Factory = item.Factory,
                    Department = item.Department,
                    DepartmentName = listDepartments.FirstOrDefault(x => x.Key == item.Department).Value,
                    EmployeeID = item.Employee_ID,
                    LocalFullName = emp?.Local_Full_Name,
                    TaxNo = taxNumber?.TaxNo,
                    Standard = wageStandard.ToString(),
                    OvertimeHours = overtimeHours,
                    OvertimeAndNightShiftAllowance = overtimeAndNightShiftAllowance,
                    A06_AMT = A06_AMT,
                    Overtime50_AMT = (overtime50_AMT + A06_AMT) * 1 / 3,
                    NHNO_AMT = nhno_AMT,
                    HO_AMT = ho_AMT,
                    INS_AMT = ins_AMT,
                    SUM_AMT = nhno_AMT + ho_AMT + ins_AMT
                });
            }

            return new OperationResult(true, result);
        }

        private async Task<List<KeyValuePair<string, string>>> GetPermissionGroup(string factory, string Language)
        {
            return await Query_BasicCode_PermissionGroup(factory, Language);
        }

        public async Task<OperationResult> Download(NightShiftExtraAndOvertimePayParam param)
        {
            var updatedPermissionGroup = new List<string>();
            var listPermissionGroup = await GetPermissionGroup(param.Factory, param.Language);
            var result = await GetData(param);
            if (!result.IsSuccess)
                return result;

            var data = (List<NightShiftExtraAndOvertimePayReport>)result.Data;

            if (data.Count == 0)
                return new OperationResult(false, "System.Message.Nodata");

            MemoryStream memoryStream = new();
            string file = Path.Combine(
                rootPath,
                "Resources\\Template\\SalaryReport\\7_2_14_TaxPayingEmployeeMonthlyNightShiftExtraPayAndOvertimePay\\Download.xlsx"
            );
            WorkbookDesigner obj = new()
            {
                Workbook = new Workbook(file)
            };
            foreach (var item in param.Permission_Group)
            {
                var updatedItem = listPermissionGroup.FirstOrDefault(x => x.Key == item).Value;
                updatedPermissionGroup.Add(updatedItem);
            }
            Worksheet worksheet = obj.Workbook.Worksheets[0];

            worksheet.Cells["B2"].PutValue(param.Factory);
            worksheet.Cells["D2"].PutValue(param.Year_Month);
            worksheet.Cells["F2"].PutValue(string.Join(",", updatedPermissionGroup));
            worksheet.Cells["H2"].PutValue(param.Department);
            worksheet.Cells["J2"].PutValue(param.EmployeeID);
            worksheet.Cells["B3"].PutValue(param.UserName);
            worksheet.Cells["D3"].PutValue(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

            var dataOvertime = Math.Max(data[0].OvertimeHours.Count, data[0].OvertimeAndNightShiftAllowance.Count);

            for (int i = 0; i < dataOvertime; i++)
            {
                if (i < data[0].OvertimeHours.Count)
                {
                    worksheet.Cells[5, i + 7].PutValue(data[0].OvertimeHours[i].Leave_Code + " - " + data[0].OvertimeHours[i].CodeName_TW);
                    worksheet.Cells[5, i + 7].SetStyle(GetStyle(obj, 255, 242, 204));
                    worksheet.Cells[4, i + 7].PutValue(data[0].OvertimeHours[i].Leave_Code + " - " + data[0].OvertimeHours[i].CodeName_EN);
                    worksheet.Cells[4, i + 7].SetStyle(GetStyle(obj, 255, 242, 204));
                }

                if (i < data[0].OvertimeAndNightShiftAllowance.Count)
                {
                    worksheet.Cells[4, i + data[0].OvertimeHours.Count + 7].PutValue(data[0].OvertimeAndNightShiftAllowance[i].Item);
                    worksheet.Cells[4, i + data[0].OvertimeHours.Count + 7].SetStyle(GetStyle(obj, 226, 239, 218));
                    worksheet.Cells[5, i + data[0].OvertimeHours.Count + 7].PutValue(data[0].OvertimeAndNightShiftAllowance[i].Item);
                    worksheet.Cells[5, i + data[0].OvertimeHours.Count + 7].SetStyle(GetStyle(obj, 226, 239, 218));
                }
                var totalIndex = data[0].OvertimeHours.Count + data[0].OvertimeAndNightShiftAllowance.Count + 7;
                worksheet.Cells[4, totalIndex].PutValue("A06加班費(參加非生產活動)");
                worksheet.Cells[5, totalIndex].PutValue("A06-Overtime Pay (Non-Production Activities)");
                var style = worksheet.Cells[4, totalIndex].GetStyle();
                style.IsTextWrapped = true;
                worksheet.Cells[4, totalIndex].SetStyle(style);
                worksheet.Cells[5, totalIndex].SetStyle(style);
                worksheet.Cells[4, totalIndex + 1].PutValue("非假日加班費差額50%");
                worksheet.Cells[5, totalIndex + 1].PutValue("Overtime Paid 50% Difference on Normal Working Day");
                worksheet.Cells[4, totalIndex + 1].SetStyle(style);
                worksheet.Cells[5, totalIndex + 1].SetStyle(style);
                worksheet.Cells[4, totalIndex + 2].PutValue("非假日夜班加班費差額100%& 110%");
                worksheet.Cells[5, totalIndex + 2].PutValue("Night Shift Overtime Paid 100% or 110% Difference on Normal Working Day");
                worksheet.Cells[4, totalIndex + 2].SetStyle(style);
                worksheet.Cells[5, totalIndex + 2].SetStyle(style);
                worksheet.Cells[4, totalIndex + 3].PutValue("假日加班費差額300%");
                worksheet.Cells[5, totalIndex + 3].PutValue("Overtime Paid 300% Difference on National Holiday");
                worksheet.Cells[4, totalIndex + 3].SetStyle(style);
                worksheet.Cells[5, totalIndex + 3].SetStyle(style);
                worksheet.Cells[4, totalIndex + 4].PutValue("保險金額");
                worksheet.Cells[5, totalIndex + 4].PutValue("Insurance Amount");
                worksheet.Cells[4, totalIndex + 4].SetStyle(style);
                worksheet.Cells[5, totalIndex + 4].SetStyle(style);
                worksheet.Cells[4, totalIndex + 5].PutValue("家境狀況獲准豁免收入稅前不負稅之金額");
                worksheet.Cells[5, totalIndex + 5].PutValue("Non-Taxable Amount before Deduction for Dependents");
                worksheet.Cells[4, totalIndex + 5].SetStyle(style);
                worksheet.Cells[5, totalIndex + 5].SetStyle(style);

            }
            for (int i = 0; i < data.Count; i++)
            {
                worksheet.Cells["A" + (i + 7)].PutValue(data[i].Factory);
                worksheet.Cells["B" + (i + 7)].PutValue(data[i].Department);
                worksheet.Cells["C" + (i + 7)].PutValue(data[i].DepartmentName);
                worksheet.Cells["D" + (i + 7)].PutValue(data[i].EmployeeID);
                worksheet.Cells["E" + (i + 7)].PutValue(data[i].LocalFullName);
                worksheet.Cells["F" + (i + 7)].PutValue(data[i].TaxNo);
                worksheet.Cells["G" + (i + 7)].PutValue(data[i].Standard);

                int columnIndex = 7;
                for (int j = 0; j < dataOvertime; j++)
                {
                    if (j < data[i].OvertimeHours.Count)
                        worksheet.Cells[i + 6, columnIndex].PutValue(data[i].OvertimeHours[j].Days);
                    if (j < data[i].OvertimeAndNightShiftAllowance.Count)
                        worksheet.Cells[i + 6, columnIndex + data[i].OvertimeHours.Count].PutValue(data[i].OvertimeAndNightShiftAllowance[j].Amount);
                    columnIndex++;
                }
                var totalIndex = data[i].OvertimeHours.Count + data[i].OvertimeAndNightShiftAllowance.Count + 7;
                worksheet.Cells[i + 6, totalIndex].PutValue(data[i].A06_AMT);
                worksheet.Cells[i + 6, totalIndex + 1].PutValue(data[i].Overtime50_AMT);
                worksheet.Cells[i + 6, totalIndex + 2].PutValue(data[i].NHNO_AMT);
                worksheet.Cells[i + 6, totalIndex + 3].PutValue(data[i].HO_AMT);
                worksheet.Cells[i + 6, totalIndex + 4].PutValue(data[i].INS_AMT);
                worksheet.Cells[i + 6, totalIndex + 5].PutValue(data[i].SUM_AMT);
            }
            var totalRowPos = worksheet.Cells.MaxRow + 2;
            Style totalStyle = obj.Workbook.CreateStyle();
            totalStyle.IsTextWrapped = true;
            worksheet.Cells["B" + totalRowPos].Value = "合計:\nTotal:";
            worksheet.Cells["B" + totalRowPos].SetStyle(totalStyle);
            for (int i = 2; i <= worksheet.Cells.MaxDataColumn; i++)
            {
                // var cellPos = MyRegex().Replace(worksheet.Cells[5, i].Name, "");
                // worksheet.Cells[cellPos + totalRowPos].Formula = "=SUM(" + cellPos + (totalRowPos - data.Count) + ":" + cellPos + (totalRowPos - 1) + ")";
            }
            CellArea area = new()
            {
                StartRow = 1,
                StartColumn = 8,
                EndRow = 6,
                EndColumn = data[0].OvertimeHours.Count + data[0].OvertimeAndNightShiftAllowance.Count + 14
            };
            worksheet.AutoFitColumns(area.StartColumn, area.EndColumn);
            worksheet.AutoFitRows(area.StartRow, area.EndRow);

            obj.Workbook.Save(memoryStream, SaveFormat.Xlsx);
            return new OperationResult(true, new { TotalRows = data.Count, Excel = memoryStream.ToArray() });
        }

        private Style GetStyle(WorkbookDesigner obj, int color1, int color2, int color3)
        {
            Style style = obj.Workbook.CreateStyle();
            style.ForegroundColor = Color.FromArgb(color1, color2, color3);
            style.Pattern = BackgroundType.Solid;
            style.IsTextWrapped = true;
            style.HorizontalAlignment = TextAlignmentType.Center;
            style.VerticalAlignment = TextAlignmentType.Center;
            style = AsposeUtility.SetAllBorders(style);
            return style;
        }

        public async Task<OperationResult> GetTotalRows(NightShiftExtraAndOvertimePayParam param)
        {
            var result = await GetData(param);
            if (!result.IsSuccess)
                return result;
            var data = (List<NightShiftExtraAndOvertimePayReport>)result.Data;
            return new OperationResult(true, data.Count);
        }

        #region Query_Att_Monthly_Detail_Item
        private async Task<decimal> Query_Att_Monthly_Detail_Item(string Factory, DateTime Year_Month, string EmployeeID, string Leave_Type, string Leave_Code)
        {
            var days = await _repositoryAccessor.HRMS_Att_Monthly_Detail.FindAll(x => x.Factory == Factory
                && x.Att_Month == Year_Month
                && x.Employee_ID == EmployeeID
                && x.Leave_Type == Leave_Type
                && x.Leave_Code == Leave_Code)
                .Select(x => x.Days)
                .FirstOrDefaultAsync();
            return days;
        }
        #endregion
        #region Query_Single_Sal_Monthly_Detail 
        private async Task<decimal> Query_Single_Sal_Monthly_Detail(string Kind, string Factory, DateTime Year_Month, string EmployeeID, string Type_Seq, string AddDed_Type, string Item)
        {
            int Amount_Values = 0;
            if (Kind == "Y")
                Amount_Values = await _repositoryAccessor.HRMS_Sal_Monthly_Detail.FindAll(x => x.Factory == Factory
                    && x.Sal_Month == Year_Month
                    && x.Employee_ID == EmployeeID
                    && x.Type_Seq == Type_Seq
                    && x.AddDed_Type == AddDed_Type
                    && x.Item == Item
                ).SumAsync(x => x.Amount);
            else
                Amount_Values = await _repositoryAccessor.HRMS_Sal_Resign_Monthly_Detail.FindAll(x => x.Factory == Factory
                    && x.Sal_Month == Year_Month
                    && x.Employee_ID == EmployeeID
                    && x.Type_Seq == Type_Seq
                    && x.AddDed_Type == AddDed_Type
                    && x.Item == Item
                ).SumAsync(x => x.Amount);

            return Amount_Values;
        }
        #endregion
        #region Query_Att_Monthly_Detail
        private async Task<List<Att_Monthly_Detail_Values>> Query_Att_Monthly_Detail(string Kind, string Factory, DateTime YearMonth, string EmployeeID, string LeaveType)
        {
            List<Att_Monthly_Detail_Temp_7_2_14> Att_Monthly_Detail_Temp;

            if (Kind == "Y")
            {
                Att_Monthly_Detail_Temp = await _repositoryAccessor.HRMS_Att_Monthly_Detail
                    .FindAll(x => x.Factory == Factory
                        && x.Att_Month == YearMonth
                        && x.Employee_ID == EmployeeID
                        && x.Leave_Type == LeaveType, true)
                    .Select(x => new Att_Monthly_Detail_Temp_7_2_14
                    {
                        Employee_ID = x.Employee_ID,
                        Leave_Code = x.Leave_Code,
                        Days = x.Days
                    })
                    .ToListAsync();
            }
            else
            {
                Att_Monthly_Detail_Temp = await _repositoryAccessor.HRMS_Att_Resign_Monthly_Detail
                    .FindAll(x => x.Factory == Factory
                        && x.Att_Month == YearMonth
                        && x.Employee_ID == EmployeeID
                        && x.Leave_Type == LeaveType, true)
                    .Select(x => new Att_Monthly_Detail_Temp_7_2_14
                    {
                        Employee_ID = x.Employee_ID,
                        Leave_Code = x.Leave_Code,
                        Days = x.Days
                    })
                    .ToListAsync();
            }

            var maxEffectiveMonth = await _repositoryAccessor.HRMS_Att_Use_Monthly_Leave
                .FindAll(x => x.Factory == Factory
                    && x.Leave_Type == LeaveType
                    && x.Effective_Month <= YearMonth, true)
                .MaxAsync(x => (DateTime?)x.Effective_Month);

            if (!maxEffectiveMonth.HasValue)
                return new List<Att_Monthly_Detail_Values>();

            var Setting_Temp = await _repositoryAccessor.HRMS_Att_Use_Monthly_Leave
                .FindAll(x => x.Factory == Factory
                    && x.Leave_Type == LeaveType
                    && x.Effective_Month == maxEffectiveMonth.Value, true)
                .Select(x => new Setting_Temp_7_2_14
                {
                    Seq = x.Seq,
                    Code = x.Code
                })
                .ToListAsync();

            var HBCL = await _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Type_Seq == "42", true).ToListAsync();
            var result = Att_Monthly_Detail_Temp
                .GroupJoin(Setting_Temp,
                    x => x.Leave_Code,
                    y => y.Code,
                    (x, y) => new { AMDT = x, Setting_Temp = y })
                .SelectMany(x => x.Setting_Temp.DefaultIfEmpty(),
                    (x, y) => new { x.AMDT, Setting_Temp = y })
                .Select(x =>
                    new Att_Monthly_Detail_Values
                    {
                        Employee_ID = x.AMDT.Employee_ID,
                        Leave_Code = x.AMDT.Leave_Code,
                        CodeName_EN = HBCL.FirstOrDefault(z => z.Code == x.AMDT.Leave_Code && z.Language_Code == "EN")?.Code_Name,
                        CodeName_TW = HBCL.FirstOrDefault(z => z.Code == x.AMDT.Leave_Code && z.Language_Code == "TW")?.Code_Name,
                        Seq = x.Setting_Temp?.Seq ?? 0,
                        Days = x.AMDT.Days
                    })
                .OrderBy(x => x.Seq)
                .ThenBy(x => x.Leave_Code)
                .ToList();

            return result;
        }
        #endregion

        public async Task<List<KeyValuePair<string, string>>> GetListDepartment(string factory, string language)
        {
            var departments = await Query_Department_List(factory);
            var departmentsWithLanguage = await _repositoryAccessor.HRMS_Org_Department
                .FindAll(x => x.Factory == factory
                    && departments.Select(y => y.Key).Contains(x.Department_Code), true)
                .GroupJoin(_repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                    x => new { x.Division, x.Factory, x.Department_Code },
                    y => new { y.Division, y.Factory, y.Department_Code },
                    (HOD, HODL) => new { HOD, HODL })
                .SelectMany(x => x.HODL.DefaultIfEmpty(),
                    (x, y) => new { x.HOD, HODL = y })
                .Select(x => new KeyValuePair<string, string>(x.HOD.Department_Code, $"{x.HOD.Department_Code} - {(x.HODL != null ? x.HODL.Name : x.HOD.Department_Name)}"))
                .Distinct()
                .ToListAsync();
            return departmentsWithLanguage;
        }

        public async Task<List<KeyValuePair<string, string>>> GetListFactory(string userName, string language)
        {
            List<string> factories = await Queryt_Factory_AddList(userName);
            var factoriesWithLanguage = await _repositoryAccessor.HRMS_Basic_Code
                .FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Factory
                    && factories.Contains(x.Code), true)
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                    x => new { x.Type_Seq, x.Code },
                    y => new { y.Type_Seq, y.Code },
                    (HBC, HBCL) => new { HBC, HBCL })
                .SelectMany(x => x.HBCL.DefaultIfEmpty(),
                    (x, y) => new { x.HBC, HBCL = y })
                .Select(x => new KeyValuePair<string, string>(x.HBC.Code, $"{x.HBC.Code} - {(x.HBCL != null ? x.HBCL.Code_Name : x.HBC.Code_Name)}"))
                .ToListAsync();
            return factoriesWithLanguage;
        }

        public async Task<List<KeyValuePair<string, string>>> GetListPermissionGroup(string factory, string language)
        {
            var permissionGroups = await Query_Permission_List(factory);

            var permissionGroupsWithLanguage = await _repositoryAccessor.HRMS_Basic_Code
                .FindAll(x => x.Type_Seq == BasicCodeTypeConstant.PermissionGroup
                    && permissionGroups.Select(y => y.Permission_Group).Contains(x.Code), true)
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                    x => new { x.Type_Seq, x.Code },
                    y => new { y.Type_Seq, y.Code },
                    (HBC, HBCL) => new { HBC, HBCL })
                .SelectMany(x => x.HBCL.DefaultIfEmpty(),
                    (x, y) => new { x.HBC, HBCL = y })
                .Select(x => new KeyValuePair<string, string>(x.HBC.Code, $"{x.HBC.Code} - {(x.HBCL != null ? x.HBCL.Code_Name : x.HBC.Code_Name)}"))
                .ToListAsync();
            return permissionGroupsWithLanguage;
        }

        private async Task<List<KeyValuePair<string, string>>> GetDepartmentName(string factory, string language)
        {
            var HOD = _repositoryAccessor.HRMS_Org_Department.FindAll(x => x.Factory == factory, true);
            var HODL = _repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Factory == factory && x.Language_Code.ToLower() == language.ToLower(), true);

            return await HOD
                .GroupJoin(HODL,
                    department => new { department.Factory, department.Department_Code },
                    lang => new { lang.Factory, lang.Department_Code },
                    (department, lang) => new { department, lang })
                    .SelectMany(x => x.lang.DefaultIfEmpty(),
                    (department, lang) => new { department.department, lang })
                .Select(x => new KeyValuePair<string, string>(x.department.Department_Code, $"{(x.lang != null ? x.lang.Name : x.department.Department_Name)}"))
                .ToListAsync();
        }
    }
}