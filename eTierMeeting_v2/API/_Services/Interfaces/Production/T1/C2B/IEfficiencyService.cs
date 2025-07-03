using System.Collections.Generic;
using System.Threading.Tasks;
using eTierV2_API.DTO;

namespace eTierV2_API._Services.Interfaces.Production.T1.C2B
{
    public interface IEfficiencyService
    {
        Task<EfficiencyDTO> GetData(string deptId);
        Task<List<eTM_MES_PT1_SummaryDTO>> GetDataChart(string deptId);
    }
}