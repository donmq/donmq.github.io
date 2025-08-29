using API.Data;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using AgileObjects.AgileMapper;
using API._Services.Interfaces.OrganizationManagement;
using API.DTOs.OrganizationManagement;

namespace API._Services.Services.OrganizationManagement
{
    public class S_3_1_4_DirectDepartmentSetting : BaseServices, I_3_1_4_DirectDepartmentSetting
    {
        public S_3_1_4_DirectDepartmentSetting(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<OperationResult> AddNew(List<Org_Direct_DepartmentParamQuery> model, string userName)
        {
            var dataAdd = new List<HRMS_Org_Direct_Department>();
            foreach (var item in model)
            {
                if (await _repositoryAccessor.HRMS_Org_Direct_Department
                    .AnyAsync(x =>
                    x.Division.ToUpper() == item.Division.ToUpper() &&
                    x.Factory.ToUpper() == item.Factory.ToUpper() &&
                    x.Department_Code.ToUpper() == item.Department_Code.ToUpper() &&
                    x.Line_Code.ToUpper() == item.Line_Code.ToUpper() &&
                    x.Direct_Department_Attribute.ToUpper() == item.Direct_Department_Attribute.ToUpper()))
                    return new OperationResult(false, $"This data with work Line Code : {item.Line_Code},Section Code : {item.Direct_Department_Attribute},   is existed");
                item.Update_By = userName;
                item.Update_Time = DateTime.Now;
                dataAdd.Add(Mapper.Map(item).ToANew<HRMS_Org_Direct_Department>(x => x.MapEntityKeys()));
            }
            if (!dataAdd.Any())
                return new OperationResult(false, $"No data added");
            _repositoryAccessor.HRMS_Org_Direct_Department.AddMultiple(dataAdd);
            try
            {
                await _repositoryAccessor.Save();
                return new OperationResult(true, "Add Successfully");
            }
            catch (Exception e)
            {
                var a = e;
                return new OperationResult(false, "Add failed");
            }
        }

        public async Task<OperationResult> Edit(List<Org_Direct_DepartmentParamQuery> model, string userName)
        {
            var check = await _repositoryAccessor.HRMS_Org_Direct_Department.FindAll(
                x => x.Division.ToUpper() == model[0].Division.ToUpper() &&
                x.Factory.ToUpper() == model[0].Factory.ToUpper() &&
                x.Department_Code.ToUpper() == model[0].Department_Code.ToUpper()).ToListAsync();
            if (!check.Any())
                return new OperationResult(false, "Data does not exist");

            await _repositoryAccessor.BeginTransactionAsync();
            {
                try
                {
                    _repositoryAccessor.HRMS_Org_Direct_Department.RemoveMultiple(check);
                    await _repositoryAccessor.Save();

                    var dataAdd = new List<HRMS_Org_Direct_Department>();
                    foreach (var item in model)
                    {
                        item.Update_By = userName;
                        item.Update_Time = DateTime.Now;
                        dataAdd.Add(Mapper.Map(item).ToANew<HRMS_Org_Direct_Department>(x => x.MapEntityKeys()));
                    }
                    _repositoryAccessor.HRMS_Org_Direct_Department.AddMultiple(dataAdd);

                    await _repositoryAccessor.Save();
                    await _repositoryAccessor.CommitAsync();
                    return new OperationResult(true, "Update successful");
                }
                catch (Exception)
                {
                    await _repositoryAccessor.RollbackAsync();
                    return new OperationResult(false, "Update failed");
                }
            }

        }
        public async Task<List<Org_Direct_DepartmentResult>> GetdataByParam(Org_Direct_DepartmentParam param)
        {
            var predicate = PredicateBuilder.New<HRMS_Org_Direct_Department>(true);

            if (!string.IsNullOrEmpty(param.Division))
                predicate.And(x => x.Division.ToLower().Contains(param.Division.ToLower()));
            if (!string.IsNullOrEmpty(param.Department_Code))
                predicate.And(x => x.Department_Code.ToLower().Contains(param.Department_Code.ToLower()));
            if (!string.IsNullOrEmpty(param.Factory))
                predicate.And(x => x.Factory.ToLower().Contains(param.Factory.ToLower()));
            var Or_temp = _repositoryAccessor.HRMS_Org_Department.FindAll(true);
            return await _repositoryAccessor.HRMS_Org_Direct_Department.FindAll(predicate, true)
                    .GroupJoin(_repositoryAccessor.HRMS_Org_Department.FindAll(true),
                                x => new { x.Division, x.Factory, x.Department_Code },
                                y => new { y.Division, y.Factory, y.Department_Code },
                                (x, y) => new { Org_Direct = x, Org = y })
                    .SelectMany(x => x.Org.DefaultIfEmpty(),
                                (x, y) => new { x.Org_Direct, Org = y })
                    .GroupJoin(_repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "7", true),
                                x => new { Code = x.Org_Direct.Direct_Department_Attribute },
                                y => new { y.Code },
                                (x, y) => new { x.Org_Direct, x.Org, BasicCode = y })
                    .SelectMany(x => x.BasicCode.DefaultIfEmpty(), (x, y) => new { x.Org_Direct, x.Org, BasicCode = y })
                    .GroupJoin(_repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == param.Lang.ToLower(), true),
                                x => new { x.Org.Department_Code, x.Org.Division, x.Org.Factory },
                                y => new { y.Department_Code, y.Division, y.Factory },
                                (x, y) => new { x.Org_Direct, x.Org, x.BasicCode, OD_Language = y })
                    .SelectMany(x => x.OD_Language.DefaultIfEmpty(), (x, y) => new { x.Org_Direct, x.Org, x.BasicCode, OD_Language = y })
                    .Select(x => new Org_Direct_DepartmentResult()
                    {
                        Department_Code = x.Org_Direct.Department_Code,
                        Department_Name = x.OD_Language.Name ?? x.Org.Department_Name,
                        Division = x.Org_Direct.Division,
                        Direct_Department_Attribute = x.Org_Direct.Direct_Department_Attribute,
                        Factory = x.Org_Direct.Factory,
                        Direct_Department_Attribute_Name = x.BasicCode.Code_Name,
                        Line_Code = x.Org_Direct.Line_Code,
                        Line_Name = Or_temp.FirstOrDefault(z => 
                            z.Division == x.Org_Direct.Division && 
                            z.Factory == x.Org_Direct.Factory && 
                            z.Department_Code == x.Org_Direct.Line_Code).Department_Name,
                    }).ToListAsync();

        }
        public async Task<PaginationUtility<Org_Direct_DepartmentResult>> Getdata(PaginationParam pagination, Org_Direct_DepartmentParam param)
        {
            var data = await GetdataByParam(param);
            return PaginationUtility<Org_Direct_DepartmentResult>.Create(data, pagination.PageNumber, pagination.PageSize);
        }
        public async Task<OperationResult> DownloadExcel(Org_Direct_DepartmentParam param)
        {

            List<Org_Direct_DepartmentResult> dataList = await GetdataByParam(param);
            foreach (var item in dataList)
            {
                item.Direct_Department_Attribute = $"{item.Direct_Department_Attribute} - {item.Direct_Department_Attribute_Name}";
            }
            ExcelResult excelResult = ExcelUtility.DownloadExcel(
                dataList, 
                "Resources\\Template\\OrganizationManagement\\3_1_4_DirectDepartmentSetting\\Download.xlsx"
            );
            return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
        }
        public async Task<List<KeyValuePair<string, string>>> GetListDepartment(string Language)
        {
            // var Predicate = PredicateBuilder.New<HRMS_Org_Department>(true);
            // if (!string.IsNullOrEmpty(Division))
            //     Predicate.And(x => x.Division.ToLower().Contains(Division.ToLower()));
            // if (!string.IsNullOrEmpty(Factory))
            //     Predicate.And(x => x.Factory.ToLower().Contains(Factory.ToLower()));

            var department_Language = _repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == Language.ToLower(), true);

