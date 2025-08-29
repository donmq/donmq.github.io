
using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_1_25_LoanedMonthAttendanceDataGeneration
    {
        Task<List<KeyValuePair<string, string>>> GetListFactory(string userName, string language);
        Task<OperationResult> Execute(LoanedDataGenerationDto dto);
    }
}