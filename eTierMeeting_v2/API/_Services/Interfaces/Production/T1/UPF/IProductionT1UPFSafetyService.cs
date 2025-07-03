using System.Collections.Generic;
using System.Threading.Tasks;
using eTierV2_API.DTO;
using eTierV2_API.Models;

namespace eTierV2_API._Services.Interfaces.Production.T1.UPF
{
    public interface IProductionT1UPFSafetyService
    {
        Task<List<eTM_VideoDTO>> GetTodayData(string deptId);
    }
}