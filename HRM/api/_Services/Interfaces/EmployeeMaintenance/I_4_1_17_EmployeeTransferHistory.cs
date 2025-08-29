using API.DTOs.EmployeeMaintenance;

namespace API._Services.Interfaces.EmployeeMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_4_1_17_EmployeeTransferHistory
    {
        Task<PaginationUtility<EmployeeTransferHistoryDTO>> GetDataPagination(PaginationParam pagination, EmployeeTransferHistoryParam param, List<string> roleList);
        Task<OperationResult> Create(EmployeeTransferHistoryDTO dto);
        Task<OperationResult> Update(EmployeeTransferHistoryDTO dto);
        Task<OperationResult> Delete(EmployeeTransferHistoryDetele dto);
        Task<OperationResult> BatchDelete(List<EmployeeTransferHistoryDetele> dto);
        Task<OperationResult> DownloadFileExcel(EmployeeTransferHistoryParam param, List<string> roleList);
        Task<OperationResult> EffectiveConfirm( List<EmployeeTransferHistoryEffectiveConfirm> dto, string currentUser);
        Task<OperationResult> CheckEffectiveConfirm(EmployeeTransferHistoryEffectiveConfirm data);
        Task<EmployeeTransferHistoryDTO> GetDataDetail(string division, string employee_ID, string factory);
        Task<List<KeyValuePair<string, string>>> GetListDivision(string language);
        Task<List<KeyValuePair<string, string>>> GetListFactory(string language, string division);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string language, string factory, string division);
        Task<List<KeyValuePair<string, string>>> GetListAssignedDivisionAfter(string language);
        Task<List<KeyValuePair<string, string>>> GetListAssignedFactoryAfter(string language, string assignedDivisionAfter);
        Task<List<KeyValuePair<string, string>>> GetListDepartmentAfter(string language, string assignedDivisionAfter, string assignedFactoryAfter);
        Task<List<KeyValuePair<decimal, decimal>>> GetListPositionGrade(string language);
        Task<List<KeyValuePair<string, string>>> GetListPositionTitle(string language, decimal? positionGrade);
        Task<List<KeyValuePair<string, string>>> GetListWorkType(string language);
        Task<List<KeyValuePair<string, string>>> GetListReasonforChange(string language);
        Task<List<KeyValuePair<string, string>>> GetListDataSource(string language);
        Task<List<string>> GetListTypeHeadEmployeeID(string factory, string division);
        Task CheckEffectiveDate();
    }
}