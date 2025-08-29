using AgileObjects.AgileMapper;
using API.Data;
using API._Services.Interfaces.OrganizationManagement;
using API.DTOs.OrganizationManagement;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.OrganizationManagement
{
    public class S_3_1_2_WorkTypeHeadCountMaintenance : BaseServices, I_3_1_2_WorkTypeHeadCountMaintenance
    {
        public S_3_1_2_WorkTypeHeadCountMaintenance(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<KeyValuePair<string, string>>> GetDivisions(string language)
        {
            var divisions = await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "1", true)
                    .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                                    x => new { x.Type_Seq, x.Code },
                                    y => new { y.Type_Seq, y.Code },
                                    (x, y) => new { basicCode = x, basicCode_lang = y })
                                    .SelectMany(x => x.basicCode_lang.DefaultIfEmpty(),
                                    (x, y) => new { x = x.basicCode, basicCode_lang = y })
                .Select(x => new KeyValuePair<string, string>(
                    x.x.Code,
                    $"{x.x.Code} - {(x.basicCode_lang != null ? x.basicCode_lang.Code_Name : x.x.Code_Name)}")
                ).ToListAsync();
            return divisions;
        }

        public async Task<List<KeyValuePair<string, string>>> GetFactories(string language)
        {
            var factories = await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "2", true)
                        .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                                    x => new { x.Type_Seq, x.Code },
                                    y => new { y.Type_Seq, y.Code },
                                    (x, y) => new { x, y })
                                    .SelectMany(x => x.y.DefaultIfEmpty(),
                                    (x, y) => new { x.x, y })
                 .Select(x => new KeyValuePair<string, string>(x.x.Code, $"{x.x.Code} - {(x.y != null ? x.y.Code_Name : x.x.Code_Name)}")).ToListAsync();
            return factories;
        }

        public async Task<List<KeyValuePair<string, string>>> GetFactories(string division, string language)
        {
            var factories = await _repositoryAccessor.HRMS_Basic_Factory_Comparison.FindAll(x => x.Division == division.Trim(), true)
                            .Join(_repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "2", true),
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

        public async Task<List<KeyValuePair<string, string>>> GetDepartments(string language)
        {
            var departments = await _repositoryAccessor.HRMS_Org_Work_Type_Headcount.FindAll(true)
                             .Join(_repositoryAccessor.HRMS_Org_Department.FindAll(true),
                                 x => new { x.Division, x.Factory, x.Department_Code },
                                 y => new { y.Division, y.Factory, y.Department_Code },
                                 (x, y) => new { typeHead = x, department = y })
                             .GroupJoin(_repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                                x => new { x.department.Division, x.department.Factory, x.department.Department_Code },
                                y => new { y.Division, y.Factory, y.Department_Code },
                                (x, y) => new { x.typeHead, x.department, department_Language = y })
                                .SelectMany(x => x.department_Language.DefaultIfEmpty(),
                                 (x, y) => new { x.typeHead, x.department, department_Language = y })
                            .Select(x => new KeyValuePair<string, string>(
                                x.typeHead.Department_Code,
                                x.typeHead.Department_Code + "-" + (x.department_Language == null ? x.department.Department_Name : x.department_Language.Name))
                            )
                            .Distinct()
                            .ToListAsync();
            return departments;
        }

        public async Task<List<KeyValuePair<string, string>>> GetDepartments(string division, string factory, string language)
        {
            var predicate = PredicateBuilder.New<HRMS_Org_Work_Type_Headcount>(true);
            if (!string.IsNullOrEmpty(division) && !string.IsNullOrEmpty(factory))
                predicate.And(x => x.Division == division && x.Factory == factory.Trim());

            return await _repositoryAccessor.HRMS_Org_Work_Type_Headcount.FindAll(predicate, true)
                            .GroupJoin(_repositoryAccessor.HRMS_Org_Department.FindAll(true),
                                x => new { x.Division, x.Factory, x.Department_Code },
                                y => new { y.Division, y.Factory, y.Department_Code },
                                (x, y) => new { typehead = x, y })
                                .SelectMany(x => x.y.DefaultIfEmpty(),
                                (x, y) => new { x.typehead, department = y })
                            .GroupJoin(_repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                                x => new { x.department.Division, x.department.Factory, x.department.Department_Code },
                                y => new { y.Division, y.Factory, y.Department_Code },
                                (x, y) => new { x.typehead, x.department, department_Language = y })
                                .SelectMany(x => x.department_Language.DefaultIfEmpty(),
                                (x, y) => new { typeHead = x.typehead, x.department, department_Language = y })
                            .Select(x => new KeyValuePair<string, string>(
                                x.typeHead.Department_Code,
                                x.typeHead.Department_Code + "-" + (x.department_Language != null ? x.department_Language.Name : x.department.Department_Name))
                            )
                            .Distinct()
                            .ToListAsync();
        }

        /// <summary>
        /// Lấy DepartmentName từ DepartmentCode
        /// Có tồn tại trong 3.1 Deparment Maintaince
        /// </summary>
        /// <param name="departmentCode"></param>
        /// <returns></returns>
        public async Task<DepartmentNameObject> GetDepartmentNameFromDepartmentCode(HRMS_Org_Work_Type_HeadcountParam param)
        {
            return await _repositoryAccessor.HRMS_Org_Department
                                    .FindAll(x => x.Division == param.Division &&
                                        x.Factory == param.Factory &&
                                        x.Department_Code == param.Department_Code.Trim()
                                    , true)
                                    .GroupJoin(_repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == param.Language.ToLower(), true),
                                        x => new { x.Division, x.Factory, x.Department_Code },
                                        y => new { y.Division, y.Factory, y.Department_Code },
                                        (x, y) => new { department = x, department_Language = y })
                                    .SelectMany(x => x.department_Language.DefaultIfEmpty(),
                                        (x, y) => new { x.department, department_Language = y })

                                    .Select(x => new DepartmentNameObject()
                                    {
                                        Department_Code = x.department.Department_Code,
                                        Department_Name = x.department_Language != null ? x.department_Language.Name : x.department.Department_Name
                                    }).FirstOrDefaultAsync();
        }

        public async Task<List<KeyValuePair<string, string>>> GetWorkCodeNames()
        {
            var factories = await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "5", true)
                .Select(x => new KeyValuePair<string, string>(x.Code, x.Code_Name)).ToListAsync();
            return factories;
        }


        public async Task<HRMS_Org_Work_Type_HeadcountDataMain> GetDataPagination(PaginationParam param, HRMS_Org_Work_Type_HeadcountParam filter)
        {
            var data = await GetData(filter);
            var result = new HRMS_Org_Work_Type_HeadcountDataMain()
            {
                TotalApprovedHeadcount = data.Sum(x => x.Approved_Headcount),
                TotalActual = data.Sum(x => x.Actual_Headcount),
                DataPagination = PaginationUtility<HRMS_Org_Work_Type_HeadcountDto>.Create(data, param.PageNumber, param.PageSize)
            };
            return result;
        }
        public async Task<OperationResult> DownloadExcel(HRMS_Org_Work_Type_HeadcountParam param)
        {
            var data = await GetData(param);
            ExcelResult excelResult = ExcelUtility.DownloadExcel(
                data, 
                "Resources\\Template\\OrganizationManagement\\3_1_2_WorkTypeHeadCountMaintenance\\Download.xlsx"
            );
            return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
        }
        private async Task<List<HRMS_Org_Work_Type_HeadcountDto>> GetData(HRMS_Org_Work_Type_HeadcountParam filter)
        {
            var predicate = PredicateBuilder.New<HRMS_Org_Work_Type_Headcount>(true);
            if (!string.IsNullOrEmpty(filter.Division))
                predicate.And(x => x.Division.Contains(filter.Division));
            if (!string.IsNullOrEmpty(filter.Factory))
                predicate.And(x => x.Factory.Contains(filter.Factory));
            if (!string.IsNullOrEmpty(filter.Department_Code))
                predicate.And(x => x.Department_Code.Contains(filter.Department_Code));
            if (!string.IsNullOrEmpty(filter.Effective_Date))
                predicate.And(x => x.Effective_Date == filter.Effective_Date);

            var workTypeNameQuery = _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "5", true);
            var department_Language = _repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == filter.Language.ToLower(), true);
            var basiccode_Language = _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == filter.Language.ToLower(), true);

            var query = _repositoryAccessor.HRMS_Org_Work_Type_Headcount.FindAll(predicate, true)
                                .GroupJoin(workTypeNameQuery,
                                        x => new { Code = x.Work_Type_Code },
                                        y => new { y.Code },
                                    (x, y) => new { wordType = x, workTypeName = y })
                                    .SelectMany(x => x.workTypeName.DefaultIfEmpty(),
                                    (x, y) => new { x.wordType, workTypeName = y })
                                // Join vs Department
                                .GroupJoin(_repositoryAccessor.HRMS_Org_Department.FindAll(true),
                                         x => new { x.wordType.Division, x.wordType.Factory, x.wordType.Department_Code },
                                        y => new { y.Division, y.Factory, y.Department_Code },
                                    (x, y) => new { x.wordType, x.workTypeName, department = y })
                                    .SelectMany(x => x.department.DefaultIfEmpty(),
                                    (x, y) => new { x.wordType, x.workTypeName, department = y })
                                .GroupJoin(department_Language,
                                        x => new { x.department.Division, x.department.Factory, x.department.Department_Code },
                                        y => new { y.Division, y.Factory, y.Department_Code },
                                    (x, y) => new { x.wordType, x.workTypeName, x.department, department_Language = y })
                                    .SelectMany(x => x.department_Language.DefaultIfEmpty(),
                                    (x, y) => new { x.wordType, x.workTypeName, x.department, department_Language = y })
                                .GroupJoin(basiccode_Language,
                                        x => new { x.workTypeName.Type_Seq, x.workTypeName.Code },
                                        y => new { y.Type_Seq, y.Code },
                                    (x, y) => new { x.wordType, x.workTypeName, x.department, x.department_Language, basiccode_Language = y })
                                    .SelectMany(x => x.basiccode_Language.DefaultIfEmpty(),
                                    (x, y) => new { x.wordType, x.workTypeName, x.department, x.department_Language, basiccode_Language = y });
            var result = await query.Select(x => new HRMS_Org_Work_Type_HeadcountDto()
            {
                Division = x.wordType.Division,
                Factory = x.wordType.Factory,
                Department_Code = x.wordType.Department_Code,
                Department_Name = x.department_Language != null ? x.department_Language.Name : x.department.Department_Name,
                Effective_Date = x.wordType.Effective_Date,
                Work_Type_Code = x.wordType.Work_Type_Code,
                Work_Type_Name = x.basiccode_Language != null ? x.basiccode_Language.Code_Name : x.workTypeName.Code_Name,
                Approved_Headcount = x.wordType.Approved_Headcount,
                Actual_Headcount = 0, // tạm thời để trống chờ confirm 
            }).ToListAsync();
            return result;
        }
        public async Task<OperationResult> Create(List<HRMS_Org_Work_Type_HeadcountDto> models, string currentUserUpdate)
        {
            var workTypeHeadcount = new List<HRMS_Org_Work_Type_Headcount>();
            foreach (var model in models)
            {
                if (await _repositoryAccessor.HRMS_Org_Work_Type_Headcount
                    .AnyAsync(x =>
                        x.Division == model.Division &&
                        x.Factory == model.Factory &&
                        x.Department_Code == model.Department_Code &&
                        x.Effective_Date == model.Effective_Date &&
                        x.Work_Type_Code == model.Work_Type_Code))
                    return new OperationResult(false, $"This data with work type code : {model.Work_Type_Code} is existed");
                model.Update_By = currentUserUpdate;
                model.Update_Time = DateTime.Now;
                workTypeHeadcount.Add(Mapper.Map(model).ToANew<HRMS_Org_Work_Type_Headcount>(x => x.MapEntityKeys()));
            }
            if (!workTypeHeadcount.Any())
                return new OperationResult(false, $"No data added");
            _repositoryAccessor.HRMS_Org_Work_Type_Headcount.AddMultiple(workTypeHeadcount);
            await _repositoryAccessor.Save();
            return new OperationResult(true);
        }


        public async Task<List<HRMS_Org_Work_Type_HeadcountDto>> GetListUpdate(HRMS_Org_Work_Type_HeadcountParam filter)
        {
            var query = await _repositoryAccessor.HRMS_Org_Work_Type_Headcount.FindAll(x =>
                                x.Division == filter.Division &&
                                x.Factory == filter.Factory &&
                                x.Department_Code == filter.Department_Code &&
                                x.Effective_Date == filter.Effective_Date
                , true).Project().To<HRMS_Org_Work_Type_HeadcountDto>().ToListAsync();
            return query;
        }


        public async Task<OperationResult> Update(HRMS_Org_Work_Type_HeadcountUpdate model, string currentUserUpdate)
        {
            var updateData = new List<HRMS_Org_Work_Type_Headcount>();
            var addData = new List<HRMS_Org_Work_Type_Headcount>();

            // Kiểm tra dữ liệu Cập nhật 
            foreach (var iUpdate in model.DataUpdate)
            {
                var isExist = await _repositoryAccessor.HRMS_Org_Work_Type_Headcount.FirstOrDefaultAsync(x =>
                                x.Division == iUpdate.Division &&
                                x.Factory == iUpdate.Factory &&
                                x.Department_Code == iUpdate.Department_Code &&
                                x.Effective_Date == iUpdate.Effective_Date &&
                                x.Work_Type_Code == iUpdate.Work_Type_Code
                                );
                if (isExist == null)
                    return new OperationResult(false, $"This data with work type code: {iUpdate.Work_Type_Code} none existed");
                iUpdate.Update_By = currentUserUpdate;
                iUpdate.Update_Time = DateTime.Now;

                isExist = Mapper.Map(iUpdate).Over(isExist);
                updateData.Add(isExist);
            }

            // Kiểm tra dữ liệu thêm mới
            foreach (var iAdd in model.DataNewAdd)
            {
                if (await _repositoryAccessor.HRMS_Org_Work_Type_Headcount
                    .AnyAsync(x =>
                        x.Division == iAdd.Division &&
                        x.Factory == iAdd.Factory &&
                        x.Department_Code == iAdd.Department_Code &&
                        x.Effective_Date == iAdd.Effective_Date &&
                        x.Work_Type_Code == iAdd.Work_Type_Code))
                    return new OperationResult(false, $"This data with work type code : {iAdd.Work_Type_Code} is existed");
                iAdd.Update_By = currentUserUpdate;
                iAdd.Update_Time = DateTime.Now;
                addData.Add(Mapper.Map(iAdd).ToANew<HRMS_Org_Work_Type_Headcount>(x => x.MapEntityKeys()));
            }
            if (addData.Any())
                _repositoryAccessor.HRMS_Org_Work_Type_Headcount.AddMultiple(addData);
            if (updateData.Any())
                _repositoryAccessor.HRMS_Org_Work_Type_Headcount.UpdateMultiple(updateData);
            await _repositoryAccessor.Save();
            return new OperationResult(true);
        }

        public async Task<OperationResult> Delete(HRMS_Org_Work_Type_HeadcountDto model)
        {
            var isExist = await _repositoryAccessor.HRMS_Org_Work_Type_Headcount
                .FirstOrDefaultAsync(x =>
                    x.Division == model.Division &&
                    x.Factory == model.Factory &&
                    x.Department_Code == model.Department_Code &&
                    x.Effective_Date == model.Effective_Date &&
                    x.Work_Type_Code == model.Work_Type_Code);
            if (isExist == null)
                return new OperationResult(false, "This data none existed");
            _repositoryAccessor.HRMS_Org_Work_Type_Headcount.Remove(isExist);
            await _repositoryAccessor.Save();
            return new OperationResult(true);
        }
    }
}