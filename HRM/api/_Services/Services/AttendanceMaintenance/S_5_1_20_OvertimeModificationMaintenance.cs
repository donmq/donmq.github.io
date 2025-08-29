using System.Globalization;
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
    public class S_5_1_20_OvertimeModificationMaintenance : BaseServices, I_5_1_20_OvertimeModificationMaintenance
    {
        private readonly List<string> char2HoursList = new() {
            "Overtime_Hours",
            "Night_Hours",
            "Night_Overtime_Hours",
            "Training_Hours"
        };
        public S_5_1_20_OvertimeModificationMaintenance(DBContext dbContext) : base(dbContext)
        {
        }

        #region GetListFactory               
        public async Task<List<KeyValuePair<string, string>>> GetListFactory(string language, string userName)
        {
            var factorys = await Queryt_Factory_AddList(userName);
            var predHBC = PredicateBuilder.New<HRMS_Basic_Code>(x => x.IsActive && factorys.Contains(x.Code) && x.Type_Seq == BasicCodeTypeConstant.Factory);
            var data = await _repositoryAccessor.HRMS_Basic_Code.FindAll(predHBC, true)
                        .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Factory && x.Language_Code.ToLower() == language.ToLower(), true),
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
        #endregion

        #region GetWorkShiftType
        public async Task<OperationResult> GetWorkShiftType(OvertimeModificationMaintenanceParam param)
        {
            var HACR = _repositoryAccessor.HRMS_Att_Change_Record.FindAll(x =>
                x.Factory == param.Factory &&
                x.Employee_ID == param.Employee_ID &&
                x.Att_Date.Date == Convert.ToDateTime(param.AttDate).Date);
            var HBC = _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == BasicCodeTypeConstant.WorkShiftType);
            var data = await HACR
                .Join(HBC,
                    x => x.Work_Shift_Type,
                    y => y.Code,
                    (x, y) => new { HACR = x })
                .FirstOrDefaultAsync();
            if (data == null)
                return new OperationResult(false);
            return new OperationResult(true, data.HACR);
        }
        #endregion

        #region GetListHoliday
        public async Task<List<KeyValuePair<string, string>>> GetListHoliday(string language)
        {
            var predHBC = PredicateBuilder.New<HRMS_Basic_Code>(x => x.Type_Seq == BasicCodeTypeConstant.Holiday && x.Char1 == "Attendance");
            var data = await _repositoryAccessor.HRMS_Basic_Code.FindAll(predHBC, true)
                        .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Holiday && x.Language_Code.ToLower() == language.ToLower(), true),
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
        #endregion

        #region GetListDepartment
        public async Task<List<KeyValuePair<string, string>>> GetListDepartment(string factory, string language)
        {
            var data = await _repositoryAccessor.HRMS_Org_Department.FindAll(x => x.Factory == factory, true)
                        .Join(_repositoryAccessor.HRMS_Basic_Factory_Comparison.FindAll(x => x.Kind == "1", true),
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
        #endregion

        #region GetData
        public async Task<PaginationUtility<OvertimeModificationMaintenanceDto>> GetData(PaginationParam pagination, OvertimeModificationMaintenanceParam param)
        {
            var pred_HRMS_Att_Overtime_Maintain = PredicateBuilder.New<HRMS_Att_Overtime_Maintain>(true);
            if (!string.IsNullOrWhiteSpace(param.Factory))
                pred_HRMS_Att_Overtime_Maintain.And(x => x.Factory == param.Factory);
            if (!string.IsNullOrWhiteSpace(param.Employee_ID))
                pred_HRMS_Att_Overtime_Maintain.And(x => x.Employee_ID.Contains(param.Employee_ID));
            if (!string.IsNullOrWhiteSpace(param.Work_Shift_Type))
                pred_HRMS_Att_Overtime_Maintain.And(x => x.Work_Shift_Type == param.Work_Shift_Type);
            if (!string.IsNullOrWhiteSpace(param.Overtime_Date_From) && !string.IsNullOrWhiteSpace(param.Overtime_Date_To))
                pred_HRMS_Att_Overtime_Maintain.And(x =>
                    x.Overtime_Date >= Convert.ToDateTime(param.Overtime_Date_From) &&
                    x.Overtime_Date <= Convert.ToDateTime(param.Overtime_Date_To));

            var HBC = _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.IsActive);
            var HBCL = _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == param.Language.ToLower());
            var HBC_Holiday = HBC.Where(x => x.Type_Seq == BasicCodeTypeConstant.Holiday && x.Char1 == "Attendance")
                .GroupJoin(HBCL.Where(x => x.Type_Seq == BasicCodeTypeConstant.Holiday),
                    x => new { x.Type_Seq, x.Code },
                    y => new { y.Type_Seq, y.Code },
                    (x, y) => new { HBC = x, HBCL = y })
                .SelectMany(x => x.HBCL.DefaultIfEmpty(),
                    (x, y) => new { x.HBC, HBCL = y })
                .Select(x => new
                {
                    x.HBC.Code,
                    Code_Name = $"{x.HBC.Code}-{(x.HBCL != null ? x.HBCL.Code_Name : x.HBC.Code_Name)}"
                });

            var HBC_WorkShiftType = HBC.Where(x => x.Type_Seq == BasicCodeTypeConstant.WorkShiftType)
                .GroupJoin(HBCL.Where(x => x.Type_Seq == BasicCodeTypeConstant.WorkShiftType),
                    x => new { x.Type_Seq, x.Code },
                    y => new { y.Type_Seq, y.Code },
                    (x, y) => new { HBC = x, HBCL = y })
                .SelectMany(x => x.HBCL.DefaultIfEmpty(),
                    (x, y) => new { x.HBC, HBCL = y })
                .Select(x => new
                {
                    x.HBC.Code,
                    Code_Name = $"{x.HBC.Code}-{(x.HBCL != null ? x.HBCL.Code_Name : x.HBC.Code_Name)}"
                });

            var HOD = _repositoryAccessor.HRMS_Org_Department.FindAll(x => x.Factory == param.Factory);
            var HODL = _repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == param.Language.ToLower());
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
                    x.HOD.Division,
                    x.HOD.Department_Code,
                    Department_Name = x.HODL != null ? x.HODL.Name : x.HOD.Department_Name
                });

            var HAOM = _repositoryAccessor.HRMS_Att_Overtime_Maintain.FindAll(pred_HRMS_Att_Overtime_Maintain);
            var HAWS = _repositoryAccessor.HRMS_Att_Work_Shift.FindAll(x => x.Effective_State);
            var HACR = _repositoryAccessor.HRMS_Att_Change_Record.FindAll();

            var permissionGroupQuery = _repositoryAccessor.HRMS_Basic_Role.FindAll(x => x.Factory == param.Factory, true).Select(x => x.Permission_Group);
            var HEP = _repositoryAccessor.HRMS_Emp_Personal
                .FindAll(x => (x.Factory == param.Factory) && permissionGroupQuery.Contains(x.Permission_Group))
                .Select(x => new
                {
                    x.USER_GUID,
                    x.Local_Full_Name,
                    Division = x.Employment_Status == "A" || x.Employment_Status == "S" ? x.Assigned_Division : x.Division,
                    Factory = x.Employment_Status == "A" || x.Employment_Status == "S" ? x.Assigned_Factory : x.Factory,
                    Department = x.Employment_Status == "A" || x.Employment_Status == "S" ? x.Assigned_Department : x.Department,
                });

            var result = await HAOM
                .Join(HEP,
                    x => x.USER_GUID,
                    y => y.USER_GUID,
                    (x, y) => new { HAOM = x, HEP = y })
                .GroupJoin(HAWS,
                    x => new { x.HAOM.Factory, x.HAOM.Work_Shift_Type, Week = (EF.Functions.DateDiffDay(new DateTime(1, 1, 1), x.HAOM.Overtime_Date.AddDays(1)) % 7).ToString() },
                    e => new { e.Factory, e.Work_Shift_Type, e.Week },
                    (x, e) => new { x.HAOM, x.HEP, HAWS = e })
                .SelectMany(x => x.HAWS.DefaultIfEmpty(),
                    (x, e) => new { x.HAOM, x.HEP, HAWS = e })
                .GroupJoin(HACR,
                    x => new { x.HAOM.Factory, x.HAOM.Employee_ID, x.HAOM.Overtime_Date },
                    f => new { f.Factory, f.Employee_ID, Overtime_Date = f.Att_Date },
                    (x, y) => new { x.HAOM, x.HEP, x.HAWS, HACR = y })
                .SelectMany(x => x.HACR.DefaultIfEmpty(),
                    (x, y) => new { x.HAOM, x.HEP, x.HAWS, HACR = y })
                .GroupJoin(HBC_Holiday,
                    x => x.HAOM.Holiday,
                    y => y.Code,
                    (x, y) => new { x.HAOM, x.HEP, x.HAWS, x.HACR, HBC_Holiday = y })
                .SelectMany(x => x.HBC_Holiday.DefaultIfEmpty(),
                    (x, y) => new { x.HAOM, x.HEP, x.HAWS, x.HACR, HBC_Holiday = y })
                .GroupJoin(HBC_WorkShiftType,
                    x => x.HAOM.Work_Shift_Type,
                    d => d.Code,
                    (x, d) => new { x.HAOM, x.HEP, x.HAWS, x.HACR, x.HBC_Holiday, HBC_WorkShiftType = d })
                .SelectMany(x => x.HBC_WorkShiftType.DefaultIfEmpty(),
                    (x, y) => new { x.HAOM, x.HEP, x.HAWS, x.HACR, x.HBC_Holiday, HBC_WorkShiftType = y })
                .GroupJoin(HOD_Lang,
                    x => new { x.HEP.Factory, x.HEP.Division, Department_Code = x.HEP.Department },
                    y => new { y.Factory, y.Division, y.Department_Code },
                    (x, y) => new { x.HAOM, x.HEP, x.HAWS, x.HACR, x.HBC_Holiday, x.HBC_WorkShiftType, HOD_Lang = y })
                .SelectMany(x => x.HOD_Lang.DefaultIfEmpty(),
                    (x, y) => new { x.HAOM, x.HEP, x.HAWS, x.HACR, x.HBC_Holiday, x.HBC_WorkShiftType, HOD_Lang = y })
                .Select(x => new OvertimeModificationMaintenanceDto()
                {
                    USER_GUID = x.HAOM.USER_GUID,
                    Factory = x.HAOM.Factory,
                    Department_Code = x.HEP.Department,
                    Department_Name = x.HOD_Lang.Department_Name,
                    Department_Code_Name = x.HOD_Lang != null && !string.IsNullOrWhiteSpace(x.HOD_Lang.Department_Name)
                        ? x.HOD_Lang.Department_Code + "-" + x.HOD_Lang.Department_Name : x.HEP.Department,
                    Employee_ID = x.HAOM.Employee_ID,
                    Local_Full_Name = x.HEP.Local_Full_Name,
                    Overtime_Date = x.HAOM.Overtime_Date,
                    Overtime_Date_Str = x.HAOM.Overtime_Date.ToString("yyyy/MM/dd"),
                    Work_Shift_Type = x.HAOM.Work_Shift_Type,
                    Work_Shift_Type_Name = x.HBC_WorkShiftType != null ? x.HBC_WorkShiftType.Code_Name : x.HAOM.Work_Shift_Type,
                    Work_Shift_Type_Time = x.HAWS != null
                        ? $"{x.HAWS.Clock_In.Substring(0, 2)}:{x.HAWS.Clock_In.Substring(2, 2)}~{x.HAWS.Clock_Out.Substring(0, 2)}:{x.HAWS.Clock_Out.Substring(2, 2)}" : "",
                    Clock_In_Time = x.HACR != null
                        ? $"{x.HACR.Clock_In.Substring(0, 2)}:{x.HACR.Clock_In.Substring(2, 2)}" : "",
                    Clock_Out_Time = x.HACR != null
                        ? $"{x.HACR.Clock_Out.Substring(0, 2)}:{x.HACR.Clock_Out.Substring(2, 2)}" : "",
                    Overtime_Start = x.HAOM.Overtime_Start,
                    Overtime_Start_Temp = $"{x.HAOM.Overtime_Start.Substring(0, 2)}:{x.HAOM.Overtime_Start.Substring(2, 2)}",
                    Overtime_End = x.HAOM.Overtime_End,
                    Overtime_End_Temp = $"{x.HAOM.Overtime_End.Substring(0, 2)}:{x.HAOM.Overtime_End.Substring(2, 2)}",
                    Overtime_Hours = x.HAOM.Overtime_Hours.ToString(),
                    Night_Hours = x.HAOM.Night_Hours.ToString(),
                    Night_Overtime_Hours = x.HAOM.Night_Overtime_Hours.ToString(),
                    Training_Hours = x.HAOM.Training_Hours.ToString(),
                    Night_Eat_Times = x.HAOM.Night_Eat_Times.ToString(),
                    Holiday = x.HAOM.Holiday,
                    Holiday_Name = x.HBC_Holiday != null ? x.HBC_Holiday.Code_Name : x.HAOM.Holiday,
                    IsOvertimeDate = x.HAOM.Overtime_Date.Date < DateTime.Today.AddDays(-90).Date,
                    Update_By = x.HAOM.Update_By,
                    Update_Time = x.HAOM.Update_Time,
                    Update_Time_Temp = x.HAOM.Update_Time.ToString("yyyy/MM/dd HH:mm:ss")
                }).ToListAsync();

            if (!string.IsNullOrWhiteSpace(param.Department))
                result = result.FindAll(x => x.Department_Code == param.Department);
            return PaginationUtility<OvertimeModificationMaintenanceDto>.Create(result, pagination.PageNumber, pagination.PageSize);
        }
        #endregion

        #region Create
        public async Task<OperationResult> Create(OvertimeModificationMaintenanceDto model, string userName)
        {
            await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                if (string.IsNullOrWhiteSpace(model.Factory)
                || string.IsNullOrWhiteSpace(model.Employee_ID)
                || string.IsNullOrWhiteSpace(model.Overtime_Date_Str)
                || !DateTime.TryParseExact(model.Overtime_Date_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime Overtime_Date))
                    return new OperationResult(false, "Invalid inputs");
                model.Overtime_Date = Overtime_Date;
                model.Update_By = userName;
                model.Update_Time = DateTime.Now;
                if (await _repositoryAccessor.HRMS_Att_Overtime_Maintain.AnyAsync(x =>
                    x.Factory == model.Factory &&
                    x.Overtime_Date.Date == Overtime_Date.Date &&
                    x.Employee_ID == model.Employee_ID))
                    return new OperationResult(false, $"Factory: {model.Factory}, Date: {model.Overtime_Date_Str}, Employee ID: {model.Employee_ID} already exists.");

                var HAY = _repositoryAccessor.HRMS_Att_Yearly.FindAll(true);
                var HBC = _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Allowance && x.Char1 == model.Holiday, true);

                var char2s = HBC.Select(x => x.Char2).ToList();
                foreach (string char2Hours in char2HoursList)
                {
                    if (char2s.Contains(char2Hours))
                    {
                        string hours = (string)model.GetType().GetProperty(char2Hours).GetValue(model);
                        var data = await SetHRMS_Att_Yearly(model, HAY, HBC, char2Hours, decimal.Parse(hours));
                        if (!data.IsSuccess)
                        {
                            await _repositoryAccessor.RollbackAsync();
                            return new OperationResult(false, data.Error);
                        }
                    }
                }
                var entity = Mapper.Map(model).ToANew<HRMS_Att_Overtime_Maintain>(x => x.MapEntityKeys());
                _repositoryAccessor.HRMS_Att_Overtime_Maintain.Add(entity);
                await _repositoryAccessor.Save();
                await _repositoryAccessor.CommitAsync();
                return new OperationResult(true);
            }
            catch (Exception)
            {
                await _repositoryAccessor.RollbackAsync();
                return new OperationResult(false, "Oops! Sorry, an error occurred while processing your request");
            }
        }
        #endregion

        #region Edit
        public async Task<OperationResult> Edit(OvertimeModificationMaintenanceDto model, string userName)
        {
            await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                if (string.IsNullOrWhiteSpace(model.Factory)
                || string.IsNullOrWhiteSpace(model.Employee_ID)
                || string.IsNullOrWhiteSpace(model.Overtime_Date_Str)
                || !DateTime.TryParseExact(model.Overtime_Date_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime Overtime_Date))
                    return new OperationResult(false, "Invalid inputs");
                model.Overtime_Date = Overtime_Date;
                model.Update_By = userName;
                model.Update_Time = DateTime.Now;
                var oldData = await _repositoryAccessor.HRMS_Att_Overtime_Maintain.FirstOrDefaultAsync(x =>
                    x.Factory == model.Factory &&
                    x.Overtime_Date.Date == Overtime_Date.Date &&
                    x.Employee_ID == model.Employee_ID);
                if (oldData == null)
                    return new OperationResult(false, "Data not found");

                var HAY = _repositoryAccessor.HRMS_Att_Yearly.FindAll(true);
                var HBC = _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Allowance && x.IsActive, true);

                var newHBC = HBC.Where(x => x.Char1 == model.Holiday);
                var char2s = newHBC.Select(x => x.Char2).ToList();
                foreach (string char2Hours in char2HoursList)
                {
                    if (char2s.Contains(char2Hours))
                    {
                        string newHours = (string)model.GetType().GetProperty(char2Hours).GetValue(model);
                        decimal oldHours = (decimal)oldData.GetType().GetProperty(char2Hours).GetValue(oldData);

                        var data = await SetHRMS_Att_Yearly(model, HAY, newHBC, char2Hours,
                            oldData.Holiday != model.Holiday
                                ? decimal.Parse(newHours)
                                : decimal.Parse(newHours) - oldHours);
                        if (!data.IsSuccess)
                        {
                            await _repositoryAccessor.RollbackAsync();
                            return new OperationResult(false, data.Error);
                        }
                    }
                }
                // Kiểm tra dữ liệu cũ tại model và thêm vô danh sách cập nhật
                if (oldData.Holiday != model.Holiday)
                {
                    var oldHBC = HBC.Where(x => x.Char1 == oldData.Holiday);
                    var char2sOld = oldHBC.Select(x => x.Char2).ToList();
                    foreach (string char2Hours in char2HoursList)
                    {
                        if (char2sOld.Contains(char2Hours))
                        {
                            decimal oldHours = (decimal)oldData.GetType().GetProperty(char2Hours).GetValue(oldData);
                            var data = await SetHRMS_Att_Yearly(model, HAY, oldHBC, char2Hours, 0 - oldHours);
                            if (!data.IsSuccess)
                            {
                                await _repositoryAccessor.RollbackAsync();
                                return new OperationResult(false, data.Error);
                            }
                        }
                    }
                }
                oldData.Work_Shift_Type = model.Work_Shift_Type;
                oldData.Overtime_Start = model.Overtime_Start;
                oldData.Overtime_End = model.Overtime_End;
                oldData.Holiday = model.Holiday;
                oldData.Night_Eat_Times = int.Parse(model.Night_Eat_Times);
                oldData.Overtime_Hours = decimal.Parse(model.Overtime_Hours);
                oldData.Night_Hours = decimal.Parse(model.Night_Hours);
                oldData.Night_Overtime_Hours = decimal.Parse(model.Night_Overtime_Hours);
                oldData.Training_Hours = decimal.Parse(model.Training_Hours);
                oldData.Update_By = model.Update_By;
                oldData.Update_Time = model.Update_Time;
                _repositoryAccessor.HRMS_Att_Overtime_Maintain.Update(oldData);
                await _repositoryAccessor.Save();
                await _repositoryAccessor.CommitAsync();
                return new OperationResult(true);
            }
            catch (Exception)
            {
                await _repositoryAccessor.RollbackAsync();
                return new OperationResult(false, "Oops! Sorry, an error occurred while processing your request");
            }
        }
        #endregion

        #region Delete
        public async Task<OperationResult> Delete(OvertimeModificationMaintenanceDto model, string userName)
        {
            await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                if (string.IsNullOrWhiteSpace(model.Factory)
                || string.IsNullOrWhiteSpace(model.Employee_ID)
                || string.IsNullOrWhiteSpace(model.Overtime_Date_Str)
                || !DateTime.TryParseExact(model.Overtime_Date_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime Overtime_Date))
                    return new OperationResult(false, "Invalid inputs");
                model.Overtime_Date = Overtime_Date;
                model.Update_By = userName;
                model.Update_Time = DateTime.Now;
                var oldData = await _repositoryAccessor.HRMS_Att_Overtime_Maintain.FirstOrDefaultAsync(x =>
                    x.Factory == model.Factory &&
                    x.Overtime_Date.Date == Overtime_Date.Date &&
                    x.Employee_ID == model.Employee_ID);
                if (oldData == null)
                    return new OperationResult(false, "Data not found");

                var HAY = _repositoryAccessor.HRMS_Att_Yearly.FindAll(true);
                var oldHBC = _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Allowance && x.Char1 == oldData.Holiday && x.IsActive, true);

                List<string> char2sOld = oldHBC.Select(x => x.Char2).ToList();
                foreach (string char2Hours in char2HoursList)
                {
                    if (char2sOld.Contains(char2Hours))
                    {
                        decimal oldHours = (decimal)oldData.GetType().GetProperty(char2Hours).GetValue(oldData);
                        var data = await SetHRMS_Att_Yearly(model, HAY, oldHBC, char2Hours, 0 - oldHours);
                        if (!data.IsSuccess)
                        {
                            await _repositoryAccessor.RollbackAsync();
                            return new OperationResult(false, data.Error);
                        }
                    }
                }
                _repositoryAccessor.HRMS_Att_Overtime_Maintain.Remove(oldData);
                await _repositoryAccessor.Save();
                await _repositoryAccessor.CommitAsync();
                return new OperationResult(true);
            }
            catch (Exception)
            {
                await _repositoryAccessor.RollbackAsync();
                return new OperationResult(false, "Oops! Sorry, an error occurred while processing your request");
            }
        }
        #endregion

        public async Task<ClockInClockOut> GetWorkShiftTypeTime(string work_Shift_Type, string date, string factory)
        {
            var data = await _repositoryAccessor.HRMS_Att_Work_Shift
                .FindAll(x =>
                    x.Work_Shift_Type == work_Shift_Type &&
                    x.Factory == factory &&
                    x.Week == (EF.Functions.DateDiffDay(new DateTime(1, 1, 1), Convert.ToDateTime(date).AddDays(1)) % 7).ToString(), true)
                .Select(x => new ClockInClockOut()
                {
                    Work_Shift_Type_Time = $"{x.Clock_In.Substring(0, 2)}:{x.Clock_In.Substring(2, 2)}~{x.Clock_Out.Substring(0, 2)}:{x.Clock_Out.Substring(2, 2)}"
                }).FirstOrDefaultAsync();
            return data;
        }

        public async Task<ClockInClockOut> GetClockInTimeAndClockOutTimeByEmpIdAndDate(string employee_ID, string date)
        {
            if (!await _repositoryAccessor.HRMS_Att_Change_Record.AnyAsync(x => x.Employee_ID == employee_ID))
                return new ClockInClockOut();
            var data = await _repositoryAccessor.HRMS_Att_Change_Record
                .FindAll(x => x.Employee_ID == employee_ID && x.Att_Date == Convert.ToDateTime(date))
                .Select(x => new ClockInClockOut()
                {
                    Clock_In_Time = x.Clock_In,
                    Clock_Out_Time = x.Clock_Out
                }).FirstOrDefaultAsync();
            return data;
        }

        public async Task<OperationResult> SetHRMS_Att_Yearly(
            OvertimeModificationMaintenanceDto model, IQueryable<HRMS_Att_Yearly> HAY, IQueryable<HRMS_Basic_Code> HBC,
            string columnChange, decimal dayscolumnChange, string Leave_Type = "2")
        {
            try
            {
                var att_Year = new DateTime(model.Overtime_Date.Year, 1, 1);
                var Leave_Code = await HBC.FirstOrDefaultAsync(o => o.Char2 == columnChange);
                if (Leave_Code == null)
                    return new OperationResult(false, string.Format(
                    "Background save: \nHRMS_Basic_Code not found\nType_Seq: {0}\nChar1: {1}\nChar2: {2}",
                    BasicCodeTypeConstant.Allowance, model.Holiday, columnChange));
                var itemUpdate = await HAY.FirstOrDefaultAsync(x =>
                    x.Factory == model.Factory &&
                    x.Att_Year.Date == att_Year.Date &&
                    x.Employee_ID == model.Employee_ID &&
                    x.Leave_Type == Leave_Type &&
                    x.Leave_Code == Leave_Code.Code);
                if (itemUpdate == null)
                    return new OperationResult(false, string.Format(
                    "Background save: \nHRMS_Att_Yearly not found\nFactory: {0}\nAtt Year: {1:yyyy/MM/dd}\nEmployee ID: {2}\nLeave Type: {3}\nLeave code: {4}",
                    model.Factory, att_Year, model.Employee_ID, Leave_Type, Leave_Code.Code));
                itemUpdate.Days += dayscolumnChange;
                itemUpdate.Update_By = model.Update_By;
                itemUpdate.Update_Time = model.Update_Time;
                _repositoryAccessor.HRMS_Att_Yearly.Update(itemUpdate);
                var result = await _repositoryAccessor.Save();
                if (!result)
                    return new OperationResult(false, "UPDATE HRMS_Att_Yearly Fail!");
                return new OperationResult(true);
            }
            catch (Exception)
            {
                return new OperationResult(false, "Oops! Sorry, an error occurred while processing your request");
            }
        }
    }
}