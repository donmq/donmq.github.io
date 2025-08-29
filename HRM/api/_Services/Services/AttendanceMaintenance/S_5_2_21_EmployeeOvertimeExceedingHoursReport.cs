using API.Data;
using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using API.Helper.Constant;
using API.Models;
using Aspose.Cells;
using LinqKit;

namespace API._Services.Services.AttendanceMaintenance
{
    public class S_5_2_21_EmployeeOvertimeExceedingHoursReport : BaseServices, I_5_2_21_EmployeeOvertimeExceedingHoursReport
    {
        private static readonly string rootPath = Directory.GetCurrentDirectory();

        public S_5_2_21_EmployeeOvertimeExceedingHoursReport(DBContext dbContext) : base(dbContext)
        {
        }

        #region Download
        public async Task<OperationResult> Download(EmployeeOvertimeExceedingHoursReportParam param)
        {
            var data = await GetData(param);

            if (!data.Any())
                return new OperationResult(false, "No data!");

            var result = ExportExcel(param, data);
            return result;
        }

        private static OperationResult ExportExcel(EmployeeOvertimeExceedingHoursReportParam param, List<EmployeeOvertimeExceedingHoursReportDto> data)
        {
            var start_Date = DateTime.Parse(param.Start_Date);
            var end_Date = DateTime.Parse(param.End_Date);

            string path = Path.Combine(
                rootPath,
                $"Resources\\Template\\AttendanceMaintenance\\5_2_21_EmployeeOvertimeExceedingHoursReport\\{param.Statistical_Method}.xlsx"
            );
            WorkbookDesigner designer = new() { Workbook = new Workbook(path) };
            Worksheet ws = designer.Workbook.Worksheets[0];

            designer.SetDataSource("result", data);
            designer.Process();

            ws.Cells["C2"].PutValue(param.Factory);
            ws.Cells["C4"].PutValue(param.UserName);
            ws.Cells["E4"].PutValue(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            ws.Cells["L2"].PutValue(param.Abnormal_Overtime_Hours);

            switch (param.Statistical_Method)
            {
                case "Weekly":
                    {
                        ws.Cells["F2"].PutValue($"{start_Date:yyyy/MM/dd}~{end_Date:yyyy/MM/dd}");
                        ws.Cells["I2"].PutValue($"每週 Weekly");
                        break;
                    }
                case "Monthly":
                    {
                        ws.Cells["A7"].HtmlString = $"人員每月加班時數超出 <Font Style='COLOR: #ff0000'>{param.Abnormal_Overtime_Hours}</Font> 小時程式禁止輸入統計 <Br>Program Prohibits Input Of Employee Monthly Overtime Hours Exceeding <Font Style='COLOR: #ff0000'>{param.Abnormal_Overtime_Hours}</Font> Hours";
                        ws.Cells["F2"].PutValue($"{start_Date:yyyy/MM/dd}~{end_Date:yyyy/MM/dd}");
                        ws.Cells["I2"].PutValue($"每月 Monthly");
                        break;
                    }
                case "Annual":
                    {
                        ws.Cells["A7"].HtmlString = $"人員每年加班時數超出 <Font Style='COLOR: #ff0000'>{param.Abnormal_Overtime_Hours}</Font> 小時程式禁止輸入統計 <Br>Program Prohibits Input Of Employee Annual Overtime Hours Exceeding <Font Style='COLOR: #ff0000'>{param.Abnormal_Overtime_Hours}</Font> Hours";
                        ws.Cells["F2"].PutValue($"{start_Date:yyyy}");
                        ws.Cells["I2"].PutValue($"年度 Annual");
                        break;
                    }
                case "DateRange":
                    {
                        ws.Cells["A7"].HtmlString = $"人員日期起迄加班時數超出 <Font Style='COLOR: #ff0000'>{param.Abnormal_Overtime_Hours}</Font> 小時程式禁止輸入統計 <Br>Program Prohibits Input Of Employee Overtime Hours Exceeding <Font Style='COLOR: #ff0000'>{param.Abnormal_Overtime_Hours}</Font> Hours Within The Specified Date Range";
                        ws.Cells["F2"].PutValue($"{start_Date:yyyy/MM/dd}~{end_Date:yyyy/MM/dd}");
                        ws.Cells["I2"].PutValue($"日期起迄 Date Range");
                        break;
                    }
                case "Details":
                    {
                        ws.Cells["A7"].HtmlString = $"人員日期起迄加班時數超出 <Font Style='COLOR: #ff0000'>{param.Abnormal_Overtime_Hours}</Font> 小時程式禁止輸入統計 <Br>Program Prohibits Input Of Employee Overtime Hours Exceeding <Font Style='COLOR: #ff0000'>{param.Abnormal_Overtime_Hours}</Font> Hours Within The Specified Date Range";
                        ws.Cells["F2"].PutValue($"{start_Date:yyyy/MM/dd}~{end_Date:yyyy/MM/dd}");
                        ws.Cells["I2"].PutValue($"明細 Details");
                        break;
                    }

                default:
                    return new OperationResult(false, "Not found Statistical method");
            }

            MemoryStream stream = new();
            designer.Workbook.Save(stream, SaveFormat.Xlsx);
            return new OperationResult(true, new { TotalRows = data.Count, Excel = stream.ToArray() });
        }
        #endregion

        #region Get Total Rows
        public async Task<int> GetTotalRows(EmployeeOvertimeExceedingHoursReportParam param)
        {
            var data = await GetData(param);
            return data.Count;
        }
        #endregion

        #region Get Data
        private async Task<List<EmployeeOvertimeExceedingHoursReportDto>> GetData(EmployeeOvertimeExceedingHoursReportParam param)
        {
            var start_Date = DateTime.Parse(param.Start_Date);
            var end_Date = DateTime.Parse(param.End_Date);
            var predPersonal = PredicateBuilder.New<HRMS_Emp_Personal>(x => x.Factory == param.Factory || x.Assigned_Factory == param.Factory && x.Resign_Date.HasValue == false);

            var HEP = _repositoryAccessor.HRMS_Emp_Personal.FindAll(predPersonal, true).ToList();
            var HAWS = _repositoryAccessor.HRMS_Att_Work_Shift.FindAll(true).ToList();
            var HALM = _repositoryAccessor.HRMS_Att_Leave_Maintain.FindAll(x => x.Leave_code != "D0", true).ToList();
            var HAOM = _repositoryAccessor.HRMS_Att_Overtime_Maintain
                .FindAll(x => x.Overtime_Date >= start_Date
                           && x.Overtime_Date <= end_Date, true).ToList();

            var HACR = _repositoryAccessor.HRMS_Att_Change_Record
                .FindAll(x => x.Factory == param.Factory
                           && x.Att_Date >= start_Date
                           && x.Att_Date <= end_Date, true).ToList();

            var PositionTitle = await GetDataBasicCode(BasicCodeTypeConstant.JobTitle, param.Language);
            var WorkType = await GetDataBasicCode(BasicCodeTypeConstant.WorkType, param.Language);

            var HOD = _repositoryAccessor.HRMS_Org_Department.FindAll(true).ToList();
            var HODL = _repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == param.Language.ToLower(), true).ToList();
            var Department = HOD
                .GroupJoin(HODL,
                    HOD => new { HOD.Division, HOD.Factory, HOD.Department_Code },
                    HODL => new { HODL.Division, HODL.Factory, HODL.Department_Code },
                    (HOD, HODL) => new { HOD, HODL })
                .SelectMany(x => x.HODL.DefaultIfEmpty(),
                    (x, HODL) => new { x.HOD, HODL })
                .Select(x => new
                {
                    x.HOD.Factory,
                    x.HOD.Division,
                    x.HOD.Department_Code,
                    Department_Name = x.HODL != null ? x.HODL.Name : x.HOD.Department_Name
                }).ToList();

            var Employee_Work_Hours = HACR
                .GroupJoin(HAWS,
                    HACR => new { HACR.Factory, HACR.Work_Shift_Type, Week = ((int)HACR.Att_Date.DayOfWeek).ToString() },
                    HAWS => new { HAWS.Factory, HAWS.Work_Shift_Type, HAWS.Week },
                    (HACR, HAWS) => new { HACR, HAWS })
                .SelectMany(x => x.HAWS.DefaultIfEmpty(),
                    (x, HAWS) => new { x.HACR, HAWS })
                .ToList();

            var Employee_Overtime = HEP.GroupJoin(HAOM,
                    HEP => new { HEP.Factory, HEP.Employee_ID },
                    HAOM => new { HAOM.Factory, HAOM.Employee_ID },
                    (HEP, HAOM) => new { HEP, HAOM })
                .SelectMany(x => x.HAOM.DefaultIfEmpty(),
                    (x, HAOM) => new { x.HEP, HAOM })
                .ToList();
            List<string> filterStatus = new() { "A", "S" };
            List<EmployeeOvertimeExceedingHoursReportDto> result = new();
            HEP.ForEach(item =>
            {
                var data = new EmployeeOvertimeExceedingHoursReportDto
                {
                    Employee_ID = item.Employee_ID,
                    Local_Full_Name = item.Local_Full_Name,
                    OnBoard_Date = item.Onboard_Date,
                    Work_Type = WorkType.FirstOrDefault(x => x.Key == item.Work_Type).Value,
                    Position_Title = PositionTitle.FirstOrDefault(x => x.Key == item.Position_Title).Value,
                    Department_Code = !string.IsNullOrWhiteSpace(item.Employment_Status) && filterStatus.Contains(item.Employment_Status)
                        ? item.Assigned_Department
                        : item.Department,
                    Department_Name = !string.IsNullOrWhiteSpace(item.Employment_Status) && filterStatus.Contains(item.Employment_Status)
                        ? Department.FirstOrDefault(x => x.Factory == item.Assigned_Factory
                            && x.Division == item.Assigned_Division
                            && x.Department_Code == item.Assigned_Department)?.Department_Name
                        : Department.FirstOrDefault(x => x.Factory == item.Factory
                            && x.Division == item.Division
                            && x.Department_Code == item.Department)?.Department_Name,
                };

                var Employee_HALM = HALM
                        .Where(x => x.Factory == item.Factory
                                 && x.Employee_ID == item.Employee_ID).ToList();

                var type = new List<string>() { "Weekly", "Monthly", "Annual" };
                if (type.Contains(param.Statistical_Method))
                {
                    var time1 = Employee_Work_Hours.Where(x => x.HACR.Factory == item.Factory
                                            && x.HACR.Employee_ID == item.Employee_ID
                                            && x.HACR.Att_Date >= start_Date
                                            && x.HACR.Att_Date <= end_Date)
                                        .Aggregate(0m, (result, value) => Calculation_Time1(value.HACR, Employee_HALM, value?.HAWS, result));

                    var time2 = Employee_Overtime
                        .Where(x => x.HEP.Factory == item.Factory
                                 && x.HEP.Employee_ID == item.Employee_ID
                                 && x.HAOM != null
                                 && x.HAOM?.Overtime_Date >= start_Date
                                 && x.HAOM?.Overtime_Date <= end_Date)
                        .Sum(x => (x.HAOM?.Overtime_Hours ?? 0) + (x.HAOM?.Night_Overtime_Hours ?? 0));

                    if (param.Statistical_Method == "Weekly")
                    {
                        if (time2 > param.Abnormal_Overtime_Hours || time1 + time2 > 60)
                        {
                            data.Weekly_Working_Hours = time1 + time2;
                            data.Overtime_Hours = time2;
                            result.Add(data);
                        }
                    }

                    if (param.Statistical_Method == "Monthly")
                    {
                        if (time2 > param.Abnormal_Overtime_Hours)
                        {
                            data.Overtime_Hours = time2;
                            result.Add(data);
                        }
                    }

                    if (param.Statistical_Method == "Annual")
                    {
                        if (time2 > param.Abnormal_Overtime_Hours)
                        {
                            data.Overtime_Hours = time2;
                            result.Add(data);
                        }
                    }
                }
                else
                {
                    List<Week_Working_Hours> week_Working_Hours = new();
                    var weeks = GetListWeek(start_Date, param.Statistical_Method == "DateRange" ? 3 : 5);

                    weeks.ForEach(week =>
                    {
                        var works = Employee_Work_Hours
                                .Where(x => x.HACR.Factory == item.Factory
                                         && x.HACR.Employee_ID == item.Employee_ID
                                         && x.HACR.Att_Date >= week.Start_Date
                                         && x.HACR.Att_Date <= week.End_Date).ToList();
                        var overtimes = Employee_Overtime
                                .Where(x => x.HEP.Factory == item.Factory
                                         && x.HEP.Employee_ID == item.Employee_ID
                                         && x.HAOM != null
                                         && x.HAOM?.Overtime_Date >= week.Start_Date
                                         && x.HAOM?.Overtime_Date <= week.End_Date).ToList();
                        Week_Working_Hours work_Hours = new()
                        {
                            Working_Hours = works.Aggregate(0m, (result, value) => Calculation_Time1(value.HACR, Employee_HALM, value?.HAWS, result)),
                            Overtime_Hours = overtimes.Sum(x => (x.HAOM?.Overtime_Hours ?? 0) + (x.HAOM?.Night_Overtime_Hours ?? 0))
                        };
                        week_Working_Hours.Add(work_Hours);
                    });

                    if (param.Statistical_Method == "DateRange")
                    {
                        if (week_Working_Hours.All(x => x.Working_Hours + x.Overtime_Hours > 60))
                        {
                            data.First_Week = week_Working_Hours[0].Working_Hours + week_Working_Hours[0].Overtime_Hours;
                            data.Second_Week = week_Working_Hours[1].Working_Hours + week_Working_Hours[1].Overtime_Hours;
                            data.Third_Week = week_Working_Hours[2].Working_Hours + week_Working_Hours[2].Overtime_Hours;
                            data.Period_Working_Hours = week_Working_Hours.Sum(x => x.Working_Hours + x.Overtime_Hours);
                            data.Overtime_Hours = week_Working_Hours.Sum(x => x.Overtime_Hours);
                            result.Add(data);
                        }
                    }

                    if (param.Statistical_Method == "Details")
                    {
                        data.First_Week = week_Working_Hours[0].Working_Hours + week_Working_Hours[0].Overtime_Hours;
                        data.Second_Week = week_Working_Hours[1].Working_Hours + week_Working_Hours[1].Overtime_Hours;
                        data.Third_Week = week_Working_Hours[2].Working_Hours + week_Working_Hours[2].Overtime_Hours;
                        data.Fourth_Week = week_Working_Hours[3].Working_Hours + week_Working_Hours[3].Overtime_Hours;
                        data.Fifth_Week = week_Working_Hours[4].Working_Hours + week_Working_Hours[4].Overtime_Hours;
                        data.Period_Working_Hours = week_Working_Hours.Sum(x => x.Working_Hours + x.Overtime_Hours);
                        data.Overtime_Hours = week_Working_Hours.Sum(x => x.Overtime_Hours);
                        result.Add(data);
                    }
                }

            });

            return result.OrderBy(x => x.Department_Code).ThenBy(x => x.Employee_ID).ToList();
        }
        #endregion

