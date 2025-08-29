using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_1_13_SwipeCardDataUpload
    {
        Task<List<KeyValuePair<string, string>>> GetFactories(string language, List<string> roleList);
        Task<OperationResult> UploadExcuteSwipeData(HRMS_Att_Swipe_Card_Upload request);
    }
}