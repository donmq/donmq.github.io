
using API._Services.Interfaces.SalaryReport;

namespace API.Controllers.SalaryReport
{
    public class C_7_2_14_TaxPayingEmployeeMonthlyNightShiftExtraPayAndOvertimePay : APIController
    {
        private readonly I_7_2_14_TaxPayingEmployeeMonthlyNightShiftExtraPayAndOvertimePay _service;

        public C_7_2_14_TaxPayingEmployeeMonthlyNightShiftExtraPayAndOvertimePay(I_7_2_14_TaxPayingEmployeeMonthlyNightShiftExtraPayAndOvertimePay service)
        {
            _service = service;
        }
    }
}