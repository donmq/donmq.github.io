using API.Data;
using API._Services.Interfaces.AttendanceMaintenance;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using API.Models;
using API.DTOs.AttendanceMaintenance;
using API.Helper.Constant;
using API.Helper.Utilities;

namespace API._Services.Services.AttendanceMaintenance
{
    public class S_5_1_14_EmployeeDailyAttendanceDataGeneration : BaseServices, I_5_1_14_EmployeeDailyAttendanceDataGeneration
    {
        private static readonly SemaphoreSlim semaphore = new(1, 1);
        private readonly TimeSpan timeSpan0000 = "0000".ToTimeSpan();
        private readonly TimeSpan timeSpan1200 = "1200".ToTimeSpan();
        private readonly TimeSpan timeSpan2400 = "2400".ToTimeSpan();
        private readonly TimeSpan timeSpan3600 = "3600".ToTimeSpan();
        public S_5_1_14_EmployeeDailyAttendanceDataGeneration(DBContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Lấy danh sách Factories theo tài khoản đăng nhập
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<List<KeyValuePair<string, string>>> GetFactories(string language, string userName)
        {
            var factorys = await Queryt_Factory_AddList(userName);
            var factories = await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Factory && factorys.Contains(x.Code), true)
                        .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                                    x => new { x.Type_Seq, x.Code },
                                    y => new { y.Type_Seq, y.Code },
                                    (x, y) => new { x, y })
                                    .SelectMany(x => x.y.DefaultIfEmpty(),
                                    (x, y) => new { x.x, y })
                        .Select(x => new KeyValuePair<string, string>(x.x.Code, $"{x.x.Code} - {(x.y != null ? x.y.Code_Name : x.x.Code_Name)}")).ToListAsync();
            return factories;
        }
        /// <summary>
        /// Validate Clock-In Date of The Current Day 
        /// <para>
        /// Nếu số lượng trong ngày hiện tại có Mark = N  bằng 0, (chưa khai báo bấm thẻ) thông báo [Not found Swipe_Card]
        /// </para>
        /// <para>
        /// Nếu số lượng trong ngày hiện tại có Mark = Y > 0, (đã khai báo bấm thẻ) thông báo [Cannot run again!]
        /// </para>
        /// </summary>
        /// <param name="factory"> Mã nhà máy </param>
        /// <param name="card_Date"> Default: Ngày hiện tại format: ddMM </param>
        /// <returns></returns>
        public async Task<OperationResult> CheckClockInDateInCurrentDate(string factory, string card_Date)
        {
            var query = await _repositoryAccessor.HRMS_Att_Swipe_Card.FindAll(x => x.Factory == factory && x.Card_Date == card_Date, true).ToListAsync();
            if (!query.Any(x => x.Mark == "N")) return new OperationResult(false, "Not found Swipe_Card");
            int card_cnt_markY = query.Where(x => x.Mark == "Y").Count();
            if (card_cnt_markY > 0) return new OperationResult(false, "Cannot run again!");
            return new OperationResult(true);
        }

        /// <summary>
        /// Lấy danh sách ngày nghỉ lễ
        /// </summary>
        /// <param name="factory">Mã nhà máy </param>
        /// <param name="offWork">Thời gian trước khi nghỉ </param>
        /// <param name="workDay">Thời gian làm việc hiện tại </param>
        /// <returns></returns>
        public async Task<OperationResult> GetHolidayDates(string factory, string offWork, string workDay) => await QueryHolidays(factory, offWork, workDay);

        /// <summary>
        /// Lấy danh sách ngày nghỉ lễ quốc khánh
        /// </summary>
        /// <param name="factory">Mã nhà máy </param>
        /// <param name="offWork">Thời gian trước khi nghỉ </param>
        /// <param name="workDay">Thời gian làm việc hiện tại </param>
        /// <returns></returns>
        public async Task<OperationResult> GetNationalHolidays(string factory, string offWork, string workDay) => await QueryHolidays(factory, offWork, workDay, true);

        private async Task<OperationResult> QueryHolidays(string factory, string offWork, string workDay, bool isNational = false)
        {
            var predicate = PredicateBuilder.New<HRMS_Att_Calendar>(x => x.Factory == factory);
            if (!string.IsNullOrWhiteSpace(offWork) && !string.IsNullOrWhiteSpace(workDay))
            {
                predicate.And(x => x.Att_Date.Date >= offWork.ToDateTime().Date
                                && x.Att_Date.Date <= workDay.ToDateTime().Date);
            }
            predicate.And(x => x.Type_Code == (isNational ? "C00" : "C05"));

            var data = await _repositoryAccessor.HRMS_Att_Calendar.FindAll(predicate).Select(x => x.Att_Date.ToString()).ToListAsync();
            return new OperationResult(true) { Data = data };
        }

