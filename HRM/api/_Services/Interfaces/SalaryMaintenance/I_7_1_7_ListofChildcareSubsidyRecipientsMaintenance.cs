using API.DTOs.SalaryMaintenance;

namespace API._Services.Interfaces.SalaryMaintenance;

[DependencyInjection(ServiceLifetime.Scoped)]
public interface I_7_1_7_ListofChildcareSubsidyRecipientsMaintenance
{
    Task<List<KeyValuePair<string, string>>> GetListFactory(string userName, string language);
    Task<PaginationUtility<D_7_7_HRMS_Sal_Childcare_SubsidyDto>> Search(PaginationParam pagination, D_7_7_HRMS_Sal_Childcare_SubsidyParam param);
    Task<OperationResult> ExcelExport(D_7_7_HRMS_Sal_Childcare_SubsidyParam param, string userName);
    Task<OperationResult> UploadExcel(IFormFile file, List<string> role_List, string userName);
    Task<OperationResult> DownloadExcelTemplate();
    Task<OperationResult> AddAsync(D_7_7_HRMS_Sal_Childcare_SubsidyDto param);
    Task<OperationResult> UpdateAsync(D_7_7_HRMS_Sal_Childcare_SubsidyDto param);
    Task<OperationResult> DeleteAsync(D_7_7_DeleteParam param);
    Task<OperationResult> CheckExistedData(string Factory, string Employee_ID, string Birthday_Child);
}
