using API.DTOs.EmployeeMaintenance;

namespace API._Services.Interfaces.EmployeeMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_4_1_12_ResignationManagement
    {
        Task<List<KeyValuePair<string, string>>> GetListDivision(string language);
        Task<List<KeyValuePair<string, string>>> GetListFactory(string division, string language);
        Task<List<KeyValuePair<string, string>>> GetListResignationType(string language);
        Task<List<KeyValuePair<string, string>>> GetListResignReason(string language, string resignationType);
        Task<List<string>> GetEmployeeID();
        Task<List<HRMS_Emp_ResignationFormDto>> GetEmployeeData(string factory, string employeeID);
        Task<OperationResult> GetVerifierName(string factory, string verifier);
        Task<OperationResult> GetVerifierTitle(string factory, string verifier, string language);
        Task<PaginationUtility<HRMS_Emp_ResignationDto>> GetDataPagination(PaginationParam pagination, ResignationManagementParam param, List<string> roleList);
        Task<OperationResult> AddNew(ResignAddAndEditParam param, string userName);
        Task<OperationResult> Edit(ResignAddAndEditParam param, string userName);
        Task<OperationResult> Delete(HRMS_Emp_ResignationDto data, string userName);
        Task<OperationResult> DownloadExcel(ResignationManagementParam param, List<string> roleList);
    }
}