using System.Globalization;
using API.Data;
using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.AttendanceMaintenance
{
    public class S_5_1_10_ShiftManagementProgram : BaseServices, I_5_1_10_ShiftManagementProgram
    {
        public S_5_1_10_ShiftManagementProgram(DBContext dbContext) : base(dbContext)
        {
        }
        #region Dropdown List
        public async Task<List<KeyValuePair<string, string>>> GetDropDownList(ShiftManagementProgram_Param param, List<string> roleList)
        {
            var HBC = await _repositoryAccessor.HRMS_Basic_Code.FindAll().ToListAsync();
            var HBCL = _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == param.Lang.ToLower()).ToList();
            var result = new List<KeyValuePair<string, string>>();
            var data = HBC.GroupJoin(HBCL,
                    x => new { x.Type_Seq, x.Code },
                    y => new { y.Type_Seq, y.Code },
                    (x, y) => new { hbc = x, hbcl = y })
                    .SelectMany(x => x.hbcl.DefaultIfEmpty(),
                    (x, y) => new { x.hbc, hbcl = y });
            result.AddRange(data.Where(x => x.hbc.Type_Seq == "1").Select(x => new KeyValuePair<string, string>("DI", $"{x.hbc.Code}-{(x.hbcl != null ? x.hbcl.Code_Name : x.hbc.Code_Name)}")).Distinct().ToList());
            result.AddRange(data.Where(x => x.hbc.Type_Seq == "41").Select(x => new KeyValuePair<string, string>("WO", $"{x.hbc.Code}-{(x.hbcl != null ? x.hbcl.Code_Name : x.hbc.Code_Name)}")).Distinct().ToList());
            if (!string.IsNullOrWhiteSpace(param.Division))
            {
                var comparisonFactories = await Query_Factory_List(param.Division);
                var authFactories = await Queryt_Factory_AddList(roleList);
                result.AddRange(data.Where(x => x.hbc.Type_Seq == "2" && comparisonFactories.Contains(x.hbc.Code) && authFactories.Contains(x.hbc.Code)).Select(x => new KeyValuePair<string, string>("FA", $"{x.hbc.Code}-{(x.hbcl != null ? x.hbcl.Code_Name : x.hbc.Code_Name)}")).Distinct().ToList());
            }
            return result;
        }
        public async Task<List<KeyValuePair<string, string>>> GetDepartmentList(ShiftManagementProgram_Param param)
        {
            var HOD = await _repositoryAccessor.HRMS_Org_Department
                .FindAll(x =>
                    x.Factory == param.Factory &&
                    x.Division == param.Division)
                .OrderBy(x => x.Department_Code).ToListAsync();
            var HODL = _repositoryAccessor.HRMS_Org_Department_Language
                .FindAll(x =>
                    x.Factory == param.Factory &&
                    x.Division == param.Division &&
                    x.Language_Code.ToLower() == param.Lang.ToLower())
                .ToList();
            var data = HOD.GroupJoin(HODL,
                    x => x.Department_Code,
                    y => y.Department_Code,
                    (x, y) => new { hod = x, hodl = y })
                    .SelectMany(x => x.hodl.DefaultIfEmpty(),
                    (x, y) => new { x.hod, hodl = y });
            List<KeyValuePair<string, string>> result = data.Select(x => new KeyValuePair<string, string>(x.hod.Department_Code, $"{x.hod.Department_Code}-{(x.hodl != null ? x.hodl.Name : x.hod.Department_Name)}")).Distinct().ToList();
            return result;
        }
        public async Task<List<KeyValuePair<string, string>>> GetWorkShiftTypeDepartmentList(ShiftManagementProgram_Param param)
        {
            var result = new List<KeyValuePair<string, string>>();
            if (!string.IsNullOrWhiteSpace(param.Division) && !string.IsNullOrWhiteSpace(param.Factory) && !string.IsNullOrWhiteSpace(param.Department))
            {
                var HEP = _repositoryAccessor.HRMS_Emp_Personal.FindAll(x =>
                               (x.Division == param.Division && x.Factory == param.Factory && x.Department == param.Department) ||
                               (x.Assigned_Division == param.Division && x.Assigned_Factory == param.Factory && x.Assigned_Department == param.Department)).ToList();
                var HBC = await _repositoryAccessor.HRMS_Basic_Code.FindAll().ToListAsync();
                var HBCL = _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == param.Lang.ToLower()).ToList();
                var newWST = HBC
                    .GroupJoin(HBCL,
                         x => new { x.Type_Seq, x.Code },
                        y => new { y.Type_Seq, y.Code },
                        (x, y) => new { HBC = x, HBCL = y })
                    .SelectMany(x => x.HBCL.DefaultIfEmpty(),
                        (x, y) => new { x.HBC, HBCL = y });
                result.AddRange(newWST.Where(x => x.HBC.Type_Seq == "41").Select(x => new KeyValuePair<string, string>("N", $"{x.HBC.Code}-{(x.HBCL != null ? x.HBCL.Code_Name : x.HBC.Code_Name)}")).Distinct().ToList());
                var oldWST = HEP
                    .GroupJoin(newWST,
                        x => x.Work_Shift_Type,
                        y => y.HBC.Code,
                        (x, y) => new { HEP = x, y })
                    .SelectMany(x => x.y.DefaultIfEmpty(),
                        (x, y) => new { x.HEP, y });
                result.AddRange(oldWST.Where(x => x.y.HBC.Type_Seq == "41").Select(x => new KeyValuePair<string, string>("O", $"{x.y.HBC.Code}-{(x.y.HBCL != null ? x.y.HBCL.Code_Name : x.y.HBC.Code_Name)}")).Distinct().ToList());
            }
            return result;
        }
        public async Task<List<TypeheadKeyValue>> GetEmployeeList(ShiftManagementProgram_Param param)
        {
            var HEP = await _repositoryAccessor.HRMS_Emp_Personal.FindAll(x =>
                x.Division == param.Division &&
                x.Factory == param.Factory &&
                x.Employee_ID.Contains(param.Employee_Id) &&
                x.Resign_Date == null &&
                string.IsNullOrWhiteSpace(x.Employment_Status)
            ).ToListAsync();
            var result = HEP
                .Select(x => new TypeheadKeyValue()
                {
                    USER_GUID = x.USER_GUID,
                    Key = x.Employee_ID,
                    Name = x.Local_Full_Name,
                    Department = x.Department,
                    Work_Shift_Type_Old = x.Work_Shift_Type
                })
                .ToList();
            return result;
        }
        public async Task<List<ShiftManagementProgram_Main>> GetEmployeeDetail(ShiftManagementProgram_Param param)
        {
            var HEP = await _repositoryAccessor.HRMS_Emp_Personal.FindAll(x =>
                x.Division == param.Division &&
                x.Factory == param.Factory &&
                x.Department == param.Department &&
                x.Resign_Date == null &&
                string.IsNullOrWhiteSpace(x.Employment_Status)
            ).ToListAsync();
            var result = HEP
                .Select(x => new ShiftManagementProgram_Main()
                {
                    USER_GUID = x.USER_GUID,
                    Employee_Id = x.Employee_ID,
                    Local_Full_Name = x.Local_Full_Name,
                    Department = x.Department,
                    Work_Shift_Type_Old = x.Work_Shift_Type
                })
                .ToList();
            return result;
        }
        #endregion

        #region Query Data
        public async Task<PaginationUtility<ShiftManagementProgram_Main>> GetSearchDetail(PaginationParam paginationParams, ShiftManagementProgram_Param searchParam)
        {
            var predicate = PredicateBuilder.New<HRMS_Att_Work_Shift_Change>(true);
            var predicatePersonal = PredicateBuilder.New<HRMS_Emp_Personal>(true);
            if (!string.IsNullOrWhiteSpace(searchParam.Division))
                predicate.And(x => x.Division == searchParam.Division);
            if (!string.IsNullOrWhiteSpace(searchParam.Factory))
                predicate.And(x => x.Factory == searchParam.Factory);
            if (!string.IsNullOrWhiteSpace(searchParam.Work_Shift_Type_New))
                predicate.And(x => x.Work_Shift_Type_New == searchParam.Work_Shift_Type_New);
            if (!string.IsNullOrWhiteSpace(searchParam.Effective_Date_Str) && DateTime.TryParseExact(searchParam.Effective_Date_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime effectiveDateValue))
                predicate.And(x => x.Effective_Date.Date == effectiveDateValue.Date);
            if (!string.IsNullOrWhiteSpace(searchParam.Department))
                predicate.And(x => x.Department == searchParam.Department);
            if (!string.IsNullOrWhiteSpace(searchParam.Employee_Id))
                predicatePersonal.And(x => x.Employee_ID.Contains(searchParam.Employee_Id));

            var HAWSC = _repositoryAccessor.HRMS_Att_Work_Shift_Change.FindAll(predicate);
            var HEP = _repositoryAccessor.HRMS_Emp_Personal.FindAll(predicatePersonal);
            var HOD = _repositoryAccessor.HRMS_Org_Department.FindAll();
            var HODL = _repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == searchParam.Lang.ToLower());
            var HBC = _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "41");
            var HBCL = _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == searchParam.Lang.ToLower());
            var depLang = HOD.GroupJoin(HODL,
                     x => new { x.Factory, x.Division, x.Department_Code },
                    y => new { y.Factory, y.Division, y.Department_Code },
                    (x, y) => new { HOD = x, HODL = y })
                .SelectMany(x => x.HODL.DefaultIfEmpty(),
                    (x, y) => new { x.HOD, HODL = y })
                .Select(x => new
                {
                    x.HOD.Division,
                    x.HOD.Factory,
                    x.HOD.Department_Code,
                    Department_Name = x.HODL != null ? x.HODL.Name : x.HOD.Department_Name
                });
            var codLang = HBC
                .GroupJoin(HBCL,
                    x => new { x.Type_Seq, x.Code },
                    y => new { y.Type_Seq, y.Code },
                    (x, y) => new { HBC = x, HBCL = y })
                .SelectMany(x => x.HBCL.DefaultIfEmpty(),
                    (x, y) => new { x.HBC, HBCL = y })
                .Select(x => new
                {
                    x.HBC.Code,
                    Code_Name = x.HBCL != null ? x.HBCL.Code_Name : x.HBC.Code_Name
                });
            var data = HAWSC
                .Join(HEP,
                    x => x.USER_GUID,
                    y => y.USER_GUID,
                    (x, y) => new { HAWSC = x, HEP = y })
                .OrderBy(x => x.HAWSC.Effective_Date)
                .ThenBy(x => x.HEP.Department)
                .ThenBy(x => x.HEP.Employee_ID);
            var result = data.Select(x => new ShiftManagementProgram_Main
            {
                Division = x.HAWSC.Division,
                Factory = x.HAWSC.Factory,
                USER_GUID = x.HAWSC.USER_GUID,
                Employee_Id = x.HAWSC.Employee_ID,
                Local_Full_Name = x.HEP != null ? x.HEP.Local_Full_Name : "",
                Department = depLang.FirstOrDefault(y => y.Division == x.HAWSC.Division && y.Factory == x.HAWSC.Factory && y.Department_Code == x.HAWSC.Department).Department_Code ?? "",
                Department_Name = depLang.FirstOrDefault(y => y.Division == x.HAWSC.Division && y.Factory == x.HAWSC.Factory && y.Department_Code == x.HAWSC.Department).Department_Name ?? "",
                Work_Shift_Type_Old = x.HAWSC.Work_Shift_Type_Old,
                Work_Shift_Type_Old_Name = codLang.FirstOrDefault(y => y.Code == x.HAWSC.Work_Shift_Type_Old).Code_Name,
                Work_Shift_Type_New = x.HAWSC.Work_Shift_Type_New,
                Work_Shift_Type_New_Name = codLang.FirstOrDefault(y => y.Code == x.HAWSC.Work_Shift_Type_New).Code_Name,
                Effective_Date = x.HAWSC.Effective_Date.ToString("yyyy/MM/dd"),
                Effective_State = x.HAWSC.Effective_State,
                Update_By = x.HAWSC.Update_By,
                Update_Time = x.HAWSC.Update_Time,
                Is_Editable = HEP.Any(y => y.Factory == x.HAWSC.Factory && y.Division == x.HAWSC.Division && y.USER_GUID == x.HAWSC.USER_GUID)
            });
            return await PaginationUtility<ShiftManagementProgram_Main>.CreateAsync(result, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<OperationResult> IsExistedData(ShiftManagementProgram_Param param)
        {
            OperationResult result = new();
            var HEP = await _repositoryAccessor.HRMS_Emp_Personal.FirstOrDefaultAsync(x => x.USER_GUID == param.USER_GUID);
            if (HEP != null)
            {
                var isExisted = await _repositoryAccessor.HRMS_Att_Work_Shift_Change.AnyAsync(x =>
                          x.Division == param.Division &&
                          x.Factory == param.Factory &&
                          x.USER_GUID == param.USER_GUID &&
                          x.Effective_Date.Date == Convert.ToDateTime(param.Effective_Date_Str).Date);
                if (isExisted)
                    return new OperationResult(isExisted);

            }
            return new OperationResult(false);
        }
        #endregion

        #region Add & Update
        public async Task<OperationResult> PostDataEmployee(ShiftManagementProgram_Main input)
        {
            var predicate = PredicateBuilder.New<HRMS_Att_Work_Shift_Change>(true);
            if (string.IsNullOrWhiteSpace(input.Division)
             || string.IsNullOrWhiteSpace(input.Factory)
             || string.IsNullOrWhiteSpace(input.Employee_Id)
             || string.IsNullOrWhiteSpace(input.Effective_Date_Str)
             || !DateTime.TryParseExact(input.Effective_Date_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime effectiveDateValue))
                return new OperationResult(false, "InvalidInput");
            predicate.And(x => x.Division == input.Division && x.Factory == input.Factory && x.Employee_ID == input.Employee_Id && x.Effective_Date.Date == effectiveDateValue.Date);
            if (await _repositoryAccessor.HRMS_Att_Work_Shift_Change.AnyAsync(predicate))
                return new OperationResult(false, "AlreadyExitedData");
            try
            {
                HRMS_Att_Work_Shift_Change data = new()
                {
                    USER_GUID = input.USER_GUID,
                    Division = input.Division,
                    Factory = input.Factory,
                    Department = input.Department,
                    Employee_ID = input.Employee_Id,
                    Effective_Date = effectiveDateValue,
                    Effective_State = input.Effective_State,
                    Work_Shift_Type_Old = input.Work_Shift_Type_Old,
                    Work_Shift_Type_New = input.Work_Shift_Type_New,
                    Update_By = input.Update_By,
                    Update_Time = Convert.ToDateTime(input.Update_Time_Str)
                };
                _repositoryAccessor.HRMS_Att_Work_Shift_Change.Add(data);
                await _repositoryAccessor.Save();
                return new OperationResult(true);
            }
            catch (Exception)
            {
                return new OperationResult(false, "ErrorException");
            }
        }

        public async Task<OperationResult> PostDataDepartment(List<ShiftManagementProgram_Main> input)
        {
            List<HRMS_Att_Work_Shift_Change> addList = new();
            foreach (var item in input)
            {
                if (string.IsNullOrWhiteSpace(item.Division)
                    || string.IsNullOrWhiteSpace(item.Factory)
                    || string.IsNullOrWhiteSpace(item.Department)
                    || string.IsNullOrWhiteSpace(item.Effective_Date))
                    return new OperationResult(false, "InvalidInput");

                var predicateShift = PredicateBuilder.New<HRMS_Att_Work_Shift_Change>(true);

                predicateShift.And(x => x.Division == item.Division
                  && x.Factory == item.Factory
                  && x.Effective_Date.Date == Convert.ToDateTime(item.Effective_Date).Date
                  && x.Employee_ID == item.Employee_Id);

                if (!await _repositoryAccessor.HRMS_Att_Work_Shift_Change.AnyAsync(predicateShift))
                    addList.Add(new()
                    {
                        USER_GUID = item.USER_GUID,
                        Division = item.Division,
                        Factory = item.Factory,
                        Department = item.Department,
                        Employee_ID = item.Employee_Id,
                        Effective_Date = Convert.ToDateTime(item.Effective_Date),
                        Effective_State = item.Effective_State,
                        Work_Shift_Type_Old = item.Work_Shift_Type_Old,
                        Work_Shift_Type_New = item.Work_Shift_Type_New,
                        Update_By = item.Update_By,
                        Update_Time = Convert.ToDateTime(item.Update_Time_Str)
                    });
            }
            try
            {
                _repositoryAccessor.HRMS_Att_Work_Shift_Change.AddMultiple(addList);
                await _repositoryAccessor.Save();
                return new OperationResult(true);
            }
            catch (Exception)
            {
                return new OperationResult(false, "ErrorException");
            }
        }

        public async Task<OperationResult> PutDataEmployee(ShiftManagementProgram_Update input)
        {
            var predicate = PredicateBuilder.New<HRMS_Att_Work_Shift_Change>(true);
            if (string.IsNullOrWhiteSpace(input.Temp_Data.Division)
             || string.IsNullOrWhiteSpace(input.Temp_Data.Factory)
             || string.IsNullOrWhiteSpace(input.Temp_Data.Employee_Id)
             || string.IsNullOrWhiteSpace(input.Temp_Data.Effective_Date_Str)
             || !DateTime.TryParseExact(input.Temp_Data.Effective_Date_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime effectiveDateTempValue)
             || string.IsNullOrWhiteSpace(input.Recent_Data.Division)
             || string.IsNullOrWhiteSpace(input.Recent_Data.Factory)
             || string.IsNullOrWhiteSpace(input.Recent_Data.Employee_Id)
             || string.IsNullOrWhiteSpace(input.Recent_Data.Effective_Date_Str)
             || !DateTime.TryParseExact(input.Recent_Data.Effective_Date_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime effectiveDateRecentValue))
                return new OperationResult(false, "InvalidInput");
            predicate.And(x => x.Division == input.Temp_Data.Division && x.Factory == input.Temp_Data.Factory && x.Employee_ID == input.Temp_Data.Employee_Id && x.Effective_Date.Date == effectiveDateTempValue.Date);
            var oldData = await _repositoryAccessor.HRMS_Att_Work_Shift_Change.FirstOrDefaultAsync(predicate);
            if (oldData == null)
                return new OperationResult(false, "NotExitedData");
            HRMS_Att_Work_Shift_Change newData = new()
            {
                USER_GUID = input.Recent_Data.USER_GUID,
                Division = input.Recent_Data.Division,
                Factory = input.Recent_Data.Factory,
                Department = input.Recent_Data.Department,
                Employee_ID = input.Recent_Data.Employee_Id,
                Effective_Date = effectiveDateRecentValue,
                Effective_State = input.Recent_Data.Effective_State,
                Work_Shift_Type_Old = input.Recent_Data.Work_Shift_Type_Old,
                Work_Shift_Type_New = input.Recent_Data.Work_Shift_Type_New,
                Update_By = input.Recent_Data.Update_By,
                Update_Time = Convert.ToDateTime(input.Recent_Data.Update_Time_Str)
            };
            await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                _repositoryAccessor.HRMS_Att_Work_Shift_Change.Remove(oldData);
                await _repositoryAccessor.Save();
                _repositoryAccessor.HRMS_Att_Work_Shift_Change.Add(newData);
                await _repositoryAccessor.Save();
                await _repositoryAccessor.CommitAsync();
                return new OperationResult(true);
            }
            catch (Exception)
            {
                await _repositoryAccessor.RollbackAsync();
                return new OperationResult(false, "ErrorException");
            }
        }

        #endregion

        #region Delete
        public async Task<OperationResult> BatchDelete(List<ShiftManagementProgram_Main> data)
        {
            List<HRMS_Att_Work_Shift_Change> removeDatas = new();
            foreach (var item in data)
            {
                var predicateShift = PredicateBuilder.New<HRMS_Att_Work_Shift_Change>(true);
                predicateShift.And(x => x.Division == item.Division && x.Factory == item.Factory && x.Employee_ID == item.Employee_Id && x.Effective_Date.Date == Convert.ToDateTime(item.Effective_Date).Date && !x.Effective_State);
                HRMS_Att_Work_Shift_Change remove = _repositoryAccessor.HRMS_Att_Work_Shift_Change.FirstOrDefault(predicateShift);
                if (remove != null && !remove.Effective_State)
                    removeDatas.Add(remove);
            }
            await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                _repositoryAccessor.HRMS_Att_Work_Shift_Change.RemoveMultiple(removeDatas);
                await _repositoryAccessor.Save();
                await _repositoryAccessor.CommitAsync();
                return new OperationResult(true);
            }
            catch (Exception)
            {
                await _repositoryAccessor.RollbackAsync();
                return new OperationResult(false, "ErrorException");
            }
        }
        #endregion
    }
}