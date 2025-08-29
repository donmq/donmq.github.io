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
    public class S_5_1_2_ShiftScheduleSetting : BaseServices, I_5_1_2_ShiftScheduleSetting
    {
        public S_5_1_2_ShiftScheduleSetting(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<OperationResult> Create(HRMS_Att_Work_ShiftDto model)
        {
            var item = await _repositoryAccessor.HRMS_Att_Work_Shift.FirstOrDefaultAsync(x =>
                                                                x.Division.ToLower() == model.Division.Trim().ToLower()
                                                            && x.Factory.ToLower() == model.Factory.Trim().ToLower()
                                                            && x.Work_Shift_Type.ToLower() == model.Work_Shift_Type.Trim().ToLower()
                                                            && x.Week.ToLower() == model.Week.ToString().Trim().ToLower()
                                                            );
            if (item != null) return new OperationResult(false, "This Shift Schedule is existed");

            var work_Shift = Mapper.Map(model).ToANew<HRMS_Att_Work_Shift>(x => x.MapEntityKeys());
            work_Shift.Update_Time = Convert.ToDateTime(model.Update_Time_Str);
            _repositoryAccessor.HRMS_Att_Work_Shift.Add(work_Shift);
            await _repositoryAccessor.Save();
            return new OperationResult(true);
        }

        public async Task<PaginationUtility<HRMS_Att_Work_ShiftDto>> GetDataPagination(PaginationParam param, HRMS_Att_Work_ShiftParam filter)
        {
            var predicate = PredicateBuilder.New<HRMS_Att_Work_Shift>(true);
            if (!string.IsNullOrWhiteSpace(filter.Division))
                predicate.And(x => x.Division == filter.Division);
            if (!string.IsNullOrWhiteSpace(filter.Factory))
                predicate.And(x => x.Factory == filter.Factory);
            if (!string.IsNullOrWhiteSpace(filter.Work_Shift_Type))
                predicate.And(x => x.Work_Shift_Type == filter.Work_Shift_Type);
            var query = await _repositoryAccessor.HRMS_Att_Work_Shift.FindAll(predicate, true)
                        // Group join vs Basic Code
                        .GroupJoin(_repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == BasicCodeTypeConstant.WorkShiftType, true),
                                x => new { Code = x.Work_Shift_Type },
                                y => new { y.Code },
                            (x, y) => new { workShift = x, basicCode = y })
                            .SelectMany(x => x.basicCode.DefaultIfEmpty(),
                            (x, y) => new { x.workShift, basicCode = y })
                        // Group join vs Basic Code Language
                        .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == filter.Language.ToLower(), true),
                                    x => new { x.basicCode.Type_Seq, x.basicCode.Code },
                                    y => new { y.Type_Seq, y.Code },
                                    (x, y) => new { x.workShift, x.basicCode, basicCode_lang = y })
                                    .SelectMany(x => x.basicCode_lang.DefaultIfEmpty(),
                                    (x, y) => new { x.workShift, x.basicCode, basicCode_lang = y })
                        .Select(x => new HRMS_Att_Work_ShiftDto()
                        {
                            Division = x.workShift.Division,
                            Factory = x.workShift.Factory,
                            Work_Shift_Type = x.workShift.Work_Shift_Type,
                            Work_Shift_Type_Title = x.basicCode_lang != null ? $"{x.workShift.Work_Shift_Type} - {x.basicCode_lang.Code_Name}" : $"{x.workShift.Work_Shift_Type} - {x.basicCode.Code_Name}",
                            Week = int.Parse(x.workShift.Week),
                            Clock_In = x.workShift.Clock_In,
                            Clock_Out = x.workShift.Clock_Out,
                            Overtime_ClockIn = x.workShift.Overtime_ClockIn,
                            Overtime_ClockOut = x.workShift.Overtime_ClockOut,
                            Lunch_Start = x.workShift.Lunch_Start,
                            Lunch_End = x.workShift.Lunch_End,
                            Overnight = x.workShift.Overnight.Trim(),
                            Work_Hours = x.workShift.Work_Hours.ToString(),
                            Work_Days = x.workShift.Work_Days.ToString(),
                            Effective_State = x.workShift.Effective_State,
                            Update_By = x.workShift.Update_By,
                            Update_Time = x.workShift.Update_Time,
                        })
                        .ToListAsync();

            return PaginationUtility<HRMS_Att_Work_ShiftDto>.Create(query, param.PageNumber, param.PageSize);
        }

        public async Task<List<KeyValuePair<string, string>>> GetDivisions(string language) =>
        await GetDataBasicCode(BasicCodeTypeConstant.Division, language);

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

        public async Task<List<KeyValuePair<string, string>>> GetFactories(string division, string language, string userName)
        {
            var factorys = await Queryt_Factory_AddList(userName);
            var factories = await _repositoryAccessor.HRMS_Basic_Factory_Comparison.FindAll(x => x.Division == division.Trim() && x.Kind == "1" && factorys.Contains(x.Factory), true)
                            .Join(_repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Factory, true),
                                x => new { Code = x.Factory },
                                z => new { z.Code },
                                (x, z) => new { compare = x, basicCode = z })
                            .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                                    x => new { x.basicCode.Type_Seq, x.basicCode.Code },
                                    y => new { y.Type_Seq, y.Code },
                                    (x, y) => new { x.compare, x.basicCode, basicCodeLang = y })
                                    .SelectMany(x => x.basicCodeLang.DefaultIfEmpty(),
                                    (x, y) => new { x.compare, x.basicCode, basicCodeLang = y })
                            .Select(x => new KeyValuePair<string, string>(x.compare.Factory, $"{x.basicCode.Code} - {(x.basicCodeLang != null ? x.basicCodeLang.Code_Name : x.basicCode.Code_Name)}")
                            ).ToListAsync();
            return factories;
        }

        public async Task<List<KeyValuePair<string, string>>> GetWorkShiftTypes(string language) => await GetDataBasicCode(BasicCodeTypeConstant.WorkShiftType, language);

        public async Task<OperationResult> Update(HRMS_Att_Work_ShiftDto model)
        {
            var workShift = await _repositoryAccessor.HRMS_Att_Work_Shift.FirstOrDefaultAsync(
                    x => x.Division == model.Division
                        && x.Factory == model.Factory
                        && x.Work_Shift_Type == model.Work_Shift_Type
                        && x.Week == model.Week.ToString()
                    );
            if (workShift == null) return new OperationResult(false, "Shift Schedule not existed");

            workShift = Mapper.Map(model).Over(workShift);
            try
            {
                workShift.Update_Time = Convert.ToDateTime(model.Update_Time_Str);
                _repositoryAccessor.HRMS_Att_Work_Shift.Update(workShift);
                await _repositoryAccessor.Save();
                return new OperationResult(true);
            }
            catch
            {
                return new OperationResult(false);
            }
        }
    }
}