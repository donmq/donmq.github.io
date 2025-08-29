using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.SalaryMaintenance;

namespace API._Services.Interfaces.SalaryMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_7_1_26_FinSalaryCloseMaintenance
    {
        Task<List<KeyValuePair<string, string>>> GetListFactory(string userName, string language);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string factory, string language);
        Task<List<KeyValuePair<string, string>>> GetListPermissionGroup(string factory, string language);
        Task<List<string>> GetListTypeHeadEmployeeID(string factory);
        Task<OperationResult> GetDataPagination(PaginationParam pagination, FinSalaryCloseMaintenance_Param param);
        Task<OperationResult> Update(FinSalaryCloseMaintenance_MainData data);
        Task<OperationResult> DownLoadExcel(FinSalaryCloseMaintenance_Param param, string userName);
        Task<OperationResult> BatchUpdateData(BatchUpdateData_Param param, string userName);
    }
}