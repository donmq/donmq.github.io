using API.DTOs.OrganizationManagement;

namespace API._Services.Interfaces.OrganizationManagement
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_3_1_3_OrganizationChart
    {
        Task<List<KeyValuePair<string, string>>> GetDropDownList(OrganizationChartParam param);
        Task<List<KeyValuePair<string, string>>> GetDepartmentList(OrganizationChartParam param);
        Task<List<INodeDtoFinal>> GetChartData(OrganizationChartParam param);
        Task<OperationResult> DownloadExcel(OrganizationChartParam param);
    }
}