using API.DTOs.BasicMaintenance;

namespace API._Services.Interfaces.BasicMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_2_1_2_AccountAuthorizationSetting
    {
        Task<PaginationUtility<AccountAuthorizationSetting_Data>> GetDataPagination(PaginationParam pagination, AccountAuthorizationSetting_Param param);
        Task<OperationResult> Create(AccountAuthorizationSetting_Data data);
        Task<OperationResult> Update(AccountAuthorizationSetting_Data data);
        Task<OperationResult> ResetPassword(AccountAuthorizationSetting_Data data);
        Task<OperationResult> DownloadFileExcel(AccountAuthorizationSetting_Param param);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string division, string factory, string language);
        Task<List<KeyValuePair<string, string>>> GetListDivision(string language);
        Task<List<KeyValuePair<string, string>>> GetListFactory(string language);
        Task<List<KeyValuePair<string, string>>> GetListRole();
    }
}