using API.DTOs.EmployeeMaintenance;

namespace API._Services.Interfaces.EmployeeMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_4_1_20_EmployeeTransferOperationOutbound
    {
        Task<PaginationUtility<EmployeeTransferOperationOutboundDto>> GetPagination(PaginationParam pagination, EmployeeTransferOperationOutboundParam param, List<string> roleList);
        Task<EmployeeTransferOperationOutboundDto> GetDetail(string History_GUID);
        Task<OperationResult> Add(EmployeeTransferOperationOutboundDto dto, string account);
        Task<OperationResult> Update(EmployeeTransferOperationOutboundDto dto, string account);
        Task<EmployeeInformationResult> GetEmployeeInformation(EmployeeInformationParam param);
        Task<List<KeyValuePair<string, string>>> GetEmployeeID();

        #region Get List
        Task<List<KeyValuePair<string, string>>> GetListDivision(string Language);
        Task<List<KeyValuePair<string, string>>> GetListFactory(string Language);
        Task<List<KeyValuePair<string, string>>> GetListFactoryByDivision(string Division, string Language);
        Task<List<KeyValuePair<string, string>>> GetListNationality(string Language);
        Task<List<KeyValuePair<string, string>>> GetListWorkType(string Language);
        Task<List<KeyValuePair<string, string>>> GetListReasonChangeOut(string Language);
        Task<List<KeyValuePair<string, string>>> GetListReasonChangeIn(string Language);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string Division, string Factory, string Language);
        Task<List<KeyValuePair<decimal, string>>> GetPositionGrade();
        Task<List<KeyValuePair<string, string>>> GetPositionTitle(decimal level, string Language);
        #endregion
    }
}