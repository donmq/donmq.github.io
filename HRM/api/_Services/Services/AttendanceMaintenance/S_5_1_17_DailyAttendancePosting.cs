using AgileObjects.AgileMapper;
using API.Data;
using API._Services.Interfaces.AttendanceMaintenance;
using API.Helper.Constant;
using API.Helper.Utilities;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.AttendanceMaintenance
{
    public class S_5_1_17_DailyAttendancePosting : BaseServices, I_5_1_17_DailyAttendancePosting
    {
        private static readonly SemaphoreSlim semaphore = new(1, 1);
        private readonly TimeSpan timeSpan0000 = "0000".ToTimeSpan();
        public S_5_1_17_DailyAttendancePosting(DBContext dbContext) : base(dbContext)
        {
        }

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

        public async Task<OperationResult> Execute(string factory, string userName)
        {
            await semaphore.WaitAsync();
            await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                #region 1.1 Lấy thông tin dữ liệu

                HRMS_Att_Posting att_Posting_Values = new();
                var predHACR = PredicateBuilder.New<HRMS_Att_Change_Record>(x => !string.IsNullOrEmpty(x.Employee_ID));
                if (!string.IsNullOrWhiteSpace(factory)) predHACR.And(x => x.Factory == factory);

                var today = DateTime.Today;
                var HACR = await _repositoryAccessor.HRMS_Att_Change_Record.FindAll(predHACR, true).ToListAsync();
                if (!HACR.Any())
                    att_Posting_Values.Att_Date = today;
                att_Posting_Values.Att_Date = HACR.Max(x => x.Att_Date);

                if (att_Posting_Values.Att_Date > today)
                    att_Posting_Values.Att_Date = today;

                // lấy năm, lấy  ngày đầu tháng và cuối tháng
                DateTime year_Posting = new(att_Posting_Values.Att_Date.Year, 1, 1);
                DateTime month_First_date = new(att_Posting_Values.Att_Date.Year, att_Posting_Values.Att_Date.Month, 1);
                DateTime month_Last_date = new(att_Posting_Values.Att_Date.Year, att_Posting_Values.Att_Date.Month, DateTime.DaysInMonth(att_Posting_Values.Att_Date.Year, att_Posting_Values.Att_Date.Month));

                #endregion

                #region 1.2 Lấy danh sách dữ liệu nghỉ , không phép, vắng
                var listLeaveCode = await _repositoryAccessor.HRMS_Basic_Code
                                .FindAll(b => b.Type_Seq == BasicCodeTypeConstant.Leave && b.Char1 == "Leave")
                                .Select(b => b.Code)
                                .ToListAsync();
                #endregion

                #region 1.3 Đọc kho lưu trữ tạm thời, tăng ca ngoài giờ


                var cr_pt1 = await _repositoryAccessor.HRMS_Att_Temp_Record
                                .FindAll(r => r.Factory == factory)
                                .OrderBy(r => r.Employee_ID)
                                .ThenBy(r => r.Att_Date)
                                .ToListAsync();

                var countHATR = 0;
                foreach (var temp_Record_Values in cr_pt1)
                {
                    countHATR++;

                    #region 1.3.1 Thông tin dữ liệu tạm thời 
                    // Nếu [lý do] dữ liệu chấm công tạm nằm trong danh sách [ Lý do ]
                    if (listLeaveCode.Contains(temp_Record_Values.Leave_Code))
                    {

                        // L0: Trễ 
                        // L1: Về sớm sẽ không được đưa vào hồ sơ bảo trì nghỉ phép
                        if (temp_Record_Values.Leave_Code != "L0" && temp_Record_Values.Leave_Code != "L1")
                            // thêm vào bảng HRMS AttLeave Maintain
                            if (!await InsHRMSAttLeaveMaintain(temp_Record_Values, userName))
                            {
                                await _repositoryAccessor.RollbackAsync();
                                return new OperationResult(false, "AttendanceMaintenance.DailyAttendancePosting.InsHRMSAttLeaveMaintainFail");
                            }

                        #region 1.3.2 Xử lý thông tin nghỉ việc & Cập nhật thông tin nhân viên
                        // kiểm tra emp vào thêm lại dữ liệu mới
                        HRMS_Emp_Personal dataHEP = await _repositoryAccessor.HRMS_Emp_Personal.FirstOrDefaultAsync(x =>
                                                                x.USER_GUID == temp_Record_Values.USER_GUID &&
                                                                x.Factory == temp_Record_Values.Factory, true);

                        if (dataHEP != null)
                        {
                            // Nếu nhân viên vắng mặt [C0]
                            if (temp_Record_Values.Leave_Code == "C0")
                            {
                                // Lấy thông tin ngày nghỉ vắng mặt của nhân viên
                                var dates = _repositoryAccessor.HRMS_Att_Leave_Maintain
                                                    .FindAll(x => x.Factory == temp_Record_Values.Factory &&
                                                                x.Employee_ID == temp_Record_Values.Employee_ID &&
                                                                x.Leave_Date >= month_First_date &&
                                                                x.Leave_Date <= month_Last_date
                                                                && x.Leave_code == "C0")
                                                    .OrderBy(x => x.Leave_Date)
                                                    .ToList();
                                // Tính số ngày nghỉ liên tiếp của nhân viên
                                var calResp = CalculateContinuousDays(dates, month_Last_date);

                                decimal count_days = calResp.Item1;
                                // Nếu Nghỉ 5 ngày liên tiếp
                                if (count_days >= 5)
                                {
                                    // Số ngày nghỉ cao nhất
                                    decimal wk_code = calResp.Item2;

                                    // D203: Tổng số ngày nghỉ làm 5 ngày/tháng hoặc tổng số ngày nghỉ làm 20 ngày/năm
                                    // D204: đơn phương chấm dứt hợp đồng
                                    // Thông tin nhân viên [ Từ chức - nghỉ việc ]
                                    HRMS_Emp_Resignation emp_Resignation = new()
                                    {
                                        Resign_Date = count_days > 5 ? today : today.AddDays(1),
                                        Resign_Reason = wk_code >= 5 ? "D204" : "D203",
                                        USER_GUID = temp_Record_Values.USER_GUID,
                                        Division = dataHEP.Division,
                                        Factory = temp_Record_Values.Factory,
                                        Employee_ID = temp_Record_Values.Employee_ID,
                                        Nationality = dataHEP.Nationality,
                                        Identification_Number = dataHEP.Identification_Number,
                                        Onboard_Date = dataHEP.Onboard_Date,
                                        Resignation_Type = "D",
                                        Remark = null,
                                        Blacklist = false,
                                        Verifier = null,
                                        Verifier_Name = null,
                                        Verifier_Title = null,
                                        Update_By = userName,
                                        Update_Time = DateTime.Now
                                    };
                                    // số lượng từ chức
                                    if (!await InsHRMSEmpResignation(emp_Resignation))
                                    {
                                        await _repositoryAccessor.RollbackAsync();
                                        return new OperationResult(false, "AttendanceMaintenance.DailyAttendancePosting.InsHRMSEmpResignationFail");
                                    }

                                    // Cập nhật lịch sử Nhân viên
                                    if (!await UpdateHRMSEmpIDcardEmpIDHistory(emp_Resignation))
                                    {
                                        await _repositoryAccessor.RollbackAsync();
                                        return new OperationResult(false, "AttendanceMaintenance.DailyAttendancePosting.UpdateHRMSEmpIDcardEmpIDHistoryFail");
                                    }

                                    // Cập nhật lại thông tin nhân viên 
                                    if (!await UpdateHRMSEmpPersonal(emp_Resignation))
                                    {
                                        await _repositoryAccessor.RollbackAsync();
                                        return new OperationResult(false, "AttendanceMaintenance.DailyAttendancePosting.UpdateHRMSEmpPersonalFail");
                                    }
                                }
                            }
                        }
                        #endregion
                    }

                    #endregion

                    #region 1.3.3 Cập nhật thông tin điểm danh nhân viên

                    // CALL Query_HRMS_Att_Change_Record
                    var att_Change_Record_Values = await GetDataAttChangeRecord(temp_Record_Values.Factory, temp_Record_Values.Att_Date, temp_Record_Values.Employee_ID);
                    if (att_Change_Record_Values.Count == 0)
                        // Thêm dữ liệu 
                        if (!await InsHRMSAttChangeRecord(temp_Record_Values))
                        {
                            await _repositoryAccessor.RollbackAsync();
                            return new OperationResult(false, "AttendanceMaintenance.DailyAttendancePosting.InsHRMSAttChangeRecordFail");
                        }
                    #endregion

                    #region 1.3.4 Kiểm tra việc làm thêm vào các ngày lễ chung

                    // [C05] - Ngày nghỉ lễ chung Chủ Nhật
                    if (temp_Record_Values.Holiday == "C05" &&
                        temp_Record_Values.Clock_In.ToTimeSpan() > timeSpan0000 &&
                        (temp_Record_Values.Clock_Out.ToTimeSpan() > timeSpan0000 ||
                        temp_Record_Values.Overtime_ClockIn.ToTimeSpan() > timeSpan0000 ||
                        temp_Record_Values.Overtime_ClockOut.ToTimeSpan() > timeSpan0000))
                    {
                        if (!await InsHRMSAttOvertimeMaintain134(temp_Record_Values.Factory, temp_Record_Values.Att_Date, temp_Record_Values.Employee_ID, temp_Record_Values, userName))
                        {
                            await _repositoryAccessor.RollbackAsync();
                            return new OperationResult(false, "AttendanceMaintenance.DailyAttendancePosting.InsHRMSAttOvertimeTempFail");
                        }
                    }
                    #endregion

                    #region 1.3.5 Sau khi hoàn tất kiểm tra, kho lưu trữ điểm danh tạm thời sẽ bị xóa.
                    if (!await DelHRMSAttTempRecord(temp_Record_Values.Factory, temp_Record_Values.Att_Date, temp_Record_Values.Employee_ID))
                    {
                        await _repositoryAccessor.RollbackAsync();
                        return new OperationResult(false, "AttendanceMaintenance.DailyAttendancePosting.DelHRMSAttTempRecordFail");
                    }
                    #endregion

                }
                #endregion

                #region 2. Thông tin dữ liệu tăng ca, dữ liệu hàng năm

                #region 2.1 Đọc dữ liệu tăng ca tạm thời

                var cr_pt2 = await _repositoryAccessor.HRMS_Att_Overtime_Temp
                                    .FindAll(x => x.Factory == factory)
                                    .OrderBy(x => x.Employee_ID)
                                    .ThenBy(x => x.Overtime_Date)
                                    .ToListAsync();
                #endregion

                var yearlyData = await _repositoryAccessor.HRMS_Att_Yearly.FindAll(x => x.Factory == factory).ToListAsync();
                var countHAOT = 0;
                var yearlyUpdate = new List<HRMS_Att_Yearly>(); // danh sách cập nhật dữ liệu hàng năm

                foreach (var overtime_Temp_Values in cr_pt2)
                {
                    countHAOT++;
                    #region 2.1.1 Dữ liệu làm thêm giờ tích lũy hàng năm
                    if (!await InsHRMSAttOvertimeMaintain211(overtime_Temp_Values))
                    {
                        await _repositoryAccessor.RollbackAsync();
                        return new OperationResult(false, "AttendanceMaintenance.DailyAttendancePosting.InsHRMSAttOvertimeMaintainFail");
                    }

                    // Xử lý Yearly
                    var iHBC = _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Allowance && x.Char1 == overtime_Temp_Values.Holiday && x.IsActive, true).Select(x => x.Code).ToList();
                    foreach (var key in iHBC)
                    {
                        // kiểm tra tồn tại trong DB
                        var checkExist = yearlyData.FirstOrDefault(x => x.Att_Year == new DateTime(overtime_Temp_Values.Overtime_Date.Year, 1, 1) && x.Employee_ID == overtime_Temp_Values.Employee_ID && x.Leave_Type == "2" && x.Leave_Code == key);
                        if (checkExist != null)
                        {
                            decimal day = (overtime_Temp_Values.Holiday, key) switch
                            {
                                ("XXX", "A01") or ("C05", "B01") or ("C00", _) => overtime_Temp_Values.Overtime_Hours,
                                ("XXX", "A02") or ("C05", "B02") => overtime_Temp_Values.Night_Hours,
                                ("XXX", "A03") or ("C05", "B03") => overtime_Temp_Values.Night_Overtime_Hours,
                                ("XXX", "A04") or ("C05", "B04") => overtime_Temp_Values.Training_Hours,
                                _ => 0,
                            };
                            checkExist.Days += day;
                            yearlyUpdate.Add(checkExist);
                        }
                    }
                    #endregion

                    #region 2.1.2 Xóa kho lưu trữ tạm thời làm thêm giờ
                    if (!await DelHRMSAttOvertimeTemp(overtime_Temp_Values))
                    {
                        await _repositoryAccessor.RollbackAsync();
                        return new OperationResult(false, "AttendanceMaintenance.DailyAttendancePosting.DelHRMSAttOvertimeTempFail");
                    }
                    #endregion
                }
                #endregion

                #region 3. Thêm & cập nhật dữ liệu mới
                att_Posting_Values.Factory = factory;
                att_Posting_Values.Posting_Date = DateTime.Now;
                att_Posting_Values.Posting_Time = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                att_Posting_Values.Update_By = userName;
                att_Posting_Values.Update_Time = DateTime.Now;

                _repositoryAccessor.HRMS_Att_Posting.Add(att_Posting_Values);
                _repositoryAccessor.HRMS_Att_Yearly.UpdateMultiple(yearlyUpdate);

                int countProcessedRecords = countHATR + countHAOT;
                await _repositoryAccessor.Save();
                await _repositoryAccessor.CommitAsync();
                return new OperationResult(true, "System.Message.ExecuteOKMsg", countProcessedRecords);
                #endregion
            }
            catch (Exception)
            {
                await _repositoryAccessor.RollbackAsync();
                return new OperationResult(false, "System.Message.ExecuteErrorMsg");
            }
            finally
            {
                semaphore.Release();
            }
        }

        /// <summary>
        /// Tính số ngày [vắng mặt] liên tục của nhân viên
        /// </summary>
        /// <param name="HALMs">Danh sách nghỉ , vắng mặt</param>
        /// <param name="endOfMonth"> Ngày kết thúc trong tháng </param>
        /// <returns>Thông tin số ngày nghỉ liên tiếp - Số ngày nghỉ cao nhất </returns>
        private static (decimal, decimal) CalculateContinuousDays(List<HRMS_Att_Leave_Maintain> HALMs, DateTime endOfMonth)
        {
            //Get continuous day groups
            var groupsContinuousDays = new List<List<HRMS_Att_Leave_Maintain>> { new() { } };
            for (int i = 0; i < HALMs.Count; i++)
            {
                HRMS_Att_Leave_Maintain currDate = HALMs[i];
                if (groupsContinuousDays.Last().Count == 0)
                    groupsContinuousDays.Last().Add(currDate);
                else
                {
                    HRMS_Att_Leave_Maintain lastDate = groupsContinuousDays.Last().Last();
                    TimeSpan timeDiff = currDate.Leave_Date - lastDate.Leave_Date.AddDays(currDate.Leave_Date.DayOfWeek == DayOfWeek.Monday ? 1 : 0);
                    if (timeDiff.Days > 1)
                    {
                        groupsContinuousDays.Add(new List<HRMS_Att_Leave_Maintain>());
                    }
                    groupsContinuousDays.Last().Add(currDate);
                }
            }
            //Get filter dates (Sunday)
            var filter_Dates = new List<DateTime>();
            for (int i = 1; i <= endOfMonth.Day; i++)
            {
                DateTime currDate = new(endOfMonth.Year, endOfMonth.Month, i);
                if (currDate.DayOfWeek == DayOfWeek.Sunday)
                    filter_Dates.Add(currDate);
            }
            //Filter queried dated with filter dates list
            for (int i = 0; i < groupsContinuousDays.Count; i++)
            {
                groupsContinuousDays[i] = groupsContinuousDays[i].Where(y => !filter_Dates.Contains(y.Leave_Date)).ToList();
            }
            //Sum leave days
            decimal count_Days = groupsContinuousDays.SelectMany(x => x).Sum(x => x.Days);
            //Get max days on 5 continuous leaved days
            decimal wk_code = groupsContinuousDays.Select(x => x.Count >= 5 ? x.Sum(y => y.Days) : 0).Max();
            return (count_Days, wk_code);
        }
        private async Task<bool> UpdateHRMSEmpPersonal(HRMS_Emp_Resignation HER)
        {
            try
            {
                HRMS_Emp_Personal dataUpdateHEP = await _repositoryAccessor.HRMS_Emp_Personal.FirstOrDefaultAsync(x => x.USER_GUID == HER.USER_GUID);
                if (dataUpdateHEP != null)
                {
                    dataUpdateHEP.Deletion_Code = "Y";
                    dataUpdateHEP.Resign_Date = HER.Resign_Date;
                    dataUpdateHEP.Resign_Reason = HER.Resign_Reason;
                    dataUpdateHEP.Blacklist = HER.Blacklist;
                    dataUpdateHEP.Update_By = HER.Update_By;
                    dataUpdateHEP.Update_Time = HER.Update_Time;

                    _repositoryAccessor.HRMS_Emp_Personal.Update(dataUpdateHEP);
                }

                await _repositoryAccessor.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> InsHRMSEmpResignation(HRMS_Emp_Resignation HER)
        {
            try
            {
                if (!await _repositoryAccessor.HRMS_Emp_Resignation.AnyAsync(x => x.USER_GUID == HER.USER_GUID &&
                                                                                x.Division == HER.Division &&
                                                                                x.Factory == HER.Factory &&
                                                                                x.Employee_ID == HER.Employee_ID))
                {
                    _repositoryAccessor.HRMS_Emp_Resignation.Add(HER);
                    return await _repositoryAccessor.Save();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private async Task<bool> InsHRMSAttOvertimeMaintain211(HRMS_Att_Overtime_Temp HAOT)
        {
            try
            {
                var existingData = await _repositoryAccessor.HRMS_Att_Overtime_Maintain.FirstOrDefaultAsync(x => x.USER_GUID == HAOT.USER_GUID
                && x.Factory == HAOT.Factory && x.Employee_ID == HAOT.Employee_ID && x.Overtime_Date == HAOT.Overtime_Date);

                if (existingData == null)
                {
                    var newItem = Mapper.Map(HAOT).ToANew<HRMS_Att_Overtime_Maintain>(x => x.MapEntityKeys());
                    _repositoryAccessor.HRMS_Att_Overtime_Maintain.Add(newItem);
                }
                await _repositoryAccessor.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }


        private async Task<bool> InsHRMSAttLeaveMaintain(HRMS_Att_Temp_Record HATR, string userName)
        {
            try
            {
                if (!await _repositoryAccessor.HRMS_Att_Leave_Maintain.AnyAsync(x =>
                    x.Factory == HATR.Factory &&
                    x.Employee_ID == HATR.Employee_ID &&
                    x.Leave_Date.Date == HATR.Att_Date.Date))
                {
                    var newItem = Mapper.Map(HATR).ToANew<HRMS_Att_Leave_Maintain>(x => x.MapEntityKeys());
                    newItem.Update_By = userName;
                    newItem.Update_Time = DateTime.Now;
                    newItem.Leave_Date = HATR.Att_Date.Date;
                    _repositoryAccessor.HRMS_Att_Leave_Maintain.Add(newItem);
                    return await _repositoryAccessor.Save();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        private async Task<bool> UpdateHRMSEmpIDcardEmpIDHistory(HRMS_Emp_Resignation HER)
        {
            try
            {
                HRMS_Emp_IDcard_EmpID_History dataHEIEH = await _repositoryAccessor.HRMS_Emp_IDcard_EmpID_History.FirstOrDefaultAsync(x =>
                                                                           x.USER_GUID == HER.USER_GUID &&
                                                                           x.Division == HER.Division &&
                                                                           x.Factory == HER.Factory &&
                                                                           x.Employee_ID == HER.Employee_ID);
                if (dataHEIEH != null)
                {
                    dataHEIEH.Resign_Date = HER.Resign_Date;
                    _repositoryAccessor.HRMS_Emp_IDcard_EmpID_History.Update(dataHEIEH);
                }

                await _repositoryAccessor.Save();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private async Task<bool> DelHRMSAttOvertimeTemp(HRMS_Att_Overtime_Temp HAOM)
        {
            try
            {
                var itemDeleteHAOT = await _repositoryAccessor.HRMS_Att_Overtime_Temp.FirstOrDefaultAsync(x => x.Factory == HAOM.Factory && x.Overtime_Date == HAOM.Overtime_Date && x.Employee_ID == HAOM.Employee_ID);

                if (itemDeleteHAOT != null)
                    _repositoryAccessor.HRMS_Att_Overtime_Temp.Remove(itemDeleteHAOT);
                await _repositoryAccessor.Save();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private async Task<bool> DelHRMSAttTempRecord(string factory, DateTime att_Date, string employee_ID)
        {
            try
            {
                var itemDelete = await _repositoryAccessor.HRMS_Att_Temp_Record.FirstOrDefaultAsync(x => x.Factory == factory
                                       && x.Employee_ID == employee_ID && x.Att_Date == att_Date);

                if (itemDelete != null)
                    _repositoryAccessor.HRMS_Att_Temp_Record.Remove(itemDelete);
                await _repositoryAccessor.Save();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private async Task<bool> InsHRMSAttOvertimeMaintain134(string factory, DateTime att_Date, string employee_ID, HRMS_Att_Temp_Record HATR, string userName)
        {
            try
            {
                HRMS_Att_Change_Record itemHACR = await _repositoryAccessor.HRMS_Att_Change_Record.FirstOrDefaultAsync(x => x.Factory == factory
                       && x.Att_Date == att_Date && x.Employee_ID == employee_ID, true);

                HRMS_Att_Overtime_Temp itemHAOT = await _repositoryAccessor.HRMS_Att_Overtime_Temp.FirstOrDefaultAsync(x => x.Factory == factory
                        && x.Overtime_Date == att_Date && x.Employee_ID == employee_ID, true);

                if (itemHAOT == null)
                {
                    HRMS_Att_Overtime_Maintain itemDataHAOT = new()
                    {
                        USER_GUID = HATR.USER_GUID,
                        Factory = HATR.Factory,
                        Overtime_Date = HATR.Att_Date,
                        Employee_ID = HATR.Employee_ID,
                        Work_Shift_Type = HATR.Work_Shift_Type,
                        Overtime_Start = HATR.Clock_In,
                        Overtime_End = HATR.Clock_Out,
                        Overtime_Hours = 8,
                        Night_Hours = 0,
                        Night_Overtime_Hours = 0,
                        Training_Hours = 0,
                        Night_Eat_Times = 0,
                        Holiday = itemHACR.Holiday,
                        Update_By = userName,
                        Update_Time = DateTime.Now,
                    };

                    _repositoryAccessor.HRMS_Att_Overtime_Maintain.Add(itemDataHAOT);
                }


                await _repositoryAccessor.Save();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private async Task<bool> InsHRMSAttChangeRecord(HRMS_Att_Temp_Record HATR)
        {
            try
            {
                var existingData = await _repositoryAccessor.HRMS_Att_Change_Record.FirstOrDefaultAsync(x => x.USER_GUID == HATR.USER_GUID
                                        && x.Factory == HATR.Factory && x.Employee_ID == HATR.Employee_ID && x.Att_Date == HATR.Att_Date);

                if (existingData == null)
                {
                    var newItem = Mapper.Map(HATR).ToANew<HRMS_Att_Change_Record>(x => x.MapEntityKeys());
                    _repositoryAccessor.HRMS_Att_Change_Record.Add(newItem);
                }

                await _repositoryAccessor.Save();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task<List<HRMS_Att_Change_Record>> GetDataAttChangeRecord(string factory, DateTime att_Date, string employee_ID)
        {
            var predHACR = PredicateBuilder.New<HRMS_Att_Change_Record>(true);
            if (!string.IsNullOrWhiteSpace(factory))
                predHACR.And(x => x.Factory == factory);
            if (att_Date != default)
                predHACR.And(x => x.Att_Date == att_Date);
            if (!string.IsNullOrWhiteSpace(employee_ID))
                predHACR.And(x => x.Employee_ID == employee_ID);

            return await _repositoryAccessor.HRMS_Att_Change_Record.FindAll(predHACR, true).ToListAsync();
        }

    }
}