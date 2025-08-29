using API.Data;
using API._Services.Interfaces.SystemMaintenance;
using API.DTOs.SystemMaintenance;
using API.Models;
using LinqKit;

namespace API._Services.Services.SystemMaintenance
{
    public class S_1_1_3_SystemLanguageSetting : BaseServices, I_1_1_3_SystemLanguageSetting
    {
        public S_1_1_3_SystemLanguageSetting(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<OperationResult> Create(SystemLanguageSetting_Data data)
        {
            var predicate = PredicateBuilder.New<HRMS_SYS_Language>(true);
            if (!string.IsNullOrEmpty(data.Language_Code))
                predicate.And(x => x.Language_Code.ToLower() == data.Language_Code.ToLower());
            if (!string.IsNullOrEmpty(data.Language_Name))
                predicate.And(x => x.Language_Name == data.Language_Name);
            if (await _repositoryAccessor.HRMS_SYS_Language.AnyAsync(predicate))
                return new OperationResult(false, "Add failed. Please check again value");
            var dataNew = new HRMS_SYS_Language
            {
                Language_Code = data.Language_Code.ToUpper(),
                Language_Name = data.Language_Name,
                IsActive = data.IsActive
            };
            _repositoryAccessor.HRMS_SYS_Language.Add(dataNew);
            if (await _repositoryAccessor.Save())
            {
                return new OperationResult(true);
            }
            return new OperationResult(false);
        }

        public async Task<PaginationUtility<HRMS_SYS_Language>> GetData(PaginationParam pagination)
        {
            var data = _repositoryAccessor.HRMS_SYS_Language.FindAll(true);
            return await PaginationUtility<HRMS_SYS_Language>.CreateAsync(data, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<OperationResult> Update(SystemLanguageSetting_Data data)
        {
            var item = await _repositoryAccessor.HRMS_SYS_Language.FirstOrDefaultAsync(x => x.Language_Code.ToLower() == data.Language_Code.ToLower());
            if (item == null)
                return new OperationResult(false);
            else
            {
                item.Language_Name = data.Language_Name;
                item.IsActive = data.IsActive;
                item.Update_By = data.Update_By;
                item.Update_Time = data.Update_Time;
                _repositoryAccessor.HRMS_SYS_Language.Update(item);
                await _repositoryAccessor.Save();
            }
            return new OperationResult(true);
        }
    }
}