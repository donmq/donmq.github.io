using NLog;
using Quartz;

namespace Machine_API.Helpers.Jobs
{
    public class CheckWorkJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Logger logger = LogManager.GetLogger("checkwork");
            logger.Log(NLog.LogLevel.Info, "Job working");
            await Task.CompletedTask;
        }
    }
}