            // var data = await _repositoryAccessor.HRMS_Org_Department.FindAll(Predicate, true)
            //                     .GroupJoin(department_Language,
            //                          x => new { x.Department_Code, x.Division, x.Factory },
            //                         y => new { y.Department_Code, y.Division, y.Factory },
            //                         (x, y) => new { department = x, department_Language = y })
            //                         .SelectMany(x => x.department_Language.DefaultIfEmpty(),
            //                         (x, y) => new { department = x.department, department_Language = y })
            //                 .Select(x => new KeyValuePair<string, string>(
            //                     x.department.Department_Code,
            //                     x.department_Language != null ? x.department_Language.Name : x.department.Department_Name
            //                 ))
            //                 .Distinct()
            //                 .ToListAsync();

            // if (!data.Any())
                var data = await _repositoryAccessor.HRMS_Org_Department.FindAll(true)
                            .GroupJoin(department_Language,
                                     x => new { x.Department_Code, x.Division, x.Factory },
                                    y => new { y.Department_Code, y.Division, y.Factory },
                                    (x, y) => new { department = x, department_Language = y })
                                    .SelectMany(x => x.department_Language.DefaultIfEmpty(),
                                    (x, y) => new { department = x.department, department_Language = y })
                            .Select(x => new KeyValuePair<string, string>(
                                x.department.Department_Code,
                                x.department_Language != null ? x.department_Language.Name : x.department.Department_Name
                            ))
                            .Distinct()
                            .ToListAsync();
            return data;
        }

        public async Task<List<KeyValuePair<string, string>>> GetListDivision(string Language)
        {
            var BC_Language = _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Type_Seq == "1" && x.Language_Code.ToLower() == Language.ToLower(), true);
            var BC = _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "1", true);
            var result = await BC
                          .GroupJoin(BC_Language,
                                x => new { x.Type_Seq, x.Code },
                                y => new { y.Type_Seq, y.Code },
                              (x, y) => new { BC = x, BC_Language = y })
                              .SelectMany(x => x.BC_Language.DefaultIfEmpty(),
                              (x, y) => new { BC = x.BC, BC_Language = y })
                            .Select(x => new KeyValuePair<string, string>(
                                x.BC.Code,
                                (x.BC_Language != null ? x.BC_Language.Code_Name : x.BC.Code_Name)))
                            .Distinct()
                            .ToListAsync();

            return result;
        }

        public async Task<List<KeyValuePair<string, string>>> GetListFactory(string Division, string Language)
        {
            var Predicate = PredicateBuilder.New<HRMS_Basic_Factory_Comparison>(true);

            if (!string.IsNullOrEmpty(Division))
                Predicate.And(x => x.Division.ToLower().Contains(Division.ToLower()));

            var data = await _repositoryAccessor.HRMS_Basic_Factory_Comparison.FindAll(Predicate, true)
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == Language.ToLower(), true),
                    x => x.Factory,
                    y => y.Code,
                    (x, y) => new { Factory = x.Factory, CodeNameLanguage = y.Select(z => z.Code_Name).FirstOrDefault() })
                .Join(_repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "2", true),
                    x => x.Factory,
                    y => y.Code,
                    (x, y) => new { Factory = x.Factory, CodeNameLanguage = x.CodeNameLanguage, CodeName = y.Code_Name })
                .Select(x => new KeyValuePair<string, string>(x.Factory, x.CodeNameLanguage ?? x.CodeName))
                .Distinct().ToListAsync();

            if (!data.Any())
            {
                var allFactories = await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "2", true)
                    .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == Language.ToLower(), true),
                        x => new { x.Type_Seq, x.Code },
                        y => new { y.Type_Seq, y.Code },
                        (x, y) => new { basicCode = x, basicCodeLang = y })
                              .SelectMany(x => x.basicCodeLang.DefaultIfEmpty(),
                              (x, y) => new { basicCode = x.basicCode, basicCodeLang = y })
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
        public async Task<List<KeyValuePair<string, string>>> GetListLine(string Division, string Factory)
        {
            return await _repositoryAccessor.HRMS_Org_Department.FindAll(x => x.Division == Division && x.Factory == Factory, true)
                 .Select(x => new KeyValuePair<string, string>(x.Department_Code, x.Department_Name))
                 .Distinct().ToListAsync();
        }

        public async Task<List<KeyValuePair<string, string>>> GetListDirectDepartmentAttribute()
        {
            return await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "7" && x.Char1 == "Y", true)
            .Select(x => new KeyValuePair<string, string>(x.Code, x.Code_Name))
            .Distinct().ToListAsync();
        }
        public async Task<OperationResult> Delete(Org_Direct_DepartmentParamQuery model)
        {
            var check = await _repositoryAccessor.HRMS_Org_Direct_Department.FirstOrDefaultAsync(
                x => x.Division.ToUpper() == model.Division.ToUpper() &&
                x.Department_Code.ToUpper() == model.Department_Code.ToUpper() &&
                x.Line_Code.ToUpper() == model.Line_Code.ToUpper() &&
                x.Direct_Department_Attribute.ToUpper() == model.Direct_Department_Attribute.ToUpper());
            if (check == null)
                return new OperationResult(false, "Data not exist");
            _repositoryAccessor.HRMS_Org_Direct_Department.Remove(check);
            if (await _repositoryAccessor.Save())
                return new OperationResult(true, "Delete Successfully");
            return new OperationResult(false, "Delete failed");
        }

        public async Task<List<Org_Direct_DepartmentResult>> Getdetail(Org_Direct_DepartmentParam model)
        {
            var data_HRMS_Basic_Code = _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "7", true);
            var data_HRMS_Org_Department = _repositoryAccessor.HRMS_Org_Department.FindAll(x => x.Division == model.Division && x.Factory == model.Factory, true);
            var data_HRMS_Org_Direct_Department = _repositoryAccessor.HRMS_Org_Direct_Department.FindAll(x => x.Division == model.Division && x.Factory == model.Factory && x.Department_Code == model.Department_Code, true);
            var data = await data_HRMS_Org_Direct_Department.Join(data_HRMS_Basic_Code,
                    Ord => Ord.Direct_Department_Attribute,
                    BS => BS.Code,
                    (Ord, BS) => new { Ord, BS })
                    .Join(data_HRMS_Org_Department,
                    last => new { last.Ord.Division, last.Ord.Factory, Department_Code = last.Ord.Line_Code },
                    Or => new { Or.Division, Or.Factory, Department_Code = Or.Department_Code },
                    (last, Or) => new { last.Ord, last.BS, Or }).Select(x => new Org_Direct_DepartmentResult
                    {
                        Line_Code = x.Ord.Line_Code,
                        Line_Name = x.Or.Department_Name,
                        Direct_Department_Attribute = x.BS.Code,
                        Direct_Department_Attribute_Name = x.BS.Code_Name
                    }).ToListAsync();
            return data;
        }

        public async Task<OperationResult> CheckDuplicate(List<Org_Direct_DepartmentResult> model)
        {
            foreach (var item in model)
            {
                if (await _repositoryAccessor.HRMS_Org_Direct_Department
                    .AnyAsync(x =>
                    x.Division.ToUpper() == item.Division.ToUpper() &&
                    x.Factory.ToUpper() == item.Factory.ToUpper() &&
                    x.Department_Code.ToUpper() == item.Department_Code.ToUpper() &&
                    x.Line_Code.ToUpper() == item.Line_Code.ToUpper()))
                    return new OperationResult(false);
            }
            return new OperationResult(true);
        }
    }
}