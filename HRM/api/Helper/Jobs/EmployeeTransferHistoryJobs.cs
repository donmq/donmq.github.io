using API._Services.Interfaces.EmployeeMaintenance;
using API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Quartz;

namespace API.Helper.Jobs
{
    public class EmployeeTransferHistoryJobs : APIController, IJob
    {
        private readonly I_4_1_17_EmployeeTransferHistory _employeeTransferHistory;

        public EmployeeTransferHistoryJobs(I_4_1_17_EmployeeTransferHistory employeeTransferHistory)
        {
            _employeeTransferHistory = employeeTransferHistory;
        }

        [HttpPut("Execute")]
        public async Task Execute(IJobExecutionContext context)
        {
            // hàm chạy vào lúc 00:10:00 hằng ngày
            await _employeeTransferHistory.CheckEffectiveDate();
            await Task.CompletedTask;
        }
    }
}