        public async Task<OperationResult> ExcuteQuery(HRMS_Att_Swipe_Card_Excute_Param param)
        {
            await semaphore.WaitAsync();
            await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                var clockInFromInput = param.WorkOnDay.ToDateTime().Date; // Ngày làm việc hiện tại
                var clockOutFromInput = param.ClockOffDay.ToDateTime().Date; // Ngày làm việc trước đó
                var updateTime = DateTime.Now;
                int processRecord = 0;

                #region I. No need to swipe the card [Không cần quẹt thẻ], người thuộc danh sách k cần quẹt thẻ
                // Danh sách nhân viên không cần quẹt thẻ
                // Lấy thông tin nghỉ phép  (trùng với ngày nghỉ)
                // Cung cấp mã danh mục tương ứng với ngày nghỉ

                var employeesNoneSwipCardAndLeave = new List<HRMS_Att_Temp_Record>(); // Danh sách nhân viên không cần quẹt thẻ và xin phép nghỉ
                                                                                      // Thời gian ngày kết thúc làm việc được chọn lớn hơn ngày hiện tại
                DateTime tmp_date = clockOutFromInput;
                while (tmp_date <= clockInFromInput)
                {
                    // - Lấy danh sách nhân viên xin nghỉ phép và đã xin nghỉ vào ngày hôm nay (kèm theo ngày nghỉ lễ để xác định ngày lễ)
                    var employeesLeaveApplication = await GetEmployeesLeaveApplication(param.Factory, tmp_date, param.NationalHoliday, param.CurrentUser);

                    foreach (var empLeave in employeesLeaveApplication)
                    {
                        var att_temp_Record = new HRMS_Att_Temp_Record()
                        {
                            USER_GUID = empLeave.USER_GUID,
                            Factory = empLeave.Factory,
                            Att_Date = empLeave.Att_Date, // Ngày hôm nay
                            Department = string.IsNullOrWhiteSpace(empLeave.EmploymentStatus) ? empLeave.Department : empLeave.Assigned_Department,
                            Employee_ID = string.IsNullOrWhiteSpace(empLeave.EmploymentStatus) ? empLeave.EmployeeId : empLeave.Assigned_Employee_ID,
                            Work_Shift_Type = empLeave.Work_Shift_Type,
                            Leave_Code = empLeave.Leave_Code,
                            Clock_In = empLeave.Clock_In, // "0000"
                            Clock_Out = empLeave.Clock_Out, // "0000"
                            Overtime_ClockIn = empLeave.Overtime_ClockIn, // "0000"
                            Overtime_ClockOut = empLeave.Overtime_ClockOut, // "0000"
                            Days = empLeave.Days, // 1 ngày
                            Holiday = empLeave.Holiday, // là ngày nghỉ hay nghỉ lễ
                            Update_By = empLeave.Update_By,
                            Update_Time = empLeave.Update_Time
                        };
                        // (Nếu nhân viên đã đăng ký ngày nghỉ) đã thêm vào danh sách nghỉ rồi thì bỏ qua
                        if (await _repositoryAccessor.HRMS_Att_Temp_Record.AnyAsync(x =>
                                        x.Factory == param.Factory &&
                                        x.Employee_ID == att_temp_Record.Employee_ID &&
                                        x.Att_Date.Date == att_temp_Record.Att_Date.Date &&
                                        x.Leave_Code == att_temp_Record.Leave_Code))
                            continue;
                        // Chưa được thêm vào danh sách nghỉ thì thêm mới 
                        employeesNoneSwipCardAndLeave.Add(att_temp_Record);
                    }

                    // Tăng cho tới ngày kết thúc làm việc
                    tmp_date = tmp_date.AddDays(1);
                }

                if (employeesNoneSwipCardAndLeave.Any())
                {
                    processRecord += employeesNoneSwipCardAndLeave.Count;
                    var insAttTempRecordResult = await CRUD_Data(new ExceptionErrorData("Ins_HRMS_Att_Temp_Record_List", "1. ", new GeneralData(employeesNoneSwipCardAndLeave)));
                    if (!insAttTempRecordResult.IsSuccess)
                    {
                        await _repositoryAccessor.RollbackAsync();
                        return new OperationResult(false, insAttTempRecordResult.Error);
                    }
                }

                #endregion
                #region II. The person who wants to swipe the card [Người muốn quẹt thẻ]
                #region 2.1: Đọc danh sách file chính của nhân viên, nhân viên của nhà máy hoặc nhân viên được đưa đi làm nơi khác, những người muốn làm việc và muốn quẹt thẻ
                List<string> permission_Foreign_List = _repositoryAccessor.HRMS_Emp_Permission_Group.FindAll(x => x.Factory == param.Factory && x.Foreign_Flag == "Y").Select(x => x.Permission_Group).ToList();
                List<string> permission_Local_List = _repositoryAccessor.HRMS_Emp_Permission_Group.FindAll(x => x.Factory == param.Factory && x.Foreign_Flag == "N").Select(x => x.Permission_Group).ToList();

                // (2)	讀取59代碼檔，出勤不產生一般假日設定 [Đọc tệp mã 59, điểm danh không tạo ra cài đặt ngày lễ chung]
                bool holidayFlag = await Get_Holiday_Flag(param.Factory);

                var HEPs = await _repositoryAccessor.HRMS_Emp_Personal.FindAll(x =>
                                     (x.Factory == param.Factory || x.Assigned_Factory == param.Factory)
                                     && ((x.Resign_Date.HasValue && x.Resign_Date.Value.Date > clockInFromInput.Date) || !x.Resign_Date.HasValue)
                                     && x.Onboard_Date.Date <= clockOutFromInput.Date
                                     && x.Swipe_Card_Option
                                     //   && x.USER_GUID == "53f68b37-0180-4353-9550-73621e71d2cc"
                                     )
                                 .OrderBy(x => x.Employee_ID)
                                 .ThenBy(x => x.Assigned_Employee_ID)
                                 .ToListAsync();
                #endregion

                // 2.1: Give the initial value of the daily attendance file variable (Worker, GUID, Date of transfer, Transferee)
                // Dữ liệu lưu trữ tạm thời
                int rowId = 0;
                var temp_Swipe_Cards = new List<Temp_Swipe_Card>();

                // processRecord = personals.Count;

                // Dữ liệu thay đổi tăng ca
                foreach (var hrms_Emp_Personal in HEPs)
                {
                    var wk_date = clockOutFromInput; //  --Quét thẻ trước khi nghỉ
                    HRMS_Att_Change_Record atT_Change_Record = new()
                    {
                        USER_GUID = hrms_Emp_Personal.USER_GUID,
                        Department = string.IsNullOrWhiteSpace(hrms_Emp_Personal.Employment_Status) ? hrms_Emp_Personal.Department : hrms_Emp_Personal.Assigned_Department,
                        Factory = param.Factory,
                        Employee_ID = string.IsNullOrWhiteSpace(hrms_Emp_Personal.Employment_Status) ? hrms_Emp_Personal.Employee_ID : hrms_Emp_Personal.Assigned_Employee_ID,
                        Update_By = param.CurrentUser,
                        Update_Time = updateTime
                    };
                    var Att_Division = string.IsNullOrWhiteSpace(hrms_Emp_Personal.Employment_Status) ? hrms_Emp_Personal.Division : hrms_Emp_Personal.Assigned_Division;


                    // Lấy thông tin bấm thẻ của nhân viên đăng ký chưa hợp lệ theo nhà máy 
                    // (thường là sau khi import từ 5.13 thì mỗi nhân viên sẽ chỉ có 1 row)
                    var swipeCards = await _repositoryAccessor.HRMS_Att_Swipe_Card.FindAll(card =>
                                                    card.Factory == param.Factory
                                                    && card.Mark == "N"
                                                    && card.Employee_ID == atT_Change_Record.Employee_ID)
                                        .Distinct()
                                        .OrderBy(card => card.Employee_ID)
                                        .ThenBy(card => card.Card_Date)
                                        .ThenBy(card => card.Card_Time)
                                        .ToListAsync();

                    // Tính toán thời gian theo lịch bấm thẻ của nhân viên trong tháng được sắp đặt
                    foreach (var swipeCard in swipeCards)
                    {
                        rowId += 1; // ID cho mỗi row theo mỗi nhân viên

                        var temm_Swipe_Card = new Temp_Swipe_Card
                        {
                            RowId = rowId,
                            Mark = swipeCard.Mark,
                            Factory = swipeCard.Factory,
                            USER_GUID = atT_Change_Record.USER_GUID,
                            Empno_ID = swipeCard.Employee_ID,
                            CurrentYear = clockInFromInput.Year,
                            Card_Date = swipeCard.Card_Date,
                            Card_Time = swipeCard.Card_Time,
                            md = swipeCard.Card_Date, // 0306: March 6
                            hm = swipeCard.Card_Time // 3000: 30 hours 00 minutes
                        };

                        // lấy năm của ngày hiện tại
                        int wk_yy = DateTime.Today.Year;

                        // Tháng & ngày quẹt thẻ hiện tại
                        string cardDateMonth = swipeCard.Card_Date[..2];
                        string cardDateDay = swipeCard.Card_Date[2..];

                        // Nếu thời gian từ ngày hiện tại (trong tháng 12) cho tới ngày kết thúc làm việc (vào tháng 1)
                        if (clockInFromInput.Month == 12 && clockOutFromInput.Month == 1)
                        {
                            // -- Tháng 12 || -- Tháng 1
                            if (clockInFromInput.Month == Convert.ToInt16(cardDateMonth) || clockOutFromInput.Month == Convert.ToInt16(cardDateMonth))
                                wk_yy = clockInFromInput.Year; // set năm hiện tại = năm của Tháng 12
                        }

                        string YMD_date1_str = $"{wk_yy}/{cardDateMonth}/{cardDateDay}"; // YYYY/MM/DD format
                        var YMD_date1 = YMD_date1_str.ToDateTime();

                        // Call function(s) to retrieve work shift details (replace with actual logic)
                        // Lấy ca làm việc (theo ngày hiện tại)
                        string Work_Shift_Type_value = await Query_Work_Shift_Change(atT_Change_Record.Factory, atT_Change_Record.Employee_ID, hrms_Emp_Personal.Work_Shift_Type, YMD_date1);
                        if (Work_Shift_Type_value == null)
                        {
                            await _repositoryAccessor.RollbackAsync();
                            return new OperationResult(false, "Work Shift Type not found");
                        }

                        int week_value = (int)YMD_date1.DayOfWeek;
                        // Lấy thời gian vào ca làm việc vào (được set lại)
                        var work_Shift_Values = await Query_HRMS_Att_Work_Shift(atT_Change_Record.Factory, Work_Shift_Type_value, week_value.ToString());
                        if (work_Shift_Values == null)
                        {
                            await _repositoryAccessor.RollbackAsync();
                            return new OperationResult(false, "Work_Shift_Values not found");
                        }

                        if (work_Shift_Values != null && !string.IsNullOrWhiteSpace(work_Shift_Values.Clock_In))
                        {
                            // Tính thời gian làm việc trong ngày
                            var clockInTimeSpan = work_Shift_Values.Clock_In.ToTimeSpan();

                            // Thời gian bấm thẻ  sau 12h mới tính làm ca đêm
                            var cardTimeSpan = swipeCard.Card_Time.ToTimeSpan();

                            // Night shift handling (adjust logic based on your work shift definition)
                            // Thời gian làm ca đêm , từ 12h trưa, đến trước 12h trưa ngày hôm sau
                            if (clockInTimeSpan > timeSpan1200 && cardTimeSpan >= timeSpan0000 && cardTimeSpan <= timeSpan1200)
                            {
                                cardTimeSpan = cardTimeSpan.Add(timeSpan2400);
                                temm_Swipe_Card.hm = cardTimeSpan.ToHourMinuteString();
                                YMD_date1 = YMD_date1.AddDays(-1); // Subtract a day from the date
                                temm_Swipe_Card.md = new DateTime(YMD_date1.Year, YMD_date1.Month, YMD_date1.Day).ToMonthDayString(); // Update swipe card date
                            }
                        }

                        // Insert swipe card data (Thêm tạm vào lịch làm việc)
                        temp_Swipe_Cards.Add(temm_Swipe_Card);
                    }

                    #region 2.2 

                    // LET wk_date = Clock-Out Date // giờ ra ca của nhân viên ***???

                    var wk_md1 = wk_date.ToMonthDayString();

                    // Trong khi thời gian ngày hôm nay  >=  Ngày quét thẻ trước khi nghỉ
                    while (clockInFromInput.Date >= wk_date.Date)
                    {
                        // 2.2.1 - Ngày quẹt thẻ theo chu kỳ : 
                        // đầu tiên thực hiện quẹt thẻ vào ngày nghỉ làm (chủ nhật), 
                        // sau đó cộng các ngày để có được ngày làm việc quẹt thẻ (thứ 2 - thứ 7)
                        // var year_date = new DateTime(wk_date.Year, 1, 1);

                        // Thời gian làm việc của nhân viên vào 1/1
                        // IF DATA = NOTFOUND THEN Ins_HRMS_Att_Yearly 
                        // --Tạo thông tin thanh toán hàng năm
                        // Mẫu data insert
                        int week_value = (int)wk_date.DayOfWeek;

                        var HAY = new HRMS_Att_Yearly()
                        {
                            USER_GUID = atT_Change_Record.USER_GUID,
                            Division = Att_Division,
                            Factory = atT_Change_Record.Factory,
                            Employee_ID = atT_Change_Record.Employee_ID,
                            Update_By = param.CurrentUser,
                            Update_Time = updateTime,
                        };
                        var insAttYearlyResult = await CRUD_Data(new ExceptionErrorData("Ins_HRMS_Att_Yearly", "2.2 ", new GeneralData(wk_date, HAY)));
                        if (!insAttYearlyResult.IsSuccess)
                        {
                            await _repositoryAccessor.RollbackAsync();
                            return new OperationResult(false, insAttYearlyResult.Error);
                        }

                        //(3) 檢查假別設定檔是否當月有新生效年月，有新假別，則要補新增年度檔
                        var insAttYearlyCodeResult = await CRUD_Data(new ExceptionErrorData("Ins_HRMS_Att_Yearly_Code", "2.2", new GeneralData(wk_date, HAY)));
                        if (!insAttYearlyCodeResult.IsSuccess)
                        {
                            await _repositoryAccessor.RollbackAsync();
                            return new OperationResult(false, insAttYearlyCodeResult.Error);
                        }
                        #region 2.2.1 Kiểm tra xem có tệp cài đặt ca làm việc cho nhà máy + nhân viên + ngày tháng hay không. Nếu cài đặt ca khác với ca chính của nhân viên thì cập nhật ca chính của nhân viên
                        // -- Lấy lịch làm việc đã thay đổi (Được set tại 5.11)
                        var work_Shift_New = _repositoryAccessor.HRMS_Att_Work_Shift_Change.FirstOrDefault(x =>
                                                        x.Effective_Date.Date == wk_date.Date &&
                                                        x.Factory == atT_Change_Record.Factory &&
                                                        x.USER_GUID == atT_Change_Record.USER_GUID, true)?.Work_Shift_Type_New;

                        // -- Nếu có ca làm việc mới , thì cập nhật ca làm việc cho Nhân viên
                        if (!string.IsNullOrWhiteSpace(work_Shift_New) && work_Shift_New != hrms_Emp_Personal.Work_Shift_Type)
                        {
                            // UPDATE HRMS_Emp_Personal SET(Work_Shift) = (Work_Shift_New) WHERE HRMS_Emp_Personal.USERS_GUID
                            if (hrms_Emp_Personal.Work_Shift_Type != work_Shift_New)
                            {
                                hrms_Emp_Personal.Work_Shift_Type = work_Shift_New;
                                var updEmpPersonalResult = await CRUD_Data(new ExceptionErrorData("Upd_HRMS_Emp_Personal", "2.2.1 ", new GeneralData(hrms_Emp_Personal)));
                                if (!updEmpPersonalResult.IsSuccess)
                                {
                                    await _repositoryAccessor.RollbackAsync();
                                    return new OperationResult(false, updEmpPersonalResult.Error);
                                }
                            }
                            // Update HRMS_Att_Work_Shift_Change SET Effective_State = 1
                            var updAttWorkShiftChangeResult = await CRUD_Data(new ExceptionErrorData("Upd_HRMS_Att_Work_Shift_Change", "2.2.1 ", new GeneralData(wk_date, atT_Change_Record)));
                            if (!updAttWorkShiftChangeResult.IsSuccess)
                            {
                                await _repositoryAccessor.RollbackAsync();
                                return new OperationResult(false, updAttWorkShiftChangeResult.Error);
                            }

                        }
                        #endregion

                        #region 2.2.2 - Kiểm tra ngày lễ.

                        atT_Change_Record.Holiday = "XXX";
                        // wk_date >= Form Holiday Date Sart AND wk_date <= Form Holiday Date
                        if (!string.IsNullOrWhiteSpace(param.Holiday) && wk_date.Date >= param.Holiday.ToDateTime().Date && wk_date.Date <= param.Holiday.ToDateTime().Date)
                            atT_Change_Record.Holiday = "C05"; // mã dành cho --Ngày nghỉ lễ chung Chủ Nhật

                        // IF wk_date >= Form National holiday Star AND wk_date <= Form National holiday End
                        if (!string.IsNullOrWhiteSpace(param.NationalHoliday) && wk_date.Date >= param.NationalHoliday.ToDateTime().Date && wk_date.Date <= param.NationalHoliday.ToDateTime().Date)
                            atT_Change_Record.Holiday = "C00"; // mã dành cho ngày nghỉ lễ

                        #endregion

                        #region 2.2.3 - Cấu hình ca làm việc (Nếu như ca làm việc bị thay đổi)
                        // CALL Query_Work_Shift_Change
                        // Lấy Loại ca làm việc
                        string Work_Shift_Type_value = await Query_Work_Shift_Change(atT_Change_Record.Factory, atT_Change_Record.Employee_ID, hrms_Emp_Personal.Work_Shift_Type, wk_date);
                        if (string.IsNullOrWhiteSpace(Work_Shift_Type_value))
                        {
                            await _repositoryAccessor.RollbackAsync();
                            return new OperationResult(false, "Work Shift Type not found");
                        }

                        // CALL Query_HRMS_Att_Work_ShiftH
                        // Lấy thời gian vào ca làm việc vào (được set lại)
                        var Work_Shift_Values = await Query_HRMS_Att_Work_Shift(atT_Change_Record.Factory, Work_Shift_Type_value, week_value.ToString());
                        if (Work_Shift_Values == null)
                        {
                            await _repositoryAccessor.RollbackAsync();
                            return new OperationResult(false, $"Work_Shift_Values not found for\n Factory: {atT_Change_Record.Factory},\n Work Shift Type: {Work_Shift_Type_value},\n Week: {week_value}");
                        }

                        #endregion

                        #region 2.2.4 - Nếu bạn thay đổi giờ làm việc của ca tối, hãy cộng thêm 24 giờ vào số giờ kéo dài trong hai ngày
                        // Work_Shift_Values.Clock_In > '1200' - thời gian vào ca hơn 12h
                        if (Work_Shift_Values.Clock_In.ToTimeSpan() > timeSpan1200)  // CALL HRMS_Att_Work_Shift_Add24_hours
                        {
                            var added_Time = HRMS_Att_Work_Shift_Add24_hours(Work_Shift_Values);
                            if (added_Time == null)
                            {
                                await _repositoryAccessor.RollbackAsync();
                                return new OperationResult(false, $"HRMS_Att_Work_Shift_Add24_hours execution failed!");
                            }
                            Work_Shift_Values = added_Time;
                        }

                        var work_Shift_Values_Clock_In_TimeSpan = Work_Shift_Values.Clock_In.ToTimeSpan();
                        var work_Shift_Values_Clock_Out_TimeSpan = Work_Shift_Values.Clock_Out.ToTimeSpan();
                        var workShiftValue_OverTime_ClockInTimeSpan = Work_Shift_Values.Overtime_ClockIn.IsTimeSpanFormat() ? Work_Shift_Values.Overtime_ClockIn.ToTimeSpan() : (TimeSpan?)null;
                        var workShiftValue_OverTime_ClockOutTimeSpan = Work_Shift_Values.Overtime_ClockOut.IsTimeSpanFormat() ? Work_Shift_Values.Overtime_ClockOut.ToTimeSpan() : (TimeSpan?)null;
                        #endregion

                        #region 2.2.5 - Khai báo giá trị ban đầu cho các biến còn lại

                        atT_Change_Record.Work_Shift_Type = Work_Shift_Values.Work_Shift_Type;
                        atT_Change_Record.Att_Date = wk_date;
                        atT_Change_Record.Leave_Code = "00";
                        atT_Change_Record.Clock_In = "0000";
                        atT_Change_Record.Clock_Out = "0000";
                        atT_Change_Record.Overtime_ClockIn = "0000";
                        atT_Change_Record.Overtime_ClockOut = "0000";
                        atT_Change_Record.Days = 0;

                        // #加上24後的時間 Variable Declaration +24hours (#Thêm thời gian sau 24 Khai báo biến +24 giờ)
                        var att_Change_Record_Add24 = new HRMS_Att_Change_Record()
                        {
                            Clock_In = "0000",
                            Clock_Out = "0000",
                            Overtime_ClockIn = "0000",
                            Overtime_ClockOut = "0000",
                        };
                        #endregion

                        #region 2.2.6 - Khi thực hiện quẹt thẻ ngày nghỉ, 
                        // dữ liệu quẹt thẻ của ngày hôm trước sẽ được truy xuất trước để bù cho thời gian quẹt thẻ ngày nghỉ.

                        if (wk_date.Date == clockOutFromInput.Date)
                        {

                            // Thời gian bấm thẻ trong ngày
                            var att_Change_RecordData = await _repositoryAccessor.HRMS_Att_Change_Record.FirstOrDefaultAsync(x =>
                                                                    x.Factory == param.Factory &&
                                                                    x.USER_GUID == atT_Change_Record.USER_GUID &&
                                                                    x.Att_Date.Date == wk_date.Date);
                            // cập nhật lại dữ liệu
                            if (att_Change_RecordData != null)
                            {
                                atT_Change_Record.Leave_Code = att_Change_RecordData.Leave_Code;
                                atT_Change_Record.Clock_In = att_Change_RecordData.Clock_In;
                                atT_Change_Record.Clock_Out = att_Change_RecordData.Clock_Out;
                                atT_Change_Record.Overtime_ClockIn = att_Change_RecordData.Overtime_ClockIn;
                                atT_Change_Record.Overtime_ClockOut = att_Change_RecordData.Overtime_ClockOut;
                                atT_Change_Record.Days = att_Change_RecordData.Days;
                            }
                        }
                        #endregion

                        #region 2.2.7 - Theo MMDD của ngày điểm danh ở mục 2.2, lấy thẻ quẹt ra và lưu tạm. Sau khi lấy dữ liệu ra ghi chú cập nhật = Y
                        var for_Temp_Swipe_Cards = temp_Swipe_Cards
                                                .OrderBy(p => p.md)
                                                .ThenBy(x => x.hm)
                                                .Where(x => x.Factory == param.Factory
                                                        && x.USER_GUID == atT_Change_Record.USER_GUID
                                                        && x.Mark == "N"
                                                        && x.md == wk_md1)
                                                .ToList();
                        var check_cnt = 0;
                        string card_Flag = "N";

                        var most_recent_swipe_Card_Values = new Temp_Swipe_Card(); // Số liệu bấm thẻ mới nhất
                        // Kiểm tra thông tin quẹt thẻ
                        foreach (var Swipe_Card_Values in for_Temp_Swipe_Cards)
                        {
                            most_recent_swipe_Card_Values = Swipe_Card_Values;
                            check_cnt += 1;

                            // UPDATE Temp_Swipe_Card SET mark = "Y" WHERE ROWID = Swipe_Card_id
                            var indexOfTempSwipeCard = temp_Swipe_Cards.FindIndex(x => x.RowId == Swipe_Card_Values.RowId);
                            temp_Swipe_Cards[indexOfTempSwipeCard].Mark = "Y";

                            // *** 2.2.7.1 ***
                            // Xác định thời gian quẹt thẻ và điền vào ô tương ứng:
                            // (1) Thời gian làm ca chéo đã được cộng thêm 24 giờ ở mục 2.2.4 và phải trừ 24 giờ
                            // (2) Ghi trạng thái quẹt thẻ = Y, không quẹt thẻ = N
                            var swipe_Card_Values_hm_TimeSpan = Swipe_Card_Values.hm.ToTimeSpan();

                            // Case 1: Work_Shift_Values.Clock_Out > Work_Shift_Values.Clock_In AND Swipe_Card_Values.hm < Work_Shift_Values.Clock_Out
                            // Bấm trước giờ tan ca & thời gian tan ca > thời gian vào ca
                            if (work_Shift_Values_Clock_Out_TimeSpan > work_Shift_Values_Clock_In_TimeSpan && swipe_Card_Values_hm_TimeSpan < work_Shift_Values_Clock_Out_TimeSpan)
                            {
                                card_Flag = "Y";
                                // Swipe_Card_Values.hm >= '2400' AND Swipe_Card_Values.hm <= '3600' 
                                if (swipe_Card_Values_hm_TimeSpan >= timeSpan2400 && swipe_Card_Values_hm_TimeSpan <= timeSpan3600)
                                    // Att_Change_Record.Clock_In  = Swipe_Card_Values.hm - '2400'
                                    atT_Change_Record.Clock_In = swipe_Card_Values_hm_TimeSpan.Subtract(timeSpan2400).ToHourMinuteString();
                                else
                                    // Att_Change_Record.Clock_In = Swipe_Card_Values.hm
                                    atT_Change_Record.Clock_In = swipe_Card_Values_hm_TimeSpan.ToHourMinuteString();
                                // LET Att_Change_Record_Add24.Clock_In = Swipe_Card_Values.hm
                                att_Change_Record_Add24.Clock_In = swipe_Card_Values_hm_TimeSpan.ToHourMinuteString();
                            }

                            // Case 2: Swipe_Card_Values.hm >= Work_Shift_Values.Clock_Out AND Swipe_Card_Values.hm < Work_Shift_Values.Overtime_ClockIn
                            // Nếu thời gian bấm thẻ sau thời gian tan ca && trước thời gian tăng ca
                            if (swipe_Card_Values_hm_TimeSpan >= work_Shift_Values_Clock_Out_TimeSpan && swipe_Card_Values_hm_TimeSpan < workShiftValue_OverTime_ClockInTimeSpan)
                            {
                                card_Flag = "Y";
                                // IF Swipe_Card_Values.hm >= '2400' AND Swipe_Card_Values.hm <= '3600' 
                                if (swipe_Card_Values_hm_TimeSpan >= timeSpan2400 && swipe_Card_Values_hm_TimeSpan <= timeSpan3600)
                                    // Att_Change_Record.Clock_Out = Swipe_Card_Values.hm - '2400' 
                                    atT_Change_Record.Clock_Out = swipe_Card_Values_hm_TimeSpan.Subtract(timeSpan2400).ToHourMinuteString();
                                else //Att_Change_Record.Clock_Out = Swipe_Card_Values.hm
                                    atT_Change_Record.Clock_Out = swipe_Card_Values_hm_TimeSpan.ToHourMinuteString();
                                // LET Att_Change_Record_Add24.Clock_Out = Swipe_Card_Values.hm
                                att_Change_Record_Add24.Clock_Out = swipe_Card_Values_hm_TimeSpan.ToHourMinuteString();
                            }

                            // Case 3: Swipe_Card_Values.hm >= Work_Shift_Values.shm3 AND Swipe_Card_Values.hm < Work_Shift_Values.Overtime_ClockOut
                            // Thời gian bấm thẻ nằm trong thời gian bắt đầu tăng ca && thời gian kết thúc tăng ca
                            if (swipe_Card_Values_hm_TimeSpan >= workShiftValue_OverTime_ClockInTimeSpan && swipe_Card_Values_hm_TimeSpan < workShiftValue_OverTime_ClockOutTimeSpan)
                            {
                                card_Flag = "Y";
                                // IF Swipe_Card_Values.hm >= '2400' AND Swipe_Card_Values.hm <= '3600' 
                                if (swipe_Card_Values_hm_TimeSpan >= timeSpan2400 && swipe_Card_Values_hm_TimeSpan <= timeSpan3600)
                                    // Att_Change_Record.Overtime_ClockIn = Swipe_Card_Values.hm - '2400' 
                                    atT_Change_Record.Overtime_ClockIn = swipe_Card_Values_hm_TimeSpan.Subtract(timeSpan2400).ToHourMinuteString();
                                else //Att_Change_Record.Overtime_ClockIn = Swipe_Card_Values.hm
                                    atT_Change_Record.Overtime_ClockIn = swipe_Card_Values_hm_TimeSpan.ToHourMinuteString();
                                // LET Att_Change_Record_Add24.Overtime_ClockIn = Swipe_Card_Values.hm
                                att_Change_Record_Add24.Overtime_ClockIn = swipe_Card_Values_hm_TimeSpan.ToHourMinuteString();
                            }

                            // Case 4: Swipe_Card_Values.hm >= Work_Shift_Values.Overtime_ClockOut
                            // Thời gian quẹt thẻ sau giờ tăng ca
                            if (swipe_Card_Values_hm_TimeSpan >= workShiftValue_OverTime_ClockOutTimeSpan)
                            {
                                card_Flag = "Y";
                                // IF Swipe_Card_Values.hm >= '2400' AND Swipe_Card_Values.hm <= '3600' 
                                if (swipe_Card_Values_hm_TimeSpan >= timeSpan2400 && swipe_Card_Values_hm_TimeSpan <= timeSpan3600)
                                    // Att_Change_Record.Overtime_ClockOut = Swipe_Card_Values.hm - '2400' 
                                    atT_Change_Record.Overtime_ClockOut = swipe_Card_Values_hm_TimeSpan.Subtract(timeSpan2400).ToHourMinuteString();
                                else //Att_Change_Record.Overtime_ClockOut = Swipe_Card_Values.hm
                                    atT_Change_Record.Overtime_ClockOut = swipe_Card_Values_hm_TimeSpan.ToHourMinuteString();
                                // LET Att_Change_Record_Add24.Overtime_ClockOut = Swipe_Card_Values.hm
                                att_Change_Record_Add24.Overtime_ClockOut = swipe_Card_Values_hm_TimeSpan.ToHourMinuteString();
                            }
                            if (card_Flag == "N") break;
                        }

                        #endregion

                        #region  2.8 Nếu bạn quẹt thẻ trong ca đêm và nghỉ làm, việc đi làm của bạn sẽ không bị đánh giá và bạn sẽ thoát khỏi vòng lặp.
                        // Giá trị ngày theo thứ trong tuần : [0: Chủ nhật, 1:Thứ 2, 2:Thứ 3, 3:Thứ 4, 4:Thứ 5, 5:Thứ 6, 6:Thứ 7]

                        // Việc chấm công sẽ không được đánh giá vào ngày kết thúc ca đêm
                        // IF Check_cnt = 0 AND wk_date = Form Clock-In Date 
                        if (check_cnt == 0 && wk_date.Date == clockInFromInput.Date)
                        {
                            // CALL Query_HRMS_Att_Work_Shift(Factory,Work_Shift_Type_value,wk_date_week_value)
                            var work_Shift_Values_Night = await Query_HRMS_Att_Work_Shift(atT_Change_Record.Factory, atT_Change_Record.Work_Shift_Type, week_value.ToString());

                            if (work_Shift_Values_Night == null)
                            {
                                await _repositoryAccessor.RollbackAsync();
                                return new OperationResult(false, $"Work_Shift_Values_Night not found for \n Factory: {atT_Change_Record.Factory},\n Work Shift Type: {atT_Change_Record.Work_Shift_Type},\n Week: {week_value}");

                            }
                            // IF Work_Shift_Values_Night.Clock_In > '1200'
                            if (work_Shift_Values_Night.Clock_In.ToTimeSpan() > timeSpan1200)
                                // EXIT WHILE  --2.2 EXIT WHILE
                                break;
                        }
                        #endregion
                        switch (card_Flag)
                        {
                            case "N":
                                #region 2.2.9 - CASE: WHEN Card_Flag ="N" : Không quẹt thẻ
                                // 2.2.9.1
                                // IF Att_Change_Record.Holiday = 'C05' (ngày nghỉ)
                                if (atT_Change_Record.Holiday == "C05")
                                {
                                    // CALL Query_HRMS_Att_Leave_Application - Lấy thông tin xin nghỉ phép
                                    var att_Leave_Application_Values = await Query_HRMS_Att_Leave_Application(atT_Change_Record.Factory, atT_Change_Record.Employee_ID, atT_Change_Record.Att_Date);
                                    if (att_Leave_Application_Values != null)
                                    {
                                        atT_Change_Record.Leave_Code = att_Leave_Application_Values.Leave_code;
                                        atT_Change_Record.Days = Math.Min(atT_Change_Record.Days, 1); // Set days to 1 for compensatory leave


                                        // Nếu Factory là SHC || CB và HRMS_Emp_Personal.Permission_Group IN Permission_Foreign_List 
                                        // Không quẹt thẻ =N:
                                        // (1) Nhập dữ liệu người làm việc vào ngày nghỉ
                                        // Nhà máy C và E không ghi dữ liệu ngày nghỉ lễ => tức là không tạo ra dữ liệu ngày Chủ Nhật                
                                        if ((param.Factory == "SHC" || param.Factory == "CB") &&
                                            permission_Foreign_List.Contains(hrms_Emp_Personal.Permission_Group))
                                        {
                                            // Check Is added 
                                            // CALL Ins_HRMS_Att_Temp_Record(Att_Change_Record.*)  Lưu trữ tạm thời chấm công
                                            var insTempResult = await CRUD_Data(new ExceptionErrorData("Ins_HRMS_Att_Temp_Record", "2.2.9.1 ", new GeneralData(atT_Change_Record)));
                                            if (!insTempResult.IsSuccess)
                                            {
                                                await _repositoryAccessor.RollbackAsync();
                                                return new OperationResult(false, insTempResult.Error);
                                            }
                                            else processRecord += 1;


                                            // CALL Upd_HRMS_Att_Yearly
                                            // Cập nhật số ngày cộng dồn vào file chấm công hàng năm
                                            var updateAttYearlyResult = await CRUD_Data(new ExceptionErrorData("Upd_HRMS_Att_Yearly", "2.2.9.1 ", new GeneralData(wk_date, atT_Change_Record)));
                                            if (!updateAttYearlyResult.IsSuccess)
                                            {
                                                await _repositoryAccessor.RollbackAsync();
                                                return new OperationResult(false, updateAttYearlyResult.Error);
                                            }
                                        }
                                    }
                                    else // #Thêm thông tin hồ sơ ngày nghỉ lễ
                                    {
                                        // Nếu được phép thêm thêm tin ngày lễ 
                                        if (holidayFlag)
                                        {
                                            atT_Change_Record.Days = 1;
                                            atT_Change_Record.Leave_Code = "08";
                                            atT_Change_Record.Holiday = "C05"; // Ngày nghỉ bth

                                            // CALL Ins_HRMS_Att_Change_Record(Att_Change_Record.*)  - dữ liệu chấm công hàng ngày
                                            var insChangeRecordResult = await CRUD_Data(new ExceptionErrorData("Ins_HRMS_Att_Change_Record", "2.2.9.1 ", new GeneralData(atT_Change_Record)));
                                            if (!insChangeRecordResult.IsSuccess)
                                            {
                                                await _repositoryAccessor.RollbackAsync();
                                                return new OperationResult(false, insChangeRecordResult.Error);
                                            }

                                            //    CALL Ins_HRMS_Att_Temp_Record(Att_Change_Record.*) - Lưu trữ tạm thời chấm công
                                            var insTempResult = await CRUD_Data(new ExceptionErrorData("Ins_HRMS_Att_Temp_Record", "2.2.9.1 ", new GeneralData(atT_Change_Record)));
                                            if (!insTempResult.IsSuccess)
                                            {
                                                await _repositoryAccessor.RollbackAsync();
                                                return new OperationResult(false, insTempResult.Error);
                                            }
                                            else processRecord += 1;
                                        }
                                    }
                                    break;
                                }

                                // 2.2.9.2 Kiểm tra dữ liệu bất thường, nghỉ ca ngày/ca đêm
                                // Ngày về = ngày kết thúc quẹt thẻ
                                if (wk_date.Date == clockOutFromInput.Date)
                                {
                                    // IF Work_Shift_Values.Clock_In < "1200" AND Work_Shift_Values.Clock_Out > Work_Shift_Values.Clock_In
                                    // Xác định thời gian ca có thuộc ca ngày hay không
                                    if (work_Shift_Values_Clock_In_TimeSpan < timeSpan1200 && work_Shift_Values_Clock_Out_TimeSpan > work_Shift_Values_Clock_In_TimeSpan)
                                    {
                                        // Lấy dữ liệu chấm công hàng ngày
                                        var Att_Change_Record_Values = await Query_HRMS_Att_Change_Record(param.Factory, atT_Change_Record.Employee_ID, atT_Change_Record.Att_Date);

                                        // Nếu không có chấm công -> nhân viên văng mặt
                                        if (Att_Change_Record_Values == null)
                                        {
                                            atT_Change_Record.Clock_In = "0000";
                                            atT_Change_Record.Leave_Code = "C0"; //-- 曠職Absent (Vắng Mặt)
                                            atT_Change_Record.Days = 1;
                                        }
                                        else // có mặt
                                        {
                                            // HRMS_Emp_Personal.Permission_Group IN Permission_Local_List AND  HRMS_Emp_Personal.Employment_Status IS NOT NULL 

                                            if (permission_Local_List.Contains(hrms_Emp_Personal.Permission_Group) &&
                                                !string.IsNullOrWhiteSpace(hrms_Emp_Personal.Employment_Status))
                                            {
                                                atT_Change_Record.Leave_Code = "04"; //Không quẹt thẻ sau giờ làm
                                                var result = await CRUD_Data(new ExceptionErrorData("Del_HRMS_Att_Change_Record", "2.2.9.2.1", new GeneralData(atT_Change_Record)));
                                                if (!result.IsSuccess)
                                                {
                                                    await _repositoryAccessor.RollbackAsync();
                                                    return new OperationResult(false, result.Error);
                                                }
                                            }
                                        }
                                    }
                                    else  //--Ca đêm
                                    {
                                        // IF Work_Shift_Values.Overnight = 'Y' 
                                        if (Work_Shift_Values.Overnight == "Y")
                                            atT_Change_Record.Leave_Code = "03"; //-- Lỗi không quẹt được thẻ
                                        else
                                        {
                                            atT_Change_Record.Leave_Code = "C0"; //-- Văng mặt
                                            atT_Change_Record.Days = 1;
                                        }
                                    }
                                }
                                else // (Vắng Mặt)
                                {
                                    atT_Change_Record.Leave_Code = "C0"; //-- Văng mặt
                                    atT_Change_Record.Days = 1;
                                }

                                // 2.2.9.3 - Nếu không có thông tin hồ sơ bảo trì nghỉ phép, vui lòng kiểm tra hồ sơ xin nghỉ phép.
                                // CALL Query_HRMS_Att_Leave_Maintain

                                // *** Chỉnh sửa DK 
                                var Att_Leave_Maintain_Values = await Query_HRMS_Att_Leave_Maintain(atT_Change_Record.Factory, atT_Change_Record.Employee_ID, atT_Change_Record.Att_Date);
                                if (Att_Leave_Maintain_Values == null)
                                {
                                    // Lấy thông tin xin phép nghỉ của nhân viên
                                    var Query_Att_Leave_Application_Values = await Query_HRMS_Att_Leave_Application(atT_Change_Record.Factory, atT_Change_Record.Employee_ID, atT_Change_Record.Att_Date);

                                    // Nếu như đã xin phép
                                    if (Query_Att_Leave_Application_Values != null)
                                    {
                                        atT_Change_Record.Leave_Code = Query_Att_Leave_Application_Values.Leave_code;
                                        atT_Change_Record.Days = Math.Min(Query_Att_Leave_Application_Values.Days, 1);

                                        // Thêm dữ liệu chấm công tạm thời
                                        var insTempResult = await CRUD_Data(new ExceptionErrorData("Ins_HRMS_Att_Temp_Record", "2.2.9.3", new GeneralData(atT_Change_Record)));
                                        if (!insTempResult.IsSuccess)
                                        {
                                            await _repositoryAccessor.RollbackAsync();
                                            return new OperationResult(false, insTempResult.Error);
                                        }
                                        // CALL Upd_HRMS_Att_Yearly
                                        // Cập nhật số ngày cộng dồn vào file chấm công hàng năm
                                        var updateAttYearlyResult = await CRUD_Data(new ExceptionErrorData("Upd_HRMS_Att_Yearly", "2.2.9.3", new GeneralData(wk_date, atT_Change_Record)));
                                        if (!updateAttYearlyResult.IsSuccess)
                                        {
                                            await _repositoryAccessor.RollbackAsync();
                                            return new OperationResult(false, updateAttYearlyResult.Error);
                                        }
                                        processRecord += 1;
                                    }
                                    else // Chưa xin phép
                                    {
                                        atT_Change_Record.Leave_Code = "C0"; //-- 曠職Absent: Văng mặt
                                        atT_Change_Record.Days = 1;

                                        // IF HRMS_Emp_Personal.Permission_Group IN Permission_Foreign_List  AND  HRMS_Emp_Personal.Employment_Status IS NOT NULL AND  Att_Change_Record.Clock_In != '0000'
                                        if (permission_Foreign_List.Contains(hrms_Emp_Personal.Permission_Group) &&
                                            !string.IsNullOrWhiteSpace(hrms_Emp_Personal.Employment_Status) &&
                                            atT_Change_Record.Clock_In.ToTimeSpan() != timeSpan0000)
                                        {
                                            atT_Change_Record.Leave_Code = "00"; //-- #đi lại bình thường
                                            atT_Change_Record.Days = 0;
                                        }

                                        // CALL Ins_HRMS_Att_Temp_Record(Att_Change_Record.*) -Thêm dữ liệu chấm công tạm 
                                        var insTempResult = await CRUD_Data(new ExceptionErrorData("Ins_HRMS_Att_Temp_Record", "2.2.9.3", new GeneralData(atT_Change_Record)));
                                        if (!insTempResult.IsSuccess)
                                        {
                                            await _repositoryAccessor.RollbackAsync();
                                            return new OperationResult(false, insTempResult.Error);
                                        }
                                        // CALL Ins_HRMS_Att_Change_Record(Att_Change_Record.*) - Thêm dữ liệu chấm công hàng ngày
                                        var insChangeRecordResult = await CRUD_Data(new ExceptionErrorData("Ins_HRMS_Att_Change_Record", "2.2.9.3", new GeneralData(atT_Change_Record)));
                                        if (!insChangeRecordResult.IsSuccess)
                                        {
                                            await _repositoryAccessor.RollbackAsync();
                                            return new OperationResult(false, insChangeRecordResult.Error);
                                        }
                                        processRecord += 1;
                                    }
                                }
                                else // Att_Leave_Maintain_Values found data --Đã xin nghỉ phép 
                                {
                                    atT_Change_Record.Leave_Code = Att_Leave_Maintain_Values.Leave_code; // lí do nghỉ
                                    atT_Change_Record.Days = Math.Min(Att_Leave_Maintain_Values.Days, 1); // thời gian nghỉ 1 ngày

                                    // CALL Ins_HRMS_Att_Temp_Record(Att_Change_Record.*) -- thêm dữ liệu chấm công tạm
                                    var insTempResult = await CRUD_Data(new ExceptionErrorData("Ins_HRMS_Att_Temp_Record", "2.2.9.3 ", new GeneralData(atT_Change_Record)));
                                    if (!insTempResult.IsSuccess)
                                    {
                                        await _repositoryAccessor.RollbackAsync();
                                        return new OperationResult(false, insTempResult.Error);
                                    }
                                    // CALL Ins_HRMS_Att_Change_Record(Att_Change_Record.*) thêm dữ liệu chấm công
                                    var insChangeRecordResult = await CRUD_Data(new ExceptionErrorData("Ins_HRMS_Att_Change_Record", "2.2.9.3 ", new GeneralData(atT_Change_Record)));
                                    if (!insChangeRecordResult.IsSuccess)
                                    {
                                        await _repositoryAccessor.RollbackAsync();
                                        return new OperationResult(false, insChangeRecordResult.Error);
                                    }
                                    processRecord += 1;
                                }
                                #endregion 2.2.9 - EXIT CASE
                                break;

                            case "Y":
                                #region 2.2.10 CASE: WHEN Card_Flag ="Y" : Có quẹt thẻ
                                // Tình huống ca ngày: quẹt thẻ sau khi tan sở
                                // IF wk_date = Clock-Out Date
                                #region 2.2.10.1
                                if (wk_date.Date == clockOutFromInput.Date)
                                {
                                    // Đi làm buổi sáng
                                    // 2.2.10.1.1
                                    if (work_Shift_Values_Clock_In_TimeSpan < timeSpan1200)
                                    {
                                        // Lấy dữ liệu chấm công hàng ngày
                                        var Att_Change_Record_Values = await Query_HRMS_Att_Change_Record(param.Factory, atT_Change_Record.Employee_ID, atT_Change_Record.Att_Date);
                                        if (Att_Change_Record_Values != null)
                                        {
                                            // CALL Del_HRMS_Att_Change_Record
                                            var result = await CRUD_Data(new ExceptionErrorData("Del_HRMS_Att_Change_Record", "2.2.10.1.1 ", new GeneralData(atT_Change_Record)));
                                            if (!result.IsSuccess)
                                            {
                                                await _repositoryAccessor.RollbackAsync();
                                                return new OperationResult(false, result.Error);
                                            }
                                        }
                                    }
                                    // Nếu không quẹt thẻ tại nơi làm việc
                                    // 2.2.10.1.2 
                                    if (atT_Change_Record.Clock_In.ToTimeSpan() == timeSpan0000)
                                    {
                                        // 2.2.10.1.2.1
                                        atT_Change_Record.Leave_Code = "03"; // Quẹt thẻ bất thường (tại nơi làm việc)
                                        atT_Change_Record.Days = 0;
                                        // lấy thông tin xin nghỉ phép
                                        var Att_Leave_Maintain_Values_03 = await Query_HRMS_Att_Leave_Maintain(atT_Change_Record.Factory, atT_Change_Record.Employee_ID, atT_Change_Record.Att_Date);
                                        if (Att_Leave_Maintain_Values_03 == null)
                                        {
                                            // CALL Query_HRMS_Att_Leave_Application -- lấy dữ liệu nghỉ
                                            var Att_Leave_Application_Values = await Query_HRMS_Att_Leave_Application(atT_Change_Record.Factory, atT_Change_Record.Employee_ID, atT_Change_Record.Att_Date);
                                            if (Att_Leave_Application_Values != null)
                                            {
                                                atT_Change_Record.Leave_Code = Att_Leave_Application_Values.Leave_code; // lí do nghỉ
                                                atT_Change_Record.Days = Math.Min(Att_Leave_Application_Values.Days, 1); // thời gian nghỉ: 1 ngày

                                                // IF HRMS_Emp_Personal.Permission_Group IN Permission_Foreign_List AND  HRMS_Emp_Personal.Employment_Status IS NOT NULL AND  Att_Change_Record.Clock_In != '0000'
                                                if (permission_Foreign_List.Contains(hrms_Emp_Personal.Permission_Group) &&
                                                    !string.IsNullOrWhiteSpace(hrms_Emp_Personal.Employment_Status) &&
                                                    atT_Change_Record.Clock_In.ToTimeSpan() != timeSpan0000)
                                                {
                                                    atT_Change_Record.Leave_Code = "00"; //-#người nước ngoài đi lại bình thường
                                                    atT_Change_Record.Days = 0;

                                                    // CALL Ins_HRMS_Att_Temp_Record(Att_Change_Record.*) thêm dữ liệu chấm công tạm
                                                    var insTempResult = await CRUD_Data(new ExceptionErrorData("Ins_HRMS_Att_Temp_Record", "2.2.10.1.2.1 ", new GeneralData(atT_Change_Record)));
                                                    if (!insTempResult.IsSuccess)
                                                    {
                                                        await _repositoryAccessor.RollbackAsync();
                                                        return new OperationResult(false, insTempResult.Error);
                                                    }
                                                    // CALL Ins_HRMS_Att_Change_Record(Att_Change_Record.*) thêm dữ liệu chấm công
                                                    var insChangeRecordResult = await CRUD_Data(new ExceptionErrorData("Ins_HRMS_Att_Change_Record", "2.2.10.1.2.1 ", new GeneralData(atT_Change_Record)));
                                                    if (!insChangeRecordResult.IsSuccess)
                                                    {
                                                        await _repositoryAccessor.RollbackAsync();
                                                        return new OperationResult(false, insChangeRecordResult.Error);
                                                    }
                                                    processRecord += 1;
                                                }
                                                else
                                                {
                                                    // CALL Ins_HRMS_Att_Temp_Record(Att_Change_Record.*) thêm dữ liệu chấm công tạm
                                                    var insTempResult = await CRUD_Data(new ExceptionErrorData("Ins_HRMS_Att_Temp_Record", "2.2.10.1.2.1 ", new GeneralData(atT_Change_Record)));
                                                    if (!insTempResult.IsSuccess)
                                                    {
                                                        await _repositoryAccessor.RollbackAsync();
                                                        return new OperationResult(false, insTempResult.Error);
                                                    }
                                                    // CALL Upd_HRMS_Att_Yearly
                                                    // Cập nhật số ngày cộng dồn vào file chấm công hàng năm
                                                    var updateAttYearlyResult = await CRUD_Data(new ExceptionErrorData("Upd_HRMS_Att_Yearly", "2.2.10.1.2.1", new GeneralData(wk_date, atT_Change_Record)));
                                                    if (!updateAttYearlyResult.IsSuccess)
                                                    {
                                                        await _repositoryAccessor.RollbackAsync();
                                                        return new OperationResult(false, updateAttYearlyResult.Error);
                                                    }
                                                    processRecord += 1;
                                                }
                                            }
                                            else
                                            {
                                                // CALL Ins_HRMS_Att_Temp_Record - thêm dữ liệu chấm công tạm
                                                var insTempResult = await CRUD_Data(new ExceptionErrorData("Ins_HRMS_Att_Temp_Record", "2.2.10.1.2.1", new GeneralData(atT_Change_Record)));
                                                if (!insTempResult.IsSuccess)
                                                {
                                                    await _repositoryAccessor.RollbackAsync();
                                                    return new OperationResult(false, insTempResult.Error);
                                                }
                                                processRecord += 1;
                                            }
                                        }
                                        else
                                        {
                                            atT_Change_Record.Leave_Code = Att_Leave_Maintain_Values_03.Leave_code;
                                            atT_Change_Record.Days = Math.Min(Att_Leave_Maintain_Values_03.Days, 1);

                                            // CALL Ins_HRMS_Att_Temp_Record(Att_Change_Record.*) - thêm dữ liệu chấm công tạm
                                            var insTempResult = await CRUD_Data(new ExceptionErrorData("Ins_HRMS_Att_Temp_Record", "2.2.10.1.2.1", new GeneralData(atT_Change_Record)));
                                            if (!insTempResult.IsSuccess)
                                            {
                                                await _repositoryAccessor.RollbackAsync();
                                                return new OperationResult(false, insTempResult.Error);
                                            }
                                            // CALL Ins_HRMS_Att_Change_Record(Att_Change_Record.*) - thêm dữ liệu chấm công
                                            var insChangeRecordResult = await CRUD_Data(new ExceptionErrorData("Ins_HRMS_Att_Change_Record", "2.2.10.1.2.1", new GeneralData(atT_Change_Record)));
                                            if (!insChangeRecordResult.IsSuccess)
                                            {
                                                await _repositoryAccessor.RollbackAsync();
                                                return new OperationResult(false, insChangeRecordResult.Error);
                                            }
                                            processRecord += 1;
                                        }

                                        // Có hồ sơ xin làm thêm giờ, tạm thời xử lý việc làm thêm giờ và người nước ngoài đóng quân => nhập vào hồ sơ chấm công hàng ngày.
                                        // Các nhân viên còn lại => Lưu trữ tạm thời điểm danh và điểm danh
                                        // Thời gian quẹt thẻ > thời gian ngoài giờ làm việc
                                        // 2.2.10.1.2.2
                                        if ((param.Factory == "SHC" || param.Factory == "CB") && most_recent_swipe_Card_Values.hm.ToTimeSpan() > work_Shift_Values_Clock_Out_TimeSpan)
                                        {
                                            // CALL Query_HRMS_Att_Overtime_Application - lấy Đơn xin làm thêm giờ
                                            var Overtime_Application_Values = await Query_HRMS_Att_Overtime_Application(atT_Change_Record.Factory, atT_Change_Record.Employee_ID, atT_Change_Record.Att_Date);
                                            // # Có Xin làm thêm giờ
                                            if (Overtime_Application_Values == null)
                                                break;
                                            else
                                            {
                                                var queryOvertimeParameterResult = await Query_HRMS_Att_Overtime_Parameter(
                                                    atT_Change_Record.Factory,
                                                    atT_Change_Record.Work_Shift_Type,
                                                    most_recent_swipe_Card_Values.hm,
                                                    atT_Change_Record.Att_Date
                                                );
                                                if (!queryOvertimeParameterResult.IsSuccess)
                                                {
                                                    await _repositoryAccessor.RollbackAsync();
                                                    return new OperationResult(false, queryOvertimeParameterResult.Error);
                                                }
                                                var Overtime_Parameter = queryOvertimeParameterResult.Data as HRMS_Att_Overtime_Parameter;
                                                var Att_Overtime_Temp_Values = new HRMS_Att_Overtime_Temp()
                                                {
                                                    USER_GUID = hrms_Emp_Personal.USER_GUID,
                                                    Factory = param.Factory,
                                                    Department = atT_Change_Record.Department,
                                                    Overtime_Date = atT_Change_Record.Att_Date,
                                                    Employee_ID = atT_Change_Record.Employee_ID,
                                                    Work_Shift_Type = atT_Change_Record.Work_Shift_Type,
                                                    Overtime_Start = Overtime_Application_Values.Overtime_Start,
                                                    Overtime_End = Overtime_Application_Values.Overtime_End,
                                                    Overtime_Hours = Overtime_Parameter?.Overtime_Hours ?? 0,
                                                    Night_Hours = Overtime_Parameter?.Night_Hours ?? 0,
                                                    Night_Overtime_Hours = 0,
                                                    Training_Hours = Overtime_Application_Values.Training_Hours,
                                                    Night_Eat_Times = Convert.ToInt16(Overtime_Application_Values.Night_Eat_Times),
                                                    Holiday = atT_Change_Record.Holiday,
                                                    Update_By = param.CurrentUser,
                                                    Update_Time = updateTime
                                                };
                                                // CALL Ins_HRMS_Att_Overtime_Temp - thêm dữ liệu tăng ca tạm
                                                var overTimeResult = await CRUD_Data(new ExceptionErrorData("Ins_HRMS_Att_Overtime_Temp", "2.2.10.1.2.2 ", new GeneralData(Att_Overtime_Temp_Values, atT_Change_Record)));
                                                if (!overTimeResult.IsSuccess)
                                                {
                                                    await _repositoryAccessor.RollbackAsync();
                                                    return new OperationResult(false, overTimeResult.Error);
                                                }
                                                // CALL Del_HRMS_Att_Change_Record  - Xoá dữ liệu chấm công
                                                var result = await CRUD_Data(new ExceptionErrorData("Del_HRMS_Att_Change_Record", "2.2.10.1.2.2", new GeneralData(atT_Change_Record)));
                                                if (!result.IsSuccess)
                                                {
                                                    await _repositoryAccessor.RollbackAsync();
                                                    return new OperationResult(false, result.Error);
                                                }
                                                // IF HRMS_Emp_Personal.Permission_Group IN Permission_Foreign_List  AND HRMS_Emp_Personal.Employment_Status IS NOT NULL AND  Att_Change_Record.Clock_In != '0000'
                                                if (permission_Foreign_List.Contains(hrms_Emp_Personal.Permission_Group) &&
                                                    !string.IsNullOrWhiteSpace(hrms_Emp_Personal.Employment_Status) &&
                                                    atT_Change_Record.Clock_In.ToTimeSpan() != timeSpan0000)
                                                {
                                                    atT_Change_Record.Leave_Code = "00"; //#người nước ngoài
                                                    atT_Change_Record.Days = 0;
                                                    // CALL Ins_HRMS_Att_Change_Record(Att_Change_Record.*) - thêm dữ liệu chấm công
                                                    var insChangeRecordResult = await CRUD_Data(new ExceptionErrorData("Ins_HRMS_Att_Change_Record", "2.2.10.1.2.2 ", new GeneralData(atT_Change_Record)));
                                                    if (!insChangeRecordResult.IsSuccess)
                                                    {
                                                        await _repositoryAccessor.RollbackAsync();
                                                        return new OperationResult(false, insChangeRecordResult.Error);
                                                    }
                                                }
                                                // CALL Ins_HRMS_Att_Temp_Record(Att_Change_Record.*) - Thêm dữ liệu chấm công tạm
                                                var insTempResult = await CRUD_Data(new ExceptionErrorData("Ins_HRMS_Att_Temp_Record", "2.2.10.1.2.2 ", new GeneralData(atT_Change_Record)));
                                                if (!insTempResult.IsSuccess)
                                                {
                                                    await _repositoryAccessor.RollbackAsync();
                                                    return new OperationResult(false, insTempResult.Error);
                                                }
                                                processRecord += 1;
                                            }
                                        }
                                        break; // 2.2.10 EXIT CASE
                                    }
                                }
                                #endregion

                                #region 2.2.10.2
                                // 2.2.10.2.1
                                if (atT_Change_Record.Clock_In.ToTimeSpan() > work_Shift_Values_Clock_In_TimeSpan && atT_Change_Record.Clock_In.ToTimeSpan() != timeSpan0000)
                                {
                                    atT_Change_Record.Leave_Code = "03";
                                    var insTempResult = await CRUD_Data(new ExceptionErrorData("Ins_HRMS_Att_Temp_Record", "2.2.10.2.1 ", new GeneralData(atT_Change_Record)));
                                    if (!insTempResult.IsSuccess)
                                    {
                                        await _repositoryAccessor.RollbackAsync();
                                        return new OperationResult(false, insTempResult.Error);
                                    }
                                    processRecord++;
                                }
                                // 2.2.10.2.2
                                else
                                {
                                    if (wk_date.Date == clockInFromInput.Date
                                        && work_Shift_Values_Clock_In_TimeSpan < work_Shift_Values_Clock_Out_TimeSpan)
                                    {
                                        var insChangeRecordResult = await CRUD_Data(new ExceptionErrorData("Ins_HRMS_Att_Change_Record", "2.2.10.2.2 ", new GeneralData(atT_Change_Record)));
                                        if (!insChangeRecordResult.IsSuccess)
                                        {
                                            await _repositoryAccessor.RollbackAsync();
                                            return new OperationResult(false, insChangeRecordResult.Error);
                                        }

                                        var insTempResult = await CRUD_Data(new ExceptionErrorData("Ins_HRMS_Att_Temp_Record", "2.2.10.2.2 ", new GeneralData(atT_Change_Record)));
                                        if (!insTempResult.IsSuccess)
                                        {
                                            await _repositoryAccessor.RollbackAsync();
                                            return new OperationResult(false, insTempResult.Error);
                                        }

                                        if (most_recent_swipe_Card_Values.hm.ToTimeSpan() > work_Shift_Values_Clock_Out_TimeSpan
                                            || most_recent_swipe_Card_Values.hm.ToTimeSpan() < work_Shift_Values_Clock_In_TimeSpan)
                                        {
                                            var Overtime_Application_Values = await Query_HRMS_Att_Overtime_Application(atT_Change_Record.Factory, atT_Change_Record.Employee_ID, atT_Change_Record.Att_Date);
                                            if (Overtime_Application_Values == null)
                                                break;
                                            else
                                            {
                                                // # So sánh thời gian quẹt thẻ với file tham số làm thêm giờ và đưa ra thời gian làm thêm giờ thực tế
                                                // # Truy vấn tệp cài đặt tham số giờ làm thêm (nhà máy, ca làm việc, thời gian quẹt thẻ thực tế, ngày chấm công)
                                                var queryOvertimeParameterResult = await Query_HRMS_Att_Overtime_Parameter(
                                                    atT_Change_Record.Factory,
                                                    atT_Change_Record.Work_Shift_Type,
                                                    most_recent_swipe_Card_Values.hm,
                                                    atT_Change_Record.Att_Date
                                                );
                                                if (!queryOvertimeParameterResult.IsSuccess)
                                                {
                                                    await _repositoryAccessor.RollbackAsync();
                                                    return new OperationResult(false, queryOvertimeParameterResult.Error);
                                                }
                                                var Overtime_Parameter = queryOvertimeParameterResult.Data as HRMS_Att_Overtime_Parameter;
                                                var Att_Overtime_Temp_Values = new HRMS_Att_Overtime_Temp()
                                                {
                                                    USER_GUID = hrms_Emp_Personal.USER_GUID,
                                                    Factory = param.Factory,
                                                    Department = atT_Change_Record.Department,
                                                    Overtime_Date = atT_Change_Record.Att_Date,
                                                    Employee_ID = atT_Change_Record.Employee_ID,
                                                    Work_Shift_Type = atT_Change_Record.Work_Shift_Type,
                                                    Overtime_Start = Overtime_Application_Values.Overtime_Start,
                                                    Overtime_End = Overtime_Application_Values.Overtime_End,
                                                    Overtime_Hours = Overtime_Parameter?.Overtime_Hours ?? 0,
                                                    Night_Hours = Overtime_Parameter?.Night_Hours ?? 0,
                                                    Night_Overtime_Hours = 0,
                                                    Training_Hours = Overtime_Application_Values.Training_Hours,
                                                    Night_Eat_Times = Convert.ToInt16(Overtime_Application_Values.Night_Eat_Times),
                                                    Holiday = atT_Change_Record.Holiday,
                                                    Update_By = param.CurrentUser,
                                                    Update_Time = updateTime
                                                };

                                                // CALL Ins_HRMS_Att_Overtime_Temp - thêm dữ liệu tăng ca tạm
                                                var overTimeResult = await CRUD_Data(new ExceptionErrorData("Ins_HRMS_Att_Overtime_Temp", "2.2.10.2.2 ", new GeneralData(Att_Overtime_Temp_Values, atT_Change_Record)));
                                                if (!overTimeResult.IsSuccess)
                                                {
                                                    await _repositoryAccessor.RollbackAsync();
                                                    return new OperationResult(false, overTimeResult.Error);
                                                }

                                                // CALL Del_HRMS_Att_Change_Record  - Xoá dữ liệu chấm công
                                                var result = await CRUD_Data(new ExceptionErrorData("Del_HRMS_Att_Change_Record", "2.2.10.2.2", new GeneralData(atT_Change_Record)));
                                                if (!result.IsSuccess)
                                                {
                                                    await _repositoryAccessor.RollbackAsync();
                                                    return new OperationResult(false, result.Error);
                                                }
                                                // IF HRMS_Emp_Personal.Permission_Group IN Permission_Foreign_List AND HRMS_Emp_Personal.Employment_Status IS NOT NULL AND  Att_Change_Record.Clock_In != '0000'
                                                if (permission_Foreign_List.Contains(hrms_Emp_Personal.Permission_Group) &&
                                                    !string.IsNullOrWhiteSpace(hrms_Emp_Personal.Employment_Status) &&
                                                    atT_Change_Record.Clock_In.ToTimeSpan() != timeSpan0000)
                                                {
                                                    atT_Change_Record.Leave_Code = "00"; //#người nước ngoài
                                                    atT_Change_Record.Days = 0;
                                                    // CALL Ins_HRMS_Att_Change_Record(Att_Change_Record.*) - thêm dữ liệu chấm công
                                                    var _insChangeRecordResult = await CRUD_Data(new ExceptionErrorData("Ins_HRMS_Att_Change_Record", "2.2.10.2.2 ", new GeneralData(atT_Change_Record)));
                                                    if (!_insChangeRecordResult.IsSuccess)
                                                    {
                                                        await _repositoryAccessor.RollbackAsync();
                                                        return new OperationResult(false, _insChangeRecordResult.Error);
                                                    }
                                                }
                                                // CALL Ins_HRMS_Att_Temp_Record(Att_Change_Record.*) - Thêm dữ liệu chấm công tạm
                                                var _insTempResult = await CRUD_Data(new ExceptionErrorData("Ins_HRMS_Att_Temp_Record", "2.2.10.2.2 ", new GeneralData(atT_Change_Record)));
                                                if (!_insTempResult.IsSuccess)
                                                {
                                                    await _repositoryAccessor.RollbackAsync();
                                                    return new OperationResult(false, _insTempResult.Error);
                                                }
                                                processRecord += 1;
                                            }
                                        }
                                        break; // 2.2.10 EXIT CASE
                                    }

                                }
                                #endregion

                                #region 2.2.10.3
                                // Không quẹt thẻ tín dụng sau giờ làm:
                                // (1) Xác định thời gian quẹt thẻ thực tế 0000
                                // (2) Kiểm tra có thông tin nghỉ phép, nhập file tạm và file điểm danh hàng ngày

                                // IF Att_Change_Record.Clock_Out = "0000" AND 
                                // Att_Change_Record.Overtime_ClockIn = "0000" AND
                                // Att_Change_Record.Overtime_ClockOut = "0000" AND 
                                // HRMS_Emp_Personal.Permission_Group IN Permission_Local_List AND
                                // HRMS_Emp_Personal.Employment_Status IS NULL)
                                if (atT_Change_Record.Clock_Out.ToTimeSpan() == timeSpan0000 &&
                                    (atT_Change_Record.Overtime_ClockIn.IsTimeSpanFormat() ? atT_Change_Record.Overtime_ClockIn.ToTimeSpan() : null) == timeSpan0000 &&
                                    (atT_Change_Record.Overtime_ClockOut.IsTimeSpanFormat() ? atT_Change_Record.Overtime_ClockOut.ToTimeSpan() : null) == timeSpan0000 &&
                                    permission_Local_List.Contains(hrms_Emp_Personal.Permission_Group) &&
                                    string.IsNullOrWhiteSpace(hrms_Emp_Personal.Employment_Status))
                                {
                                    atT_Change_Record.Leave_Code = "04"; //04.Lỗi thẻ ngoài giờ làm việc
                                                                         // CALL Query_HRMS_Att_Leave_Maintain -- Lấy thông tin xin nghỉ phép
                                    var Att_Leave_Maintain_Values_04 = await Query_HRMS_Att_Leave_Maintain(atT_Change_Record.Factory, atT_Change_Record.Employee_ID, atT_Change_Record.Att_Date);
                                    // - Chưa xin phép
                                    if (Att_Leave_Maintain_Values_04 == null)
                                    {
                                        // - lấy dữ liệu nghỉ
                                        var Att_Leave_Application_Values = await Query_HRMS_Att_Leave_Application(atT_Change_Record.Factory, atT_Change_Record.Employee_ID, atT_Change_Record.Att_Date);
                                        if (Att_Leave_Application_Values != null)
                                        {
                                            atT_Change_Record.Leave_Code = Att_Leave_Application_Values.Leave_code;
                                            atT_Change_Record.Days = Math.Min(Att_Leave_Application_Values.Days, 1);
                                            // IF HRMS_Emp_Personal.Permission_Group IN Permission_Foreign_List AND  HRMS_Emp_Personal.Employment_Status IS NOT NULL AND  Att_Change_Record.Clock_In != '0000'
                                            if (permission_Foreign_List.Contains(hrms_Emp_Personal.Permission_Group) &&
                                                !string.IsNullOrWhiteSpace(hrms_Emp_Personal.Employment_Status) &&
                                                atT_Change_Record.Clock_In.ToTimeSpan() != timeSpan0000)
                                            {
                                                atT_Change_Record.Leave_Code = "00"; //-- người nc ngoài
                                                atT_Change_Record.Days = 0;

                                                // CALL Ins_HRMS_Att_Temp_Record(Att_Change_Record.*) - thêm dữ liệu chấm công tạm
                                                var insTempResult = await CRUD_Data(new ExceptionErrorData("Ins_HRMS_Att_Temp_Record", "2.2.10.3", new GeneralData(atT_Change_Record)));
                                                if (!insTempResult.IsSuccess)
                                                {
                                                    await _repositoryAccessor.RollbackAsync();
                                                    return new OperationResult(false, insTempResult.Error);
                                                }
                                                // CALL Ins_HRMS_Att_Change_Record(Att_Change_Record.*)
                                                var insChangeRecordResult = await CRUD_Data(new ExceptionErrorData("Ins_HRMS_Att_Change_Record", "2.2.10.3", new GeneralData(atT_Change_Record)));
                                                if (!insChangeRecordResult.IsSuccess)
                                                {
                                                    await _repositoryAccessor.RollbackAsync();
                                                    return new OperationResult(false, insChangeRecordResult.Error);
                                                }
                                                processRecord++;
                                            }
                                            else
                                            {
                                                // CALL Ins_HRMS_Att_Temp_Record(Att_Change_Record.*) - thêm dữ liệu chấm công tạm
                                                var insTempResult = await CRUD_Data(new ExceptionErrorData("Ins_HRMS_Att_Temp_Record", "2.2.10.3", new GeneralData(atT_Change_Record)));
                                                if (!insTempResult.IsSuccess)
                                                {
                                                    await _repositoryAccessor.RollbackAsync();
                                                    return new OperationResult(false, insTempResult.Error);
                                                }
                                                // CALL Upd_HRMS_Att_Yearly
                                                // Cập nhật số ngày cộng dồn vào file chấm công hàng năm
                                                var updateAttYearlyResult = await CRUD_Data(new ExceptionErrorData("Upd_HRMS_Att_Yearly", "2.2.10.3", new GeneralData(wk_date, atT_Change_Record)));
                                                if (!updateAttYearlyResult.IsSuccess)
                                                {
                                                    await _repositoryAccessor.RollbackAsync();
                                                    return new OperationResult(false, updateAttYearlyResult.Error);
                                                }
                                                processRecord++;
                                                break; // 2.2.10 EXIT CASE
                                            }
                                        }
                                        else
                                        {
                                            // CALL Ins_HRMS_Att_Temp_Record(Att_Change_Record.*) - thêm dữ liệu chấm công tạm
                                            var insTempResult = await CRUD_Data(new ExceptionErrorData("Ins_HRMS_Att_Temp_Record", "2.2.10.3", new GeneralData(atT_Change_Record)));
                                            if (!insTempResult.IsSuccess)
                                            {
                                                await _repositoryAccessor.RollbackAsync();
                                                return new OperationResult(false, insTempResult.Error);
                                            }
                                        }
                                    }
                                    else // -- Đã xin phép
                                    {
                                        atT_Change_Record.Leave_Code = Att_Leave_Maintain_Values_04.Leave_code;
                                        atT_Change_Record.Days = Math.Min(Att_Leave_Maintain_Values_04.Days, 1);
                                        // CALL Ins_HRMS_Att_Temp_Record(Att_Change_Record.*)
                                        var insTempResult = await CRUD_Data(new ExceptionErrorData("Ins_HRMS_Att_Temp_Record", "2.2.10.3 ", new GeneralData(atT_Change_Record)));
                                        if (!insTempResult.IsSuccess)
                                        {
                                            await _repositoryAccessor.RollbackAsync();
                                            return new OperationResult(false, insTempResult.Error);
                                        }
                                        else processRecord += 1;
                                        // CALL Ins_HRMS_Att_Change_Record(Att_Change_Record.*)
                                        var insChangeRecordResult = await CRUD_Data(new ExceptionErrorData("Ins_HRMS_Att_Change_Record", "2.2.10.3 ", new GeneralData(atT_Change_Record)));
                                        if (!insChangeRecordResult.IsSuccess)
                                        {
                                            await _repositoryAccessor.RollbackAsync();
                                            return new OperationResult(false, insChangeRecordResult.Error);
                                        }
                                        break; // 2.2.10 EXIT CASE
                                    }
                                }
                                #endregion

                                #region 2.2.10.4 
                                // Quẹt thẻ bất thường sau khi tan sở vào buổi chiều (Bấm thẻ sớm)
                                // IF Swipe_Card_Values.hm < Work_Shift_Values.Clock_Out AND
                                // HRMS_Emp_Personal.Permission_Group IN Permission_Local_List AND
                                // HRMS_Emp_Personal.Employment_Status IS NULL)
                                // Thời gian bấm thẻ trước thời gian tan ca quy định
                                if (most_recent_swipe_Card_Values.hm.ToTimeSpan() < work_Shift_Values_Clock_Out_TimeSpan &&
                                    permission_Local_List.Contains(hrms_Emp_Personal.Permission_Group) &&
                                    string.IsNullOrWhiteSpace(hrms_Emp_Personal.Employment_Status))
                                {
                                    // CALL Query_HRMS_Att_Leave_Maintain - lấy thông tin xin phép nghỉ
                                    var query_HRMS_Att_Leave_Maintain = await Query_HRMS_Att_Leave_Maintain(atT_Change_Record.Factory, atT_Change_Record.Employee_ID, atT_Change_Record.Att_Date);
                                    if (query_HRMS_Att_Leave_Maintain == null) // -- Chưa xin phép
                                    {
                                        // -- Lấy dữ liệu nghỉ
                                        var Att_Leave_Application_Values = await Query_HRMS_Att_Leave_Application(atT_Change_Record.Factory, atT_Change_Record.Employee_ID, atT_Change_Record.Att_Date);
                                        if (Att_Leave_Application_Values != null)
                                        {
                                            atT_Change_Record.Leave_Code = Att_Leave_Application_Values.Leave_code; // lí do nghỉ
                                            atT_Change_Record.Days = Math.Min(Att_Leave_Application_Values.Days, 1); // thời gian 1 ngày
                                        }
                                        else
                                        {
                                            // IF (HRMS_Emp_Personal.Permission_Group IN Permission_Local_List AND HRMS_Emp_Personal.Employment_Status IS NULL)
                                            if (permission_Local_List.Contains(hrms_Emp_Personal.Permission_Group) &&
                                                string.IsNullOrWhiteSpace(hrms_Emp_Personal.Employment_Status))
                                                atT_Change_Record.Leave_Code = "J3"; // nghỉ phép không lương J3
                                        }
                                    }
                                    else
                                    {
                                        atT_Change_Record.Leave_Code = query_HRMS_Att_Leave_Maintain.Leave_code;
                                        atT_Change_Record.Days = Math.Min(query_HRMS_Att_Leave_Maintain.Days, 1);
                                    }
                                    // CALL Ins_HRMS_Att_Temp_Record(Att_Change_Record.*)
                                    var insTempResult = await CRUD_Data(new ExceptionErrorData("Ins_HRMS_Att_Temp_Record", "2.2.10.4", new GeneralData(atT_Change_Record)));
                                    if (!insTempResult.IsSuccess)
                                    {
                                        await _repositoryAccessor.RollbackAsync();
                                        return new OperationResult(false, insTempResult.Error);
                                    }
                                    processRecord += 1;
                                    break; // 2.2.10 EXIT CASE
                                }
                                else
                                {
                                    // CALL Ins_HRMS_Att_Temp_Record(Att_Change_Record.*)
                                    var insTempResult = await CRUD_Data(new ExceptionErrorData("Ins_HRMS_Att_Temp_Record", "2.2.10.4", new GeneralData(atT_Change_Record)));
                                    if (!insTempResult.IsSuccess)
                                    {
                                        await _repositoryAccessor.RollbackAsync();
                                        return new OperationResult(false, insTempResult.Error);
                                    }

                                    // CALL Ins_HRMS_Att_Change_Record(Att_Change_Record.*)
                                    var insChangeRecordResult = await CRUD_Data(new ExceptionErrorData("Ins_HRMS_Att_Change_Record", "2.2.10.4", new GeneralData(atT_Change_Record)));
                                    if (!insChangeRecordResult.IsSuccess)
                                    {
                                        await _repositoryAccessor.RollbackAsync();
                                        return new OperationResult(false, insChangeRecordResult.Error);
                                    }

                                    // IF Att_Change_Record.Holiday  = "C05" ( Nếu là ngày nghỉ )
                                    if (atT_Change_Record.Holiday == "C05")
                                    {
                                        var att_Overtime_Temp_Values = new HRMS_Att_Overtime_Temp()
                                        {
                                            USER_GUID = hrms_Emp_Personal.USER_GUID,
                                            Factory = param.Factory,
                                            Department = atT_Change_Record.Department,
                                            Overtime_Date = atT_Change_Record.Att_Date,
                                            Employee_ID = atT_Change_Record.Employee_ID,
                                            Work_Shift_Type = atT_Change_Record.Work_Shift_Type,
                                            Overtime_Start = atT_Change_Record.Clock_In,
                                            Overtime_End = atT_Change_Record.Clock_Out,
                                            Overtime_Hours = 8,
                                            Night_Hours = 0,
                                            Night_Overtime_Hours = 0,
                                            Training_Hours = 0,
                                            Night_Eat_Times = 0,
                                            Holiday = atT_Change_Record.Holiday,
                                            Update_By = param.CurrentUser,
                                            Update_Time = updateTime
                                        };

                                        // CALL Ins_HRMS_Att_Overtime_Temp - thêm dữ liệu tăng ca tạm
                                        var overTimeResult = await CRUD_Data(new ExceptionErrorData("Ins_HRMS_Att_Overtime_Temp", "2.2.10.4 ", new GeneralData(att_Overtime_Temp_Values, atT_Change_Record)));
                                        if (!overTimeResult.IsSuccess)
                                        {
                                            await _repositoryAccessor.RollbackAsync();
                                            return new OperationResult(false, overTimeResult.Error);
                                        }
                                    }
                                }
                                #endregion

                                #region 2.2.10.5 Nhận giờ làm thêm
                                // IF Swipe_Card_Values.hm > Work_Shift_Values.Clock_Out OR  Swipe_Card_Values.hm < Work_Shift_Values.Clock_In THEN  --Ca thường xuyên và tăng ca trước khi làm việc
                                // Nếu giờ bấm > giờ tan ca hoặc giờ bấm < giờ vào ca (được tính là làm thêm giờ)
                                if (most_recent_swipe_Card_Values.hm.ToTimeSpan() > work_Shift_Values_Clock_Out_TimeSpan || most_recent_swipe_Card_Values.hm.ToTimeSpan() < work_Shift_Values_Clock_In_TimeSpan)
                                {
                                    // CALL Query_HRMS_Att_Overtime_Application -- lấy dữ liệu tăng ca
                                    var Overtime_Application_Values = await Query_HRMS_Att_Overtime_Application(atT_Change_Record.Factory, atT_Change_Record.Employee_ID, atT_Change_Record.Att_Date);
                                    if (Overtime_Application_Values == null)
                                        break;
                                    else // -- Có đăng ký tăng ca
                                    {
                                        var queryOvertimeParameterResult = await Query_HRMS_Att_Overtime_Parameter(
                                            atT_Change_Record.Factory,
                                            atT_Change_Record.Work_Shift_Type,
                                            most_recent_swipe_Card_Values.hm,
                                            atT_Change_Record.Att_Date
                                        );
                                        if (!queryOvertimeParameterResult.IsSuccess)
                                        {
                                            await _repositoryAccessor.RollbackAsync();
                                            return new OperationResult(false, queryOvertimeParameterResult.Error);
                                        }
                                        var Overtime_Parameter = queryOvertimeParameterResult.Data as HRMS_Att_Overtime_Parameter;
                                        var att_Overtime_Temp_Values = new HRMS_Att_Overtime_Temp()
                                        {
                                            USER_GUID = atT_Change_Record.USER_GUID,
                                            Factory = param.Factory,
                                            Department = atT_Change_Record.Department,
                                            Overtime_Date = atT_Change_Record.Att_Date,
                                            Employee_ID = atT_Change_Record.Employee_ID,
                                            Work_Shift_Type = atT_Change_Record.Work_Shift_Type,
                                            Overtime_Start = Overtime_Application_Values.Overtime_Start,
                                            Overtime_End = Overtime_Application_Values.Overtime_End,
                                            Overtime_Hours = Overtime_Parameter?.Overtime_Hours ?? 0,
                                            Night_Hours = Overtime_Parameter?.Night_Hours ?? 0,
                                            Night_Overtime_Hours = 0,
                                            Training_Hours = Overtime_Application_Values.Training_Hours,
                                            Night_Eat_Times = Convert.ToInt16(Overtime_Application_Values.Night_Eat_Times),
                                            Holiday = atT_Change_Record.Holiday,
                                            Update_By = param.CurrentUser,
                                            Update_Time = updateTime
                                        };

                                        // CALL Ins_HRMS_Att_Overtime_Temp
                                        var overTimeResult = await CRUD_Data(new ExceptionErrorData("Ins_HRMS_Att_Overtime_Temp", "2.2.10.5 ", new GeneralData(att_Overtime_Temp_Values)));
                                        if (!overTimeResult.IsSuccess)
                                        {
                                            await _repositoryAccessor.RollbackAsync();
                                            return new OperationResult(false, overTimeResult.Error);
                                        }
                                        else
                                        {
                                            // CALL Del_HRMS_Att_Change_Record
                                            var del_attChangeRecord_result = await CRUD_Data(new ExceptionErrorData("Del_HRMS_Att_Change_Record", "2.2.10.5", new GeneralData(atT_Change_Record)));
                                            if (!del_attChangeRecord_result.IsSuccess)
                                            {
                                                await _repositoryAccessor.RollbackAsync();
                                                return new OperationResult(false);
                                            }
                                            else
                                            {
                                                //  IF HRMS_Emp_Personal.Permission_Group IN Permission_Foreign_List AND 
                                                //  HRMS_Emp_Personal.Employment_Status IS NOT NULL AND 
                                                //  Att_Change_Record.Clock_In != '0000'
                                                if (permission_Foreign_List.Contains(hrms_Emp_Personal.Permission_Group) &&
                                                    !string.IsNullOrWhiteSpace(hrms_Emp_Personal.Employment_Status) &&
                                                    atT_Change_Record.Clock_In.ToTimeSpan() != timeSpan0000)
                                                {
                                                    atT_Change_Record.Leave_Code = "00"; // Đi lại bình thường (dành cho người nc ngoài)
                                                    atT_Change_Record.Days = 0;

                                                    // CALL Ins_HRMS_Att_Change_Record(Att_Change_Record.*)
                                                    var insChangeRecordResult = await CRUD_Data(new ExceptionErrorData("Ins_HRMS_Att_Change_Record", "2.2.10.5", new GeneralData(atT_Change_Record)));
                                                    if (!insChangeRecordResult.IsSuccess)
                                                    {
                                                        await _repositoryAccessor.RollbackAsync();
                                                        return new OperationResult(false, insChangeRecordResult.Error);
                                                    }
                                                }
                                                //--Lưu trữ tạm thời hàng ngày
                                                // CALL Ins_HRMS_Att_Temp_Record(Att_Change_Record.*)
                                                var insTempResult = await CRUD_Data(new ExceptionErrorData("Ins_HRMS_Att_Temp_Record", "2.2.10.5", new GeneralData(atT_Change_Record)));
                                                if (!insTempResult.IsSuccess)
                                                {
                                                    await _repositoryAccessor.RollbackAsync();
                                                    return new OperationResult(false, insTempResult.Error);
                                                }
                                                processRecord += 1;
                                            }
                                        }
                                    }
                                    // EXIT CASE --2.2.10 EXIT CASE
                                }

                                #endregion 2.2.10 EXIT CASE
                                break;
                        }

                        wk_date = wk_date.AddDays(1);
                        wk_md1 = wk_date.ToMonthDayString();
                        #endregion
                    }
                    #endregion EXIT 2.2
                }

                #endregion
                #region Đọc tệp dữ liệu quẹt thẻ và cập nhật dấu trạng thái của tệp quẹt thẻ="Y" để ngăn chặn lần thứ hai xảy ra trong cùng ngày.
                // (1) Thực hiện: Nhập ngày nghỉ, ngày làm, ngày nghỉ, ngày lễ bằng cách vuốt thẻ theo màn hình để xử lý dữ liệu chấm công, dữ liệu này được tạo mỗi ngày một lần cho nhà máy.
                // (2) Đọc tệp dữ liệu quẹt thẻ và cập nhật dấu trạng thái của tệp quẹt thẻ=’Y’ để ngăn chặn lần thứ hai xảy ra trong cùng ngày.
                var swipeCardToUpdateMarks = temp_Swipe_Cards.Where(x => x.Mark == "Y").ToList();
                if (swipeCardToUpdateMarks.Any())
                {
                    List<HRMS_Att_Swipe_Card> swipe_Cards = new();
                    foreach (var item in swipeCardToUpdateMarks)
                    {
                        var swcs = await _repositoryAccessor.HRMS_Att_Swipe_Card
                            .FirstOrDefaultAsync(x =>
                                x.Factory == item.Factory &&
                                x.Employee_ID == item.Empno_ID &&
                                x.Card_Date == item.Card_Date &&
                                x.Card_Time == item.Card_Time);
                        if (swcs != null && !swipe_Cards.Any(x => x.Factory == swcs.Factory && x.Employee_ID == swcs.Employee_ID && x.Card_Time == swcs.Card_Time))
                        {
                            swcs.Mark = item.Mark;
                            swipe_Cards.Add(swcs);
                        }
                    }
                    if (swipe_Cards.Any())
                    {
                        var updAttSwipeCard = await CRUD_Data(new ExceptionErrorData("Upd_HRMS_Att_Swipe_Card_List", "End of process", new GeneralData(swipe_Cards)));
                        if (!updAttSwipeCard.IsSuccess)
                        {
                            await _repositoryAccessor.RollbackAsync();
                            return new OperationResult(false, updAttSwipeCard.Error);
                        }
                    }
                }
                #endregion
                await _repositoryAccessor.CommitAsync();
                return new OperationResult(true) { Data = processRecord };
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


        /// <summary>
        /// Lấy danh sách nhân viên có đơn xin nghỉ và xin nghỉ phép
        /// từ ngày làm việc hiện tại cho tới ngày kết thúc làm việc (T7)
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="currentDate"> Ngày hiện tại </param>
        /// <param name="nationalHolidayStart"> Ngày bắt đầu nghỉ lễ</param>
        /// <param name="nationalHolidayEnd"> Ngày kết thúc nghỉ lễ </param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        private async Task<List<EmployyeeLeaveApplication>> GetEmployeesLeaveApplication(string factory, DateTime currentDate, string nationalHoliday, string currentUser)
        {
            var personalPredicate = PredicateBuilder.New<HRMS_Emp_Personal>(x =>
                                            (x.Factory == factory || x.Assigned_Factory == factory)//Nhân viên nhà máy địa phương hoặc nhân viên được cử đi
                                            && !x.Swipe_Card_Option  // Không cần quẹt thẻ
                                            && !x.Resign_Date.HasValue);

            // Đơn xin nghỉ từ ngày làm việc hiện tại cho tới ngày kết thúc làm việc
            var leaveApplicationPredicate = PredicateBuilder.New<HRMS_Att_Leave_Application>(x =>
                                            x.Factory == factory
                                            && x.Leave_Start.Date <= currentDate.Date
                                            && x.Leave_End.Date >= currentDate.Date);
            // danh sách nhân viên xin nghỉ phép
            var HEP = _repositoryAccessor.HRMS_Emp_Personal.FindAll(personalPredicate);
            var HALA = _repositoryAccessor.HRMS_Att_Leave_Application.FindAll(leaveApplicationPredicate);
            var data = await HALA.Join(HEP,
                        x => new { x.Factory, x.Employee_ID },
                        y => new { y.Factory, y.Employee_ID },
                        (x, y) => new { a = x, b = y })
                    .Union(HALA.Join(HEP,
                        x => new { x.Factory, x.Employee_ID },
                        y => new { y.Factory, Employee_ID = y.Assigned_Employee_ID },
                        (x, y) => new { a = x, b = y }))
                    .Union(HALA.Join(HEP,
                        x => new { x.Factory, x.Employee_ID },
                        y => new { Factory = y.Assigned_Factory, y.Employee_ID },
                        (x, y) => new { a = x, b = y }))
                    .Union(HALA.Join(HEP,
                        x => new { x.Factory, x.Employee_ID },
                        y => new { Factory = y.Assigned_Factory, Employee_ID = y.Assigned_Employee_ID },
                        (x, y) => new { a = x, b = y }))
                    .ToListAsync();
            var query = data.Select(x => new EmployyeeLeaveApplication()
            {
                USER_GUID = x.b.USER_GUID,
                Factory = x.a.Factory,
                Department = x.a.Department,
                Assigned_Department = x.b.Assigned_Department,
                Att_Date = currentDate,
                EmployeeId = x.a.Employee_ID,
                Work_Shift_Type = "",
                Leave_Code = x.a.Leave_code,
                Clock_In = "0000",
                Clock_Out = "0000",
                Overtime_ClockIn = "0000",
                Overtime_ClockOut = "0000",
                Days = 1,
                Holiday = IsNationalHoliday(currentDate.Date, nationalHoliday),
                Update_By = currentUser,
                Update_Time = DateTime.Now,
                EmploymentStatus = x.b.Employment_Status,
                Assigned_Employee_ID = x.b.Assigned_Employee_ID
            }).ToList();
            return query;
        }
        private static string IsNationalHoliday(DateTime currentDate, string nationalHoliday)
        {
            if (!string.IsNullOrWhiteSpace(nationalHoliday))
                return (currentDate.Date == nationalHoliday.ToDateTime().Date) ? "C00" : "";
            return ""; // Ngày bình thường
        }

        /// <summary>
        /// Lấy thông tin ngày lễ sẽ được tạo.
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        private async Task<bool> Get_Holiday_Flag(string factory)
        {
            var holidayFlag = await _repositoryAccessor.HRMS_Basic_Code.FirstOrDefaultAsync(
                            x => x.Type_Seq == BasicCodeTypeConstant.FactoryDoNotGenerateHolidayCommon
                             && x.Code == factory);
            return holidayFlag == null || holidayFlag.Char1 == null;
        }

        #region Data processing
        private async Task<ExceptionErrorData> CRUD_Data(ExceptionErrorData initial)
        {
            try
            {
                switch (initial.Name)
                {
                    case "Ins_HRMS_Att_Yearly":
                        var year_Date = new DateTime(initial.Data.wk_date.Year, 1, 1);
                        if (!await _repositoryAccessor.HRMS_Att_Yearly.AnyAsync(x =>
                           x.Division == initial.Data.HRMS_Att_Yearly.Division &&
                           x.Factory == initial.Data.HRMS_Att_Yearly.Factory &&
                           x.Att_Year == year_Date &&
                           x.Employee_ID == initial.Data.HRMS_Att_Yearly.Employee_ID))
                        {
                            var HAUML = await _repositoryAccessor.HRMS_Att_Use_Monthly_Leave.FindAll(x => x.Factory == initial.Data.HRMS_Att_Yearly.Factory).ToListAsync();
                            // Thời gian gần nhất
                            var maxEffectiveMonth_Leave = HAUML.Where(x => x.Leave_Type == "1" && x.Effective_Month <= initial.Data.wk_date).Max(x => x.Effective_Month);
                            var maxEffectiveMonth_Allowance = HAUML.Where(x => x.Leave_Type == "2" && x.Effective_Month <= initial.Data.wk_date).Max(x => x.Effective_Month);
                            List<HRMS_Att_Yearly> addList_HAY = new();
                            // Leave
                            addList_HAY.AddRange(HAUML
                                .Where(x =>
                                    x.Leave_Type == "1" &&
                                    x.Effective_Month == maxEffectiveMonth_Leave &&
                                    x.Year_Total.HasValue &&
                                    x.Year_Total.Value == true)
                                .Select(x => new HRMS_Att_Yearly()
                                {
                                    USER_GUID = initial.Data.HRMS_Att_Yearly.USER_GUID,
                                    Division = initial.Data.HRMS_Att_Yearly.Division,
                                    Factory = x.Factory,
                                    Att_Year = year_Date,
                                    Employee_ID = initial.Data.HRMS_Att_Yearly.Employee_ID,
                                    Leave_Code = x.Code,
                                    Leave_Type = x.Leave_Type,
                                    Days = 0,
                                    Update_By = initial.Data.HRMS_Att_Yearly.Update_By,
                                    Update_Time = initial.Data.HRMS_Att_Yearly.Update_Time,
                                }).ToList());
                            // Allowance
                            addList_HAY.AddRange(HAUML
                                .Where(x =>
                                    x.Leave_Type == "2" &&
                                    x.Effective_Month == maxEffectiveMonth_Allowance &&
                                    x.Year_Total.HasValue &&
                                    x.Year_Total.Value == true)
                                .Select(x => new HRMS_Att_Yearly()
                                {
                                    USER_GUID = initial.Data.HRMS_Att_Yearly.USER_GUID,
                                    Division = initial.Data.HRMS_Att_Yearly.Division,
                                    Factory = x.Factory,
                                    Att_Year = year_Date,
                                    Employee_ID = initial.Data.HRMS_Att_Yearly.Employee_ID,
                                    Leave_Code = x.Code,
                                    Leave_Type = x.Leave_Type,
                                    Days = 0,
                                    Update_By = initial.Data.HRMS_Att_Yearly.Update_By,
                                    Update_Time = initial.Data.HRMS_Att_Yearly.Update_Time,
                                }).ToList());
                            if (addList_HAY.Any())
                            {
                                _repositoryAccessor.HRMS_Att_Yearly.AddMultiple(addList_HAY);
                                initial.IsSuccess = await _repositoryAccessor.Save();
                            }
                        }
                        break;

                    case "Ins_HRMS_Att_Temp_Record":
                        if (!await _repositoryAccessor.HRMS_Att_Temp_Record.AnyAsync(x =>
                            x.Factory == initial.Data.HRMS_Att_Change_Record.Factory &&
                            x.Att_Date == initial.Data.HRMS_Att_Change_Record.Att_Date &&
                            x.Employee_ID == initial.Data.HRMS_Att_Change_Record.Employee_ID))
                        {
                            var tempRecord = new HRMS_Att_Temp_Record()
                            {
                                Factory = initial.Data.HRMS_Att_Change_Record.Factory,
                                Att_Date = initial.Data.HRMS_Att_Change_Record.Att_Date,
                                Employee_ID = initial.Data.HRMS_Att_Change_Record.Employee_ID,
                                Department = initial.Data.HRMS_Att_Change_Record.Department,
                                USER_GUID = initial.Data.HRMS_Att_Change_Record.USER_GUID,
                                Work_Shift_Type = initial.Data.HRMS_Att_Change_Record.Work_Shift_Type,
                                Leave_Code = initial.Data.HRMS_Att_Change_Record.Leave_Code,
                                Clock_In = initial.Data.HRMS_Att_Change_Record.Clock_In,
                                Clock_Out = initial.Data.HRMS_Att_Change_Record.Clock_Out,
                                Overtime_ClockIn = initial.Data.HRMS_Att_Change_Record.Overtime_ClockIn,
                                Overtime_ClockOut = initial.Data.HRMS_Att_Change_Record.Overtime_ClockOut,
                                Days = initial.Data.HRMS_Att_Change_Record.Days,
                                Holiday = initial.Data.HRMS_Att_Change_Record.Holiday,
                                Update_By = initial.Data.HRMS_Att_Change_Record.Update_By,
                                Update_Time = initial.Data.HRMS_Att_Change_Record.Update_Time,
                            };

                            _repositoryAccessor.HRMS_Att_Temp_Record.Add(tempRecord);
                            initial.IsSuccess = await _repositoryAccessor.Save();
                        }
                        break;
                    case "Ins_HRMS_Att_Change_Record":
                        if (!await _repositoryAccessor.HRMS_Att_Change_Record.AnyAsync(x =>
                            x.Factory == initial.Data.HRMS_Att_Change_Record.Factory &&
                            x.Att_Date == initial.Data.HRMS_Att_Change_Record.Att_Date &&
                            x.Employee_ID == initial.Data.HRMS_Att_Change_Record.Employee_ID))
                        {
                            var changeRecord = new HRMS_Att_Change_Record()
                            {
                                Factory = initial.Data.HRMS_Att_Change_Record.Factory,
                                Att_Date = initial.Data.HRMS_Att_Change_Record.Att_Date,
                                Employee_ID = initial.Data.HRMS_Att_Change_Record.Employee_ID,
                                Department = initial.Data.HRMS_Att_Change_Record.Department,
                                USER_GUID = initial.Data.HRMS_Att_Change_Record.USER_GUID,
                                Work_Shift_Type = initial.Data.HRMS_Att_Change_Record.Work_Shift_Type,
                                Leave_Code = initial.Data.HRMS_Att_Change_Record.Leave_Code,
                                Clock_In = initial.Data.HRMS_Att_Change_Record.Clock_In,
                                Clock_Out = initial.Data.HRMS_Att_Change_Record.Clock_Out,
                                Overtime_ClockIn = initial.Data.HRMS_Att_Change_Record.Overtime_ClockIn,
                                Overtime_ClockOut = initial.Data.HRMS_Att_Change_Record.Overtime_ClockOut,
                                Days = initial.Data.HRMS_Att_Change_Record.Days,
                                Holiday = initial.Data.HRMS_Att_Change_Record.Holiday,
                                Update_By = initial.Data.HRMS_Att_Change_Record.Update_By,
                                Update_Time = initial.Data.HRMS_Att_Change_Record.Update_Time,
                            };
                            _repositoryAccessor.HRMS_Att_Change_Record.Add(changeRecord);
                            initial.IsSuccess = await _repositoryAccessor.Save();
                        }
                        break;
                    case "Ins_HRMS_Att_Overtime_Temp":
                        if (!await _repositoryAccessor.HRMS_Att_Overtime_Temp.AnyAsync(x =>
                            x.Factory == initial.Data.HRMS_Att_Overtime_Temp.Factory &&
                            x.Overtime_Date.Date == initial.Data.HRMS_Att_Overtime_Temp.Overtime_Date.Date &&
                            x.Employee_ID == initial.Data.HRMS_Att_Overtime_Temp.Employee_ID))
                        {
                            _repositoryAccessor.HRMS_Att_Overtime_Temp.Add(initial.Data.HRMS_Att_Overtime_Temp);
                            initial.IsSuccess = await _repositoryAccessor.Save();
                        }
                        break;
                    case "Ins_HRMS_Att_Temp_Record_List":
                        _repositoryAccessor.HRMS_Att_Temp_Record.AddMultiple(initial.Data.HRMS_Att_Temp_Record_List);
                        initial.IsSuccess = await _repositoryAccessor.Save();
                        break;
                    case "Upd_HRMS_Att_Yearly":
                        // Cập nhật số ngày cộng dồn vào file chấm công hàng năm
                        var dateYearly = await _repositoryAccessor.HRMS_Att_Yearly.FirstOrDefaultAsync(x =>
                            x.Factory == initial.Data.HRMS_Att_Change_Record.Factory &&
                            x.Att_Year == new DateTime(initial.Data.wk_date.Year, 1, 1) &&
                            x.Employee_ID == initial.Data.HRMS_Att_Change_Record.Employee_ID &&
                            x.USER_GUID == initial.Data.HRMS_Att_Change_Record.USER_GUID &&
                            x.Leave_Type == "1" &&
                            x.Leave_Code == initial.Data.HRMS_Att_Change_Record.Leave_Code);
                        if (dateYearly != null)
                        {
                            dateYearly.Days += initial.Data.HRMS_Att_Change_Record.Days;
                            dateYearly.Update_By = initial.Data.HRMS_Att_Change_Record.Update_By;
                            dateYearly.Update_Time = initial.Data.HRMS_Att_Change_Record.Update_Time;
                            _repositoryAccessor.HRMS_Att_Yearly.Update(dateYearly);
                            initial.IsSuccess = await _repositoryAccessor.Save();
                        }
                        break;
                    case "Upd_HRMS_Emp_Personal":
                        _repositoryAccessor.HRMS_Emp_Personal.Update(initial.Data.HRMS_Emp_Personal);
                        initial.IsSuccess = await _repositoryAccessor.Save();
                        break;
                    case "Upd_HRMS_Att_Work_Shift_Change":
                        var affectedChanges = await _repositoryAccessor.HRMS_Att_Work_Shift_Change.FirstOrDefaultAsync(x =>
                            x.Effective_Date == initial.Data.wk_date &&
                            x.Factory == initial.Data.HRMS_Att_Change_Record.Factory &&
                            x.USER_GUID == initial.Data.HRMS_Att_Change_Record.USER_GUID);
                        if (affectedChanges != null)
                        {
                            affectedChanges.Effective_State = true;
                            _repositoryAccessor.HRMS_Att_Work_Shift_Change.Update(affectedChanges);
                            initial.IsSuccess = await _repositoryAccessor.Save();
                        }
                        break;
                    case "Upd_HRMS_Att_Swipe_Card_List":
                        _repositoryAccessor.HRMS_Att_Swipe_Card.UpdateMultiple(initial.Data.HRMS_Att_Swipe_Card_List);
                        initial.IsSuccess = await _repositoryAccessor.Save();
                        break;
                    case "Del_HRMS_Att_Change_Record":
                        var attChangeRecordItem = await _repositoryAccessor.HRMS_Att_Change_Record.FirstOrDefaultAsync(x =>
                            x.Factory == initial.Data.HRMS_Att_Change_Record.Factory &&
                            x.Employee_ID == initial.Data.HRMS_Att_Change_Record.Employee_ID &&
                            x.Att_Date.Date == initial.Data.HRMS_Att_Change_Record.Att_Date.Date);
                        if (attChangeRecordItem != null)
                        {
                            _repositoryAccessor.HRMS_Att_Change_Record.Remove(attChangeRecordItem);
                            initial.IsSuccess = await _repositoryAccessor.Save();
                        }
                        break;
                    case "Ins_HRMS_Att_Yearly_Code":
                        var year_date = new DateTime(initial.Data.wk_date.Year, 1, 1);
                        DateTime month_date = DateTime.Parse($"{initial.Data.wk_date.Year}/{initial.Data.wk_date.Month}/01");
                        var HAUMLs = _repositoryAccessor.HRMS_Att_Use_Monthly_Leave.FindAll(x => x.Factory == initial.Data.HRMS_Att_Yearly.Factory);
                        var HAY = _repositoryAccessor.HRMS_Att_Yearly.FindAll(x =>
                            x.Division == initial.Data.HRMS_Att_Yearly.Division &&
                            x.Factory == initial.Data.HRMS_Att_Yearly.Factory &&
                            x.Att_Year == year_date &&
                            x.Employee_ID == initial.Data.HRMS_Att_Yearly.Employee_ID
                        );
                        var leave_HAUML = HAUMLs.Where(x => x.Leave_Type == "1");
                        if (!await leave_HAUML.AnyAsync(x => x.Effective_Month.Date == month_date.Date))
                            break;
                        List<HRMS_Att_Yearly> addList = new();
                        var before_Month = leave_HAUML.Where(x => x.Effective_Month.Date < month_date.Date).Max(x => x.Effective_Month);
                        var codeList_Leave = leave_HAUML.Where(x => x.Effective_Month.Date == before_Month.Date).Select(x => x.Code).ToList();
                        addList.AddRange(leave_HAUML
                            .Where(x =>
                                x.Effective_Month.Date == month_date.Date &&
                                x.Year_Total.HasValue &&
                                x.Year_Total.Value &&
                                !codeList_Leave.Contains(x.Code))
                            .GroupJoin(HAY,
                                x => new { x.Leave_Type, x.Code },
                                y => new { y.Leave_Type, Code = y.Leave_Code },
                                (x, y) => new { HAUML = x, HAY = y })
                            .SelectMany(x => x.HAY.DefaultIfEmpty(),
                                (x, y) => new { x.HAUML, HAY = y })
                            .Where(x => x.HAY == null)
                            .Select(x => new HRMS_Att_Yearly
                            {
                                USER_GUID = initial.Data.HRMS_Att_Yearly.USER_GUID,
                                Division = initial.Data.HRMS_Att_Yearly.Division,
                                Factory = initial.Data.HRMS_Att_Yearly.Factory,
                                Att_Year = year_date,
                                Employee_ID = initial.Data.HRMS_Att_Yearly.Employee_ID,
                                Leave_Code = x.HAUML.Code,
                                Leave_Type = x.HAUML.Leave_Type,
                                Days = 0,
                                Update_By = initial.Data.HRMS_Att_Yearly.Update_By,
                                Update_Time = initial.Data.HRMS_Att_Yearly.Update_Time,
                            }));
                        var attendace_HAUML = HAUMLs.Where(x => x.Leave_Type == "2");
                        var codeList_Attendance = attendace_HAUML.Where(x => x.Effective_Month.Date == before_Month.Date).Select(x => x.Code).ToList();
                        addList.AddRange(attendace_HAUML
                            .Where(x =>
                                x.Effective_Month.Date == month_date.Date &&
                                x.Year_Total.HasValue &&
                                x.Year_Total.Value &&
                                !codeList_Attendance.Contains(x.Code))
                            .GroupJoin(HAY,
                                x => new { x.Leave_Type, x.Code },
                                y => new { y.Leave_Type, Code = y.Leave_Code },
                                (x, y) => new { HAUML = x, HAY = y })
                            .SelectMany(x => x.HAY.DefaultIfEmpty(),
                                (x, y) => new { x.HAUML, HAY = y })
                            .Where(x => x.HAY == null)
                            .Select(x => new HRMS_Att_Yearly
                            {
                                USER_GUID = initial.Data.HRMS_Att_Yearly.USER_GUID,
                                Division = initial.Data.HRMS_Att_Yearly.Division,
                                Factory = initial.Data.HRMS_Att_Yearly.Factory,
                                Att_Year = year_date,
                                Employee_ID = initial.Data.HRMS_Att_Yearly.Employee_ID,
                                Leave_Code = x.HAUML.Code,
                                Leave_Type = x.HAUML.Leave_Type,
                                Days = 0,
                                Update_By = initial.Data.HRMS_Att_Yearly.Update_By,
                                Update_Time = initial.Data.HRMS_Att_Yearly.Update_Time,
                            }));
                        if (addList.Any())
                        {
                            _repositoryAccessor.HRMS_Att_Yearly.AddMultiple(addList);
                            initial.IsSuccess = await _repositoryAccessor.Save();
                        }
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
                    initial.Error = $"{initial.Location} - " + (initial.Name switch
                    {
                        "Ins_HRMS_Att_Yearly" => "Insert HRMS_Att_Yearly Fail!",
                        "Ins_HRMS_Att_Temp_Record" => "Insert HRMS_Att_Temp_Record Fail!",
                        "Ins_HRMS_Att_Change_Record" => "Insert HRMS_Att_Change_Record Fail!",
                        "Ins_HRMS_Att_Overtime_Temp" => "Insert HRMS_Att_Overtime_Temp Fail!",
                        "Ins_HRMS_Att_Temp_Record_List" => "Insert HRMS_Att_Temp_Record Fail!",
                        "Ins_HRMS_Att_Yearly_Code" => "INSERT HRMS_Att_Yearly Fail!",
                        "Upd_HRMS_Att_Yearly" => "Update HRMS_Att_Yearly Fail!",
                        "Upd_HRMS_Emp_Personal" => "Update Upd_HRMS_Emp_Personal Fail!",
                        "Upd_HRMS_Att_Work_Shift_Change" => "Update HRMS_Att_Work_Shift_Change Fail!",
                        "Upd_HRMS_Att_Swipe_Card_List" => "Update HRMS_Att_Swipe_Card Fail!",
                        "Del_HRMS_Att_Change_Record" => "Delete HRMS_Att_Change_Record Fail!",
                        _ => "Insert Fail!"
                    });
            }
        }
        #endregion

        #region Query Documents
        private async Task<string> Query_Work_Shift_Change(string factory, string employeeId, string currentWorkShift, DateTime date)
        {
            var effectives = await _repositoryAccessor.HRMS_Att_Work_Shift_Change
                            .FindAll(x => x.Effective_Date.Date >= date.Date
                                        && x.Factory == factory
                                        && x.Employee_ID == employeeId, true)
                            .Select(x => new { x.Effective_Date, x.Work_Shift_Type_New, x.Work_Shift_Type_Old })
                            .ToListAsync();
            // .MinAsync(x => x.Effective_Date);

            if (effectives.Any())
            {
                // Lấy Min effectiveDate
                var effectiveDate = effectives.Min(x => x.Effective_Date);


                var workShift = effectives.Where(x => x.Effective_Date == effectiveDate).FirstOrDefault();
                return workShift != null ? (effectiveDate.Date == date.Date ? workShift.Work_Shift_Type_New : workShift.Work_Shift_Type_Old) : currentWorkShift;
            }
            return currentWorkShift;
        }

        private async Task<HRMS_Att_Work_Shift> Query_HRMS_Att_Work_Shift(string factory, string workShiftType, string weekDay)
        {
            var result = await _repositoryAccessor.HRMS_Att_Work_Shift.FirstOrDefaultAsync(x => x.Factory == factory && x.Work_Shift_Type == workShiftType && x.Week == weekDay);
            return result;
        }

        private async Task<HRMS_Att_Leave_Maintain> Query_HRMS_Att_Leave_Maintain(string factory, string employeeId, DateTime leave_Date) =>
            await _repositoryAccessor.HRMS_Att_Leave_Maintain.FirstOrDefaultAsync(x => x.Factory == factory && x.Employee_ID == employeeId && x.Leave_Date.Date == leave_Date.Date);

        private async Task<HRMS_Att_Overtime_Application> Query_HRMS_Att_Overtime_Application(string factory, string employeeId, DateTime overTime_Date) =>
            await _repositoryAccessor.HRMS_Att_Overtime_Application.FirstOrDefaultAsync(x => x.Factory == factory && x.Employee_ID == employeeId && x.Overtime_Date.Date == overTime_Date.Date);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="timeSpan0000">Thời gian buổi tối 00:00 </param>
        /// <param name="timeSpan1200"> Thời gian buổi trưa  12:00</param>
        /// <returns></returns>
        private HRMS_Att_Work_Shift HRMS_Att_Work_Shift_Add24_hours(HRMS_Att_Work_Shift Work_Shift_Values)
        {
            try
            {
                var clockInTimeSpan = Work_Shift_Values.Clock_In.ToTimeSpan();
                var clockOutTimeSpan = Work_Shift_Values.Clock_Out.ToTimeSpan();
                var overtime_ClockInTimeSpan = Work_Shift_Values.Overtime_ClockIn.IsTimeSpanFormat() ? Work_Shift_Values.Overtime_ClockIn.ToTimeSpan() : (TimeSpan?)null;
                var overtime_ClockOutTimeSpan = Work_Shift_Values.Overtime_ClockOut.IsTimeSpanFormat() ? Work_Shift_Values.Overtime_ClockOut.ToTimeSpan() : (TimeSpan?)null;
                var lunch_startTimeSpan = Work_Shift_Values.Lunch_Start.ToTimeSpan();
                var lunch_endTimeSpan = Work_Shift_Values.Lunch_End.ToTimeSpan();

                // Clock In - giờ vào ca
                if (clockInTimeSpan >= timeSpan0000 && clockInTimeSpan <= timeSpan1200)
                    Work_Shift_Values.Clock_In = clockInTimeSpan.Add(new TimeSpan(24, 0, 0)).ToHourMinuteString();
                //  Clock Out - giờ ra ca
                if (clockOutTimeSpan >= timeSpan0000 && clockOutTimeSpan <= timeSpan1200)
                    Work_Shift_Values.Clock_Out = clockOutTimeSpan.Add(new TimeSpan(24, 0, 0)).ToHourMinuteString();
                // Overtime_ClockIn - giờ vào tăng ca
                if (overtime_ClockInTimeSpan.HasValue && overtime_ClockInTimeSpan >= timeSpan0000 && overtime_ClockInTimeSpan <= timeSpan1200)
                    Work_Shift_Values.Overtime_ClockIn = overtime_ClockInTimeSpan.Value.Add(new TimeSpan(24, 0, 0)).ToHourMinuteString();
                // Overtime_ClockOut - giờ ra tăng ca
                if (overtime_ClockOutTimeSpan >= timeSpan0000 && overtime_ClockOutTimeSpan <= timeSpan1200)
                    Work_Shift_Values.Overtime_ClockOut = overtime_ClockOutTimeSpan.Value.Add(new TimeSpan(24, 0, 0)).ToHourMinuteString();
                // Lunch Start - giờ vào ăn trưa
                if (lunch_startTimeSpan >= timeSpan0000 && lunch_startTimeSpan <= timeSpan1200)
                    Work_Shift_Values.Lunch_Start = lunch_startTimeSpan.Add(new TimeSpan(24, 0, 0)).ToHourMinuteString();
                // Lunch End - giờ két thúc ăn trưa
                if (lunch_endTimeSpan >= timeSpan0000 && lunch_endTimeSpan <= timeSpan1200)
                    Work_Shift_Values.Lunch_End = lunch_endTimeSpan.Add(new TimeSpan(24, 0, 0)).ToHourMinuteString();

                return Work_Shift_Values;
            }
            catch (Exception)
            {
                return null;
            }
        }


        private async Task<HRMS_Att_Leave_Application> Query_HRMS_Att_Leave_Application(string factory, string employeeId, DateTime att_Date) =>
            await _repositoryAccessor.HRMS_Att_Leave_Application.FirstOrDefaultAsync(x => x.Factory == factory && x.Employee_ID == employeeId && x.Leave_Start.Date <= att_Date.Date && x.Leave_End.Date >= att_Date.Date);


        private async Task<HRMS_Att_Change_Record> Query_HRMS_Att_Change_Record(string factory, string employeeId, DateTime att_Date) =>
            await _repositoryAccessor.HRMS_Att_Change_Record.FirstOrDefaultAsync(x => x.Factory == factory && x.Att_Date.Date == att_Date.Date && x.Employee_ID == employeeId);

        private async Task<OperationResult> Query_HRMS_Att_Overtime_Parameter(string factory, string workShiftType, string swipeCardTime, DateTime attDate)
        {
            try
            {
                var HAOP = await _repositoryAccessor.HRMS_Att_Overtime_Parameter.FindAll(x =>
                               x.Factory == factory &&
                               x.Work_Shift_Type == workShiftType &&
                               swipeCardTime.ToTimeSpan() >= x.Overtime_Start.ToTimeSpan() &&
                               swipeCardTime.ToTimeSpan() <= x.Overtime_End.ToTimeSpan() &&
                               x.Effective_Month.Date <= attDate.Date
                           ).OrderByDescending(x => x.Effective_Month).FirstOrDefaultAsync();
                return new OperationResult(true, HAOP);
            }
            catch (Exception)
            {
                return new OperationResult(false, $"Query_HRMS_Att_Overtime_Parameter Fail! \nFactory: {factory}\nWork Shift Type: {workShiftType}\nAtt_Date: {attDate}\nSwipe Card Time:{swipeCardTime}");
            }
        }
        #endregion
    }
}