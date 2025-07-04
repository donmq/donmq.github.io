using System.Collections.Generic;
using System.Threading.Tasks;
using eTierV2_API.DTO.Production.T6.EfficiencyKanban;

namespace eTierV2_API._Services.Interfaces.Production.T6
{
    public interface IEfficiencyKanbanT6Service
    {
        Task<List<DataGrouped>> GetDataChart(EffiencyKanbanParam param);
        Task<List<FactoryInfo>> GetListFactory();
        Task<List<string>> GetListBrand();
    }
}