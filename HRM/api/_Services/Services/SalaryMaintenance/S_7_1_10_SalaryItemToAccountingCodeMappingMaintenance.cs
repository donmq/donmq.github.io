using API.Data;
using API._Services.Interfaces;
using API._Services.Interfaces.SalaryMaintenance;
using API.DTOs.SalaryMaintenance;
using API.Helper.Constant;
using API.Models;

namespace API._Services.Services.SalaryMaintenance
{
    public class S_7_1_10_SalaryItemToAccountingCodeMappingMaintenance : BaseServices, I_7_1_10_SalaryItemToAccountingCodeMappingMaintenance
    {
        private readonly I_Common _common;
        public S_7_1_10_SalaryItemToAccountingCodeMappingMaintenance(DBContext dbContext,I_Common common) : base(dbContext)
        {
            _common = common;
        }

        public async Task<OperationResult> Create(D_7_10_SalaryItemToAccountingCodeMappingMaintenanceDto model, string userName)
        {
            DateTime now = DateTime.Now;
            model.DC_Code = model.DC_Code.ToUpper();
            if (await _repositoryAccessor.HRMS_Sal_SalaryItem_AccountCode.AnyAsync(x => x.Factory == model.Factory
                && x.Salary_Item == model.Salary_Item && x.DC_Code.ToUpper() == model.DC_Code))
                return new OperationResult(false, $"Factory: {model.Factory},\r\nSalary_Item: {model.Salary_Item}, \r\nDC_Code: {model.DC_Code} already exists!");
            var item = new HRMS_Sal_SalaryItem_AccountCode()
            {
                Factory = model.Factory,
                Salary_Item = model.Salary_Item,
                Main_Acc = model.Main_Acc,
                Sub_Acc = model.Sub_Acc,
                DC_Code = model.DC_Code,
                Update_By = userName,
                Update_Time = now
            };
            _repositoryAccessor.HRMS_Sal_SalaryItem_AccountCode.Add(item);
            return new OperationResult(await _repositoryAccessor.Save());
        }

        public async Task<OperationResult> Edit(D_7_10_SalaryItemToAccountingCodeMappingMaintenanceDto model)
        {
            var item = await _repositoryAccessor.HRMS_Sal_SalaryItem_AccountCode.FirstOrDefaultAsync(x => x.Factory == model.Factory
                                                                            && x.Salary_Item == model.Salary_Item
                                                                            && x.DC_Code.ToUpper() == model.DC_Code);
            if (item != null)
            {
                item.Main_Acc = model.Main_Acc;
                item.Sub_Acc = model.Sub_Acc;
                item.Update_By = model.Update_By;
                item.Update_Time = DateTime.Now;
                _repositoryAccessor.HRMS_Sal_SalaryItem_AccountCode.Update(item);
                return new OperationResult(await _repositoryAccessor.Save());
            }
            return new OperationResult(false);
        }

        public async Task<PaginationUtility<D_7_10_SalaryItemToAccountingCodeMappingMaintenanceDto>> GetDataPagination(PaginationParam pagination, D_7_10_SalaryItemToAccountingCodeMappingMaintenanceParam param)
        {
            var datas = _repositoryAccessor.HRMS_Sal_SalaryItem_AccountCode.FindAll(x => x.Factory == param.Factory, true)
                        .GroupJoin(_repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == BasicCodeTypeConstant.SalaryItem, true),
                            x => x.Salary_Item,
                            y => y.Code,
                            (x, y) => new { HSSA = x, HBC = y }
                        ).SelectMany(x => x.HBC.DefaultIfEmpty(), (x, y) => new { x.HSSA, HBC = y })
                        .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == param.Language.ToLower(), true),
                            x => new { x.HBC.Type_Seq, x.HBC.Code },
                            y => new { y.Type_Seq, y.Code },
                            (x, y) => new { x.HSSA, x.HBC, HBCL = y }
                        ).SelectMany(x => x.HBCL.DefaultIfEmpty(), (x, y) => new { x.HSSA, x.HBC, HBCL = y })
                        .Select(x => new D_7_10_SalaryItemToAccountingCodeMappingMaintenanceDto
                        {
                            Factory = x.HSSA.Factory,
                            Salary_Item = x.HSSA.Salary_Item,
                            Salary_Item_Name = x.HBCL != null ? $"{x.HSSA.Salary_Item} {x.HBCL.Code_Name}" : $"{x.HSSA.Salary_Item} {x.HBC.Code_Name}",
                            Main_Acc = x.HSSA.Main_Acc,
                            Sub_Acc = x.HSSA.Sub_Acc,
                            DC_Code = x.HSSA.DC_Code,
                            Update_By = x.HSSA.Update_By,
                            Update_Time = x.HSSA.Update_Time,
                            Update_Time_Str = x.HSSA.Update_Time.ToString("yyyy/MM/dd HH:mm:ss")
                        });
            return await PaginationUtility<D_7_10_SalaryItemToAccountingCodeMappingMaintenanceDto>.CreateAsync(datas, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<List<KeyValuePair<string, string>>> GetFactory(string userName, string language)
        {
            var factoryAccounts = await Queryt_Factory_AddList(userName);
            var factories = await GetDataBasicCode(BasicCodeTypeConstant.Factory, language);
            return factories.IntersectBy(factoryAccounts, x => x.Key).ToList();
        }

        public async Task<OperationResult> CheckDupplicate(string factory, string Salary_Item, string DC_Code)
        {
            return new OperationResult(await _repositoryAccessor.HRMS_Sal_SalaryItem_AccountCode.AnyAsync(x => x.Factory == factory 
                                                                && x.Salary_Item == Salary_Item 
                                                                && x.DC_Code.ToUpper() == DC_Code.ToUpper()));
        }

        public async Task<OperationResult> Delete(string factory, string Salary_Item, string DC_Code)
        {
            var item = await _repositoryAccessor.HRMS_Sal_SalaryItem_AccountCode.FirstOrDefaultAsync(x => x.Factory == factory 
                                                                && x.Salary_Item == Salary_Item 
                                                                && x.DC_Code.ToUpper() == DC_Code.ToUpper());
            if (item != null)
            {
                _repositoryAccessor.HRMS_Sal_SalaryItem_AccountCode.Remove(item);              
                return new OperationResult(await _repositoryAccessor.Save());
            }
            return new OperationResult(false);
        }
    }
}