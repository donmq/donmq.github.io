using System.Collections.Generic;
using System.Threading.Tasks;
using eTierV2_API.DTO;

namespace eTierV2_API._Services.Interfaces.Production.T1.UPF
{
    public interface IProductionT1UPFKaizenService
    {
        Task<List<eTM_VideoDTO>> GetListVideo(string deptId);
    }
}