        private readonly Func<HRMS_Att_Change_Record, List<HRMS_Att_Leave_Maintain>, HRMS_Att_Work_Shift, decimal, decimal> Calculation_Time1 = (HACR, HALM, HAWS, time1) =>
        {
            var time_tmp = HAWS?.Work_Hours ?? 0;
            var w_dat1 = HALM != null ? HALM.Where(x => x.Leave_Date.Date == HACR.Att_Date.Date).Sum(x => x.Days) : 0;
            return time1 + (w_dat1 > 0 ? time_tmp - (w_dat1 * time_tmp) : time_tmp);
        };

        private static List<Week> GetListWeek(DateTime start_Date, int number_Week)
        {
            List<Week> weeks = new();
            var temp_Date = start_Date;
            var count = 1;
            while (count <= number_Week)
            {
                var week = new Week
                {
                    Start_Date = temp_Date,
                    End_Date = temp_Date.AddDays(6),
                };
                weeks.Add(week);
                count++;
                temp_Date = temp_Date.AddDays(7);
            }
            return weeks;
        }

        public async Task<List<KeyValuePair<string, string>>> GetListFactory(string language, List<string> roleList)
        {
            var factoriesByAccount = await Queryt_Factory_AddList(roleList);
            var factories = await GetDataBasicCode(BasicCodeTypeConstant.Factory, language);

            return factories.IntersectBy(factoriesByAccount, x => x.Key).ToList();
        }

        class Week
        {
            public DateTime Start_Date { get; set; }
            public DateTime End_Date { get; set; }
        }
        class Week_Working_Hours
        {
            public decimal Working_Hours { get; set; }
            public decimal Overtime_Hours { get; set; }
        }
    }
}