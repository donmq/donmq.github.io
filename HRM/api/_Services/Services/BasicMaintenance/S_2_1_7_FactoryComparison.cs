using AgileObjects.AgileMapper;
using API.Data;
using API._Services.Interfaces.BasicMaintenance;
using API.DTOs.BasicMaintenance;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.BasicMaintenance
{
    public class S_2_1_7_FactoryComparison : BaseServices, I_2_1_7_FactoryComparison
    {
        public S_2_1_7_FactoryComparison(DBContext dbContext) : base(dbContext)
        {
        }

        private async Task<List<HRMS_Basic_Factory_ComparisonDto>> QueryData(string kind)
        {
            var pred_Factory_Comparison = PredicateBuilder.New<HRMS_Basic_Factory_Comparison>(true);
            if (!string.IsNullOrWhiteSpace(kind))
                pred_Factory_Comparison.And(x => x.Kind.Trim() == kind.Trim());

            var factories = _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "2");
            var divisions = _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "1");
            var main = _repositoryAccessor.HRMS_Basic_Factory_Comparison.FindAll(pred_Factory_Comparison);

            var data = await main.GroupJoin(factories, x => x.Factory, y => y.Code, (x, y) => new { main = x, factory = y })
                                            .SelectMany(x => x.factory.DefaultIfEmpty(),
                                            (x, y) => new { x.main, factory = y })
                                 .GroupJoin(divisions, x => x.main.Division, y => y.Code, (x, y) => new { x.main, x.factory, division = y })
                                            .SelectMany(x => x.division.DefaultIfEmpty(),
                                            (x, y) => new { x.main, x.factory, division = y })
                                 .Select(x => new HRMS_Basic_Factory_ComparisonDto
                                 {
                                     Kind = x.main.Kind,
                                     Factory = x.main.Factory + "-" + x.factory.Code_Name,
                                     Division = x.main.Division + "-" + x.division.Code_Name,
                                     Modified_By = x.main.Modified_by,
                                     Modification_Date = x.main.Modification_Date
                                 }).ToListAsync();
            return data;
        }
        public async Task<PaginationUtility<HRMS_Basic_Factory_ComparisonDto>> GetData(PaginationParam pagination, string kind)
        {
            var data = await QueryData(kind);
            return PaginationUtility<HRMS_Basic_Factory_ComparisonDto>.Create(data, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<OperationResult> Create(List<HRMS_Basic_Factory_ComparisonDto> models, string currentUserUpdate)
        {
            var factory_Comparison = new List<HRMS_Basic_Factory_Comparison>();
            foreach (var model in models)
            {
                if (await _repositoryAccessor.HRMS_Basic_Factory_Comparison
                    .AnyAsync(x => x.Kind == model.Kind &&
                                   x.Factory == model.Factory &&
                                   x.Division == model.Division))
                    return new OperationResult(false, $" Factory: {model.Factory} , Division: {model.Division}  is existed");
                model.Modified_By = currentUserUpdate;
                model.Modification_Date = DateTime.Now;
                factory_Comparison.Add(Mapper.Map(model).ToANew<HRMS_Basic_Factory_Comparison>(x => x.MapEntityKeys()));
            }
            if (!factory_Comparison.Any())
                return new OperationResult(false, $"No data added");
            _repositoryAccessor.HRMS_Basic_Factory_Comparison.AddMultiple(factory_Comparison);
            await _repositoryAccessor.Save();
            return new OperationResult(true);
        }

        public async Task<List<KeyValuePair<string, string>>> GetFactories()
        {
            var factories = await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "2")
                 .Select(x => new KeyValuePair<string, string>(x.Code, $"{x.Code} - {x.Code_Name}")).ToListAsync();
            return factories;
        }

        public async Task<List<KeyValuePair<string, string>>> GetDivisions()
        {
            var divisions = await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "1")
                .Select(x => new KeyValuePair<string, string>(x.Code, $"{x.Code} - {x.Code_Name}")).ToListAsync();
            return divisions;
        }

        public async Task<OperationResult> Delete(HRMS_Basic_Factory_ComparisonDto model)
        {
            var isExist = await _repositoryAccessor.HRMS_Basic_Factory_Comparison
                .FirstOrDefaultAsync(x => x.Kind == model.Kind &&
                                          x.Factory == model.Factory &&
                                          x.Division == model.Division);
            if (isExist == null)
                return new OperationResult(false, "This data none existed");
            _repositoryAccessor.HRMS_Basic_Factory_Comparison.Remove(isExist);
            await _repositoryAccessor.Save();
            return new OperationResult(true);
        }
    }
}