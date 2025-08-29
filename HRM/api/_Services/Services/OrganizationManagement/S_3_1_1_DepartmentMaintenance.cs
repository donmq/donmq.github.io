using AgileObjects.AgileMapper;
using API.Data;
using API._Services.Interfaces.OrganizationManagement;
using API.DTOs.OrganizationManagement;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.OrganizationManagement
{
    public class S_3_1_1_DepartmentMaintenance : BaseServices, I_3_1_1_DepartmentMaintenance
    {
        public S_3_1_1_DepartmentMaintenance(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<OperationResult> Add(HRMS_Org_Department model)
        {
            if (await _repositoryAccessor.HRMS_Org_Department
                .AnyAsync(x =>
                    x.Division == model.Division.Trim() &&
                    x.Factory == model.Factory.Trim() &&
                    x.Department_Code == model.Department_Code.Trim()))
                return new OperationResult(false, "Department Code corresponding to Division and Factory already exist");
            var item = Mapper.Map(model).ToANew<HRMS_Org_Department>(x => x.MapEntityKeys());
            _repositoryAccessor.HRMS_Org_Department.Add(item);

            try
            {
                await _repositoryAccessor.Save();
                return new OperationResult { IsSuccess = true, Error = "IsSuccess" };
            }
            catch
            {
                return new OperationResult { IsSuccess = false, Error = "Error" };
            }
        }

        public async Task<OperationResult> Update(HRMS_Org_Department model)
        {
            var data = await _repositoryAccessor.HRMS_Org_Department
                .FirstOrDefaultAsync(x =>
                    x.Division == model.Division.Trim() &&
                    x.Factory == model.Factory.Trim() &&
                    x.Department_Code == model.Department_Code.Trim());
            if (data == null)
                return new OperationResult(false, "Department code corresponding to Division and Factory is incorrect");
            data = Mapper.Map(model).Over(data);
            _repositoryAccessor.HRMS_Org_Department.Update(data);
            var dataLang = await _repositoryAccessor.HRMS_Org_Department_Language
                .FirstOrDefaultAsync(x =>
                    x.Division == model.Division.Trim() &&
                    x.Factory == model.Factory.Trim() &&
                    x.Department_Code == model.Department_Code.Trim() && x.Language_Code == "TW");
            if (dataLang != null)
            {
                dataLang.Name = model.Department_Name;
                _repositoryAccessor.HRMS_Org_Department_Language.Update(dataLang);
            }
            try
            {
                await _repositoryAccessor.Save();
                return new OperationResult { IsSuccess = true, Error = "IsSuccess" };
            }
            catch (Exception ex)
            {
                return new OperationResult { IsSuccess = false, Error = $"{ex.Message}. Inner exception: {ex.InnerException?.Message ?? "No inner exception message available"}" };
            }
        }

        public async Task<List<HRMS_Org_DepartmentDto>> GetData(HRMS_Org_Department_Param param)
        {
            var pred = PredicateBuilder.New<HRMS_Org_Department>(true);
            if (param.Status != "All")
                pred = pred.And(x => x.IsActive == (param.Status == "Y"));
            if (!string.IsNullOrWhiteSpace(param.Division))
                pred = pred.And(x => x.Division.ToLower().Contains(param.Division.Trim().ToLower()));
            if (!string.IsNullOrWhiteSpace(param.Factory))
                pred = pred.And(x => x.Factory.ToLower().Contains(param.Factory.Trim().ToLower()));
            if (!string.IsNullOrWhiteSpace(param.Department_Code))
                pred = pred.And(x => x.Department_Code.ToLower().Contains(param.Department_Code.Trim().ToLower()));
            if (!string.IsNullOrWhiteSpace(param.Department_Name))
                pred = pred.And(x => x.Department_Name.ToLower().Contains(param.Department_Name.Trim().ToLower()));

            List<HRMS_Org_Department> departments = await _repositoryAccessor.HRMS_Org_Department.FindAll(pred, true).ToListAsync();
            List<HRMS_Org_Department> department = _repositoryAccessor.HRMS_Org_Department.FindAll(true).ToList();

            List<HRMS_Org_Department_Language> departmentlang = _repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == param.lang.ToLower()).ToList();
            List<HRMS_Basic_Code_Language> basiccodelang = _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == param.lang.ToLower()).ToList();

            return departments
                        .GroupJoin(_repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "8", true),
                            x => x.Org_Level,
                            y => y.Code,
                            (x, y) => new { dpm = x, basicCode = y })
                        .SelectMany(x => x.basicCode.DefaultIfEmpty(),
                            (x, y) => new { depart = x.dpm, basicCode = y })
                        .Select(x => new HRMS_Org_DepartmentDto
                        {
                            Division = x.depart.Division,
                            Factory = x.depart.Factory,
                            Center_Code = x.depart.Center_Code,
                            Org_Level = x.depart.Org_Level,
                            Org_Level_Name = basiccodelang.Any(y => y.Type_Seq == x.basicCode.Type_Seq && y.Code == x.basicCode.Code)
                                ? x.depart.Org_Level + "." + basiccodelang.FirstOrDefault(y => y.Type_Seq == x.basicCode.Type_Seq && y.Code == x.basicCode.Code).Code_Name
                                : x.depart.Org_Level + "." + x.basicCode.Code_Name,
                            Department_Code = x.depart.Department_Code,
                            Department_Name = x.depart.Department_Name,
                            Department_Name_Lang = departmentlang.Any(y => y.Division == x.depart.Division && y.Factory == x.depart.Factory && y.Department_Code == x.depart.Department_Code)
                                ? departmentlang.FirstOrDefault(y => y.Division == x.depart.Division && y.Factory == x.depart.Factory && y.Department_Code == x.depart.Department_Code).Name
                                : x.depart.Department_Name,
                            Upper_Department = x.depart.Upper_Department,
                            Attribute = x.depart.Attribute,
                            Virtual_Department = x.depart.Virtual_Department,
                            IsActive = x.depart.IsActive,
                            Supervisor_Employee_ID = x.depart.Supervisor_Employee_ID,
                            Supervisor_Type = x.depart.Supervisor_Type,
                            Approved_Headcount = x.depart.Approved_Headcount,
                            Cost_Center = x.depart.Cost_Center,
                            Effective_Date = x.depart.Effective_Date,
                            Expiration_Date = x.depart.Expiration_Date,
                            Upper_Department_Name = x.depart.Upper_Department + " - " +
                                (departmentlang.Any(y => y.Division == x.depart.Division && y.Factory == x.depart.Factory && y.Department_Code == x.depart.Upper_Department)
                                    ? departmentlang.FirstOrDefault(y => y.Division == x.depart.Division && y.Factory == x.depart.Factory && y.Department_Code == x.depart.Upper_Department).Name
                                    : department.FirstOrDefault(y => y.Division == x.depart.Division && y.Factory == x.depart.Factory && y.Department_Code == x.depart.Upper_Department)?.Department_Name),
                            Virtual_Department_Name = x.depart.Virtual_Department + " - " +
                                (departmentlang.Any(y => y.Division == x.depart.Division && y.Factory == x.depart.Factory && y.Department_Code == x.depart.Virtual_Department)
                                    ? departmentlang.FirstOrDefault(y => y.Division == x.depart.Division && y.Factory == x.depart.Factory && y.Department_Code == x.depart.Virtual_Department).Name
                                    : department.FirstOrDefault(y => y.Division == x.depart.Division && y.Factory == x.depart.Factory && y.Department_Code == x.depart.Virtual_Department)?.Department_Name),
                            status = param.lang == "tw" ? (x.depart.IsActive == true ? "Y.啟用" : "N.停用") : (x.depart.IsActive == true ? "Y.Enabled" : "N.Disabled")
                        })
                        .OrderBy(x => x.Division)
                        .ThenBy(x => x.Factory)
                        .ThenBy(x => x.Center_Code)
                        .ThenBy(x => x.Upper_Department)
                        .ThenBy(x => x.Org_Level)
                        .ThenBy(x => x.Department_Code)
                        .ToList();
        }
        public async Task<PaginationUtility<HRMS_Org_DepartmentDto>> GetDataPagination(PaginationParam pagination, HRMS_Org_Department_Param param)
        {
            var data = await GetData(param);
            return PaginationUtility<HRMS_Org_DepartmentDto>.Create(data, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<OperationResult> DownloadExcel(HRMS_Org_Department_Param param)
        {
            ExcelResult excelResult = ExcelUtility.DownloadExcel(
                await GetData(param),
                "Resources\\Template\\OrganizationManagement\\3_1_1_DepartmentMaintenance\\Download.xlsx"
            );
            return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
        }


        public async Task<List<KeyValuePair<string, string>>> GetListDepartment(string division, string factory, string lang)
        {
            var pred = PredicateBuilder.New<HRMS_Org_Department>(true);
            if (!string.IsNullOrWhiteSpace(division))
                pred.And(x => x.Division.ToLower().Contains(division.Trim().ToLower()));
            if (!string.IsNullOrWhiteSpace(factory))
                pred.And(x => x.Factory.ToLower().Contains(factory.Trim().ToLower()));

            var department_Language = _repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == lang.ToLower(), true);

            var data = await _repositoryAccessor.HRMS_Org_Department.FindAll(pred, true)
                                .GroupJoin(department_Language,
                                    x => new { x.Department_Code, x.Division, x.Factory },
                                    y => new { y.Department_Code, y.Division, y.Factory },
                                    (x, y) => new { department = x, department_Language = y })
                                    .SelectMany(x => x.department_Language.DefaultIfEmpty(),
                                    (x, y) => new { x.department, department_Language = y })
                            .Select(x => new KeyValuePair<string, string>(
                                x.department.Department_Code,
                                x.department_Language != null ? x.department_Language.Name : x.department.Department_Name
                            ))
                            .Distinct()
                            .ToListAsync();

            if (!data.Any())
                data = await _repositoryAccessor.HRMS_Org_Department.FindAll(true)
                            .GroupJoin(department_Language,
                                    x => new { x.Department_Code, x.Division, x.Factory },
                                    y => new { y.Department_Code, y.Division, y.Factory },
                                    (x, y) => new { department = x, department_Language = y })
                                    .SelectMany(x => x.department_Language.DefaultIfEmpty(),
                                    (x, y) => new { x.department, department_Language = y })
                            .Select(x => new KeyValuePair<string, string>(
                                x.department.Department_Code,
                                x.department_Language != null ? x.department_Language.Name : x.department.Department_Name
                            ))
                            .Distinct()
                            .ToListAsync();
            return data;
        }

        public async Task<List<KeyValuePair<string, string>>> GetListDivision(string lang)
        {
            var result = await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.IsActive == true && x.Type_Seq == "1", true)
                          .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == lang.ToLower(), true),
                                x => new { x.Type_Seq, x.Code },
                                y => new { y.Type_Seq, y.Code },
                              (x, y) => new { basicCode = x, basicCodeLang = y })
                              .SelectMany(x => x.basicCodeLang.DefaultIfEmpty(),
                              (x, y) => new { x.basicCode, basicCodeLang = y })
                            .Select(x => new KeyValuePair<string, string>(
                                x.basicCode.Code,
                                x.basicCodeLang != null ? x.basicCodeLang.Code_Name : x.basicCode.Code_Name))
                            .Distinct()
                            .ToListAsync();

            return result;
        }

        public async Task<List<KeyValuePair<string, string>>> GetListFactory(string division, string lang)
        {
            var pred = PredicateBuilder.New<HRMS_Basic_Factory_Comparison>(x => x.Kind == "1");
            if (!string.IsNullOrWhiteSpace(division))
                pred.And(x => x.Division.ToLower().Contains(division.Trim().ToLower()));

            var data = await _repositoryAccessor.HRMS_Basic_Factory_Comparison.FindAll(pred, true)
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == lang.ToLower(), true),
                    x => x.Factory,
                    y => y.Code,
                    (x, y) => new { x.Factory, CodeNameLanguage = y.Select(z => z.Code_Name).FirstOrDefault() })
                .Join(_repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "2", true),
                    x => x.Factory,
                    y => y.Code,
                    (x, y) => new { x.Factory, x.CodeNameLanguage, CodeName = y.Code_Name })
                .Select(x => new KeyValuePair<string, string>(x.Factory, x.CodeNameLanguage ?? x.CodeName))
                .Distinct().ToListAsync();

            if (!data.Any())
            {
                var allFactories = await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "2", true)
                    .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == lang.ToLower(), true),
                        x => new { x.Type_Seq, x.Code },
                        y => new { y.Type_Seq, y.Code },
                        (x, y) => new { basicCode = x, basicCodeLang = y })
                              .SelectMany(x => x.basicCodeLang.DefaultIfEmpty(),
                              (x, y) => new { x.basicCode, basicCodeLang = y })
                    .Select(x => new KeyValuePair<string, string>
                    (
                        x.basicCode.Code,
                        x.basicCodeLang != null ? x.basicCodeLang.Code_Name : x.basicCode.Code_Name)
                    )
                    .Distinct()
                    .ToListAsync();
                return allFactories;
            }

            return data;
        }

        public async Task<bool> CheckListDeptCode(string division, string factory, string deptCode)
        {
            var allDepartments = await _repositoryAccessor.HRMS_Org_Department.FindAll(true).ToListAsync();
            return allDepartments.Any(x => string.Compare(x.Division, division, StringComparison.Ordinal) == 0
                && string.Compare(x.Factory, factory, StringComparison.Ordinal) == 0
                && string.Compare(x.Department_Code, deptCode, StringComparison.Ordinal) == 0);
        }

        public async Task<List<KeyValuePair<string, string>>> GetListLevel(string lang)
        {
            var result = await _repositoryAccessor.HRMS_Basic_Code
                .FindAll(x => x.Type_Seq == "8", true)
                .GroupJoin(
                    _repositoryAccessor.HRMS_Basic_Code_Language
                        .FindAll(
                            x => x.Type_Seq == "8" && 
                            x.Language_Code.ToLower() == lang.ToLower() && 
                            !string.IsNullOrWhiteSpace(x.Code_Name), true),
                    x => x.Code,
                    y => y.Code,
                    (x, y) => new { codes = x, codeLangs = y }
                ).SelectMany(
                    x => x.codeLangs.DefaultIfEmpty(),
                    (x, y) => new { x.codes, codeLangs = y }
                ).Select(x => new KeyValuePair<string, string>
                (
                    x.codes.Code,
                    $"{(x.codeLangs != null ? x.codeLangs.Code_Name : x.codes.Code_Name)}"
                )).Distinct().ToListAsync();

            return result;
        }

        public async Task<List<ListUpperVirtual>> GetListUpperVirtual(string departmentCode, string division, string factory, string lang)
        {
            var result = await _repositoryAccessor.HRMS_Org_Department
                .FindAll(x => x.Division == division && x.Factory == factory && x.Department_Code != departmentCode, true)
                .GroupJoin(
                    _repositoryAccessor.HRMS_Org_Department_Language
                        .FindAll(
                            x => x.Division == division && 
                            x.Factory == factory && 
                            x.Department_Code != departmentCode && 
                            x.Language_Code.ToLower() == lang.ToLower() && 
                            !string.IsNullOrWhiteSpace(x.Name), true),
                    x => x.Department_Code,
                    y => y.Department_Code,
                    (x, y) => new { dpets = x, deptLangs = y }
                ).SelectMany(
                    x => x.deptLangs.DefaultIfEmpty(),
                    (x, y) => new { x.dpets, deptLangs = y }
                ).Select(x => new ListUpperVirtual()
                {
                    Department = x.dpets.Department_Code,
                    DepartmentName = $"{x.dpets.Department_Code} - {(x.deptLangs != null ? x.deptLangs.Name : x.dpets.Department_Name)}"
                }).Distinct().ToListAsync();

            return result;
        }
        public async Task<List<KeyValuePair<string, string>>> GetLanguage()
        {
            return await _repositoryAccessor.HRMS_SYS_Language.FindAll(x => x.IsActive == true).Select(x => new KeyValuePair<string, string>(x.Language_Code, x.Language_Name)).Distinct().ToListAsync();
        }

        public async Task<List<LanguageParam>> GetDetail(string departmentCode, string division, string factory)
        {
            var data = await _repositoryAccessor.HRMS_SYS_Language.FindAll(x => x.IsActive == true, true)
                            .GroupJoin(_repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Department_Code == departmentCode && x.Division == division && x.Factory == factory, true),
                            a => a.Language_Code,
                            b => b.Language_Code,
                            (a, b) => new { a, b })
                            .SelectMany(x => x.b.DefaultIfEmpty(),
                            (x, y) => new { x.a.Language_Code, y.Name })
                            .ToListAsync();
            if (!data.Any())
                return null;
            var detail = data.Select(x => new LanguageParam
            {

                Language_Code = x.Language_Code,
                Department_Name = x.Name
            }).ToList();
            return detail;
        }

        public async Task<OperationResult> AddLanguage(LanguageDeparment model)
        {
            List<HRMS_Org_Department> data = new();
            List<HRMS_Org_Department_Language> dataLanguages = new();
            foreach (var item in model.Detail)
            {
                if (!string.IsNullOrWhiteSpace(item.Department_Name))
                {
                    HRMS_Org_Department_Language add = new()
                    {
                        Division = model.Division,
                        Factory = model.Factory,
                        Department_Code = model.Department_Code,
                        Language_Code = item.Language_Code,
                        Name = item.Department_Name,
                        Update_By = model.userName,
                        Update_Time = DateTime.Now
                    };
                    dataLanguages.Add(add);
                }
            }
            _repositoryAccessor.HRMS_Org_Department.UpdateMultiple(data);
            _repositoryAccessor.HRMS_Org_Department_Language.AddMultiple(dataLanguages);
            try
            {
                await _repositoryAccessor.Save();
                return new OperationResult { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new OperationResult { IsSuccess = false, Error = $"An error occurred: {ex.Message}. Inner exception: {ex.InnerException?.Message ?? "No inner exception message available"}" };
            }

        }
        public async Task<OperationResult> UpdateLanguage(LanguageDeparment model)
        {
            var res = await _repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Department_Code == model.Department_Code
                                                                             && x.Division == model.Division && x.Factory == model.Factory).ToListAsync();
            _repositoryAccessor.HRMS_Org_Department_Language.RemoveMultiple(res);
            List<HRMS_Org_Department_Language> program_Languages = new();
            List<HRMS_Org_Department> program = new();
            foreach (var item in model.Detail)
            {
                if (item.Language_Code == "TW")
                {
                    var data = await _repositoryAccessor.HRMS_Org_Department.FirstOrDefaultAsync(x => x.Division == model.Division.Trim() && x.Factory == model.Factory.Trim()
                     && x.Department_Code == model.Department_Code.Trim());

                    data.Department_Name = item.Department_Name;
                    data.Update_By = model.userName;
                    data.Update_Time = DateTime.Now;

                    program.Add(data);
                }
                if (!string.IsNullOrWhiteSpace(item.Department_Name))
                {
                    HRMS_Org_Department_Language data = new()
                    {

                        Division = model.Division,
                        Factory = model.Factory,
                        Department_Code = model.Department_Code,
                        Language_Code = item.Language_Code,
                        Name = item.Department_Name,
                        Update_By = model.userName,
                        Update_Time = DateTime.Now,
                    };
                    program_Languages.Add(data);
                }
            }
            _repositoryAccessor.HRMS_Org_Department_Language.AddMultiple(program_Languages);
            _repositoryAccessor.HRMS_Org_Department.UpdateMultiple(program);
            try
            {
                await _repositoryAccessor.Save();
                return new OperationResult { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new OperationResult { IsSuccess = false, Error = $"An error occurred: {ex.Message}. Inner exception: {ex.InnerException?.Message ?? "No inner exception message available"}" };
            }
        }
    }
}