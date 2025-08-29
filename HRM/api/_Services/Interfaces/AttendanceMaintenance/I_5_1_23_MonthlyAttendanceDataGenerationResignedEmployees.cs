using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_1_23_MonthlyAttendanceDataGenerationResignedEmployees
    {
        Task<OperationResult> CheckParam(GenerationResignedParam param);
        Task<OperationResult> MonthlyAttendanceDataGeneration(GenerationResigned param);

        Task<OperationResult> MonthlyDataCloseExecute(MonthlyAttendanceDataGenerationResignedEmployees_MonthlyDataCloseParam param);

        Task<List<KeyValuePair<string, string>>> Queryt_Factory_AddList(string userName, string language);
        Task<List<KeyValuePair<string, string>>> Query_DropDown_List(string factory, string language);
    }
}