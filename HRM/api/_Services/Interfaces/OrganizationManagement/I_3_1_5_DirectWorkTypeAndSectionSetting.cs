using API.DTOs.OrganizationManagement;

namespace API._Services.Interfaces.OrganizationManagement
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_3_1_5_DirectWorkTypeAndSectionSetting
    {
        Task<PaginationUtility<HRMS_Org_Direct_SectionDto>> GetDataPagination(PaginationParam pagination, DirectWorkTypeAndSectionSettingParam param);
        Task<OperationResult> Create(HRMS_Org_Direct_SectionDto data);
        Task<OperationResult> Update(HRMS_Org_Direct_SectionDto data);
        Task<OperationResult> DownloadFileExcel(DirectWorkTypeAndSectionSettingParam param);
        Task<List<KeyValuePair<string, string>>> GetListDivision(string language);
        Task<List<KeyValuePair<string, string>>> GetListFactory(string division, string language);
        Task<List<KeyValuePair<string, string>>> GetListWorkType(string language);
        Task<List<KeyValuePair<string, string>>> GetListSection(string language);
        Task<OperationResult> CheckDuplicate(DirectWorkTypeAndSectionSettingParam param);
    }
}