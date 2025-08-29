using API.Data;
using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using API.Helper.Constant;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.AttendanceMaintenance
{
    public class S_5_1_25_LoanedMonthAttendanceDataGeneration : BaseServices, I_5_1_25_LoanedMonthAttendanceDataGeneration
    {

        public S_5_1_25_LoanedMonthAttendanceDataGeneration(DBContext dbContext) : base(dbContext)
        {
        }

        #region Execute
        public async Task<OperationResult> Execute(LoanedDataGenerationDto dto)
        {
            return dto.Selected_Tab == "dataGeneration" ? await CallGenerate(dto) : await CallClose(dto);
        }
        private async Task<OperationResult> CallGenerate(LoanedDataGenerationDto dto)
        {

            var emps = await _repositoryAccessor.HRMS_Emp_Personal.FindAll(x => x.Factory == dto.Factory).ToListAsync();
            if (dto.Employee_ID_Start.Any(char.IsLetter))
                emps = emps.Where(x => x.Employee_ID.Any(char.IsLetter) && x.Employee_ID.CompareTo(dto.Employee_ID_Start) >= 0).ToList();
            else
                emps = emps.Where(x => x.Employee_ID.All(char.IsDigit) && x.Employee_ID.Length == dto.Employee_ID_Start.Length && x.Employee_ID.CompareTo(dto.Employee_ID_Start) >= 0).ToList();

            if (dto.Employee_ID_End.Any(char.IsLetter))
                emps = emps.Where(x => x.Employee_ID.Any(char.IsLetter) && x.Employee_ID.CompareTo(dto.Employee_ID_End) <= 0).ToList();
            else
                emps = emps.Where(x => x.Employee_ID.All(char.IsDigit) && x.Employee_ID.Length == dto.Employee_ID_End.Length && x.Employee_ID.CompareTo(dto.Employee_ID_End) <= 0).ToList();

            if (!emps.Any())
                return new OperationResult() { IsSuccess = false, Error = "No employee query" };

            DateTime now = DateTime.Now;
            List<HRMS_Att_Loaned_Monthly> loanedMonthlies = new();
            List<HRMS_Att_Loaned_Monthly_Detail> loanedMonthlyDetails = new();

            DateTime loanedDateStart = Convert.ToDateTime(dto.Loaned_Date_Start_Str);
            DateTime loanedDateEnd = Convert.ToDateTime(dto.Loaned_Date_End_Str);

            var attDates = await _repositoryAccessor.HRMS_Att_Calendar
                .FindAll(
                    x => x.Factory == dto.Factory &&
                    x.Att_Date >= loanedDateStart &&
                    x.Att_Date <= loanedDateEnd &&
                    (x.Type_Code == "C00" || x.Type_Code == "C05")
                ).Select(x => x.Att_Date)
                .Distinct().ToListAsync();

            var records = _repositoryAccessor.HRMS_Att_Change_Record
                .FindAll(
                    x => x.Factory == dto.Factory &&
                    x.Att_Date >= loanedDateStart &&
                    x.Att_Date <= loanedDateEnd
                );

            IQueryable<HRMS_Att_Use_Monthly_Leave> leaves = _repositoryAccessor.HRMS_Att_Use_Monthly_Leave
                .FindAll(x => x.Factory == dto.Factory);

            var effectiveMonths = await leaves
                .Where(x => x.Effective_Month <= Convert.ToDateTime(dto.Loaned_Year_Month_Str))
                .Select(x => x.Effective_Month)
                .ToListAsync();

            DateTime? effectiveMonth = effectiveMonths.Any() ? effectiveMonths.Max() : null;

            List<string> leaveCodes = new();
            List<MainSumHours> allowanceCodes = new();
            if (effectiveMonth.HasValue)
            {
                leaves = leaves
                    .Where(x => x.Factory == dto.Factory && x.Effective_Month == effectiveMonth);

                leaveCodes = await leaves.Where(x => x.Leave_Type == "1").Select(x => x.Code).ToListAsync();

                allowanceCodes = await leaves.Where(x => x.Leave_Type == "2")
                    .Join(_repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Allowance),
                        x => x.Code,
                        y => y.Code,
                        (leave, basic) => new MainSumHours() { Overtime_Code = leave.Code, Holiday_Code = basic.Char1, Overtime_Column_Name = basic.Char2 })
                    .ToListAsync();
            }

            foreach (var item in emps)
            {
                decimal actualDays = await QueryDetailSumDays(new()
                {
                    Factory = dto.Factory,
                    Att_Month = dto.Loaned_Year_Month_Str,
                    Employee_ID = item.Employee_ID,
                    Normal_Working_Days = dto.Normal_Working_Days
                });

                var department = string.IsNullOrEmpty(item.Employment_Status) ? item.Department
                                        : (item.Employment_Status == "A" || item.Employment_Status == "S") ? item.Assigned_Department
                                        : null;

                var recordByEmps = records.Where(x => x.Employee_ID == item.Employee_ID);

                int delayEarly = await QueryChangeRecordCount(recordByEmps, new() { "L0", "L1" });

                int noSwipCard = await QueryChangeRecordCount(recordByEmps, new() { "11" });

                int foodExpenses = await QueryFoodExpensesSum(recordByEmps);

                int nightEat = await QueryNightEatSum(attDates, recordByEmps);

                string resignStatus = GetResignStatus(item, loanedDateStart, loanedDateEnd);

                HRMS_Att_Loaned_Monthly loanedMonthly = new()
                {
                    Factory = dto.Factory,
                    USER_GUID = item.USER_GUID,
                    Employee_ID = item.Employee_ID,
                    Department = department,
                    Division = item.Division,
                    Permission_Group = item.Permission_Group,
                    Salary_Type = "10",
                    Att_Month = Convert.ToDateTime(dto.Loaned_Year_Month_Str),
                    Pass = false,
                    Salary_Days = dto.Normal_Working_Days,
                    Actual_Days = actualDays,
                    Delay_Early = delayEarly,
                    No_Swip_Card = noSwipCard,
                    Food_Expenses = foodExpenses,
                    Night_Eat_Times = nightEat,
                    Resign_Status = resignStatus,
                    Update_By = dto.UserName,
                    Update_Time = now
                };
                loanedMonthlies.Add(loanedMonthly);
                if (leaveCodes.Any())
                {
                    foreach (var code in leaveCodes)
                    {
                        decimal days = await QueryLeaveMaintainSumDays(new()
                        {
                            Factory = loanedMonthly.Factory,
                            Employee_ID = loanedMonthly.Employee_ID,
                            Leave_Date_Start = dto.Loaned_Date_Start_Str,
                            Leave_Date_End = dto.Loaned_Date_End_Str,
                            Leave_Code = code
                        });

                        loanedMonthlyDetails.Add(new()
                        {
                            Factory = loanedMonthly.Factory,
                            Division = loanedMonthly.Division,
                            Employee_ID = loanedMonthly.Employee_ID,
                            USER_GUID = loanedMonthly.USER_GUID,
                            Att_Month = loanedMonthly.Att_Month,
                            Leave_Type = "1",
                            Leave_Code = code,
                            Days = days,
                            Update_By = loanedMonthly.Update_By,
                            Update_Time = loanedMonthly.Update_Time
                        });
                    }
                }
                if (allowanceCodes.Any())
                {
                    foreach (var x in allowanceCodes)
                    {
                        decimal days = await QueryOvertimeMaintainSumHours(new()
                        {
                            Factory = loanedMonthly.Factory,
                            Employee_ID = loanedMonthly.Employee_ID,
                            Overtime_Date_Start = dto.Loaned_Date_Start_Str,
                            Overtime_Date_End = dto.Loaned_Date_End_Str,
                            Holiday_Code = x.Holiday_Code,
                            Overtime_Column_Name = x.Overtime_Column_Name
                        });

                        loanedMonthlyDetails.Add(new()
                        {
                            Factory = loanedMonthly.Factory,
                            Division = loanedMonthly.Division,
                            Employee_ID = loanedMonthly.Employee_ID,
                            USER_GUID = loanedMonthly.USER_GUID,
                            Att_Month = loanedMonthly.Att_Month,
                            Leave_Type = "2",
                            Leave_Code = x.Overtime_Code,
                            Days = days,
                            Update_By = loanedMonthly.Update_By,
                            Update_Time = loanedMonthly.Update_Time
                        });
                    }
                }
            }
            await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                _repositoryAccessor.HRMS_Att_Loaned_Monthly.AddMultiple(loanedMonthlies);
                _repositoryAccessor.HRMS_Att_Loaned_Monthly_Detail.AddMultiple(loanedMonthlyDetails);
                await _repositoryAccessor.Save();
                await _repositoryAccessor.CommitAsync();
                return new OperationResult() { IsSuccess = true };
            }
            catch (Exception ex)
            {
                await _repositoryAccessor.RollbackAsync();
                return new OperationResult() { IsSuccess = false, Error = ex.InnerException.Message };
            }
        }
        private async Task<OperationResult> CallClose(LoanedDataGenerationDto dto)
        {
            var HALMs = await _repositoryAccessor.HRMS_Att_Loaned_Monthly.FindAll(x => x.Factory == dto.Factory && x.Att_Month.Date == Convert.ToDateTime(dto.Loaned_Year_Month_Str).Date).ToListAsync();
            HALMs.ForEach(HALM =>
            {
                HALM.Pass = dto.Close_Status == "Y";
            });
            await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                _repositoryAccessor.HRMS_Att_Loaned_Monthly.UpdateMultiple(HALMs);
                await _repositoryAccessor.Save();
                await _repositoryAccessor.CommitAsync();
                return new OperationResult() { IsSuccess = true };
            }
            catch (Exception ex)
            {
                await _repositoryAccessor.RollbackAsync();
                return new OperationResult() { IsSuccess = false, Error = ex.InnerException.Message };
            }
        }
        #endregion

        #region GetListFactory
        public async Task<List<KeyValuePair<string, string>>> GetListFactory(string userName, string language)
        {
            var factories = await _repositoryAccessor.HRMS_Basic_Account_Role.FindAll(x => x.Account.ToLower() == userName.ToLower(), true)
                .Join(_repositoryAccessor.HRMS_Basic_Role.FindAll(true),
                    x => x.Role,
                    y => y.Role,
                    (x, y) => new { accRole = x, role = y })
                .Select(x => x.role.Factory)
                .Distinct().ToListAsync();

            if (!factories.Any())
                return new List<KeyValuePair<string, string>>();

            var predicate = PredicateBuilder.New<HRMS_Basic_Code>(x => x.Type_Seq == BasicCodeTypeConstant.Factory && factories.Contains(x.Code));
            var data = await GetBasicCodes(language, predicate);

            return data;
        }
        #endregion

        #region Frivate functions
        private async Task<List<KeyValuePair<string, string>>> GetBasicCodes(string language, ExpressionStarter<HRMS_Basic_Code> predicate)
        {
            var data = await _repositoryAccessor.HRMS_Basic_Code.FindAll(predicate, true)
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                    x => x.Code,
                    y => y.Code,
                    (x, y) => new { code = x, codeLang = y })
                .SelectMany(
                    x => x.codeLang.DefaultIfEmpty(),
                    (x, y) => new { x.code, codeLang = y })
                .Select(x => new KeyValuePair<string, string>(x.code.Code, $"{x.code.Code} - {x.codeLang.Code_Name ?? x.code.Code_Name}"))
                .Distinct().ToListAsync();

            return data;
        }

        private async Task<decimal> QueryLeaveMaintainSumDays(QueryMainSumDayParam param)
        {
            var result = await _repositoryAccessor.HRMS_Att_Leave_Maintain
                .FindAll(
                    x => x.Factory == param.Factory &&
                    x.Employee_ID == param.Employee_ID &&
                    x.Leave_Date >= Convert.ToDateTime(param.Leave_Date_Start) &&
                    x.Leave_Date <= Convert.ToDateTime(param.Leave_Date_End) &&
                    x.Leave_code == param.Leave_Code
                ).ToListAsync();

            return result.Any() ? result.Sum(x => x.Days) : 0;
        }

        private async Task<decimal> QueryOvertimeMaintainSumHours(QueryMainSumHoursParam param)
        {
            var result = await _repositoryAccessor.HRMS_Att_Overtime_Maintain
                .FindAll(
                    x => x.Factory == param.Factory &&
                    x.Employee_ID == param.Employee_ID &&
                    x.Overtime_Date >= Convert.ToDateTime(param.Overtime_Date_Start) &&
                    x.Overtime_Date <= Convert.ToDateTime(param.Overtime_Date_End) &&
                    x.Holiday == param.Holiday_Code
                ).ToListAsync();
            return result.Any() ? result.Sum(x => (decimal)x.GetType().GetProperties().FirstOrDefault(y => y.Name == param.Overtime_Column_Name).GetValue(x)) : 0;
        }

        private async Task<decimal> QueryDetailSumDays(QuerySumDayParam param)
        {
            var result = await _repositoryAccessor.HRMS_Att_Monthly_Detail
                .FindAll(
                    x => x.Factory == param.Factory &&
                    x.Employee_ID == param.Employee_ID &&
                    x.Att_Month == Convert.ToDateTime(param.Att_Month) &&
                    x.Leave_Type == param.Leave_Type &&
                    param.LeaveCodes.Contains(x.Leave_Code)
                ).ToListAsync();

            decimal total = param.Normal_Working_Days - (result.Any() ? result.Sum(x => x.Days) : 0);

            return total;
        }

        private static async Task<int> QueryChangeRecordCount(IQueryable<HRMS_Att_Change_Record> records, List<string> leaveCodes)
        {
            if (!records.Any())
                return 0;

            return await records.CountAsync(x => leaveCodes.Contains(x.Leave_Code));
        }

        private static async Task<int> QueryFoodExpensesSum(IQueryable<HRMS_Att_Change_Record> records)
        {
            int total = 0;
            List<string> workShiftTypes = new() { "A0", "B0", "C0" };
            List<string> leaveCodeClockIns = new() { "00", "01", "M0", "N0" };
            List<string> leaveCodeCompares = new() { "00", "02" };

            await records.ForEachAsync(x =>
                {
                    if (!workShiftTypes.Contains(x.Work_Shift_Type))
                    {
                        if (leaveCodeClockIns.Contains(x.Leave_Code) && x.Clock_In.CompareTo("1130") <= 0)
                            total += 1;
                        else if (leaveCodeCompares.Contains(x.Leave_Code) &&
                            (
                                x.Overtime_ClockIn.CompareTo("1900") >= 0 ||
                                x.Overtime_ClockOut.CompareTo("1900") >= 0 ||
                                x.Clock_Out.CompareTo("1900") >= 0)
                            )
                            total += 1;
                        else if (x.Leave_Code == "D0")
                            total += 1;
                        else if (!string.IsNullOrWhiteSpace(x.Clock_In))
                            total += 1;
                    }
                });

            return total;
        }

        private static async Task<int> QueryNightEatSum(List<DateTime> attDates, IQueryable<HRMS_Att_Change_Record> records)
        {
            int night_Eat = 0;
            var data = await records.Where(x => !attDates.Contains(x.Att_Date)).ToListAsync();
            if (!data.Any())
                return night_Eat;

            data.ForEach(x =>
                {
                    string formattedClockIn = x.Overtime_ClockIn.Insert(2, ":");
                    string formattedClockOut = x.Overtime_ClockOut.Insert(2, ":");

                    // Chuyển đổi chuỗi thành TimeSpan hoặc DateTime
                    TimeSpan clockInTime = TimeSpan.ParseExact(formattedClockIn, @"hh\:mm", null);
                    TimeSpan clockOutTime = TimeSpan.ParseExact(formattedClockOut, @"hh\:mm", null);

                    // Tính sự chênh lệch thời gian
                    TimeSpan difference = clockOutTime.Subtract(clockInTime);

                    // Lấy sự chênh lệch theo phút
                    int minutesDifference = (int)difference.TotalMinutes;
                    var work_Shift_Type = new List<string>() { "00", "10", "40", "50", "60", "G0", "H0", "S0", "T0" };
                    bool flag = false;
                    if (work_Shift_Type.Contains(x.Work_Shift_Type))
                    {
                        if (minutesDifference >= 90 && minutesDifference <= 120 && x.Clock_Out.CompareTo("1900") < 0)
                            flag = true;
                    }
                    else if (minutesDifference >= 90 && minutesDifference <= 120)
                        flag = true;

                    if (flag == true)
                        night_Eat += 1;

                });

            return night_Eat;
        }

        private static string GetResignStatus(HRMS_Emp_Personal emp, DateTime start, DateTime end)
        {
            return emp.Onboard_Date >= start && emp.Onboard_Date <= end || emp.Resign_Date >= start && emp.Resign_Date <= end ? "Y" : "N";
        }
        #endregion
    }
}