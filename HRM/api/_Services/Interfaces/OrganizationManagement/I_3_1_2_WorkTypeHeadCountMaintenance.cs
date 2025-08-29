using API.DTOs.OrganizationManagement;

namespace API._Services.Interfaces.OrganizationManagement
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_3_1_2_WorkTypeHeadCountMaintenance
    {
        #region  Main
        Task<List<KeyValuePair<string, string>>> GetDivisions(string language);
        Task<List<KeyValuePair<string, string>>> GetFactories(string language);
        Task<List<KeyValuePair<string, string>>> GetFactories(string division, string language);
        Task<List<KeyValuePair<string, string>>> GetDepartments(string language);
        Task<List<KeyValuePair<string, string>>> GetDepartments(string division, string factory ,string language);
        Task<DepartmentNameObject> GetDepartmentNameFromDepartmentCode(HRMS_Org_Work_Type_HeadcountParam param);
        Task<List<KeyValuePair<string, string>>> GetWorkCodeNames();
        Task<List<HRMS_Org_Work_Type_HeadcountDto>> GetListUpdate(HRMS_Org_Work_Type_HeadcountParam filter);

        Task<HRMS_Org_Work_Type_HeadcountDataMain> GetDataPagination(PaginationParam param, HRMS_Org_Work_Type_HeadcountParam filter);

        Task<OperationResult> Create(List<HRMS_Org_Work_Type_HeadcountDto> models, string currentUserUpdate);
        Task<OperationResult> Update(HRMS_Org_Work_Type_HeadcountUpdate models, string currentUserUpdate);
        Task<OperationResult> Delete(HRMS_Org_Work_Type_HeadcountDto model);

        Task<OperationResult> DownloadExcel(HRMS_Org_Work_Type_HeadcountParam param);

        #endregion


        #region Form

        #endregion
    }
}