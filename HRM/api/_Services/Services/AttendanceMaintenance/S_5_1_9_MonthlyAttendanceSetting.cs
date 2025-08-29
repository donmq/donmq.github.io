using AgileObjects.AgileMapper;
using API.Data;
using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using API.Helper.Constant;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.AttendanceMaintenance
{
    public class S_5_1_9_MonthlyAttendanceSetting : BaseServices, I_5_1_9_MonthlyAttendanceSetting
    {
        public S_5_1_9_MonthlyAttendanceSetting(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<PaginationUtility<HRMS_Att_Use_Monthly_LeaveDto>> GetDataPagination(PaginationParam pagination, MonthlyAttendanceSettingParam param, string userName)
        {
            var preHAUML = PredicateBuilder.New<HRMS_Att_Use_Monthly_Leave>(x => x.Factory == param.Factory);
            if (!string.IsNullOrWhiteSpace(param.Factory))
                preHAUML.And(x => x.Factory == param.Factory);
            if (!string.IsNullOrWhiteSpace(param.Effective_Month_Str))
                preHAUML.And(x => x.Effective_Month == Convert.ToDateTime(param.Effective_Month_Str));

            var HAM = _repositoryAccessor.HRMS_Att_Monthly.FindAll(x => x.Pass);
            var HARM = _repositoryAccessor.HRMS_Att_Resign_Monthly.FindAll(x => x.Pass);
            var HALM = _repositoryAccessor.HRMS_Att_Loaned_Monthly.FindAll(x => x.Pass);

            var result = await _repositoryAccessor.HRMS_Att_Use_Monthly_Leave
                .FindAll(preHAUML)
                .GroupBy(t1 => new { t1.Factory, t1.Effective_Month })
                .Select(x => new HRMS_Att_Use_Monthly_LeaveDto
                {
                    Factory = x.Key.Factory,
                    Effective_Month = x.Key.Effective_Month,
                    Update_By = x.FirstOrDefault().Update_By,
                    Update_Time = x.Max(t1 => t1.Update_Time),
                    Is_Delete = HAM.Any(o => o.Att_Month.Date >= x.Key.Effective_Month.Date)
                        || HARM.Any(i => i.Att_Month.Date >= x.Key.Effective_Month.Date)
                        || HALM.Any(l => l.Att_Month.Date >= x.Key.Effective_Month.Date)
                })
                .OrderBy(x => x.Factory).ThenBy(x => x.Effective_Month)
                .ToListAsync();
            return PaginationUtility<HRMS_Att_Use_Monthly_LeaveDto>.Create(result, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<List<KeyValuePair<string, string>>> GetListFactory(string language, string userName)
        {
            var factorys = await Queryt_Factory_AddList(userName);
            var data = await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Factory && factorys.Contains(x.Code), true)
                        .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                           x => new { x.Type_Seq, x.Code },
                           y => new { y.Type_Seq, y.Code },
                           (x, y) => new { HBC = x, HBCL = y }
                        ).SelectMany(x => x.HBCL.DefaultIfEmpty(),
                            (x, y) => new { x.HBC, HBCL = y }
                        ).Select(x => new KeyValuePair<string, string>(
                            x.HBC.Code.Trim(),
                            x.HBC.Code.Trim() + "-" + (x.HBCL != null ? x.HBCL.Code_Name.Trim() : x.HBC.Code_Name.Trim())
                        )).Distinct().ToListAsync();
            return data;
        }

        public async Task<List<KeyValuePair<string, string>>> GetLeaveTypes(string language)
        {
            var data = await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Leave && x.IsActive == true && x.Char1 == "Leave", true)
                        .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                           x => new { x.Type_Seq, x.Code },
                           y => new { y.Type_Seq, y.Code },
                           (x, y) => new { HBC = x, HBCL = y }
                        ).SelectMany(x => x.HBCL.DefaultIfEmpty(),
                            (x, y) => new { x.HBC, HBCL = y }
                        ).Select(x => new KeyValuePair<string, string>(
                            x.HBC.Code.Trim(),
                            x.HBC.Code.Trim() + "-" + (x.HBCL != null ? x.HBCL.Code_Name.Trim() : x.HBC.Code_Name.Trim())
                        )).Distinct().ToListAsync();
            return data;
        }

        public async Task<List<KeyValuePair<string, string>>> GetAllowances(string language)
        {
            var data = await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Allowance && x.IsActive == true, true)
                        .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                           x => new { x.Type_Seq, x.Code },
                           y => new { y.Type_Seq, y.Code },
                           (x, y) => new { HBC = x, HBCL = y }
                        ).SelectMany(x => x.HBCL.DefaultIfEmpty(),
                            (x, y) => new { x.HBC, HBCL = y }
                        ).Select(x => new KeyValuePair<string, string>(
                            x.HBC.Code.Trim(),
                            x.HBC.Code.Trim() + "-" + (x.HBCL != null ? x.HBCL.Code_Name.Trim() : x.HBC.Code_Name.Trim())
                        )).Distinct().ToListAsync();
            return data;
        }

        public async Task<OperationResult> Create(List<HRMS_Att_Use_Monthly_LeaveDto> models, string userName)
        {
            var timeNow = DateTime.Now;
            List<HRMS_Att_Use_Monthly_Leave> newDatas = new();
            await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                foreach (var item in models)
                {
                    if (await _repositoryAccessor.HRMS_Att_Use_Monthly_Leave.AnyAsync(x =>
                        x.Factory == item.Factory &&
                        x.Effective_Month.Date == Convert.ToDateTime(models[0].Effective_Month_Str).Date &&
                        x.Code == item.Code &&
                        x.Leave_Type == item.Leave_Type))
                        return new OperationResult(false, $"Factory: {item.Factory}, Effective Month: {item.Effective_Month}, Type: {(item.Leave_Type == "1" ? "Leave" : "Allowance")}, Code: {item.Code} exsited!");
                    HRMS_Att_Use_Monthly_Leave data = new()
                    {
                        Factory = item.Factory,
                        Effective_Month = Convert.ToDateTime(item.Effective_Month_Str),
                        Leave_Type = item.Leave_Type,
                        Code = item.Code,
                        Seq = item.Seq,
                        Month_Total = item.Month_Total,
                        Year_Total = item.Year_Total,
                        Update_By = userName,
                        Update_Time = timeNow
                    };
                    newDatas.Add(data);
                }
                _repositoryAccessor.HRMS_Att_Use_Monthly_Leave.AddMultiple(newDatas);
                await _repositoryAccessor.Save();
                await _repositoryAccessor.CommitAsync();
                return new OperationResult(true);
            }
            catch
            {
                await _repositoryAccessor.RollbackAsync();
                return new OperationResult(false);
            }

        }

        public async Task<List<HRMS_Att_Use_Monthly_LeaveDto>> GetCloneData(string factory, string leave_Type, string effective_Month, string userName)
        {
            if (!await _repositoryAccessor.HRMS_Att_Use_Monthly_Leave.AnyAsync(x => x.Factory == factory && x.Leave_Type == leave_Type && x.Effective_Month < Convert.ToDateTime(effective_Month)))
                return new List<HRMS_Att_Use_Monthly_LeaveDto>();
            DateTime? max_Effective_Month = await _repositoryAccessor.HRMS_Att_Use_Monthly_Leave
                .FindAll(x => x.Factory == factory && x.Leave_Type == leave_Type && x.Effective_Month < Convert.ToDateTime(effective_Month))
                .MaxAsync(x => x.Effective_Month);

            if (max_Effective_Month == null)
                return new List<HRMS_Att_Use_Monthly_LeaveDto>();

            var results = await _repositoryAccessor.HRMS_Att_Use_Monthly_Leave
                .FindAll(x => x.Factory == factory
                    && x.Effective_Month == max_Effective_Month
                    && x.Leave_Type == leave_Type, true)
                .Select(x => new HRMS_Att_Use_Monthly_LeaveDto()
                {
                    Factory = x.Factory,
                    Effective_Month = x.Effective_Month,
                    Leave_Type = x.Leave_Type,
                    Code = x.Code,
                    Seq = x.Seq,
                    Month_Total = x.Month_Total,
                    Year_Total = x.Year_Total,
                    Update_By = userName,
                    Update_Time = DateTime.Now
                }).OrderBy(x => x.Factory).ThenBy(x => x.Effective_Month).ThenBy(x => x.Seq).ToListAsync();
            return results;
        }
        public async Task<List<HRMS_Att_Use_Monthly_LeaveDto>> GetRecentData(string factory, string effective_Month, string leave_Type, string action)
        {
            var dataHRMS_Att_Monthly = _repositoryAccessor.HRMS_Att_Monthly.FindAll(x => x.Pass == true);
            var dataHRMS_Att_Resign_Monthly = _repositoryAccessor.HRMS_Att_Resign_Monthly.FindAll(x => x.Pass == true);
            var dataHRMS_Att_Loaned_Monthly = _repositoryAccessor.HRMS_Att_Loaned_Monthly.FindAll(x => x.Pass == true);
            var data = _repositoryAccessor.HRMS_Att_Use_Monthly_Leave
                .FindAll(x => x.Factory == factory
                    && x.Effective_Month.Date == Convert.ToDateTime(effective_Month).Date
                    && x.Leave_Type == leave_Type, true);
            var result = await data
                .Select(x => new HRMS_Att_Use_Monthly_LeaveDto()
                {
                    Factory = x.Factory,
                    Effective_Month = x.Effective_Month,
                    Is_Function_Edit = action == "Edit",
                    Leave_Type = x.Leave_Type,
                    Code = x.Code,
                    Seq = x.Seq,
                    Month_Total = x.Month_Total,
                    Year_Total = x.Year_Total,
                    Update_By = x.Update_By,
                    Update_Time = x.Update_Time,
                    Is_Delete = dataHRMS_Att_Monthly.Count(o => o.Att_Month >= x.Effective_Month) > 0
                                || dataHRMS_Att_Resign_Monthly.Count(i => i.Att_Month >= x.Effective_Month) > 0
                                || dataHRMS_Att_Loaned_Monthly.Count(l => l.Att_Month >= x.Effective_Month) > 0
                }).OrderBy(x => x.Factory).ThenBy(x => x.Effective_Month).ThenBy(x => x.Seq).ToListAsync();
            return result;
        }
        public async Task<OperationResult> Edit(List<HRMS_Att_Use_Monthly_LeaveDto> models, string userName)
        {
            await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                var removeDatas = await _repositoryAccessor.HRMS_Att_Use_Monthly_Leave.FindAll(x =>
                    x.Factory == models[0].Factory &&
                    x.Effective_Month.Date == Convert.ToDateTime(models[0].Effective_Month_Str).Date)
                    .ToListAsync();
                if (removeDatas.Any())
                {
                    _repositoryAccessor.HRMS_Att_Use_Monthly_Leave.RemoveMultiple(removeDatas);
                    await _repositoryAccessor.Save();
                }
                var timeNow = DateTime.Now;
                var newDatas = new List<HRMS_Att_Use_Monthly_Leave>();
                foreach (var item in models)
                {
                    HRMS_Att_Use_Monthly_Leave data = new()
                    {
                        Factory = item.Factory,
                        Effective_Month = Convert.ToDateTime(item.Effective_Month_Str),
                        Leave_Type = item.Leave_Type,
                        Code = item.Code,
                        Seq = item.Seq,
                        Month_Total = item.Month_Total,
                        Year_Total = item.Year_Total,
                        Update_By = userName,
                        Update_Time = timeNow
                    };
                    newDatas.Add(data);
                }
                _repositoryAccessor.HRMS_Att_Use_Monthly_Leave.AddMultiple(newDatas);
                await _repositoryAccessor.Save();
                await _repositoryAccessor.CommitAsync();
                return new OperationResult(true);
            }
            catch
            {
                await _repositoryAccessor.RollbackAsync();
                return new OperationResult(false);
            }
        }

        public async Task<OperationResult> Delete(string factory, string effective_Month)
        {
            var data = await _repositoryAccessor.HRMS_Att_Use_Monthly_Leave.FindAll(x => x.Factory == factory
                && x.Effective_Month == Convert.ToDateTime(effective_Month)).ToListAsync();
            if (!data.Any())
                return new OperationResult(false, "No data!");
            _repositoryAccessor.HRMS_Att_Use_Monthly_Leave.RemoveMultiple(data);
            return new OperationResult(await _repositoryAccessor.Save());
        }

        public async Task<OperationResult> CheckDuplicateEffectiveMonth(string factory, string effective_Month)
        {
            var isDupplicate = await _repositoryAccessor.HRMS_Att_Use_Monthly_Leave.AnyAsync(x => x.Factory == factory
                && x.Effective_Month.Date == Convert.ToDateTime(effective_Month).Date);
            return new OperationResult(isDupplicate);
        }
    }
}