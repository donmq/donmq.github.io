using Aspose.Cells;
using Machine_API.DTO;
using Machine_API.Helpers.Attributes;
using Machine_API.Helpers.Params;
using Machine_API.Helpers.Utilities;

namespace Machine_API._Service.interfaces
{
    [DependencyInjectionAttribute(ServiceLifetime.Scoped)]
    public interface IPreliminaryPlnoService
    {

        Task<PageListUtility<PreliminaryPlnoDTO>> GetAllPreliminaryPlno(PaginationParams paginationParams, string search);
        Task<List<PreliminaryPlnoExportDTO>> ExportExcel(string search);
        Task<List<PreliminaryPlnoExportAllDTO>> ExportPreminaryInLocationBuildingExcel();
        Task<List<PreliminaryPlnoExportAllDTO>> ExportPreminaryOtherLocationBuildingExcel();

        Task<OperationResult> AddPreliminaryPlno(PreliminaryPlnoAddDTO preliminaryPlno, string lang);
        Task<OperationResult> UpdatePreliminaryPlno(PreliminaryPlnoAddDTO preliminaryPlno, string lang);
        Task<OperationResult> RemovePreliminaryPlno(string empNumber, string lang);

        Task<PreliminaryPlnoDTO> GetPreliminaryPlnos(string empNumber);
        void CustomStyle(ref Cell cellCustom);
        void PutStaticValue(ref Worksheet ws);
        void PutStaticAllInBuildingValue(WorkbookDesigner designer, ref Worksheet wsheet, List<PreliminaryPlnoExportAllDTO> resultAll);
        void PutStaticAllOtherBuildingValue(WorkbookDesigner designer, ref Worksheet wsheet, List<PreliminaryPlnoExportAllDTO> resultAll);
    }
}