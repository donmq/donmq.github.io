using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.RewardandPenaltyMaintenance;
using API.DTOs.SalaryMaintenance;

namespace API._Services.Interfaces.RewardandPenaltyMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_8_1_1_RewardAndPenaltyReasonCodeMaintenance
    {
        Task<List<KeyValuePair<string, string>>> GetListFactory(string userName, string language);
        Task<List<KeyValuePair<string, string>>> Query_Reason(string factory);
        Task<PaginationUtility<RewardandPenaltyMaintenanceDTO>> GetDataPagination(PaginationParam pagination, RewardandPenaltyMaintenanceParam param);
        Task<OperationResult> Create(RewardandPenaltyMaintenance_form data);
        Task<OperationResult> Update(RewardandPenaltyMaintenanceDTO data);
        Task<OperationResult> Delete(RewardandPenaltyMaintenanceDTO data);
        Task<OperationResult> DownloadFileExcel(RewardandPenaltyMaintenanceParam param, string userName);
        Task<OperationResult> IsDuplicatedData(RewardandPenaltyMaintenanceDTO param);

    }
}