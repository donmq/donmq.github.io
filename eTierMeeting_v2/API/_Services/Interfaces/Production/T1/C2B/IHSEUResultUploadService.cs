using System.Collections.Generic;
using System.Threading.Tasks;
using eTierV2_API.DTO.HSEUpload;
using eTierV2_API.Helpers.Params;
using Microsoft.AspNetCore.Http;

namespace eTierV2_API._Services.Interfaces.Production.T1.C2B
{
    public interface IHSEUResultUploadService
    {
        Task<List<string>> GetBuildings();
        Task<List<string>> GetDeptInBuilding(string building);
        Task<List<string>> GetEvalutionCategory();
        Task<List<DataDownloadTemplate>> GetETMUnits();
        Task<bool> UploadExcel(IFormFile file, string updatedBy);
        Task<PagedList<HSEDataSearchDto>> Search(PaginationParam param, HSESearchParam paramSearch);
        Task<bool> Remove(int score_ID);
        Task<bool> UpdateScoreData(HSEDataSearchDto model, string updatedBy);


        Task<bool> UploadImages(ImageDataUpload data, string updatedBy);
        Task<List<ImageRemark>> GetListImageToHseID(int hseID);
        Task<bool> EditImages(ImageDataUpload data, string updatedBy);
    }
}