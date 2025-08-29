using System.Drawing;
using API.Data;
using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using API.Helper.Constant;
using API.Helper.Utilities;
using API.Models;
using Aspose.Cells;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.AttendanceMaintenance
{
    public class S_5_2_2_WorkingHoursReport : BaseServices, I_5_2_2_WorkingHoursReport
    {
        private readonly TimeSpan timeSpan1900 = "1900".ToTimeSpan();

        public S_5_2_2_WorkingHoursReport(DBContext dbContext) : base(dbContext)
        {
        }

        #region GetList
        public async Task<List<KeyValuePair<string, string>>> GetListFactory(string language, string userName)
        {
            var factoryList = await Queryt_Factory_AddList(userName);

            var query = _repositoryAccessor.HRMS_Basic_Code
                .FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Factory && factoryList.Contains(x.Code), true);

            var data = await query
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language
                    .FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Factory && x.Language_Code.ToLower() == language.ToLower(), true),
                    code => code.Code,
                    language => language.Code,
                    (code, language) => new { Code = code, Language = language })
                .SelectMany(
                    x => x.Language.DefaultIfEmpty(),
                    (x, language) => new { x.Code, Language = language })
                .Select(x => new KeyValuePair<string, string>(x.Code.Code, $"{x.Code.Code} - {x.Language.Code_Name ?? x.Code.Code_Name}"))
                .Distinct()
                .ToListAsync();

            return data;
        }

        public async Task<List<KeyValuePair<string, string>>> GetListDepartment(string language, string factory)
        {
            return await _repositoryAccessor.HRMS_Org_Department.FindAll(x => x.Factory == factory, true)
                .Join(
                    _repositoryAccessor.HRMS_Basic_Factory_Comparison.FindAll(x => x.Kind == "1" && x.Factory == factory, true),
                    department => department.Division,
                    factoryComparison => factoryComparison.Division,
                    (department, factoryComparison) => department
                )
                .GroupJoin(
                    _repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                    department => new { department.Factory, department.Department_Code },
                    language => new { language.Factory, language.Department_Code },
                    (department, language) => new { Department = department, Language = language }
                )
                .SelectMany(
                    x => x.Language.DefaultIfEmpty(),
                    (x, language) => new { x.Department, Language = language }
                )
                .OrderBy(x => x.Department.Department_Code)
                .Select(
                    x => new KeyValuePair<string, string>(
                        x.Department.Department_Code,
                        $"{(x.Language != null ? x.Language.Name : x.Department.Department_Name)}"
                    )
                )
                .ToListAsync();
        }
        #endregion

        #region GetData
        public async Task<List<WorkingHoursReportDto>> GetData(WorkingHoursReportParam param)
        {
            var Start_Date = Convert.ToDateTime(param.Date_From);
            var End_Date = Convert.ToDateTime(param.Date_To);
            var pred = PredicateBuilder.New<HRMS_Emp_Personal>(x =>
                (x.Factory == param.Factory || x.Assigned_Factory == param.Factory) &&
                (x.Resign_Date == null || x.Resign_Date > Start_Date) &&
                x.Onboard_Date <= End_Date);
            var predLeave = PredicateBuilder.New<HRMS_Att_Leave_Maintain>(x =>
                x.Factory == param.Factory && x.Leave_Date >= Start_Date && x.Leave_Date <= End_Date);
            var predOvertime = PredicateBuilder.New<HRMS_Att_Overtime_Maintain>(x =>
                x.Factory == param.Factory && x.Overtime_Date >= Start_Date && x.Overtime_Date <= End_Date);
            var predCalendar = PredicateBuilder.New<HRMS_Att_Calendar>(x =>
                x.Factory == param.Factory &&
                (x.Type_Code == "C05" || x.Type_Code == "C00") &&
                x.Att_Date >= Start_Date && x.Att_Date <= End_Date);
            var predChangeRecord = PredicateBuilder.New<HRMS_Att_Change_Record>(x =>
                x.Factory == param.Factory && x.Att_Date >= Start_Date && x.Att_Date <= End_Date);
            if (!string.IsNullOrWhiteSpace(param.Department))
                pred.And(x => x.Department == param.Department || x.Assigned_Department == param.Department);

            var HEP = _repositoryAccessor.HRMS_Emp_Personal.FindAll(pred, true).OrderByDescending(x => x.Employee_ID).ToList();
            var departmentQuery = _repositoryAccessor.HRMS_Org_Department.FindAll(true);
            var departmentLanguage = _repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == param.Lang.ToLower(), true);
            var leaveData = _repositoryAccessor.HRMS_Att_Leave_Maintain.FindAll(predLeave, true).ToList();
            var allowanceData = _repositoryAccessor.HRMS_Att_Overtime_Maintain.FindAll(predOvertime, true).ToList();
            var leaveCodeList = new List<string>
            {
                "A0", "J3", "B0", "C0", "I0", "I1", "N0", "D0", "E0", "F0",
                "G0", "G2", "G1", "H0", "K0", "J0", "J2", "J5", "J1", "O0", "J4"
            };
            var localPermissionlist = _repositoryAccessor.HRMS_Emp_Permission_Group
                .FindAll(x => x.Factory == param.Factory && x.Foreign_Flag == "N")
                .Select(x => x.Permission_Group)
                .ToList();
            var leaveCode = await GetCodeList(param.Lang, param.Factory, param.Date_To, BasicCodeTypeConstant.Leave);


            #region Get allowanceCode
            var listCode = await GetLeaveCodeAllowances(param.Factory, param.Date_To);
            var allowanceCode = await GetLeaveCodeAllowancesLanguage(listCode, param.Lang);
            #endregion

            var validShiftTypes = new List<string> { "00", "10", "40", "50", "60", "G0", "H0", "S0", "T0" };
            var calendarDates = _repositoryAccessor.HRMS_Att_Calendar.FindAll(predCalendar, true).Select(x => x.Att_Date).ToList();
            predChangeRecord.And(x => !calendarDates.Contains(x.Att_Date));
            var changeRecords = _repositoryAccessor.HRMS_Att_Change_Record.FindAll(predChangeRecord).ToList();
            var personalQuery = HEP.Select(personal =>
            {
                var leaveDataUser = leaveData.FindAll(x => x.Employee_ID == personal.Employee_ID && leaveCodeList.Contains(x.Leave_code)).ToList();
                var leaveSumDays = leaveDataUser.Any() ? leaveDataUser.Sum(x => x.Days) : 0;

                decimal actualWorkDays = param.Salary_WorkDays - leaveSumDays;

                int wk_day = (End_Date.Date - Start_Date.Date).Days;
                if (personal.Onboard_Date.Date > Start_Date.Date && personal.Onboard_Date.Date <= End_Date.Date)
                {
                    for (int tm_cnt = 0; tm_cnt < wk_day; tm_cnt++)
                    {
                        var tm_date = Start_Date.AddDays(tm_cnt);
                        if (tm_date.Date == personal.Onboard_Date.Date) break;
                        if (localPermissionlist.Contains(personal.Permission_Group) && tm_date.DayOfWeek != DayOfWeek.Sunday)
                            actualWorkDays--;
                    }
                }
                if (personal.Resign_Date.HasValue && personal.Resign_Date.Value <= End_Date && personal.Resign_Date.Value >= Start_Date)
                {
                    for (int tm_cnt = 0; tm_cnt < wk_day; tm_cnt++)
                    {
                        var tm_date = End_Date.AddDays(-tm_cnt);
                        if (localPermissionlist.Contains(personal.Permission_Group) && tm_date.DayOfWeek != DayOfWeek.Sunday)
                            actualWorkDays--;
                        if (tm_date == personal.Resign_Date) break;
                    }
                }

                return new
                {
                    personal.Division,
                    personal.Factory,
                    personal.Department,
                    personal.Employee_ID,
                    personal.Local_Full_Name,
                    personal.Onboard_Date,
                    personal.Resign_Date,
                    Leave_Days = leaveCode
                        .Select(lc => leaveData
                            .FindAll(ld => ld.Employee_ID == personal.Employee_ID && ld.Leave_code == lc.Code)
                            .Sum(ld => ld.Days))
                        .ToList(),
                    Allowance_Days = allowanceCode
                        .Select(ac => allowanceData
                            .FindAll(ad => ad.Employee_ID == personal.Employee_ID && ad.Holiday == ac.Char1 && ad.GetType().GetProperty(ac.Char2) != null)
                            .Sum(ad => Convert.ToDecimal(ad.GetType().GetProperty(ac.Char2)?.GetValue(ad, null)))
                        ).ToList(),

                    Actual_Work_Days = Math.Max(actualWorkDays, 0),         // Tính Actual Work Days 
                    Night_Shift_Allowance_Times = changeRecords.Count(x =>  // Tính Night_Shift_Allowance_Times
                    x.Employee_ID == personal.Employee_ID && (
                        (validShiftTypes.Contains(x.Work_Shift_Type) && x.Clock_Out.ToTimeSpan() < timeSpan1900 &&
                        x.Overtime_ClockOut.ToTimeSpan() - x.Overtime_ClockIn.ToTimeSpan() >= new TimeSpan(1, 30, 0) &&
                        x.Overtime_ClockOut.ToTimeSpan() - x.Overtime_ClockIn.ToTimeSpan() <= new TimeSpan(2, 0, 0)) ||
                        (x.Overtime_ClockOut.ToTimeSpan() - x.Overtime_ClockIn.ToTimeSpan() >= new TimeSpan(1, 30, 0) &&
                        x.Overtime_ClockOut.ToTimeSpan() - x.Overtime_ClockIn.ToTimeSpan() <= new TimeSpan(2, 0, 0))))
                };
            }).ToList();
            var result = personalQuery
                .GroupJoin(departmentQuery,
                    personal => new { personal.Division, personal.Factory, personal.Department },
                    department => new { department.Division, department.Factory, Department = department.Department_Code },
                    (personal, department) => new { Personal = personal, Department = department })
                .SelectMany(x => x.Department.DefaultIfEmpty(),
                    (x, department) => new { x.Personal, Department = department })
                .GroupJoin(
                    departmentLanguage,
                    x => new { x.Department?.Division, x.Department?.Factory, x.Department?.Department_Code },
                    lang => new { lang.Division, lang.Factory, lang.Department_Code },
                    (x, lang) => new { x.Personal, x.Department, LanguageDepartment = lang })
                .SelectMany(
                    x => x.LanguageDepartment.DefaultIfEmpty(),
                    (x, lang) => new { x.Personal, x.Department, LanguageDepartment = lang })
                .Select(x => new WorkingHoursReportDto
                {
                    Department = x.Personal?.Department,
                    Department_Name = x.LanguageDepartment?.Name ?? x.Department?.Department_Name,
                    Employee_ID = x.Personal.Employee_ID,
                    Local_Full_Name = x.Personal.Local_Full_Name,
                    Onboard_Date = x.Personal.Onboard_Date.ToString("yyyy/MM/dd"),
                    Resign_Date = x.Personal.Resign_Date.HasValue ? x.Personal.Resign_Date.Value.ToString("yyyy/MM/dd") : "",
                    Actual_Work_Days = x.Personal.Actual_Work_Days,
                    Night_Shift_Allowance_Times = x.Personal.Night_Shift_Allowance_Times,
                    Leave_Days = x.Personal.Leave_Days,
                    Allowance_Days = x.Personal.Allowance_Days,
                })
                .OrderBy(x => x.Department).ThenBy(x => x.Employee_ID)
                .ToList();
            var groupedResult = result.GroupBy(x => new { x.Department, x.Employee_ID }).Select(x => x.First()).ToList();

            return groupedResult;
        }
        #endregion

        #region GetCodeList
        public async Task<List<CodeDto>> GetCodeList(string language, string factory, string endDate, string codeType)
        {
            var HAUML = await _repositoryAccessor.HRMS_Att_Use_Monthly_Leave
                .FindAll(x => x.Factory == factory && codeType == BasicCodeTypeConstant.Leave ? x.Leave_Type == "1" : x.Leave_Type == "2")
                .ToListAsync();

            var HBC = _repositoryAccessor.HRMS_Basic_Code
                .FindAll(x => x.Type_Seq == codeType && x.IsActive)
                .ToList();

            var maxEffectiveMonth = HAUML
                .FindAll(x => x.Effective_Month <= Convert.ToDateTime(endDate))
                .Max(x => x.Effective_Month);

            var result = HAUML.FindAll(x => x.Effective_Month == maxEffectiveMonth)
                .Join(HBC,
                    HAUML => HAUML.Code,
                    HEP => HEP.Code,
                    (HAUML, HBC) => new { HAUML, HBC })
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(
                    x => x.Language_Code.ToLower() == language.ToLower() && x.Type_Seq == codeType, true),
                    last => last.HBC.Code,
                    HBCL => HBCL.Code,
                    (last, HBCL) => new { last.HAUML, last.HBC, HBCL })
                .SelectMany(
                    x => x.HBCL.DefaultIfEmpty(),
                    (last, HBCL) => new { last.HAUML, last.HBC, HBCL })

                .Select(x => new CodeDto
                {
                    Seq = x.HAUML.Seq,
                    Code = x.HBC.Code,
                    Char1 = x.HBC.Char1,
                    Char2 = x.HBC.Char2,
                    CodeName = x.HBCL.Code_Name ?? x.HBC.Code_Name
                })
                .OrderBy(x => x.Seq)
                .Distinct()
                .ToList();
            return result;
        }

        private async Task<List<string>> GetLeaveCodeAllowances(string factory, string endDate)
        {
            var monthly_Leaves = await _repositoryAccessor.HRMS_Att_Use_Monthly_Leave.FindAll(x => x.Factory == factory && x.Leave_Type == "2", true).ToListAsync();
            DateTime maxEffectiveMonth = monthly_Leaves.Where(x => x.Effective_Month <= endDate.ToDateTime()).Max(x => x.Effective_Month);
            var result = monthly_Leaves.Where(x => x.Effective_Month == maxEffectiveMonth)
                        .OrderBy(x => x.Seq)
                        .Select(x => x.Code)
                        .Distinct()
                        .ToList();

            return result;
        }

        private async Task<WorkingHoursHeaderMultipleLanguage> GetLeaveCodeAllowancesHeader(string factory, string endDate)
        {
            var result = await GetLeaveCodeAllowances(factory, endDate);
            var headersEN = await GetLeaveCodeAllowancesLanguage(result, "en");
            var headersTW = await GetLeaveCodeAllowancesLanguage(result, "tw");
            return new WorkingHoursHeaderMultipleLanguage()
            {
                HeadersEN = headersEN.Select(x => $"{x.Code} - {x.CodeName}").ToList(),
                HeadersTW = headersTW.Select(x => $"{x.Code} - {x.CodeName}").ToList(),
            };
        }

        private async Task<List<CodeDto>> GetLeaveCodeAllowancesLanguage(List<string> monthly_Leaves, string language)
        {
            var basicCodes = await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Allowance && x.IsActive)
                                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Allowance && x.Language_Code.ToLower() == language.ToLower()),
                                    x => x.Code,
                                    z => z.Code,
                                    (x, z) => new { x, z })
                                .SelectMany(x => x.z.DefaultIfEmpty(), (x, z) => new { x.x, z })
                                .Select(x => new CodeDto
                                {
                                    Code = x.x.Code,
                                    CodeName = x.z.Code_Name ?? x.x.Code_Name,
                                    Char1 = x.x.Char1,
                                    Char2 = x.x.Char2
                                }).ToListAsync();

            var data = monthly_Leaves
                        .GroupJoin(basicCodes,
                            x => x,
                            z => z.Code,
                        (x, z) => new { x, z })
                        .SelectMany(x => x.z.DefaultIfEmpty(),
                        (x, z) => new { x.x, z })
                        .Select(x => new CodeDto
                        {
                            Code = x.x,
                            CodeName = x.z.CodeName,
                            Char1 = x.z.Char1,
                            Char2 = x.z.Char2
                        })
                        .ToList();
            return data;
        }
        #endregion

        #region Total-Excel
        public async Task<int> GetTotal(WorkingHoursReportParam param)
        {
            var data = await GetData(param);
            return data.Count;
        }

        public async Task<OperationResult> DownloadExcel(WorkingHoursReportParam param, string userName)
        {
            var leaveCodesTw = await GetCodeList("tw", param.Factory, param.Date_To, BasicCodeTypeConstant.Leave);
            var leaveCodesEn = await GetCodeList("en", param.Factory, param.Date_To, BasicCodeTypeConstant.Leave);

            var leaveCodes = new List<WorkingHoursHeader>{
                new (leaveCodesTw.Select(x => x.DisplayName).ToList(), 5),
                new (leaveCodesEn.Select(x => x.DisplayName).ToList(), 6)
            };

            var allowanceCodeData = await GetLeaveCodeAllowancesHeader(param.Factory, param.Date_To);

            var allowanceCodes = new List<WorkingHoursHeader>{
                new(allowanceCodeData.HeadersTW, 5),
                new(allowanceCodeData.HeadersEN, 6)
            };

            var data = await GetData(param);
            var factory = await GetListFactory(param.Lang, userName);
            if (!data.Any()) return new OperationResult(false, "No Data");

            var dataCells = new List<SDCores.Cell>()
            {
                new("A1", param.Lang == "en" ? "5.2.2 Working Hours Report" : "5.2.2 工時統計表"),
                new("A2", param.Lang == "en" ? "Factory" : "廠別"),
                new("A4", param.Lang == "en" ? "Print By" : "列印人員"),
                new("B2", factory.FirstOrDefault(x => x.Key == param.Factory).Value),
                new("B4", userName),
                new("C4", param.Lang == "en" ? "Print Date" : "列印日期"),
                new("D2", param.Lang == "en" ? "Date" : "資料日期"),
                new("D4", DateTimeOffset.Now.LocalDateTime.ToString("yyyy/MM/dd HH:mm:ss")),
                new("E2", $"{param.Date_From} ~ {param.Date_To}"),
                new("I2", param.Lang == "en" ? "Salary Work Days" : "應上班天數"),
                new("J2", param.Salary_WorkDays),
            };
            int columnStartIndex = 8;
            int dataRowStartIndex = 7;
            int allowanceColumnStartIndex = 8 + leaveCodesEn.Count;
            Style common = new CellsFactory().CreateStyle();
            AsposeUtility.SetAllBorders(common);
            // Header Leave Code

            GetHeaders(dataCells, leaveCodes, columnStartIndex);
            GetHeaders(dataCells, allowanceCodes, allowanceColumnStartIndex, true);

            for (int i = 0; i < data.Count; i++)
            {
                if (leaveCodes.Any())
                {
                    int dataColumnIndex = columnStartIndex;
                    foreach (var days in data[i].Leave_Days)
                    {
                        dataCells.Add(new(i + dataRowStartIndex, dataColumnIndex, days, common));
                        dataColumnIndex++;
                    }
                }
                if (allowanceCodes.Any())
                {
                    int dataColumnIndex = allowanceColumnStartIndex;
                    foreach (var days in data[i].Allowance_Days)
                    {
                        dataCells.Add(new(i + dataRowStartIndex, dataColumnIndex, days, common));
                        dataColumnIndex++;
                    }
                }
            }
            var tables = new List<Table>() { new("result", data) };
            ExcelResult excelResult = ExcelUtility.DownloadExcel(
                tables, dataCells,
                "Resources\\Template\\AttendanceMaintenance\\5_2_2_WorkingHoursReport\\Download.xlsx"
            );
            return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
        }


        /// <summary>
        /// Generate Header Column Follow language
        /// </summary>
        /// <param name="dataCells">Dữ liệu Cột đang có</param>
        /// <param name="headers"> Danh sách Header cần thêm </param>
        /// <param name="startColumn"> Số cột bắt đầu </param>
        /// <param name="isAllowance"> Mặc định: LeaveCode, True: Allowance </param>
        /// <returns></returns>
        private static List<SDCores.Cell> GetHeaders(List<SDCores.Cell> dataCells, List<WorkingHoursHeader> headers, int startColumn, bool isAllowance = false)
        {
            Style style = new CellsFactory().CreateStyle();
            style.Pattern = BackgroundType.Solid;
            style.ForegroundColor = isAllowance ? Color.FromArgb(226, 239, 218) : Color.FromArgb(255, 242, 204);
            AsposeUtility.SetAllBorders(style);

            foreach (var data in headers)
            {
                // Số cột bắt đầu
                int currentColumn = startColumn;
                if (data.Headers.Any())
                {
                    foreach (var header in data.Headers)
                    {
                        dataCells.Add(new(data.RowIndex, currentColumn, header, style));
                        currentColumn++;
                    }
                }
            }
            return dataCells;
        }
        #endregion
    }
}