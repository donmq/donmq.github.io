using API.DTOs.EmployeeMaintenance;

namespace API._Services.Interfaces.EmployeeMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_4_1_2_EmployeeEmergencyContacts
    {
        Task<DataMain> GetData(EmployeeEmergencyContactsParam param);
        Task<List<KeyValuePair<string, string>>> GetRelationships(string Language);
        Task<OperationResult> Create(EmployeeEmergencyContactsDto model);
        Task<OperationResult> Update(EmployeeEmergencyContactsDto model);
        Task<OperationResult> Delete(EmployeeEmergencyContactsDto model);
        Task<OperationResult> DownloadExcelTemplate();
        Task<OperationResult> UploadData(IFormFile file, List<string> role_List, string userName);
        Task<int> GetMaxSeq(string USER_GUID);
    }
}