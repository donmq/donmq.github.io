using API.Data;
using API._Services.Interfaces.SalaryMaintenance;
using API.DTOs.SalaryMaintenance;
using API.Helper.Constant;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.SalaryMaintenance
{
    public class S_7_1_11_AdditionDeductionItemToAccountingCodeMappingMaintenance : BaseServices, I_7_1_11_AdditionDeductionItemToAccountingCodeMappingMaintenance
    {
        public S_7_1_11_AdditionDeductionItemToAccountingCodeMappingMaintenance(DBContext dbContext) : base(dbContext)
        {
        }

        #region Get Data Pagination
        public async Task<PaginationUtility<AdditionDeductionItemToAccountingCodeMappingMaintenanceDto>> GetDataPagination(PaginationParam pagination, AdditionDeductionItemToAccountingCodeMappingMaintenanceParam param)
        {
            var pred = PredicateBuilder.New<HRMS_Sal_AddDedItem_AccountCode>(true);

            if (!string.IsNullOrWhiteSpace(param.Factory))
                pred.And(x => x.Factory == param.Factory);

            var addDedItem = await GetListAdditionsAndDeductionsItem(param.Language);

            var data = await _repositoryAccessor.HRMS_Sal_AddDedItem_AccountCode.FindAll(pred, true).ToListAsync();

            var result = data
                .Select(x => new AdditionDeductionItemToAccountingCodeMappingMaintenanceDto
                {
                    Factory = x.Factory,
                    AddDed_Item = x.AddDed_Item,
                    AddDed_Item_Title = !string.IsNullOrWhiteSpace(x.AddDed_Item) ? addDedItem.FirstOrDefault(y => y.Key == x.AddDed_Item).Value : string.Empty,
                    Main_Acc = x.Main_Acc,
                    Sub_Acc = x.Sub_Acc,
                    Update_By = x.Update_By,
                    Update_Time = x.Update_Time
                }).ToList();

            return PaginationUtility<AdditionDeductionItemToAccountingCodeMappingMaintenanceDto>.Create(result, pagination.PageNumber, pagination.PageSize);
        }
        #endregion

        #region Create
        public async Task<OperationResult> Create(AdditionDeductionItemToAccountingCodeMappingMaintenanceDto dto, string userName)
        {
            if (await _repositoryAccessor.HRMS_Sal_AddDedItem_AccountCode.AnyAsync(
                x => x.Factory == dto.Factory
                  && x.AddDed_Item == dto.AddDed_Item))
                return new OperationResult(false, "Data already exists");

            HRMS_Sal_AddDedItem_AccountCode data = new()
            {
                Factory = dto.Factory,
                AddDed_Item = dto.AddDed_Item,
                Main_Acc = dto.Main_Acc?.Trim(),
                Sub_Acc = dto.Sub_Acc?.Trim(),
                Update_By = userName,
                Update_Time = DateTime.Now
            };

            try
            {
                _repositoryAccessor.HRMS_Sal_AddDedItem_AccountCode.Add(data);
                await _repositoryAccessor.Save();
                return new OperationResult(true);
            }
            catch (Exception)
            {
                return new OperationResult(false);
            }
        }
        #endregion

        #region Update
        public async Task<OperationResult> Update(AdditionDeductionItemToAccountingCodeMappingMaintenanceDto dto, string userName)
        {
            var data = await _repositoryAccessor.HRMS_Sal_AddDedItem_AccountCode.FirstOrDefaultAsync(
                x => x.Factory == dto.Factory
                  && x.AddDed_Item == dto.AddDed_Item);
            if (data is null)
                return new OperationResult(false, "Data not existed");

            data.Main_Acc = dto.Main_Acc?.Trim();
            data.Sub_Acc = dto.Sub_Acc?.Trim();
            data.Update_By = userName;
            data.Update_Time = DateTime.Now;

            try
            {
                _repositoryAccessor.HRMS_Sal_AddDedItem_AccountCode.Update(data);
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
        public async Task<OperationResult> Delete(AdditionDeductionItemToAccountingCodeMappingMaintenanceDto dto)
        {
            var data = await _repositoryAccessor.HRMS_Sal_AddDedItem_AccountCode.FirstOrDefaultAsync(
                x => x.Factory == dto.Factory
                  && x.AddDed_Item == dto.AddDed_Item);
            if (data is null)
                return new OperationResult(false, "Data not existed");

            try
            {
                _repositoryAccessor.HRMS_Sal_AddDedItem_AccountCode.Remove(data);
                await _repositoryAccessor.Save();
                return new OperationResult(true);
            }
            catch (Exception)
            {
                return new OperationResult(false);
            }
        }
        #endregion

        #region Get List
        public async Task<List<KeyValuePair<string, string>>> GetListFactoryByUser(string userName, string language)
        {
            var factoriesByAccount = await Queryt_Factory_AddList(userName);
            var factories = await GetDataBasicCode(BasicCodeTypeConstant.Factory, language);

            return factories.IntersectBy(factoriesByAccount, x => x.Key).ToList();
        }

        public async Task<List<KeyValuePair<string, string>>> GetListAdditionsAndDeductionsItem(string language)
        {
            return await GetDataBasicCode(BasicCodeTypeConstant.AdditionsAndDeductionsItem, language);
        }
        #endregion

    }
}