using API.DTOs.SalaryMaintenance;

namespace API._Services.Interfaces.SalaryMaintenance;

[DependencyInjection(ServiceLifetime.Scoped)]
public interface I_7_1_8_SAPCostCenterSetting
{
    Task<List<KeyValuePair<string, string>>> GetListFactory(string userName, string language);
    Task<List<KeyValuePair<string, string>>> GetListKind(string language);
    Task<PaginationUtility<D_7_8_SAPCostCenterSettingDto>> Search(PaginationParam pagination, D_7_8_SAPCostCenterSettingParam param);
    Task<OperationResult> ExcelExport(D_7_8_SAPCostCenterSettingParam param, List<string> roleList, string userName);
    Task<OperationResult> UploadExcel(IFormFile file, List<string> role_List, string userName);
    Task<OperationResult> DownloadExcelTemplate();
    Task<OperationResult> AddAsync(D_7_8_SAPCostCenterSettingDto param);
    Task<OperationResult> UpdateAsync(D_7_8_SAPCostCenterSettingDto param);
    Task<OperationResult> DeleteAsync(D_7_8_DeleteParam param);
    Task<OperationResult> CheckExistedDataOrCostCenter(D_7_8_CheckDuplicateParam param); // Cost Center
    Task<OperationResult> CheckExistedDataCompanyCode(string factory, string companyCode);
}
