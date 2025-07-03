using System.Threading.Tasks;
using eTierV2_API.Helpers.Utilities;
using eTierV2_API.Models;
using Microsoft.AspNetCore.Http;

namespace eTierV2_API._Services.Interfaces.Production.T1.C2B
{
    public interface IT5ExternalUploadService
    {
        Task<PaginationUtility<eTM_HP_Efficiency_Data_External>> GetData(PaginationParam pagination);
        Task<OperationResult> UploadExcel(IFormFile file);
    }
}