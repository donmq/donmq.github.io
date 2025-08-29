using API.DTOs.SalaryMaintenance;

namespace API._Services.Interfaces.SalaryMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_7_1_9_DepartmentToSAPCostCenterMappingMaintenance
    {
        Task<PaginationUtility<D_7_9_Sal_Dept_SAPCostCenter_MappingDTO>> GetDataPagination(PaginationParam pagination, D_7_9_Sal_Dept_SAPCostCenter_MappingParam param);
        Task<List<KeyValuePair<string, string>>> GetListFactory(List<string> roleList, string language);
        Task<List<KeyValuePair<string, string>>> GetListCostCenter( D_7_9_Sal_Dept_SAPCostCenter_MappingParam param);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string factory, string language);
        Task<bool> CheckDuplicate(string factory, string year, string department);
        Task<OperationResult> DownloadTemplate();
        Task<OperationResult> DownloadExcel(D_7_9_Sal_Dept_SAPCostCenter_MappingParam param, string userName);
        Task<OperationResult> UploadExcel(IFormFile file, List<string> role_List, string userName);
        Task<OperationResult> Create(D_7_9_Sal_Dept_SAPCostCenter_MappingDTO data, string userName);
        Task<OperationResult> Update(D_7_9_Sal_Dept_SAPCostCenter_MappingDTO data, string userName);
        Task<OperationResult> Delete(D_7_9_Sal_Dept_SAPCostCenter_MappingDTO data);

    }
}