using API.DTOs.EmployeeMaintenance;

namespace API._Services.Interfaces.EmployeeMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_4_1_19_ExitEmployeeMasterFileHistoricalData
    {
        Task<PaginationUtility<ExitEmployeeMasterFileHistoricalDataView>> GetPagination(PaginationParam pagination, ExitEmployeeMasterFileHistoricalDataParam param, string account);
        Task<ExitEmployeeMasterFileHistoricalDataDto> GetDetail(string USER_GUID, string Resign_Date);

        #region Get List
        Task<List<KeyValuePair<string, string>>> GetListNationality(string Language);
        Task<List<KeyValuePair<string, string>>> GetListDivision(string Language);
        Task<List<KeyValuePair<string, string>>> GetListFactory(string Division, string Language);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string Division, string Factory, string Language);
        Task<List<KeyValuePair<string, string>>> GetListPermission(string Language);
        Task<List<KeyValuePair<string, string>>> GetListIdentityType(string Language);
        Task<List<KeyValuePair<string, string>>> GetListEducation(string Language);
        Task<List<KeyValuePair<string, string>>> GetListReligion(string Language);
        Task<List<KeyValuePair<string, string>>> GetListTransportationMethod(string Language);
        Task<List<KeyValuePair<string, string>>> GetListVehicleType(string Language);
        Task<List<KeyValuePair<string, string>>> GetListProvinceDirectly(string char1, string Language);
        Task<List<KeyValuePair<string, string>>> GetListCity(string char1, string Language);
        Task<List<KeyValuePair<decimal, string>>> GetPositionGrade();
        Task<List<KeyValuePair<string, string>>> GetPositionTitle(decimal level, string Language);
        Task<List<KeyValuePair<string, string>>> GetListWorkType(string Language);
        Task<List<KeyValuePair<string, string>>> GetListRestaurant(string Language);
        Task<List<KeyValuePair<string, string>>> GetListWorkLocation(string Language);
        Task<List<KeyValuePair<string, string>>> GetListReasonResignation(string Language);
        Task<List<KeyValuePair<string, string>>> GetListWorkTypeShift(string Language);
        #endregion
    }
}