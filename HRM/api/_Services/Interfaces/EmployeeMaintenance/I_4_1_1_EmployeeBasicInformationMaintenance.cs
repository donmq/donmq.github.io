using API.DTOs.EmployeeMaintenance;

namespace API._Services.Interfaces.EmployeeMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_4_1_1_EmployeeBasicInformationMaintenance
    {
        Task<PaginationUtility<EmployeeBasicInformationMaintenanceView>> GetPagination(PaginationParam pagination, EmployeeBasicInformationMaintenanceParam param, List<string> roleList);
        Task<EmployeeBasicInformationMaintenanceDto> GetDetail(string USER_GUID);
        Task<bool> CheckDuplicateCase1(string Nationality, string IdentificationNumber);
        Task<bool> CheckDuplicateCase2(CheckDuplicateParam param);
        Task<bool> CheckDuplicateCase3(CheckDuplicateParam param);
        Task<bool> CheckBlackList(CheckBlackList param);

        Task<OperationResult> Add(EmployeeBasicInformationMaintenanceDto dto, string account);
        Task<OperationResult> Update(EmployeeBasicInformationMaintenanceDto dto, string account);
        Task<OperationResult> Rehire(EmployeeBasicInformationMaintenanceDto dto, string account);
        Task<OperationResult> Delete(string USER_GUID);
        Task<OperationResult> DownloadExcelTemplate();
        Task<OperationResult> UploadData(IFormFile file, List<string> role_List, string userName);
        #region Get List
        Task<List<KeyValuePair<string, string>>> GetListDivision(string Language);
        Task<List<KeyValuePair<string, string>>> GetListFactory(string Division, string Language);
        Task<List<KeyValuePair<string, string>>> GetListNationality(string Language);
        Task<List<KeyValuePair<string, string>>> GetListPermission(string Language);
        Task<List<KeyValuePair<string, string>>> GetListIdentityType(string Language);
        Task<List<KeyValuePair<string, string>>> GetListEducation(string Language);
        Task<List<KeyValuePair<string, string>>> GetListWorkType(string Language);
        Task<List<KeyValuePair<string, string>>> GetListRestaurant(string Language);
        Task<List<KeyValuePair<string, string>>> GetListReligion(string Language);
        Task<List<KeyValuePair<string, string>>> GetListTransportationMethod(string Language);
        Task<List<KeyValuePair<string, string>>> GetListVehicleType(string Language);
        Task<List<KeyValuePair<string, string>>> GetListWorkLocation(string Language);
        Task<List<KeyValuePair<string, string>>> GetListReasonResignation(string Language);
        Task<List<KeyValuePair<string, string>>> GetListWorkTypeShift(string Language);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string Division, string Factory, string Language);
        Task<List<KeyValuePair<string, string>>> GetListProvinceDirectly(string char1, string Language);
        Task<List<KeyValuePair<string, string>>> GetListCity(string char1, string Language);
        Task<List<KeyValuePair<decimal, string>>> GetPositionGrade();
        Task<List<KeyValuePair<string, string>>> GetPositionTitle(decimal level, string Language);
        Task<List<DepartmentSupervisorList>> GetDepartmentSupervisor(string USER_GUID, string Language);
        #endregion

    }
}