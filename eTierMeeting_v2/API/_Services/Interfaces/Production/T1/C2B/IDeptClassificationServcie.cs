using System.Threading.Tasks;
using System.Collections.Generic;
using eTierV2_API.DTO;
using eTierV2_API.Models;
using eTierV2_API.Helpers.Params;

namespace eTierV2_API._Services.Interfaces.Production.T1.C2B
{
    public interface IDeptClassificationServcie : IMainService<eTM_Dept_ClassificationDTO>
    {
        Task<bool> CheckDeptClassificationExists(string classkind, string deptid);
        Task<string> ChkDeptIDBeforeInsert(string deptid);
        Task<object> GetDept();
        Task<object> GetDeptInClassification();
        
        Task<ProductionT2SelectLineDTO> getFactoryIndex();
        
        Task<PagedList<eTM_Dept_ClassificationDTO>> SearchDeptClassification(PaginationParam paginationParams, DeptClassificationParam deptClassificationParam);
    }
}