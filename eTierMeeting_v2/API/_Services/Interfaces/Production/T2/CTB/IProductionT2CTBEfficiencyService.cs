using System.Collections.Generic;
using System.Threading.Tasks;
using eTierV2_API.DTO.Production.T2.C2B;

namespace eTierV2_API._Services.Interfaces.Production.T2.CTB
{
    public interface IProductionT2CTBEfficiencyService
    {
        Task<EfficiencyChart> GetData(string deptId, string param);
    }
}