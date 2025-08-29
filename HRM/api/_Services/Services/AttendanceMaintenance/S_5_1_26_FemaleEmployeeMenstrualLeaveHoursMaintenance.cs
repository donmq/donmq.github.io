
using System.Globalization;
using API.Data;
using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using API.Helper.Constant;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.AttendanceMaintenance
{
    public class S_5_1_26_FemaleEmployeeMenstrualLeaveHoursMaintenance : BaseServices, I_5_1_26_FemaleEmployeeMenstrualLeaveHoursMaintenance
    {
        public S_5_1_26_FemaleEmployeeMenstrualLeaveHoursMaintenance(DBContext dbContext) : base(dbContext)
        {
        }

        #region Add
        public async Task<OperationResult> Add(FemaleEmpMenstrualMain data, string userName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(data.Factory)
                   || string.IsNullOrWhiteSpace(data.Employee_ID)
                   || string.IsNullOrWhiteSpace(data.Time_Start)
                   || string.IsNullOrWhiteSpace(data.Att_Month_Str)
                   || !DateTime.TryParseExact(data.Att_Month_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime Att_Month)
                   || string.IsNullOrWhiteSpace(data.Breaks_Date_Str)
                   || !DateTime.TryParseExact(data.Breaks_Date_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime Breaks_Date))
                    return new OperationResult(false, "Invalid inputs");
                if (await _repositoryAccessor.HRMS_Att_Female_Menstrual_Hours.AnyAsync(
                    x => x.Att_Month.Date == Att_Month.Date &&
                    x.Factory == data.Factory &&
                    x.Employee_ID == data.Employee_ID &&
                    x.Breaks_Date.Date == Breaks_Date.Date &&
                    x.Time_Start == data.Time_Start))
                    return new OperationResult(false, "Data already existed");
                var model = new HRMS_Att_Female_Menstrual_Hours()
                {
                    Factory = data.Factory,
                    Att_Month = Att_Month.Date,
                    Department = data.Department_Code,
                    Employee_ID = data.Employee_ID,
                    USER_GUID = data.USER_GUID,
                    Breaks_Date = Breaks_Date.Date,
                    Breaks_Hours = data.Breaks_Hours,
                    Time_Start = data.Time_Start,
                    Time_End = data.Time_End,
                    Update_By = userName,
                    Update_Time = DateTime.Now
                };
                _repositoryAccessor.HRMS_Att_Female_Menstrual_Hours.Add(model);
                await _repositoryAccessor.Save();
                return new OperationResult { IsSuccess = true };
            }
            catch (Exception)
            {
                return new OperationResult { IsSuccess = false };
            }
        }
        #endregion

        #region Edit
        public async Task<OperationResult> Edit(FemaleEmpMenstrualMain data, string userName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(data.Factory)
                   || string.IsNullOrWhiteSpace(data.Employee_ID)
                   || string.IsNullOrWhiteSpace(data.Time_Start)
                   || string.IsNullOrWhiteSpace(data.Att_Month_Str)
                   || !DateTime.TryParseExact(data.Att_Month_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime Att_Month)
                   || string.IsNullOrWhiteSpace(data.Breaks_Date_Str)
                   || !DateTime.TryParseExact(data.Breaks_Date_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime Breaks_Date))
                    return new OperationResult(false, "Invalid inputs");
                var oldData = await _repositoryAccessor.HRMS_Att_Female_Menstrual_Hours.FirstOrDefaultAsync(
                    x => x.Att_Month.Date == Att_Month.Date &&
                    x.Factory == data.Factory &&
                    x.Employee_ID == data.Employee_ID &&
                    x.Breaks_Date.Date == Breaks_Date.Date &&
                    x.Time_Start == data.Time_Start);
                if (oldData is null)
                    return new OperationResult(false, "The selected data does not exist");
                oldData.Breaks_Hours = data.Breaks_Hours;
                oldData.Time_End = data.Time_End;
                oldData.Update_By = userName;
                oldData.Update_Time = DateTime.Now;

                _repositoryAccessor.HRMS_Att_Female_Menstrual_Hours.Update(oldData);
                await _repositoryAccessor.Save();
                return new OperationResult(true);
            }
            catch (Exception)
            {
                return new OperationResult(false);
            }
        }
        #endregion

        #region Delete
        public async Task<OperationResult> Delete(FemaleEmpMenstrualMain data)
        {
            var result = await _repositoryAccessor.HRMS_Att_Female_Menstrual_Hours
                .FirstOrDefaultAsync(
                    x => x.Att_Month.Date == Convert.ToDateTime(data.Att_Month).Date &&
                    x.Factory == data.Factory &&
                    x.Employee_ID == data.Employee_ID &&
                    x.Breaks_Date.Date == Convert.ToDateTime(data.Breaks_Date).Date &&
                    x.Time_Start == data.Time_Start);

            if (result is null)
                return new OperationResult(false, "Data not existed");

            try
            {
                _repositoryAccessor.HRMS_Att_Female_Menstrual_Hours.Remove(result);
                await _repositoryAccessor.Save();
                return new OperationResult(true, "Delete data successfully");
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.ToString());
            }
        }
        #endregion

        #region GetListFactory
        public async Task<List<KeyValuePair<string, string>>> GetListFactory(string language)
        {
            var predicate = PredicateBuilder.New<HRMS_Basic_Code>(x => x.Type_Seq == BasicCodeTypeConstant.Factory);
            var data = await GetBasicCodes(language, predicate);

            return data;
        }
        #endregion

        #region GetListFactoryAdd
        public async Task<List<KeyValuePair<string, string>>> GetListFactoryAdd(string userName, string language)
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

        #region GetListDepartment
        public async Task<List<KeyValuePair<string, string>>> GetListDepartment(string factory, string language)
        {
            ExpressionStarter<HRMS_Org_Department> predDept = PredicateBuilder.New<HRMS_Org_Department>(x => x.Factory == factory);
            ExpressionStarter<HRMS_Basic_Factory_Comparison> predCom = PredicateBuilder.New<HRMS_Basic_Factory_Comparison>(x => x.Factory == factory && x.Kind == "1");
            var data = await QueryDepartment(predDept, predCom, language)
                .Select(
                    x => new KeyValuePair<string, string>(
                        x.Department.Department_Code,
                        $"{x.Department.Department_Code} - {(x.Language != null ? x.Language.Name : x.Department.Department_Name)}"
                    )
                ).ToListAsync();

            return data;
        }
        #endregion

        #region GetDataPagination
        public async Task<PaginationUtility<FemaleEmpMenstrualMain>> GetDataPagination(PaginationParam pagination, FemaleEmpMenstrualParam param, bool isPaging = true)
        {
            var data = await GetData(param);
            return PaginationUtility<FemaleEmpMenstrualMain>.Create(data, pagination.PageNumber, pagination.PageSize, isPaging);
        }
        #endregion

        #region Excel
        public async Task<OperationResult> DownloadExcel(FemaleEmpMenstrualParam param, string userName)
        {
            var data = await GetData(param);
            List<Table> tables = new()
            {
                new Table("result", data)
            };
            List<Cell> cells = new()
            {
                new Cell("B1", param.Factory),
                new Cell("E1", userName),
                new Cell("H1", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")),
            };
            ExcelResult excelResult = ExcelUtility.DownloadExcel(
                tables, cells,
                "Resources\\Template\\AttendanceMaintenance\\5_1_26_FemaleEmployeeMenstrualLeaveHoursMaintenance\\Download.xlsx"
            );
            return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
        }
        #endregion

        #region Private functions
        private async Task<List<FemaleEmpMenstrualMain>> GetData(FemaleEmpMenstrualParam param)
        {
            var predFemaleMenstrual = PredicateBuilder.New<HRMS_Att_Female_Menstrual_Hours>(true);
            if (!string.IsNullOrWhiteSpace(param.Factory))
                predFemaleMenstrual.And(x => x.Factory == param.Factory);
            if (!string.IsNullOrWhiteSpace(param.Employee_ID))
                predFemaleMenstrual.And(x => x.Employee_ID.Contains(param.Employee_ID));
            if (!string.IsNullOrWhiteSpace(param.Att_Month_Str))
                predFemaleMenstrual.And(x => x.Att_Month.Date >= Convert.ToDateTime(param.Att_Month_Str).Date);

            var HAFMH = _repositoryAccessor.HRMS_Att_Female_Menstrual_Hours.FindAll(predFemaleMenstrual);

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

            var permissionGroupQuery = _repositoryAccessor.HRMS_Basic_Role
                .FindAll(x => x.Factory == param.Factory, true)
                .Select(x => x.Permission_Group).ToList();
            var HEP = _repositoryAccessor.HRMS_Emp_Personal
                .FindAll(x => (x.Factory == param.Factory) && permissionGroupQuery.Contains(x.Permission_Group))
                .Select(x => new
                {
                    x.USER_GUID,
                    x.Local_Full_Name,
                    x.Onboard_Date,
                    Division = x.Employment_Status == "A" || x.Employment_Status == "S" ? x.Assigned_Division : x.Division,
                    Factory = x.Employment_Status == "A" || x.Employment_Status == "S" ? x.Assigned_Factory : x.Factory,
                    Department = x.Employment_Status == "A" || x.Employment_Status == "S" ? x.Assigned_Department : x.Department,
                });

            var result = await HAFMH
                .Join(HEP,
                    x => x.USER_GUID,
                    y => y.USER_GUID,
                    (x, y) => new { HAFMH = x, HEP = y })
                .GroupJoin(HOD_Lang,
                    x => new { x.HEP.Factory, x.HEP.Division, Department_Code = x.HEP.Department },
                    y => new { y.Factory, y.Division, y.Department_Code },
                    (x, y) => new { x.HAFMH, x.HEP, HOD_Lang = y })
                .SelectMany(x => x.HOD_Lang.DefaultIfEmpty(),
                    (x, y) => new { x.HAFMH, x.HEP, HOD_Lang = y })
                .Select(x => new FemaleEmpMenstrualMain()
                {
                    USER_GUID = x.HAFMH.USER_GUID,
                    Factory = x.HAFMH.Factory,
                    Att_Month = x.HAFMH.Att_Month,
                    Att_Month_Str = x.HAFMH.Att_Month.ToString("yyyy/MM/dd"),
                    Employee_ID = x.HAFMH.Employee_ID,
                    Local_Full_Name = x.HEP.Local_Full_Name,
                    Department_Code = x.HEP.Department,
                    Department_Name = x.HOD_Lang.Department_Name,
                    Department_Code_Name = x.HOD_Lang != null && !string.IsNullOrWhiteSpace(x.HOD_Lang.Department_Name)
                        ? x.HOD_Lang.Department_Code + "-" + x.HOD_Lang.Department_Name : x.HEP.Department,
                    Onboard_Date = x.HEP.Onboard_Date,
                    Onboard_Date_Str = x.HEP.Onboard_Date.ToString("yyyy/MM/dd"),
                    Breaks_Date = x.HAFMH.Breaks_Date,
                    Breaks_Date_Str = x.HAFMH.Breaks_Date.ToString("yyyy/MM/dd"),
                    Breaks_Hours = x.HAFMH.Breaks_Hours,
                    Time_Start = x.HAFMH.Time_Start,
                    Time_End = x.HAFMH.Time_End,
                    Update_By = x.HAFMH.Update_By,
                    Update_Time = x.HAFMH.Update_Time,
                    Update_Time_Str = x.HAFMH.Update_Time.ToString("yyyy/MM/dd HH:mm:ss")
                }).ToListAsync();
            if (!string.IsNullOrWhiteSpace(param.Department))
                result = result.FindAll(x => x.Department_Code == param.Department);
            return result;
        }

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

        private IOrderedQueryable<DepartmentJoinResult> QueryDepartment(ExpressionStarter<HRMS_Org_Department> predDept, ExpressionStarter<HRMS_Basic_Factory_Comparison> predCom, string language)
        {
            var data = _repositoryAccessor.HRMS_Org_Department.FindAll(predDept, true)
                .Join(_repositoryAccessor.HRMS_Basic_Factory_Comparison.FindAll(predCom, true),
                    department => department.Division,
                    factoryComparison => factoryComparison.Division,
                    (department, factoryComparison) => department)
                .GroupJoin(_repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                    department => new { department.Factory, department.Department_Code },
                    language => new { language.Factory, language.Department_Code },
                    (department, language) => new { Department = department, Language = language })
                .SelectMany(
                    x => x.Language.DefaultIfEmpty(),
                    (x, language) => new DepartmentJoinResult { Department = x.Department, Language = language })
                .OrderBy(x => x.Department.Department_Code);

            return data;
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
        #endregion
    }
}