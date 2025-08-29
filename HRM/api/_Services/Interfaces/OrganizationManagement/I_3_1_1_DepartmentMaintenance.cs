using API.DTOs.OrganizationManagement;
using API.Models;

namespace API._Services.Interfaces.OrganizationManagement
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_3_1_1_DepartmentMaintenance
    {
        Task<PaginationUtility<HRMS_Org_DepartmentDto>> GetDataPagination(PaginationParam pagination, HRMS_Org_Department_Param param);
        Task<OperationResult> Add(HRMS_Org_Department model);
        Task<OperationResult> Update(HRMS_Org_Department model);
        Task<OperationResult> DownloadExcel(HRMS_Org_Department_Param param);
        Task<List<KeyValuePair<string, string>>> GetLanguage();
        Task<List<LanguageParam>> GetDetail(string departmentCode , string division , string factory);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string division, string factory, string lang);
        Task<List<KeyValuePair<string, string>>> GetListDivision(string lang);
        Task<List<KeyValuePair<string, string>>> GetListFactory(string division, string lang);
        Task<List<KeyValuePair<string, string>>> GetListLevel(string lang);
        Task<List<ListUpperVirtual>> GetListUpperVirtual(string departmentCode, string division, string factory, string lang);
        Task<OperationResult> AddLanguage(LanguageDeparment model);
        Task<OperationResult> UpdateLanguage(LanguageDeparment model);
        Task<bool> CheckListDeptCode(string division, string factory, string deptCode);
    }
}