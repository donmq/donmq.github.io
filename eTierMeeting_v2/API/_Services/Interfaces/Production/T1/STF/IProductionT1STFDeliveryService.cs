using System.Collections.Generic;
using System.Threading.Tasks;
using eTierV2_API.DTO;

namespace eTierV2_API._Services.Interfaces.Production.T1.STF
{
    public interface IProductionT1STFDeliveryService
    {
        Task<List<VW_Production_T1_STF_Delivery_RecordDTO>> GetData(string deptId);
    }
}