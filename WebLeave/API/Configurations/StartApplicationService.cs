using API.Helpers.Enums;
using API.Helpers.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace API.Configurations
{
    public class StartApplicationService : BackgroundService
    {
        private readonly IHubContext<HostApplicationLifetimeHub> _hubContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public StartApplicationService(IHubContext<HostApplicationLifetimeHub> hubContext, IWebHostEnvironment webHostEnvironment)
        {
            _hubContext = hubContext;
            _webHostEnvironment = webHostEnvironment;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            if (_webHostEnvironment.IsProduction())
            {
            await Task.Delay(10000, stoppingToken);
            await _hubContext.Clients.All.SendAsync(CommonConstants.SR_START_APPLICATION, cancellationToken: stoppingToken);
            }
        }
    }
}