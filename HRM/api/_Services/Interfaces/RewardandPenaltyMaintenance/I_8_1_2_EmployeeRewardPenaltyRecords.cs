
using API.DTOs;
using API.DTOs.RewardandPenaltyMaintenance;

namespace API._Services.Interfaces.RewardandPenaltyMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_8_1_2_EmployeeRewardPenaltyRecords
    {
        Task<PaginationUtility<D_8_1_2_EmployeeRewardPenaltyRecordsData>> GetSearch(PaginationParam paginationParams, D_8_1_2_EmployeeRewardPenaltyRecordsParam searchParam);
        Task<D_8_1_2_EmployeeRewardPenaltyRecordsSubParam> Data_Detail(string History_GUID, string Language);
        //Task<OperationResult> GetSearch(PaginationParam paginationParams, D_8_1_2_EmployeeRewardPenaltyRecordsParam searchParam);

        Task<OperationResult> DownloadTemplate();
        Task<OperationResult> DownloadFile(EmployeeRewardPenaltyRecordsReportDownloadFileModel param);
        Task<OperationResult> UploadFileExcel(IFormFile file, List<string> role_List, string userName);

        Task<OperationResult> Create(D_8_1_2_EmployeeRewardPenaltyRecordsSubParam data, string userName);
        Task<OperationResult> Delete(D_8_1_2_EmployeeRewardPenaltyRecordsData data);
        Task<OperationResult> Update(D_8_1_2_EmployeeRewardPenaltyRecordsSubParam data, string userName);

        Task<List<KeyValuePair<string, string>>> GetListFactory(string language, string userName);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string language, string factory);
        Task<List<KeyValuePair<string, string>>> GetListRewardType(string language);
        Task<List<KeyValuePair<string, string>>> GetListReasonCode(string Factory);
        Task<List<EmployeeCommonInfo>> GetEmployeeList(D_8_1_2_EmployeeRewardPenaltyRecordsParam param);
    }
}
