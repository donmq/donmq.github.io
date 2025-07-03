using System.Collections.Generic;
using System.Threading.Tasks;
using eTierV2_API.DTO;

namespace eTierV2_API._Services.Interfaces.Production.T1.UPF
{
    public interface IProductionT1UPFQualityService
    {
        Task<QualityDTO> GetData(string deptId);
        Task<List<DefectTop3DTO>> GetDefectTop3(string deptId);
    }
}