using Machine_API.DTO;
using Machine_API.Helpers.Attributes;
using Machine_API.Helpers.Params;
using Machine_API.Helpers.Utilities;

namespace Machine_API._Service.interfaces
{
    [DependencyInjectionAttribute(ServiceLifetime.Scoped)]
    public interface IPDCService
    {
        Task<List<PdcDto>> GetAllPDC();
        Task<PageListUtility<PdcDto>> GetListAllPDC(PaginationParams paginationParams);
        Task<PageListUtility<PdcDto>> SearchPDC(PaginationParams paginationParams, string keyword);
        Task<OperationResult> AddPDC(PdcDto model_Dto, string lang);
        Task<OperationResult> UpdatePDC(PdcDto model_Dto, string lang);
        Task<OperationResult> RemovePDC(PdcDto model_Dto, string lang);
    }
}