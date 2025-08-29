using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_1_10_ShiftManagementProgram
    {
        Task<List<KeyValuePair<string, string>>> GetDropDownList(ShiftManagementProgram_Param param, List<string> roleList);
        Task<PaginationUtility<ShiftManagementProgram_Main>> GetSearchDetail(PaginationParam paginationParams, ShiftManagementProgram_Param searchParam);
        Task<OperationResult> IsExistedData(ShiftManagementProgram_Param param);
        Task<List<TypeheadKeyValue>> GetEmployeeList(ShiftManagementProgram_Param param);
        Task<List<ShiftManagementProgram_Main>> GetEmployeeDetail(ShiftManagementProgram_Param param);
        Task<List<KeyValuePair<string, string>>> GetDepartmentList(ShiftManagementProgram_Param param);
        Task<OperationResult> PostDataEmployee(ShiftManagementProgram_Main input);
        Task<OperationResult> PostDataDepartment(List<ShiftManagementProgram_Main> input);
        Task<OperationResult> PutDataEmployee(ShiftManagementProgram_Update input);
        Task<List<KeyValuePair<string, string>>> GetWorkShiftTypeDepartmentList(ShiftManagementProgram_Param param);
        Task<OperationResult> BatchDelete(List<ShiftManagementProgram_Main> data);
    }
}