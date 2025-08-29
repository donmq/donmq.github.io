using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_1_7_MaintenanceOfAnnualLeaveEntitlement
    {
        Task<List<KeyValuePair<string, string>>> GetListFactory(string userName, string language);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string factory, string language);
        Task<List<KeyValuePair<string, string>>> GetListLeaveCode(string language);
        Task<PaginationUtility<MaintenanceOfAnnualLeaveEntitlementDto>> Query(PaginationParam pagination, MaintenanceOfAnnualLeaveEntitlementParam param);
        Task<FileContentResult> ExportExcel();
        Task<OperationResult> DownloadExcel(MaintenanceOfAnnualLeaveEntitlementParam param, string userName);
        Task<OperationResult> UploadExcel(UploadFormData formData);
        Task<OperationResult> Add(MaintenanceOfAnnualLeaveEntitlementDto datas, string userName);
        Task<OperationResult> Edit(MaintenanceOfAnnualLeaveEntitlementDto dto);
        Task<OperationResult> Delete(MaintenanceOfAnnualLeaveEntitlementDto dto);
        Task<OperationResult> CheckExistedData(string Annual_Start, string Factory, string Employee_ID, string Leave_Code);
    }
}