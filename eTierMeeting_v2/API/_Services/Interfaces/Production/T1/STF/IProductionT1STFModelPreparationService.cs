using System.Collections.Generic;
using System.Threading.Tasks;
using eTierV2_API.DTO;

namespace eTierV2_API._Services.Interfaces.Production.T1.STF
{
    public interface IProductionT1STFModelPreparationService
    {
        Task<List<eTM_VideoDTO>> GetModelPreparation(string deptId);
    }
}