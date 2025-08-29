
namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_1_17_DailyAttendancePosting
    {
        Task<List<KeyValuePair<string, string>>> GetListFactory (string language, List<string> roleList);
        Task<OperationResult> Execute(string factory, string userName);
    }
} 