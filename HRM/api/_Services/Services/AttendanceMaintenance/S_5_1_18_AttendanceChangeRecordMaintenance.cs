using System.Globalization;
using API.Data;
using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using API.Helper.Constant;
using API.Helper.Utilities;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.AttendanceMaintenance;
public class S_5_1_18_AttendanceChangeRecordMaintenance : BaseServices, I_5_1_18_AttendanceChangeRecordMaintenance
{
    public S_5_1_18_AttendanceChangeRecordMaintenance(DBContext dbContext) : base(dbContext) { }

    #region Add, Update, Delete
    /*
    * step 1 Ins_HRMS_Att_Change_Record (*)
    * step 2 Ins_HRMS_Att_Change_Reason(*)
    * step 3 Query_HRMS_Basic_Code_Char1(40,kind=1,'Leave') => Leave_Code List
    * step 4 IF HRMS_Att_Change_Record.Leave_Code IN Leave_Code List THEN CALL Upd_HRMS_Att_Yearly(USER_GUID,Factory,Employee_ID,Year(Att_Date)/01/01,Leave_Type=1,Leave_Code, 0- Days)
    */
    public async Task<OperationResult> AddAsync(HRMS_Att_Change_RecordDto param, string userAccount, string lang)
    {
        await _repositoryAccessor.BeginTransactionAsync();
        try
        {
            if (string.IsNullOrWhiteSpace(param.Factory)
            || string.IsNullOrWhiteSpace(param.Employee_ID)
            || string.IsNullOrWhiteSpace(param.Att_Date_Str)
            || !DateTime.TryParseExact(param.Att_Date_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime Att_Date))
                return new OperationResult(false, "Invalid inputs");
            param.Att_Date = Att_Date;
            param.Update_By = userAccount;
            param.Update_Time = DateTime.Now;

            // step 1 Ins_HRMS_Att_Change_Record (*)
            var insRecordResult = await Ins_HRMS_Att_Change_Record(param);
            if (!insRecordResult.IsSuccess)
            {
                await _repositoryAccessor.RollbackAsync();
                return insRecordResult;
            }
            // step 2 Ins_HRMS_Att_Change_Reason(*)
            var insReasonResult = await Ins_HRMS_Att_Change_Reason(param);
            if (!insReasonResult.IsSuccess)
            {
                await _repositoryAccessor.RollbackAsync();
                return insReasonResult;
            }
            // step 3 Query_HRMS_Basic_Code_Char1() => Leave_Code List
            var leave_Code_List = await _repositoryAccessor.HRMS_Basic_Code
                .FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Leave && x.Char1 == "Leave")
                .Select(x => x.Code).ToListAsync();
            // step 4 IF HRMS_Att_Change_Record.Leave_Code IN Leave_Code List 
            //THEN CALL Upd_HRMS_Att_Yearly(USER_GUID,Factory,Employee_ID,Year(Att_Date)/01/01,Leave_Type=1,Leave_Code,  Days)
            List<HRMS_Att_Leave_Maintain> listLeave_Maintain = new();
            if (leave_Code_List.Contains(param.Leave_Code))
            {
                var updYearResult = await Upd_HRMS_Att_Yearly(param, param.Leave_Code, param.Days);
                if (!updYearResult.IsSuccess)
                {
                    await _repositoryAccessor.RollbackAsync();
                    return updYearResult;
                }
            }
            var insLeaveResult = await Ins_HRMS_Att_Leave_Maintain(param, param.Leave_Code);
            if (!insLeaveResult.IsSuccess)
            {
                await _repositoryAccessor.RollbackAsync();
                return insLeaveResult;
            }
            await _repositoryAccessor.CommitAsync();
            return new OperationResult(true, "Create successfully");
        }
        catch (Exception)
        {
            await _repositoryAccessor.RollbackAsync();
            return new OperationResult(false, "Oops! Sorry, an error occurred while processing your request");
        }
    }

    /*
    * step 1 Upd_HRMS_Att_Temp_Record(*)
    * step 2 Upd_HRMS_Att_Change_Reason(*)
    * step 3 Query_HRMS_Basic_Code_Char1(40,kind=1,'Leave') => Leave_Code List
    * step 4 IF HRMS_Att_Change_Record.before Leave_Code IN Leave_Code List THEN CALL Upd_HRMS_Att_Yearly(USER_GUID,Factory,Employee_ID,Year(Att_Date)/01/01,Leave_Type=1,Leave_Code,0- before Days)
    * step 5 IF after Leave_Code IN Leave_Code List THEN 在累加維護後_新天數 CALL Upd_HRMS_Att_Yearly(USER_GUID,Factory,Employee_ID,Year(Att_Date)/01/01,Leave_Type=1,Leave_Code,after Days)
    */
    public async Task<OperationResult> UpdateAsync(HRMS_Att_Change_RecordDto param, string lang)
    {
        await _repositoryAccessor.BeginTransactionAsync();
        try
        {
            if (string.IsNullOrWhiteSpace(param.Factory)
            || string.IsNullOrWhiteSpace(param.Employee_ID)
            || string.IsNullOrWhiteSpace(param.Att_Date_Str)
            || !DateTime.TryParseExact(param.Att_Date_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime Att_Date))
                return new OperationResult(false, "Invalid inputs");
            param.Att_Date = Att_Date;
            param.Update_Time = Convert.ToDateTime(param.Update_Time_Str);

            // step 1 Upd_HRMS_Att_Record(*)
            var updRecordResult = await Upd_HRMS_Att_Change_Record(param);
            if (!updRecordResult.IsSuccess)
            {
                await _repositoryAccessor.RollbackAsync();
                return updRecordResult;
            }

            // step 2 Upd_HRMS_Att_Change_Reason(*)
            var updReasonResult = await Upd_HRMS_Att_Change_Reason(param);
            if (!updReasonResult.IsSuccess)
            {
                await _repositoryAccessor.RollbackAsync();
                return updReasonResult;
            }

            // step 3 Query_HRMS_Basic_Code_Char1() => Leave_Code List Background save yearly
            var leave_Code_List = await _repositoryAccessor.HRMS_Basic_Code
               .FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Leave && x.Char1 == "Leave")
               .Select(x => x.Code).ToListAsync();

            // step 3.1 IF HRMS_Att_Change_Record.before Leave_Code IN Leave_Code List THEN CALL Upd_HRMS_Att_Yearly()
            if (leave_Code_List.Contains(param.Before_Leave_Code))
            {
                var updYearResult = await Upd_HRMS_Att_Yearly(param, param.Before_Leave_Code, 0 - param.Before_Days);
                if (!updYearResult.IsSuccess)
                {
                    await _repositoryAccessor.RollbackAsync();
                    return updYearResult;
                }
                var remLeaveResult = await Rem_HRMS_Att_Leave_Maintain(param, param.Before_Leave_Code);
                if (!remLeaveResult.IsSuccess)
                {
                    await _repositoryAccessor.RollbackAsync();
                    return remLeaveResult;
                }
            }

            // step 3.2 IF after Leave_Code IN Leave_Code List THEN
            if (leave_Code_List.Contains(param.After_Leave_Code))
            {
                var updYearResult = await Upd_HRMS_Att_Yearly(param, param.After_Leave_Code, param.After_Days);
                if (!updYearResult.IsSuccess)
                {
                    await _repositoryAccessor.RollbackAsync();
                    return updYearResult;
                }
                // chỉ thêm và HRMS_Att_Leave_Maintain khi Leave_code tồn tại trong leave_Code_List
                var remLeaveResult = await Rem_HRMS_Att_Leave_Maintain(param, param.After_Leave_Code);
                if (!remLeaveResult.IsSuccess)
                {
                    await _repositoryAccessor.RollbackAsync();
                    return remLeaveResult;
                }
                var insLeaveResult = await Ins_HRMS_Att_Leave_Maintain(param, param.After_Leave_Code);
                if (!insLeaveResult.IsSuccess)
                {
                    await _repositoryAccessor.RollbackAsync();
                    return insLeaveResult;
                }
            }
            await _repositoryAccessor.CommitAsync();
            return new OperationResult(true, "Update successfully");
        }
        catch (Exception)
        {
            await _repositoryAccessor.RollbackAsync();
            return new OperationResult(false, "Oops! Sorry, an error occurred while processing your request");
        }
    }

    /*
     // step 1 Del_HRMS_Att_Change_Record, Del_HRMS_Att_Change_Reason
     // step 2 Query_HRMS_Basic_Code_Char1(40,kind=1,’ Leave’) => Leave_Code List
     // step 3 check IF HRMS_Att_Change_Record.Leave_Code IN Leave_Code List THEN  CALL Upd_HRMS_Att_Yearly
    */
    public async Task<OperationResult> DeleteAsync(HRMS_Att_Change_RecordDto param)
    {
        await _repositoryAccessor.BeginTransactionAsync();
        try
        {
            if (string.IsNullOrWhiteSpace(param.Factory)
            || string.IsNullOrWhiteSpace(param.Employee_ID)
            || string.IsNullOrWhiteSpace(param.Att_Date_Str)
            || !DateTime.TryParseExact(param.Att_Date_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime Att_Date))
                return new OperationResult(false, "Invalid inputs");

            param.Att_Date = Att_Date;
            // step 1 Del_HRMS_Att_Change_Record
            var resultRecordDeleted = await Del_HRMS_Att_Change_Record(param);
            if (!resultRecordDeleted.IsSuccess)
            {
                await _repositoryAccessor.RollbackAsync();
                return resultRecordDeleted;
            }
            var resultReasonDeleted = await Del_HRMS_Att_Change_Reason(param);
            if (!resultReasonDeleted.IsSuccess)
            {
                await _repositoryAccessor.RollbackAsync();
                return resultReasonDeleted;
            }

            // step 2 Query_HRMS_Basic_Code_Char1() => Leave_Code List
            var leave_Code_List = await _repositoryAccessor.HRMS_Basic_Code
               .FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Leave && x.Char1 == "Leave")
               .Select(x => x.Code).ToListAsync();
            if (leave_Code_List is null)
            {
                await _repositoryAccessor.RollbackAsync();
                return new OperationResult(false, "Leave_Code List empty!");
            }

            // step 3 check IF HRMS_Att_Change_Record.Leave_Code IN Leave_Code List THEN  CALL Upd_HRMS_Att_Yearly
            var _record = resultRecordDeleted.Data as HRMS_Att_Change_Record;
            if (leave_Code_List.Contains(_record.Leave_Code))
            {
                var resultBackground = await Upd_HRMS_Att_Yearly(param, _record.Leave_Code, 0 - param.Days);
                if (!resultBackground.IsSuccess)
                {
                    await _repositoryAccessor.RollbackAsync();
                    return resultBackground;
                }
            }
            var remLeaveResult = await Rem_HRMS_Att_Leave_Maintain(param, _record.Leave_Code);
            if (!remLeaveResult.IsSuccess)
            {
                await _repositoryAccessor.RollbackAsync();
                return remLeaveResult;
            }
            await _repositoryAccessor.CommitAsync();
            return new OperationResult(true, "Delete successfully");
        }
        catch (Exception)
        {
            await _repositoryAccessor.RollbackAsync();
            return new OperationResult(false, "Oops! Sorry, an error occurred while processing your request");
        }
    }
    #endregion

    #region  GetDataPagination
    public async Task<PaginationUtility<HRMS_Att_Change_RecordDto>> GetDataPagination(PaginationParam pagination, HRMS_Att_Change_Record_Params param)
    {
        var predicateRecord = PredicateBuilder.New<HRMS_Att_Change_Record>(x => x.Factory == param.Factory);
        var predicateReason = PredicateBuilder.New<HRMS_Att_Change_Reason>(x => x.Factory == param.Factory);

        if (!string.IsNullOrWhiteSpace(param.Employee_ID))
            predicateRecord.And(x => x.Employee_ID.Contains(param.Employee_ID.Trim()) && x.Employee_ID.Contains(param.Factory));
        if (!string.IsNullOrWhiteSpace(param.Department))
            predicateRecord.And(x => x.Department == param.Department);
        if (!string.IsNullOrWhiteSpace(param.Work_Shift_Type))
            predicateRecord.And(x => x.Work_Shift_Type == param.Work_Shift_Type);
        if (!string.IsNullOrWhiteSpace(param.Leave_Code))
            predicateRecord.And(x => x.Leave_Code == param.Leave_Code);
        if (!string.IsNullOrWhiteSpace(param.Date_Start_Str) && !string.IsNullOrWhiteSpace(param.Date_End_Str))
            predicateRecord.And(x => x.Att_Date.Date >= param.Date_Start_Str.ToDateTime() && x.Att_Date <= param.Date_End_Str.ToDateTime());

        var HAC_Record = _repositoryAccessor.HRMS_Att_Change_Record.FindAll(predicateRecord, true);
        var HAC_Reason = _repositoryAccessor.HRMS_Att_Change_Reason.FindAll(predicateReason, true);

        var permissionGroupQuery = _repositoryAccessor.HRMS_Basic_Role.FindAll(x => x.Factory == param.Factory, true).Select(x => x.Permission_Group);
        var HEP = _repositoryAccessor.HRMS_Emp_Personal.FindAll(x => (x.Factory == param.Factory) && permissionGroupQuery.Contains(x.Permission_Group));

        var HBC = _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.IsActive);
        var HBCL = _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == param.Lang.ToLower());
        var HBC_Lang = HBC
            .GroupJoin(HBCL,
                x => new { x.Type_Seq, x.Code },
                y => new { y.Type_Seq, y.Code },
                (x, y) => new { HBC = x, HBCL = y })
            .SelectMany(x => x.HBCL.DefaultIfEmpty(),
                (x, y) => new { x.HBC, HBCL = y })
            .Select(x => new
            {
                x.HBC.Type_Seq,
                x.HBC.Code,
                x.HBC.Char1,
                Code_Name = $"{x.HBC.Code}-{(x.HBCL != null ? x.HBCL.Code_Name : x.HBC.Code_Name)}"
            });
        var HBC_Holiday = HBC_Lang.Where(x => x.Type_Seq == BasicCodeTypeConstant.Holiday && x.Char1 == "Attendance");
        var HBC_Reason = HBC_Lang.Where(x => x.Type_Seq == BasicCodeTypeConstant.ReasonCode);
        var HBC_WorkTypeShift = HBC_Lang.Where(x => x.Type_Seq == BasicCodeTypeConstant.WorkShiftType);
        var HBC_Leave = HBC_Lang.Where(x => x.Type_Seq == BasicCodeTypeConstant.Leave);

        var HOD = _repositoryAccessor.HRMS_Org_Department.FindAll(x => x.Factory == param.Factory);
        var HODL = _repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == param.Lang.ToLower());
        var HOD_Lang = HOD
            .GroupJoin(HODL,
                x => new { x.Department_Code, x.Factory },
                y => new { y.Department_Code, y.Factory },
                (x, y) => new { HOD = x, HODL = y })
            .SelectMany(x => x.HODL.DefaultIfEmpty(),
                (x, y) => new { x.HOD, HODL = y })
            .Select(x => new
            {
                x.HOD.Factory,
                Department = x.HOD.Department_Code,
                Department_Name = x.HODL != null ? x.HODL.Name : x.HOD.Department_Name
            });

        var result = await HAC_Record
            .Join(HEP,
                x => x.USER_GUID,
                y => y.USER_GUID,
                (x, y) => new { HAC_Record = x, HEP = y })
            .GroupJoin(HAC_Reason,
                x => new { x.HAC_Record.USER_GUID, x.HAC_Record.Att_Date, x.HAC_Record.Employee_ID },
                y => new { y.USER_GUID, y.Att_Date, y.Employee_ID },
                (x, y) => new { x.HAC_Record, x.HEP, HAC_Reason = y })
            .SelectMany(x => x.HAC_Reason.DefaultIfEmpty(),
                (x, y) => new { x.HAC_Record, x.HEP, HAC_Reason = y })
            .GroupJoin(HBC_Holiday,
                x => x.HAC_Record.Holiday,
                y => y.Code,
                (x, y) => new { x.HAC_Record, x.HEP, x.HAC_Reason, HBC_Holiday = y })
            .SelectMany(x => x.HBC_Holiday.DefaultIfEmpty(),
                (x, y) => new { x.HAC_Record, x.HEP, x.HAC_Reason, HBC_Holiday = y })
            .GroupJoin(HBC_Reason,
                x => x.HAC_Reason != null ? x.HAC_Reason.Reason_Code : "ZZ",
                y => y.Code,
                (x, y) => new { x.HAC_Record, x.HEP, x.HAC_Reason, x.HBC_Holiday, HBC_Reason = y })
            .SelectMany(x => x.HBC_Reason.DefaultIfEmpty(),
                (x, y) => new { x.HAC_Record, x.HEP, x.HAC_Reason, x.HBC_Holiday, HBC_Reason = y })
            .GroupJoin(HBC_WorkTypeShift,
                x => x.HAC_Record.Work_Shift_Type,
                y => y.Code,
                (x, y) => new { x.HAC_Record, x.HEP, x.HAC_Reason, x.HBC_Holiday, x.HBC_Reason, HBC_WorkTypeShift = y })
            .SelectMany(x => x.HBC_WorkTypeShift.DefaultIfEmpty(),
                (x, y) => new { x.HAC_Record, x.HEP, x.HAC_Reason, x.HBC_Holiday, x.HBC_Reason, HBC_WorkTypeShift = y })
            .GroupJoin(HBC_Leave,
                x => x.HAC_Record.Leave_Code,
                y => y.Code,
                (x, y) => new { x.HAC_Record, x.HEP, x.HAC_Reason, x.HBC_Holiday, x.HBC_Reason, x.HBC_WorkTypeShift, HBC_Leave = y })
            .SelectMany(x => x.HBC_Leave.DefaultIfEmpty(),
                (x, y) => new { x.HAC_Record, x.HEP, x.HAC_Reason, x.HBC_Holiday, x.HBC_Reason, x.HBC_WorkTypeShift, HBC_Leave = y })
            .GroupJoin(HOD_Lang,
                x => new { x.HAC_Record.Factory, x.HAC_Record.Department },
                y => new { y.Factory, y.Department },
                (x, y) => new { x.HAC_Record, x.HEP, x.HAC_Reason, x.HBC_Holiday, x.HBC_Reason, x.HBC_WorkTypeShift, x.HBC_Leave, HOD_Lang = y })
            .SelectMany(x => x.HOD_Lang.DefaultIfEmpty(),
                (x, y) => new { x.HAC_Record, x.HEP, x.HAC_Reason, x.HBC_Holiday, x.HBC_Reason, x.HBC_WorkTypeShift, x.HBC_Leave, HOD_Lang = y })
            .Select(x => new HRMS_Att_Change_RecordDto
            {
                USER_GUID = x.HAC_Record.USER_GUID,
                Factory = x.HAC_Record.Factory,
                Employee_ID = x.HAC_Record.Employee_ID,
                Att_Date = x.HAC_Record.Att_Date,
                Att_Date_Str = x.HAC_Record.Att_Date.ToString("yyyy/MM/dd"),
                Local_Full_Name = x.HEP.Local_Full_Name,

                Department_Code = x.HAC_Record.Department,
                Department_Name = x.HOD_Lang.Department_Name,
                Department_Code_Name = x.HOD_Lang != null && !string.IsNullOrWhiteSpace(x.HOD_Lang.Department_Name)
                        ? x.HOD_Lang.Department + "-" + x.HOD_Lang.Department_Name : x.HAC_Record.Department,

                Clock_In = x.HAC_Reason != null ? x.HAC_Reason.Clock_In : x.HAC_Record.Clock_In,
                Clock_Out = x.HAC_Reason != null ? x.HAC_Reason.Clock_Out : x.HAC_Record.Clock_Out,
                Overtime_ClockIn = x.HAC_Reason != null ? x.HAC_Reason.Overtime_ClockIn : x.HAC_Record.Overtime_ClockIn,
                Overtime_ClockOut = x.HAC_Reason != null ? x.HAC_Reason.Overtime_ClockOut : x.HAC_Record.Overtime_ClockOut,

                Modified_Clock_In = x.HAC_Record.Clock_In,
                Modified_Clock_Out = x.HAC_Record.Clock_Out,
                Modified_Overtime_ClockIn = x.HAC_Record.Overtime_ClockIn,
                Modified_Overtime_ClockOut = x.HAC_Record.Overtime_ClockOut,

                Holiday = x.HAC_Record.Holiday,
                Holiday_Str = x.HBC_Holiday.Code_Name,

                Reason_Code = x.HAC_Reason != null ? x.HAC_Reason.Reason_Code : "ZZ",
                Reason_Code_Str = x.HBC_Reason != null ? x.HBC_Reason.Code_Name : string.Empty,

                Work_Shift_Type = x.HAC_Record.Work_Shift_Type,
                Work_Shift_Type_Str = x.HBC_WorkTypeShift != null ? x.HBC_WorkTypeShift.Code_Name : string.Empty,

                Leave_Code = x.HAC_Record.Leave_Code,
                Leave_Code_Str = x.HBC_Leave != null ? x.HBC_Leave.Code_Name : string.Empty,

                Days = x.HAC_Record.Days,
                Update_By = x.HAC_Record.Update_By,
                Update_Time = x.HAC_Record.Update_Time,
                Update_Time_Str = x.HAC_Record.Update_Time.ToString("yyyy/MM/dd HH:mm:ss"),

                IsAttDate = x.HAC_Record.Att_Date.Date < DateTime.Today.AddDays(-40).Date
            })
            .OrderBy(x => x.Factory).ThenBy(x => x.Att_Date).ThenBy(x => x.Employee_ID)
            .ToListAsync();

        if (!string.IsNullOrWhiteSpace(param.Reason_Code))
            result = result.Where(x => x.Reason_Code == param.Reason_Code).ToList();

        return PaginationUtility<HRMS_Att_Change_RecordDto>.Create(result, pagination.PageNumber, pagination.PageSize);
    }
    #endregion
    public async Task<OperationResult> CheckExistedData(string Factory, string Att_Date, string Employee_ID)
    {
        return new OperationResult(await _repositoryAccessor.HRMS_Att_Change_Record.AnyAsync(x =>
            x.Employee_ID == Employee_ID &&
            x.Factory == Factory &&
            x.Att_Date.Date == Convert.ToDateTime(Att_Date).Date));
    }

    public async Task<List<KeyValuePair<string, string>>> GetListFactoryByUser(string language, string userName)
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

    #region Del_HRMS_Att_Change_Record 
    private async Task<OperationResult> Del_HRMS_Att_Change_Record(HRMS_Att_Change_RecordDto data)
    {
        try
        {
            var oldData = await _repositoryAccessor.HRMS_Att_Change_Record.FirstOrDefaultAsync(r =>
                r.Factory == data.Factory &&
                r.Att_Date.Date == data.Att_Date.Date &&
                r.Employee_ID == data.Employee_ID);
            if (oldData != null)
            {
                _repositoryAccessor.HRMS_Att_Change_Record.Remove(oldData);
                if (!await _repositoryAccessor.Save())
                    return new OperationResult(false, "Del_HRMS_Att_Change_Record failed !");
            }
            return new OperationResult(true, oldData);
        }
        catch (Exception)
        {
            return new OperationResult(false, "Oops! Sorry, an error occurred while processing your request");
        }
    }
    #endregion
    // 
    #region Del_HRMS_Att_Change_Reason
    private async Task<OperationResult> Del_HRMS_Att_Change_Reason(HRMS_Att_Change_RecordDto data)
    {
        try
        {
            var oldData = await _repositoryAccessor.HRMS_Att_Change_Reason.FirstOrDefaultAsync(r =>
                r.Factory == data.Factory &&
                r.Att_Date.Date == data.Att_Date.Date &&
                r.Employee_ID == data.Employee_ID);
            if (oldData != null)
            {
                _repositoryAccessor.HRMS_Att_Change_Reason.Remove(oldData);
                if (!await _repositoryAccessor.Save())
                    return new OperationResult(false, "Del_HRMS_Att_Change_Reason failed !");
            }
            return new OperationResult(true);
        }
        catch (Exception)
        {
            return new OperationResult(false, "Oops! Sorry, an error occurred while processing your request");
        }
    }
    #endregion

    #region Upd_HRMS_Att_Yearly
    private async Task<OperationResult> Upd_HRMS_Att_Yearly(HRMS_Att_Change_RecordDto data, string leaveCode, decimal days, string leaveType = "1")
    {
        try
        {
            var Att_Year = new DateTime(data.Att_Date.Year, 1, 1);
            HRMS_Att_Yearly oldData = await _repositoryAccessor.HRMS_Att_Yearly.FirstOrDefaultAsync(r =>
                r.Factory == data.Factory &&
                r.Att_Year.Date == Att_Year.Date &&
                r.Employee_ID == data.Employee_ID &&
                r.USER_GUID == data.USER_GUID &&
                r.Leave_Type == leaveType &&
                r.Leave_Code == leaveCode);
            if (oldData is null)
                return new OperationResult(isSuccess: false, $"Background save Upd_HRMS_Att_Yearly record not found for Employee ID {data.Employee_ID}, Year {Att_Year.Year}, Leave Code {leaveCode}, Factory {data.Factory}, User GUID {data.USER_GUID}, Leave Type {leaveType}");

            oldData.Days += days;
            oldData.Update_By = data.Update_By;
            oldData.Update_Time = data.Update_Time;

            _repositoryAccessor.HRMS_Att_Yearly.Update(oldData);
            if (!await _repositoryAccessor.Save())
                return new OperationResult(false, "Upd_HRMS_Att_Yearly failed !");
            return new OperationResult(true);
        }
        catch (Exception)
        {
            return new OperationResult(false, "Oops! Sorry, an error occurred while processing your request");
        }
    }
    #endregion

    #region Ins_HRMS_Att_Leave_Maintain
    private async Task<OperationResult> Ins_HRMS_Att_Leave_Maintain(HRMS_Att_Change_RecordDto data, string leave_Code)
    {
        try
        {
            var oldData = _repositoryAccessor.HRMS_Att_Leave_Maintain.FirstOrDefault(x =>
               x.Leave_code == leave_Code &&
               x.Employee_ID == data.Employee_ID &&
               x.Factory == data.Factory &&
               x.Leave_Date.Date == data.Att_Date.Date);
            if (oldData == null)
            {
                var addHALM = new HRMS_Att_Leave_Maintain
                {
                    USER_GUID = data.USER_GUID,
                    Leave_code = leave_Code,
                    Employee_ID = data.Employee_ID,
                    Factory = data.Factory,
                    Leave_Date = data.Att_Date,
                    Days = data.Days,
                    Work_Shift_Type = data.Work_Shift_Type,
                    Update_By = data.Update_By,
                    Update_Time = data.Update_Time,
                    Department = data.Department_Code
                };
                _repositoryAccessor.HRMS_Att_Leave_Maintain.Add(addHALM);
                if (!await _repositoryAccessor.Save())
                    return new OperationResult(false, "Insert HRMS_Att_Leave_Maintain failed !");
            }
            return new OperationResult(true);
        }
        catch (Exception)
        {
            return new OperationResult(false, "Oops! Sorry, an error occurred while processing your request");
        }
    }
    #endregion

    #region Rem_HRMS_Att_Leave_Maintain
    private async Task<OperationResult> Rem_HRMS_Att_Leave_Maintain(HRMS_Att_Change_RecordDto data, string leave_Code)
    {
        try
        {
            var leave_Maintain = _repositoryAccessor.HRMS_Att_Leave_Maintain.FirstOrDefault(x =>
                x.Leave_code == leave_Code &&
                x.Employee_ID == data.Employee_ID &&
                x.Factory == data.Factory &&
                x.Leave_Date.Date == data.Att_Date.Date);
            if (leave_Maintain != null)
            {
                _repositoryAccessor.HRMS_Att_Leave_Maintain.Remove(leave_Maintain);
                if (!await _repositoryAccessor.Save())
                    return new OperationResult(false, "Delete HRMS_Att_Leave_Maintain failed !");
            }
            return new OperationResult(true);
        }
        catch (Exception)
        {
            return new OperationResult(false, "Oops! Sorry, an error occurred while processing your request");
        }
    }
    #endregion

    #region Ins_HRMS_Att_Change_Record
    private async Task<OperationResult> Ins_HRMS_Att_Change_Record(HRMS_Att_Change_RecordDto data)
    {
        try
        {
            if (await _repositoryAccessor.HRMS_Att_Change_Record.AnyAsync(x =>
                x.Factory == data.Factory &&
                x.Att_Date.Date == data.Att_Date.Date &&
                x.Employee_ID == data.Employee_ID))
                return new OperationResult(false, $"Data Change_Record already exists: Factory: {data.Factory}, Date: {data.Att_Date:yyyy/MM/dd}, Employee ID: {data.Employee_ID}");

            HRMS_Att_Change_Record addData = new()
            {
                USER_GUID = data.USER_GUID,
                Factory = data.Factory,
                Att_Date = data.Att_Date,
                Employee_ID = data.Employee_ID,
                Department = data.Department_Code,
                Work_Shift_Type = data.Work_Shift_Type,
                Leave_Code = data.Leave_Code,
                Clock_In = data.Modified_Clock_In,
                Clock_Out = data.Modified_Clock_Out,
                Overtime_ClockIn = data.Modified_Overtime_ClockIn,
                Overtime_ClockOut = data.Modified_Overtime_ClockOut,
                Days = data.Days,
                Holiday = data.Holiday,
                Update_By = data.Update_By,
                Update_Time = data.Update_Time
            };
            _repositoryAccessor.HRMS_Att_Change_Record.Add(addData);
            if (!await _repositoryAccessor.Save())
                return new OperationResult(false, "Ins_HRMS_Att_Change_Record failed !");
            return new OperationResult(true);
        }
        catch (Exception)
        {
            return new OperationResult(false, "Oops! Sorry, an error occurred while processing your request");
        }
    }
    #endregion

    #region Ins_HRMS_Att_Change_Reason
    private async Task<OperationResult> Ins_HRMS_Att_Change_Reason(HRMS_Att_Change_RecordDto data)
    {
        try
        {
            if (await _repositoryAccessor.HRMS_Att_Change_Reason.AnyAsync(x =>
                x.Factory == data.Factory &&
                x.Att_Date.Date == data.Att_Date.Date &&
                x.Employee_ID == data.Employee_ID))
                return new OperationResult(false, $"Data Change_Reason already exists: Factory: {data.Factory}, Date: {data.Att_Date:yyyy/MM/dd}, Employee ID: {data.Employee_ID}");

            HRMS_Att_Change_Reason addData = new()
            {
                USER_GUID = data.USER_GUID,
                Factory = data.Factory,
                Att_Date = data.Att_Date,
                Employee_ID = data.Employee_ID,
                Work_Shift_Type = data.Work_Shift_Type,
                Leave_Code = data.Leave_Code,
                Reason_Code = data.Reason_Code,
                Clock_In = "0000",
                Clock_Out = "0000",
                Overtime_ClockIn = "0000",
                Overtime_ClockOut = "0000",
                Update_By = data.Update_By,
                Update_Time = data.Update_Time
            };
            _repositoryAccessor.HRMS_Att_Change_Reason.Add(addData);
            if (!await _repositoryAccessor.Save())
                return new OperationResult(false, "Ins_HRMS_Att_Change_Reason failed !");
            return new OperationResult(true);
        }
        catch (Exception)
        {
            return new OperationResult(false, "Oops! Sorry, an error occurred while processing your request");
        }
    }
    #endregion

    #region Upd_HRMS_Att_Change_Record
    private async Task<OperationResult> Upd_HRMS_Att_Change_Record(HRMS_Att_Change_RecordDto data)
    {
        try
        {
            HRMS_Att_Change_Record oldData = await _repositoryAccessor.HRMS_Att_Change_Record.FirstOrDefaultAsync(r =>
                r.Factory == data.Factory &&
                r.Att_Date.Date == data.Att_Date.Date &&
                r.Employee_ID == data.Employee_ID);
            if (oldData is null)
                return new OperationResult(false, $"Update HRMS_Att_Change_Record Fail! Record not found for Factory: {data.Factory}, Employee_ID: {data.Employee_ID}, Att_Date: {data.Att_Date:yyyy/MM/dd}.");

            oldData.Days = data.Days;
            oldData.Leave_Code = data.Leave_Code;
            oldData.Clock_In = data.Modified_Clock_In;
            oldData.Clock_Out = data.Modified_Clock_Out;
            oldData.Overtime_ClockIn = data.Modified_Overtime_ClockIn;
            oldData.Overtime_ClockOut = data.Modified_Overtime_ClockOut;
            oldData.Update_By = data.Update_By;
            oldData.Update_Time = data.Update_Time;

            _repositoryAccessor.HRMS_Att_Change_Record.Update(oldData);
            if (!await _repositoryAccessor.Save())
                return new OperationResult(false, "Ins_HRMS_Att_Change_Reason failed !");
            return new OperationResult(true);
        }
        catch (Exception)
        {
            return new OperationResult(false, "Oops! Sorry, an error occurred while processing your request");
        }
    }
    #endregion

    #region Upd_HRMS_Att_Change_Reason
    private async Task<OperationResult> Upd_HRMS_Att_Change_Reason(HRMS_Att_Change_RecordDto data)
    {
        try
        {
            HRMS_Att_Change_Reason oldData = await _repositoryAccessor.HRMS_Att_Change_Reason.FirstOrDefaultAsync(r =>
                r.Factory == data.Factory &&
                r.Att_Date.Date == data.Att_Date.Date &&
                r.Employee_ID == data.Employee_ID);
            if (oldData == null)
            {
                HRMS_Att_Change_Reason addData = new()
                {
                    USER_GUID = data.USER_GUID,
                    Factory = data.Factory,
                    Att_Date = data.Att_Date,
                    Employee_ID = data.Employee_ID,
                    Work_Shift_Type = data.Work_Shift_Type,
                    Leave_Code = data.Leave_Code,
                    Reason_Code = data.Reason_Code,
                    Clock_In = data.Clock_In,
                    Clock_Out = data.Clock_Out,
                    Overtime_ClockIn = data.Overtime_ClockIn,
                    Overtime_ClockOut = data.Overtime_ClockOut,
                    Update_By = data.Update_By,
                    Update_Time = data.Update_Time
                };
                _repositoryAccessor.HRMS_Att_Change_Reason.Add(addData);
                if (!await _repositoryAccessor.Save())
                    return new OperationResult(false, "Ins_HRMS_Att_Change_Reason failed !");
                return new OperationResult(true);
            }
            oldData.Reason_Code = data.Reason_Code;
            oldData.Clock_In = data.Clock_In;
            oldData.Clock_Out = data.Clock_Out;
            oldData.Overtime_ClockIn = data.Overtime_ClockIn;
            oldData.Overtime_ClockOut = data.Overtime_ClockOut;
            oldData.Update_By = data.Update_By;
            oldData.Update_Time = data.Update_Time;

            _repositoryAccessor.HRMS_Att_Change_Reason.Update(oldData);
            if (!await _repositoryAccessor.Save())
                return new OperationResult(false, "Upd_HRMS_Att_Change_Reason failed !");
            return new OperationResult(true);
        }
        catch (Exception)
        {
            return new OperationResult(false, "Oops! Sorry, an error occurred while processing your request");
        }
    }
    #endregion

}
