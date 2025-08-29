using API.DTOs.BasicMaintenance;

namespace API._Services.Interfaces.BasicMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_2_1_4_CodeMaintenance
    {
        // Danh sách Type Seq
        Task<List<KeyValuePair<string, string>>> GetTypeSeqs();

        // Danh sách Code maintenance
        Task<PaginationUtility<HRMS_Basic_CodeDto>> GetDataPagination(PaginationParam param, CodeMaintenanceParam filter);
        Task<List<HRMS_Basic_CodeDto>> QueryData(CodeMaintenanceParam filter);
        Task<OperationResult> Create(HRMS_Basic_CodeDto model);
        Task<OperationResult> Update(HRMS_Basic_CodeDto model);
        Task<OperationResult> Delete(string typeSeq, string code);
        Task<OperationResult> ExportExcel(CodeMaintenanceParam param);

    }
}