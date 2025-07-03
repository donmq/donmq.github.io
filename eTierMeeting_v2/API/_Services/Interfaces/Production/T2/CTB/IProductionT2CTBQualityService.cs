using System.Collections.Generic;
using System.Threading.Tasks;
using eTierV2_API.DTO;

namespace eTierV2_API._Services.Interfaces.Production.T2.C2B
{
    public interface IProductionT2CTBQualityService
    {
       Task<T2CTBQualityDTO> GetData(string tuCode, bool switchDate);

        Task<List<DefectTop3DTO>> GetDefectTop3Photos(string tuCode, bool switchDate);

        Task<List<FRI_BA_DefectDTO>> GetBADefectTop3Chart(string deptId, bool switchDate);
    }
}