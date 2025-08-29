using API.DTOs;
using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_1_5_PregnancyAndMaternityDataMaintenance
    {
        Task<List<KeyValuePair<string, string>>> GetListFactory(string language, string userName);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string factory, string language);
        Task<List<KeyValuePair<string, string>>> GetListWorkType(string language, bool? isWorkShiftType);
        Task<PaginationUtility<PregnancyMaternityDetail>> Query(PaginationParam pagination, PregnancyMaternityParam param);
        Task<OperationResult> ExportExcel(PregnancyMaternityParam param, string userName);
        Task<OperationResult> Add(PregnancyMaternityDetail dto);
        Task<OperationResult> Edit(PregnancyMaternityDetail dto);
        Task<OperationResult> Delete(PregnancyMaternityDetail dto);
        Task<object> GetSpecialRegularWorkType(string Factory, string Work_Type_Before);
    }
}