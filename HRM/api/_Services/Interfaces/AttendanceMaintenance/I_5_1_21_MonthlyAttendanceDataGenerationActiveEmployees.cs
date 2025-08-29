using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_1_21_MonthlyAttendanceDataGenerationActiveEmployees
    {
        Task<OperationResult> CheckParam(GenerationActiveParam param);
        Task<OperationResult> MonthlyAttendanceDataGeneration(GenerationActiveParam param);
        Task<PaginationUtility<SearchAlreadyDeadlineDataMain>> SearchAlreadyDeadlineData(PaginationParam pagination, SearchAlreadyDeadlineDataParam param);
        Task<OperationResult> MonthlyDataCloseExecute(MonthlyAttendanceDataGenerationActiveEmployees_MonthlyDataCloseParam param);

        Task<List<KeyValuePair<string, string>>> Queryt_Factory_AddList(string userName, string language);
        Task<List<KeyValuePair<string, string>>> Query_DropDown_List(string factory, string language);
    }
}