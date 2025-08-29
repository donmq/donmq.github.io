using API.Data;
using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using API.Helper.Constant;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.AttendanceMaintenance
{
    public class S_5_2_18_EmployeeOvertimeDataSheet : BaseServices, I_5_2_18_EmployeeOvertimeDataSheet
    {
        public S_5_2_18_EmployeeOvertimeDataSheet(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<OperationResult> ExportExcel(EmployeeOvertimeDataSheetParam param)
        {
            List<EmployeeOvertimeDataSheetDto> data = await GetData(param);
            if (!data.Any()) return new OperationResult(false, "No Data");

            // xử lí report data 
            var dataTables = new List<Table>() { new("result", data) };

            // Thông tin print [Factory, PrintBy,  PrintDay]
            var dataCells = new List<Cell>(){
                new("B1", param.Factory),
                new("E1", param.Department),
                new("H1", param.Employee_ID),
                new("K1", $"{param.Overtime_Date_Start} - {param.Overtime_Date_End}"),

                new("B3", param.UserName),
                new("D3", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"))
            };

            ConfigDownload config = new() { IsAutoFitColumn = true };
            ExcelResult excelResult = ExcelUtility.DownloadExcel(
                dataTables, 
                dataCells, 
                "Resources\\Template\\AttendanceMaintenance\\5_2_18_EmployeeOvertimeDataSheet\\Download.xlsx", 
                config
            );
            return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
        }

        public async Task<List<KeyValuePair<string, string>>> GetListDepartment(string factory, string language)
        {
            var data = await _repositoryAccessor.HRMS_Org_Department.FindAll(x => x.Factory == factory, true)
                        .Join(_repositoryAccessor.HRMS_Basic_Factory_Comparison.FindAll(x => x.Kind == BasicCodeTypeConstant.Division, true),
                            x => new { x.Division, x.Factory },
                            y => new { y.Division, y.Factory },
                            (x, y) => new { HOD = x, HBFC = y }
                        ).GroupJoin(_repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Factory == factory && x.Language_Code.ToLower() == language.ToLower(), true),
                            x => new { x.HOD.Division, x.HOD.Factory, x.HOD.Department_Code },
                            y => new { y.Division, y.Factory, y.Department_Code },
                            (x, y) => new { x.HOD, x.HBFC, HODL = y }
                        ).SelectMany(x => x.HODL.DefaultIfEmpty(),
                            (x, y) => new { x.HOD, x.HBFC, HODL = y }
                        ).Select(x => new KeyValuePair<string, string>(
                            x.HOD.Department_Code.Trim(),
                            $"{x.HOD.Department_Code.Trim()}-{(x.HODL != null ? x.HODL.Name.Trim() : x.HOD.Department_Name.Trim())}"
                        )).Distinct().ToListAsync();
            return data;
        }

        public async Task<List<KeyValuePair<string, string>>> GetListFactory(string language, string userName)
        {
            ExpressionStarter<HRMS_Basic_Code> predHBC = PredicateBuilder.New<HRMS_Basic_Code>(x => x.Type_Seq == BasicCodeTypeConstant.Factory);

            List<string> factorys = await Queryt_Factory_AddList(userName);
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

        public async Task<OperationResult> GetPagination(EmployeeOvertimeDataSheetParam param)
        {
            List<EmployeeOvertimeDataSheetDto> result = await GetData(param);
            return new OperationResult() { Data = result.Count };
        }

        #region GetData
        private async Task<List<EmployeeOvertimeDataSheetDto>> GetData(EmployeeOvertimeDataSheetParam param)
        {
            ExpressionStarter<HRMS_Att_Overtime_Maintain> pred_HAOM = PredicateBuilder.New<HRMS_Att_Overtime_Maintain>(true);
            ExpressionStarter<HRMS_Emp_Personal> pred_HEP = PredicateBuilder.New<HRMS_Emp_Personal>(true);
            List<EmployeeOvertimeDataSheetDto> employeeOvertimeReport = new();

            if (!string.IsNullOrWhiteSpace(param.Factory))
                pred_HAOM.And(x => x.Factory == param.Factory);

            if (!string.IsNullOrWhiteSpace(param.Department))
                pred_HEP.And(x => x.Department == param.Department || x.Assigned_Department == param.Department);

            if (!string.IsNullOrWhiteSpace(param.Employee_ID))
                pred_HAOM.And(x => x.Employee_ID.Contains(param.Employee_ID.Trim()));

            pred_HAOM.And(x => x.Overtime_Date >= Convert.ToDateTime(param.Overtime_Date_Start) && x.Overtime_Date <= Convert.ToDateTime(param.Overtime_Date_End));

            // Fetch data sequentially
            List<HRMS_Att_Overtime_Maintain> overtimeMaintainData = await _repositoryAccessor.HRMS_Att_Overtime_Maintain.FindAll(pred_HAOM, true).ToListAsync();
            List<HRMS_Emp_Personal> personalData = _repositoryAccessor.HRMS_Emp_Personal.FindAll(pred_HEP, true).ToList();
            var changeRecordData = _repositoryAccessor.HRMS_Att_Change_Record.FindAll(c => overtimeMaintainData.Select(o => o.USER_GUID).Contains(c.USER_GUID), true).ToHashSet();
            var departmentQuery = _repositoryAccessor.HRMS_Org_Department.FindAll(true).ToHashSet();
            var departmentLanguage = _repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == param.Language.ToLower(), true).ToHashSet();
            var basicCode = _repositoryAccessor.HRMS_Basic_Code.FindAll(true).ToHashSet();
            var basicLanguage = _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == param.Language.ToLower(), true).ToHashSet();
            if (!personalData.Any()) return employeeOvertimeReport;

            var Query_Department_Report = departmentQuery
                    .GroupJoin(departmentLanguage,
                        x => new { x.Division, x.Factory, x.Department_Code },
                        y => new { y.Division, y.Factory, y.Department_Code },
                        (x, y) => new { HRMS_Org_Department = x, HRMS_Org_Department_Language = y })
                    .SelectMany(x => x.HRMS_Org_Department_Language.DefaultIfEmpty(),
                        (x, y) => new { x.HRMS_Org_Department, HRMS_Org_Department_Language = y })
                    .Select(x => new
                    {
                        x.HRMS_Org_Department.Division,
                        x.HRMS_Org_Department.Factory,
                        x.HRMS_Org_Department.Department_Code,
                        Department_Name = $"{(x.HRMS_Org_Department_Language != null ? x.HRMS_Org_Department_Language.Name : x.HRMS_Org_Department.Department_Name)}"
                    }).ToList();
            var codeLang = basicCode
                .GroupJoin(basicLanguage,
                    x => new { x.Type_Seq, x.Code },
                    y => new { y.Type_Seq, y.Code },
                    (x, y) => new { Code = x, Language = y })
                .SelectMany(x => x.Language.DefaultIfEmpty(),
                    (x, y) => new { x.Code, Language = y })
                .Select(x => new
                {
                    x.Code.Code,
                    x.Code.Type_Seq,
                    Code_Name = x.Language != null ? x.Language.Code_Name : x.Code.Code_Name
                })
                .Distinct().ToList();
            // Join data in memory
            var result = overtimeMaintainData
                .Join(personalData,
                    x => x.USER_GUID,
                    y => y.USER_GUID,
                    (x, y) => new { HAOM = x, HEP = y })
                .Select(personal => new
                {
                    personal.HAOM,
                    personal.HEP,
                    Division = personal.HEP.Employment_Status == null ? personal.HEP.Division : personal.HEP.Assigned_Division,
                    Factory = personal.HEP.Employment_Status == null ? personal.HEP.Factory : personal.HEP.Assigned_Factory,
                    Department = personal.HEP.Employment_Status == null ? personal.HEP.Department : personal.HEP.Assigned_Department
                })
                .GroupJoin(departmentQuery,
                    personal => new { personal.Division, personal.Factory, personal.Department },
                    department => new { department.Division, department.Factory, Department = department.Department_Code },
                    (x, department) => new { x.HAOM, x.HEP, Department = department })
                .SelectMany(x => x.Department.DefaultIfEmpty(),
                    (x, department) => new { x.HAOM, x.HEP, Department = department });

            foreach (var item in result)
            {
                HRMS_Att_Overtime_Maintain overtime_Maintain = item.HAOM;
                HRMS_Emp_Personal personal = item.HEP;

                decimal? workHours = await CalculateWorkHours(
                                    overtime_Maintain.Holiday,
                                    overtime_Maintain.Factory,
                                    overtime_Maintain.Employee_ID,
                                    overtime_Maintain.Work_Shift_Type,
                                    overtime_Maintain.Overtime_Date,
                                    personal.Swipe_Card_Option
                                );
                HRMS_Att_Change_Record firstRecord = changeRecordData
                    .FirstOrDefault(record => record.Factory == overtime_Maintain.Factory && record.USER_GUID == overtime_Maintain.USER_GUID && record.Att_Date.Date == overtime_Maintain.Overtime_Date);

                string clockIn = firstRecord?.Clock_In != null ? $"{firstRecord.Clock_In.Substring(0, 2)}:{firstRecord.Clock_In.Substring(2, 2)}" : "";
                string clockOut = firstRecord != null ? CalculateClockOut(firstRecord) : "";
                clockOut = !string.IsNullOrEmpty(clockOut) ? $"{clockOut.Substring(0, 2)}:{clockOut.Substring(2, 2)}" : "";

                employeeOvertimeReport.Add(new EmployeeOvertimeDataSheetDto
                {
                    USER_GUID = personal.USER_GUID,
                    Factory = overtime_Maintain.Factory,
                    Department_Code = personal.Employment_Status is null ? personal.Department : (personal.Employment_Status == "A" || personal.Employment_Status == "S") ? personal.Assigned_Department : "",
                    Department_Name = personal.Employment_Status is null ? Query_Department_Report.FirstOrDefault(y => y.Division == personal.Division
                                            && y.Factory == personal.Factory
                                            && y.Department_Code == personal.Department)?.Department_Name : (personal.Employment_Status == "A" || personal.Employment_Status == "S") ?
                                            Query_Department_Report.FirstOrDefault(y => y.Division == personal.Assigned_Division
                                            && y.Factory == personal.Assigned_Factory
                                            && y.Department_Code == personal.Assigned_Department)?.Department_Name : "",
                    Employee_ID = overtime_Maintain.Employee_ID,
                    Local_Full_Name = personal.Local_Full_Name,
                    Position_Title = $"{item.HEP.Position_Title} - {codeLang.FirstOrDefault(y => y.Code == item.HEP.Position_Title && y.Type_Seq == "3")?.Code_Name ?? string.Empty}",
                    Work_Type = $"{item.HEP.Work_Type} - {codeLang.FirstOrDefault(y => y.Code == item.HEP.Work_Type && y.Type_Seq == "5")?.Code_Name ?? string.Empty}",
                    Overtime_Date = overtime_Maintain.Overtime_Date.ToString("yyyy/MM/dd"),
                    Work_Shift_Type = overtime_Maintain.Work_Shift_Type,
                    Clock_In = clockIn,
                    Clock_Out = clockOut,
                    Overtime_Hour = overtime_Maintain.Overtime_Hours,
                    Night_Hours = overtime_Maintain.Night_Hours,
                    Night_Overtime_Hours = overtime_Maintain.Night_Overtime_Hours,
                    Training_Hours = overtime_Maintain.Training_Hours,
                    Holiday = overtime_Maintain.Holiday,
                    Work_Hours = workHours,
                    Last_Clock_In_Time = clockOut,
                });
            }

            return employeeOvertimeReport
                .OrderBy(x => x.Department_Code)
                .ThenBy(x => x.Employee_ID)
                .ThenBy(x => x.Overtime_Date)
                .Distinct()
                .ToList();
        }
        #endregion

        private static string CalculateClockOut(HRMS_Att_Change_Record record)
        {
            if (record == null)
                return string.Empty;

            if (record.Overtime_ClockOut == "0000")
            {
                if (record.Overtime_ClockIn == "0000")
                    return record.Clock_Out;
                return record.Overtime_ClockIn;
            }
            return record.Overtime_ClockOut;
        }

        private async Task<decimal?> CalculateWorkHours(string holiday, string factory, string employeeId, string workShiftType, DateTime overtimeDate, bool swipeCardOption)
        {
            decimal? totalWorkHours = null;
            decimal? time0 = null;
            decimal? time1 = null;
            DateTime startDate = new(overtimeDate.Year, overtimeDate.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);
            // SELECT  Att_Date FROM HRMS_Att_Change_Record WHERE Factory = @Factory AND Employee_ID =A.Employee_ID AND Att_Date =@InputDate) IS NOT NULL
            // 上班時數
            var data_Change_Record = _repositoryAccessor.HRMS_Att_Change_Record.FirstOrDefault(x => x.Employee_ID == employeeId && x.Att_Date == overtimeDate && x.Factory == factory);
            if (data_Change_Record is not null)
            {
                var data_Work_Shift = await _repositoryAccessor.HRMS_Att_Work_Shift.FirstOrDefaultAsync(x => x.Work_Shift_Type == data_Change_Record.Work_Shift_Type && x.Factory == factory && x.Week == ((int)overtimeDate.DayOfWeek).ToString());
                if (data_Work_Shift is not null)
                {
                    time0 = data_Work_Shift.Work_Hours;
                }
            }
            // 請假時數
            var data_Leave_Maintain = _repositoryAccessor.HRMS_Att_Leave_Maintain.FirstOrDefault(x => x.Factory == factory && x.Employee_ID == employeeId && x.Leave_Date == overtimeDate && x.Leave_code != "D0"); // Z
            if (data_Leave_Maintain is not null)
            {
                var data_Work_Shift = await _repositoryAccessor.HRMS_Att_Work_Shift.FirstOrDefaultAsync(x => x.Work_Shift_Type == data_Leave_Maintain.Work_Shift_Type && x.Factory == factory && x.Week == ((int)overtimeDate.DayOfWeek).ToString());
                if (data_Work_Shift is not null)
                {
                    time1 = data_Work_Shift.Work_Hours * data_Leave_Maintain.Days;
                }
            }
            if (time0 is null || time1 is null)
                return totalWorkHours;
            totalWorkHours = time0 - time1;
            return totalWorkHours;
        }
    }
}


