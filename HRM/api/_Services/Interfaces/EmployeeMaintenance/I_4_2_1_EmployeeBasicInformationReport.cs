using API.DTOs.EmployeeMaintenance;

namespace API._Services.Interfaces.EmployeeMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_4_2_1_EmployeeBasicInformationReport
    {
        Task<OperationResult> GetPagination(EmployeeBasicInformationReport_Param param, List<string> roleList);
        Task<OperationResult> ExportExcel(EmployeeBasicInformationReport_Param param, List<string> roleList);

        Task<List<KeyValuePair<string, string>>> GetListNationality(string language);
        Task<List<KeyValuePair<string, string>>> GetListDivision(string language);
        Task<List<KeyValuePair<string, string>>> GetListFactory(string division, List<string> roleList, string language);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string division, string factory, string language);
        Task<List<KeyValuePair<string, string>>> GetListPermission(string language);
        Task<List<KeyValuePair<decimal, decimal>>> GetListPositonGrade();
    }
}