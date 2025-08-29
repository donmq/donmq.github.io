using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance;

[DependencyInjection(ServiceLifetime.Scoped)]
public interface I_5_1_8_CardSwipingDataFormatSetting
{
    Task<PaginationUtility<CardSwipingDataFormatSettingMain>> GetDataPagination(PaginationParam pagination, string factory);
    Task<HRMS_Att_Swipecard_SetDto> GetDataByFactory(string factory);
    Task<List<KeyValuePair<string, string>>> GetFactoryMain(string language);
    Task<List<KeyValuePair<string, string>>> GetFactoryByAccountAndLanguage(string userName, string language);
    Task<OperationResult> AddNew(HRMS_Att_Swipecard_SetDto param);
    Task<OperationResult> Edit(HRMS_Att_Swipecard_SetDto param);
}