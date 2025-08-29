using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_2_13_MonthlyEmployeeStatusChangesSheet_ByWorkTypeJob
    {
        Task<List<KeyValuePair<string, string>>> GetFactories(List<string> roleList, string language);
        Task<List<KeyValuePair<string, string>>> GetPermistionGroups(string factory, string language);
        Task<List<KeyValuePair<string, string>>> GetLevels(string language);
        Task<List<KeyValuePair<string, string>>> GetWorkTypeJobs(string language);

        Task<OperationResult> GetTotalRecords(Monthly_Employee_Status_Changes_Sheet_By_WorkType_Job_Param param);
        Task<OperationResult> ExportExcel(Monthly_Employee_Status_Changes_Sheet_By_WorkType_Job_Param param);
    }
}