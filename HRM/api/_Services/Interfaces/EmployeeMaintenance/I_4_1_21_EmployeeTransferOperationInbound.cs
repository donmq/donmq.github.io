using API.DTOs.EmployeeMaintenance;

namespace API._Services.Interfaces.EmployeeMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_4_1_21_EmployeeTransferOperationInbound
    {
        Task<PaginationUtility<EmployeeTransferOperationInboundDto>> GetPagination(PaginationParam pagination, EmployeeTransferOperationInboundParam param, List<string> roleList);
        Task<EmployeeTransferOperationInboundDto> GetDetail(string History_GUID);
        Task<OperationResult> Update(EmployeeTransferOperationInboundDto dto, string account, bool onConfirm = false);
        Task<OperationResult> Confirm(EmployeeTransferOperationInboundDto dto, string account);

        #region Get List
        Task<List<KeyValuePair<string, string>>> GetListDivision(string Language);
        Task<List<KeyValuePair<string, string>>> GetListFactory(string Language);
        Task<List<KeyValuePair<string, string>>> GetListFactoryByDivision(string Division, string Language);
        Task<List<KeyValuePair<string, string>>> GetListNationality(string Language);
        Task<List<KeyValuePair<string, string>>> GetListWorkType(string Language);
        Task<List<KeyValuePair<string, string>>> GetListReasonChange(string Language);
        Task<List<KeyValuePair<string, string>>> GetListReasonChangeOut(string Language);
        Task<List<KeyValuePair<string, string>>> GetListReasonChangeIn(string Language);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string Division, string Factory, string Language);
        Task<List<KeyValuePair<decimal, string>>> GetPositionGrade();
        Task<List<KeyValuePair<string, string>>> GetPositionTitle(decimal level, string Language);
        #endregion
    }
}