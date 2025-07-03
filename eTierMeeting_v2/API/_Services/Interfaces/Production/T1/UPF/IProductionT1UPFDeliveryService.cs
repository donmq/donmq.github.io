using System.Collections.Generic;
using System.Threading.Tasks;
using eTierV2_API.DTO;

namespace eTierV2_API._Services.Interfaces.Production.T1.UPF
{
    public interface IProductionT1UPFDeliveryService
    {
        Task<List<VW_Production_T1_UPF_Delivery_RecordDTO>> GetData(string deptId);
    }
}