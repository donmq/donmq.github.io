using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_1_2_ShiftScheduleSetting
    {
        //#region Arrays
        Task<List<KeyValuePair<string, string>>> GetDivisions(string language);
        Task<List<KeyValuePair<string, string>>> GetWorkShiftTypes(string language);
        Task<List<KeyValuePair<string, string>>> GetFactories(string language, string userName);
        Task<List<KeyValuePair<string, string>>> GetFactories(string division, string language, string userName);
        //#endregion

        Task<PaginationUtility<HRMS_Att_Work_ShiftDto>> GetDataPagination(PaginationParam param, HRMS_Att_Work_ShiftParam filter);
        Task<OperationResult> Create(HRMS_Att_Work_ShiftDto model);
        Task<OperationResult> Update(HRMS_Att_Work_ShiftDto model);
    }
}