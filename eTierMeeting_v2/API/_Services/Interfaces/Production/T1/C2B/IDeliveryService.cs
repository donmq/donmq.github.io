using System.Collections.Generic;
using System.Threading.Tasks;
using eTierV2_API.DTO;

namespace eTierV2_API._Services.Interfaces.Production.T1.C2B
{
    public interface IDeliveryService
    {
        Task<List<DeliveryDTO>> GetAllDelivery(string deptId);

    }
}