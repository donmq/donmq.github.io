using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_2_14_MonthlyEmployeeStatusChangesSheet_ByReasonforResignation
    {
        Task<List<KeyValuePair<string, string>>> GetFactories(List<string> roleList, string language);
        Task<List<KeyValuePair<string, string>>> GetPermistionGroups(string factory, string language);

        Task<Monthly_Employee_Status_Changes_Sheet_By_Reason_for_Resignation_Result> GetTotalRecords(Monthly_Employee_Status_Changes_Sheet_By_Reason_for_Resignation_Param param);
        Task<OperationResult> Export(Monthly_Employee_Status_Changes_Sheet_By_Reason_for_Resignation_Param param);
    }
}
