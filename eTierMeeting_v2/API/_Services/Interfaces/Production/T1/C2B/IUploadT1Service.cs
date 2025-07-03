using System.Collections.Generic;
using System.Threading.Tasks;
using eTierV2_API.DTO.UploadT1;
using eTierV2_API.Helpers.Params;
using eTierV2_API.Models;

namespace eTierV2_API._Services.Interfaces.Production.T1.C2B
{
    public interface IUploadT1Service
    {
        Task<List<string>> GetListVideoKind();
        Task<PagedList<TMVideoDto>> Search(PaginationParam param, ProductVideoT1Param filterParam);
        Task<bool> DeleteProductVideoT1(TMVideoDto model);
        Task<bool> UploadVideo(UploadVideoT1Dto data,string insertBy);
        Task<PagedList<eTM_Video>> SearchOfBatch(PaginationParam param, BatchDeleteParam filterParam, string user);
        Task<bool> DeleteAllBySearch(List<eTM_Video> data);

        Task<List<string>> GetCenters();
        Task<List<string>> GetTiers();
        Task<List<string>> GetSections();
        Task<List<KeyValuePair<string,string>>> GetUnits();
        Task<List<KeyValuePair<string,string>>> GetUnits(string center, string tier, string section);
    }
}