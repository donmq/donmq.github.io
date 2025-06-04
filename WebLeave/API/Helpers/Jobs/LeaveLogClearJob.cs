using API._Services.Interfaces.Common;
using API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Quartz;

namespace API.Helpers.Jobs
{
    [DisallowConcurrentExecution]
    public class LeaveLogClearJob : ApiController, IJob
    {
        private readonly ILeaveCommonService _leaveCommonService;

        public LeaveLogClearJob(ILeaveCommonService leaveCommonService)
        {
            _leaveCommonService = leaveCommonService;
        }

        [HttpDelete("Execute")]
        public async Task Execute(IJobExecutionContext context)
        {
            await _leaveCommonService.LeaveLogClear();
            await Task.CompletedTask;
        }
    }
}