using System.Globalization;
using AgileObjects.AgileMapper.Extensions;
using API.Data;
using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using API.Helper.Constant;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.AttendanceMaintenance
{
    public class S_5_2_17_MonthlyWorkingHoursLeaveHoursReport : BaseServices, I_5_2_17_MonthlyWorkingHoursLeaveHoursReport
    {
        public S_5_2_17_MonthlyWorkingHoursLeaveHoursReport(DBContext dbContext) : base(dbContext)
        {
        }

        #region GetListPermissionGroup
        public async Task<List<KeyValuePair<string, string>>> GetListPermissionGroup(string factory, string language) => await Query_BasicCode_PermissionGroup(factory, language);
        #endregion

        #region Queryt_Factory_AddList
        public async Task<List<KeyValuePair<string, string>>> Queryt_Factory_AddList(string userName, string language)
        {
            var factoriesByAccount = await Queryt_Factory_AddList(userName);
            var factories = await GetDataBasicCode(BasicCodeTypeConstant.Factory, language);
            return factories.IntersectBy(factoriesByAccount, x => x.Key).ToList();
        }
        #endregion

        public async Task<List<KeyValuePair<string, string>>> GetFactories(List<string> roleList, string language) => await Query_Factory_AddList(roleList, language);

        #region ExportExcel
        public async Task<OperationResult> ExportExcel(MonthlyWorkingHoursLeaveHoursReportParam param)
        {
            var data = await GetDataDownload(param);
            if (!data.Any())
                return new OperationResult(false, "No data");
            // xử lí report data 
            var dataTables = new List<Table>() { new("result", data) };
            var yearMonth = DateTime.Parse(param.YearMonth).ToString("yyyy/MM");
            // Thông tin print [Factory, PrintBy,  PrintDay]
            var permission = await GetListPermissionGroup(param.Factory, param.Language);
            var listPermission = permission.Where(x => param.PermissionGroup.Contains(x.Key)).Select(x => x.Value);
            var dataCells = new List<Cell>(){
                new("B1", param.Factory),
                new("D1", yearMonth),
                new("F1", string.Join(" / ", listPermission)),
                new("B2", param.UserName),
                new("D2", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"))
            };
            bool isLocal = param.option == "WorkingHours";
            ConfigDownload config = new() { IsAutoFitColumn = true };

            string templatePath = isLocal
                   ? "Resources\\Template\\AttendanceMaintenance\\5_2_17_MonthlyWorkingHoursLeaveHoursReport\\WorkingHours.xlsx"
                   : "Resources\\Template\\AttendanceMaintenance\\5_2_17_MonthlyWorkingHoursLeaveHoursReport\\LeaveHours.xlsx";
            ExcelResult excelResult = ExcelUtility.DownloadExcel(dataTables, dataCells, templatePath, config);
            return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
        }
        #endregion

        #region GetTotalRows
        public async Task<int> GetTotalRows(MonthlyWorkingHoursLeaveHoursReportParam param)
        {
            var data = await GetDataDownload(param);
            return data.Count;
        }
        #endregion

        #region GetDataDownload
        private async Task<List<MonthlyWorkingHoursLeaveHoursReportDto>> GetDataDownload(MonthlyWorkingHoursLeaveHoursReportParam param)
        {

            if (string.IsNullOrWhiteSpace(param.Factory)
             || !param.PermissionGroup.Any()
             || string.IsNullOrWhiteSpace(param.YearMonth)
             || !DateTime.TryParseExact(param.YearMonth, "yyyy/MM", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime yearMonthValue))
                return new();
            DateTime firstDate = yearMonthValue;
            DateTime lastDate = yearMonthValue.AddMonths(1).AddDays(-1);
            var predHEP = PredicateBuilder.New<HRMS_Emp_Personal>(x => x.Resign_Date == null || x.Resign_Date.HasValue && x.Resign_Date.Value.Date >= firstDate.Date);
            #region WorkingHours
            if (param.option == "WorkingHours")
            {
                var HEP = _repositoryAccessor.HRMS_Emp_Personal.FindAll(predHEP);
                var HAM = _repositoryAccessor.HRMS_Att_Monthly.FindAll(x => x.Factory == param.Factory && x.Att_Month == firstDate && param.PermissionGroup.Contains(x.Permission_Group));
                var HAWS = _repositoryAccessor.HRMS_Att_Work_Shift.FindAll(x => x.Factory == param.Factory);
                var HACR = _repositoryAccessor.HRMS_Att_Change_Record.FindAll(x => x.Factory == param.Factory && x.Att_Date >= firstDate.Date && x.Att_Date <= lastDate.Date);
                var HALM = _repositoryAccessor.HRMS_Att_Leave_Maintain.FindAll(x => x.Factory == param.Factory);
                var HAOM = _repositoryAccessor.HRMS_Att_Overtime_Maintain.FindAll(x => x.Factory == param.Factory && x.Overtime_Date >= firstDate.Date && x.Overtime_Date <= lastDate.Date);

                var data = await HAM
                    .Join(HEP,
                        x => new { x.Factory, x.USER_GUID },
                        y => new { y.Factory, y.USER_GUID },
                        (x, y) => new { attMonth = x, empPer = y })
                    .Select(x => new
                    {
                        x.attMonth.Att_Month,
                        x.attMonth.Employee_ID,
                    }).Distinct()
                    .GroupJoin(HAOM,
                        x => x.Employee_ID,
                        y => y.Employee_ID,
                        (x, y) => new { emp_cur = x, HAOM = y })
                    .SelectMany(x => x.HAOM.DefaultIfEmpty(),
                        (x, y) => new { x.emp_cur, HAOM = y })
                    .GroupJoin(HACR,
                        x => x.emp_cur.Employee_ID,
                        y => y.Employee_ID,
                        (x, y) => new { x.emp_cur, x.HAOM, HACR = y })
                    .SelectMany(x => x.HACR.DefaultIfEmpty(),
                        (x, y) => new { x.emp_cur, x.HAOM, HACR = y })
                    .GroupBy(x => x.emp_cur)
                    .Select(x => new
                    {
                        x.Key.Att_Month,
                        x.Key.Employee_ID,
                        HAOMs = x.Select(y => y.HAOM).Distinct(),
                        HACRs = x.Select(y => y.HACR).Distinct()
                    }).ToListAsync();
                var total = data.Select(emp_cur => new
                {
                    SUM_Overtime_Hours = emp_cur.HAOMs.Sum(y => y.Overtime_Hours + y.Night_Overtime_Hours + y.Training_Hours),
                    SUM_Work_Hours = emp_cur.HACRs.Select(dat_cur =>
                    {
                        var workHours = HAWS.FirstOrDefault(y => y.Work_Shift_Type == dat_cur.Work_Shift_Type && y.Week == ((int)dat_cur.Att_Date.DayOfWeek).ToString())?.Work_Hours ?? 0;
                        var leaveDays = HALM.Where(y => y.Employee_ID == emp_cur.Employee_ID && y.Leave_Date.Date == dat_cur.Att_Date.Date).Sum(y => y.Days);
                        return leaveDays > 0 ? workHours - (leaveDays * workHours) : workHours;
                    }).Sum()
                }).ToList();
                var result = new MonthlyWorkingHoursLeaveHoursReportDto()
                {
                    YearMonth = param.YearMonth,
                    GS14 = total.Count,
                    GS17 = total.Sum(x => x.SUM_Work_Hours) + total.Sum(x => x.SUM_Overtime_Hours),
                    GS26 = !HAM.Any() ? 0 : HAM.Max(x => x.Actual_Days),
                    GS27 = null
                };
                return new List<MonthlyWorkingHoursLeaveHoursReportDto> { result };
            }
            #endregion
            #region Leave Hours
            else
            {
                List<string> listLeaveCodes = new() { "C0", "B0", "H0", "J1" };
                predHEP.And(x => param.PermissionGroup.Contains(x.Permission_Group));
                var HEP = _repositoryAccessor.HRMS_Emp_Personal.FindAll(predHEP);
                var HAMD = _repositoryAccessor.HRMS_Att_Monthly_Detail.FindAll(x => x.Factory == param.Factory && x.Att_Month.Date == firstDate.Date && x.Leave_Type == "1" && listLeaveCodes.Contains(x.Leave_Code));
                var leaveData = await HAMD
                    .Join(HEP,
                        x => new { x.Factory, x.USER_GUID },
                        y => new { y.Factory, y.USER_GUID },
                        (x, y) => new { HAMD = x, HEP = y })
                    .GroupBy(x => new { x.HAMD.Att_Month, x.HAMD.Leave_Code })
                    .Select(x => new
                    {
                        x.Key.Att_Month,
                        x.Key.Leave_Code,
                        Sum_days = x.Sum(x => x.HAMD.Days)
                    }).ToListAsync();

                decimal absentDays = 0;
                decimal sickDays = 0;
                decimal lockoutDays = 0;

                foreach (var item in leaveData)
                {
                    switch (item.Leave_Code)
                    {
                        case "C0":
                            absentDays = item.Sum_days;
                            break;
                        case "B0":
                            sickDays += item.Sum_days;
                            break;
                        case "H0":
                            sickDays += item.Sum_days;
                            break;
                        case "J1":
                            lockoutDays = item.Sum_days;
                            break;
                    }
                }
                var result = new MonthlyWorkingHoursLeaveHoursReportDto()
                {
                    YearMonth = firstDate.Date.ToString("yyyy/MM"),
                    AR1 = absentDays,
                    AR2 = sickDays,
                    AR3 = lockoutDays,
                };
                return new List<MonthlyWorkingHoursLeaveHoursReportDto> { result };
                #endregion
            }
        }
        #endregion
    }
}