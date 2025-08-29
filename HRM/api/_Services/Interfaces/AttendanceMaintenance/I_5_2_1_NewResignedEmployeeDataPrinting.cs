using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_2_1_NewResignedEmployeeDataPrinting
    {
        Task<List<KeyValuePair<string, string>>> GetListFactory(string language, string userName);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string language, string factory);
        Task<List<NewResignedEmployeeDataPrintingDto>> GetData(NewResignedEmployeeDataPrintingParam param);
        Task<int> GetTotal(NewResignedEmployeeDataPrintingParam param);
        Task<OperationResult> DownloadExcel(NewResignedEmployeeDataPrintingParam param, string userName);
    }
}