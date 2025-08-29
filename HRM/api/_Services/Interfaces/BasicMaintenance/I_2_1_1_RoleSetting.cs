using API.DTOs.BasicMaintenance;

namespace API._Services.Interfaces.BasicMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_2_1_1_RoleSetting
    {
        Task<List<KeyValuePair<string, string>>> GetDropDownList(string lang);
        Task<PaginationUtility<RoleSettingDetail>> GetSearchDetail(PaginationParam paginationParams, RoleSettingParam searchParam);
        Task<List<TreeviewItem>> GetProgramGroupDetail(RoleSettingParam param);
        Task<List<TreeviewItem>> GetProgramGroupTemplate(string lang);
        Task<RoleSettingDto> GetRoleSettingEdit(RoleSettingParam param);
        Task<OperationResult> PostRole(RoleSettingDto data, string userName);
        Task<OperationResult> PutRole(RoleSettingDto data, string userName);
        Task<OperationResult> DownloadExcel(RoleSettingParam param);
        Task<OperationResult> DeleteRole(string role, string factory);
        Task<OperationResult> CheckRole(string role, List<string> roleList);
    }
}