
using API.DTOs.EmployeeMaintenance;

namespace API._Services.Interfaces.EmployeeMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_4_1_3_Education
    {
        // Upload file
        Task<OperationResult> UploadFiles(EducationUpload model);

        // Download File
        Task<OperationResult> DownloadFile(EducationFile model);

        // Thêm mới 
        Task<OperationResult> Create(HRMS_Emp_EducationalDto model);
        // Chỉnh sửa
        Task<OperationResult> Update(HRMS_Emp_EducationalDto model);

        Task<OperationResult> Delete(HRMS_Emp_EducationalDto model);
        Task<OperationResult> DeleteEducationFile(EducationFile model);

        // Lấy danh sách phân trang 
        Task<List<HRMS_Emp_EducationalDto>> GetDataPagination(HRMS_Emp_EducationalParam filter);

        // Lây danh sách Files
        Task<List<HRMS_Emp_Educational_FileUpload>> GetEducationFiles(string user_GUID);

        // Danh sách [Degrees]
        Task<List<KeyValuePair<string, string>>> GetDegrees(string language);

        // Danh sách [Academic System]
        Task<List<KeyValuePair<string, string>>> GetAcademicSystems(string language);
        // Danh sách [Major]
        Task<List<KeyValuePair<string, string>>> GetMajors(string language);
    }
}