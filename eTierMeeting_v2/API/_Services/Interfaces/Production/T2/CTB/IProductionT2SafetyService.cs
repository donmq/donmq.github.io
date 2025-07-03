using System.Collections.Generic;
using System.Threading.Tasks;
using eTierV2_API.DTO.Production.T2.CTB;

namespace eTierV2_API._Services.Interfaces.Production.T2.CTB
{
    public interface IProductionT2SafetyService
    {
        Task<SefetyViewModel> GetData(string building);
        Task<List<eTM_HSE_Score_ImageDTO>> GetDetailScoreUnPass(int hseScoreID);
    }
}