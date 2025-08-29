
using API.DTOs.CompulsoryInsuranceManagement;

namespace API._Services.Interfaces.CompulsoryInsuranceManagement
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_6_1_2_ContributionRateSetting
    {
        Task<PaginationUtility<ContributionRateSettingDto>> GetDataPagination(PaginationParam pagination, ContributionRateSettingParam param);
        Task<List<ContributionRateSettingSubData>> GetDetail(ContributionRateSettingSubParam param);
        Task<OperationResult> Create(ContributionRateSettingForm data);
        Task<OperationResult> Update(ContributionRateSettingForm data);
        Task<OperationResult> Delete(ContributionRateSettingDto data);
        Task<List<KeyValuePair<string, string>>> GetListFactory(List<string> roleList, string language);
        Task<List<KeyValuePair<string, string>>> GetListPermissionGroup(string factory, string language);
        Task<List<KeyValuePair<string, string>>> GetListInsuranceType(string language);
        Task<ContributionRateSettingCheckEffectiveMonth> CheckEffectiveMonth(ContributionRateSettingSubParam param);
        Task<bool> CheckSearch(ContributionRateSettingParam param);

    }
}