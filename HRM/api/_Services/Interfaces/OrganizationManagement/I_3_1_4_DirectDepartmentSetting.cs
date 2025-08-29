using API.DTOs.OrganizationManagement;

namespace API._Services.Interfaces.OrganizationManagement
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_3_1_4_DirectDepartmentSetting
    {
        Task<PaginationUtility<Org_Direct_DepartmentResult>> Getdata(PaginationParam pagination, Org_Direct_DepartmentParam param);
        Task<List<KeyValuePair<string, string>>> GetListDivision(string Language);
        Task<List<KeyValuePair<string, string>>> GetListFactory(string Division,string Language);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string Language);
        Task<List<KeyValuePair<string, string>>> GetListLine(string Division, string Factory);
        Task<List<Org_Direct_DepartmentResult>> Getdetail(Org_Direct_DepartmentParam model);
        Task<OperationResult> DownloadExcel(Org_Direct_DepartmentParam param);
        Task<List<KeyValuePair<string, string>>> GetListDirectDepartmentAttribute();
        Task<OperationResult> AddNew(List<Org_Direct_DepartmentParamQuery> model,string userName);
        Task<OperationResult> Edit(List<Org_Direct_DepartmentParamQuery> model,string userName);
        Task<OperationResult> Delete(Org_Direct_DepartmentParamQuery model);
        Task<OperationResult> CheckDuplicate(List<Org_Direct_DepartmentResult> model);

    }
}