using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.SalaryMaintenance;

namespace API._Services.Interfaces.SalaryMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_7_2_1_SalaryApprovalForm
    {
        Task<List<KeyValuePair<string, string>>> GetListFactory(string language, string userName);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string language, string factory);
        Task<List<KeyValuePair<string, string>>> GetListPermissionGroup(string factory, string language);
        Task<List<KeyValuePair<string, string>>> GetPositionTitles(string language);
        Task<int> Search(D_7_2_1_SalaryApprovalForm_Param param);
        Task<OperationResult> ExportPDF(D_7_2_1_SalaryApprovalForm_Param param, string userName);
    }
}