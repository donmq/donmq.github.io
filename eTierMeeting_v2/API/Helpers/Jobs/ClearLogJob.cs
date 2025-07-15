using System.Globalization;
using Quartz;

namespace Machine_API.Helpers.Jobs
{
    // Job work at 19:30:00pm, on the 1st day, every month
    // Delete logs for the first 15 days of the previous month
    public class ClearLogJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            List<string> folders = new() {
                "CreateDataFromSAP\\Work",
                "CreateDataFromSAP\\Error",
                "CreateDataFromSAP\\Success",
                "UpdateLocation\\Work",
                "UpdateLocation\\Error",
                "UpdateLocation\\Success",
                "Works"
            };
            folders.ForEach(folder => DeleteLogs(folder));
            await Task.CompletedTask;
        }

        private static void DeleteLogs(string folder)
        {
            string _webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Logs", folder);
            if (Directory.Exists(_webRootPath))
            {
                DirectoryInfo di = new(_webRootPath);

                if (di != null)
                {
                    DateTime date = DateTime.Now.AddDays(-15);
                    FileInfo[] files = di.GetFiles().Where(f => DateTime.ParseExact(f.Name.Split("_")[1].Replace(".log", ""), "yyyy-MM-dd", CultureInfo.InvariantCulture) < date).ToArray();

                    if (files != null)
                    {
                        foreach (FileInfo file in files)
                        {
                            file.Delete();
                        }
                    }
                }
            }
        }
    }
}