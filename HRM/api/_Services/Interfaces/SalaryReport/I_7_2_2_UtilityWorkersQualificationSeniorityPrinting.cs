using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.SalaryMaintenance;

namespace API._Services.Interfaces.SalaryReport
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_7_2_2_UtilityWorkersQualificationSeniorityPrinting
    {
        Task<List<KeyValuePair<string, string>>> GetListFactory(string language, string userName);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string language, string factory);
        Task<int> Search(UtilityWorkersQualificationSeniorityPrintingParam param);
        Task<OperationResult> DownloadFileExcel(UtilityWorkersQualificationSeniorityPrintingParam param, string userName);
    }
}