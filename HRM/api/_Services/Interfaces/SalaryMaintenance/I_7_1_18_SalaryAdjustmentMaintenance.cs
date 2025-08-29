using API.DTOs.SalaryMaintenance;

namespace API._Services.Interfaces.SalaryMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_7_1_18_SalaryAdjustmentMaintenance
    {
        Task<PaginationUtility<SalaryAdjustmentMaintenanceMain>> GetDataPagination(PaginationParam pagination, SalaryAdjustmentMaintenanceParam param);
        Task<SalaryAdjustmentMaintenance_PersonalDetail> GetDetailPersonal(string factory, string employee_ID, string language);
        Task<OperationResult> Create(SalaryAdjustmentMaintenanceMain data);
        Task<OperationResult> Update(SalaryAdjustmentMaintenanceMain data);
        Task<OperationResult> DownloadTemplate(string factory);
        Task<OperationResult> DownloadExcel(SalaryAdjustmentMaintenanceParam param, string userName);
        Task<OperationResult> UploadExcel(IFormFile file, List<string> role_List, string userName);
        Task<List<string>> GetListEmployeeID(string factory);
        Task<CheckEffectiveDateResult> CheckEffectiveDate(string factory, string employee_ID, string inputEffectiveDate);
        Task<List<KeyValuePair<string, string>>> GetListFactory(string language, List<string> roleList);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string factory, string language);
        Task<List<KeyValuePair<string, string>>> GetListReason(string language);
        Task<List<KeyValuePair<string, string>>> GetListTechnicalType(string language);
        Task<List<KeyValuePair<string, string>>> GetListExpertiseCategory(string language);
        Task<List<KeyValuePair<string, string>>> GetListPermissionGroup(string language);
        Task<List<KeyValuePair<string, string>>> GetListSalaryType(string language);
        Task<List<SalaryAdjustmentMaintenance_SalaryItem>> GetListSalaryItem(string history_GUID, string language);
        Task<List<KeyValuePair<string, string>>> GetListPositionTitle(string language);
        Task<List<KeyValuePair<string, string>>> GetListCurrency(string language);
        Task<List<SalaryAdjustmentMaintenance_SalaryItem>> GetSalaryItemsAsync(string factory, string permissionGroup, string salaryType, string type, string language, string employeeID);
        Task<bool> CheckReasonForChange(string reasonForChange);
    }
}