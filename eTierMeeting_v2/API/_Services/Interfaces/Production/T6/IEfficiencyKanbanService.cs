using System.Collections.Generic;
using System.Threading.Tasks;
using eTierV2_API.DTO.Production.T5.EfficiencyKanban;

namespace eTierV2_API._Services.Interfaces.Production.T5
{
    public interface IEfficiencyKanbanService
    {
        Task<List<DataGrouped>> GetDataChart(EffiencyKanbanParam param);
        Task<List<FactoryInfo>> GetListFactory();
    }
}