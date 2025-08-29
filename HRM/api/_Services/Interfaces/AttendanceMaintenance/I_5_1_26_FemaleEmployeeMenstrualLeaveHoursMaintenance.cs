
using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_1_26_FemaleEmployeeMenstrualLeaveHoursMaintenance
    {
        Task<List<KeyValuePair<string, string>>> GetListFactory(string language);
        Task<List<KeyValuePair<string, string>>> GetListFactoryAdd(string userName, string language);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string factory, string language);
        Task<PaginationUtility<FemaleEmpMenstrualMain>> GetDataPagination(PaginationParam pagination, FemaleEmpMenstrualParam param, bool isPaging = true);
        Task<OperationResult> Add(FemaleEmpMenstrualMain data, string userName);
        Task<OperationResult> Edit(FemaleEmpMenstrualMain data, string userName);
        Task<OperationResult> Delete(FemaleEmpMenstrualMain data);
        Task<OperationResult> DownloadExcel(FemaleEmpMenstrualParam param, string userName);
        Task<List<KeyValuePair<string, string>>> GetListFactoryByUser(string language, string userName);
    }
}