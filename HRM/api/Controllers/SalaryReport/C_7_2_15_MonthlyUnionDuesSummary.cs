
using API._Services.Interfaces.SalaryReport;

namespace API.Controllers.SalaryReport
{
    public class C_7_2_15_MonthlyUnionDuesSummary : APIController
    {
        private readonly I_7_2_15_MonthlyUnionDuesSummary _service;

        public C_7_2_15_MonthlyUnionDuesSummary(I_7_2_15_MonthlyUnionDuesSummary service)
        {
            _service = service;
        }
    }
}