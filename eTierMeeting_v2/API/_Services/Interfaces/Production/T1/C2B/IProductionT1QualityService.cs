using System.Collections.Generic;
using System.Threading.Tasks;
using eTierV2_API.DTO;

namespace eTierV2_API._Services.Interfaces.Production.T1.C2B
{
    public interface IProductionT1QualityService
    {
        Task<QualityDTO> GetData(string deptId);

        Task<List<DefectTop3DTO>> GetDefectTop3(string deptId);

        Task<List<FRI_BA_DefectDTO>> GetBADefectTop3Chart(string deptId);
    }
}