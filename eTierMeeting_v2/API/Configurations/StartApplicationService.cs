
using API.Helpers.Enums;
using API.Helpers.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace API.Configurations
{
    public class StartApplicationService : BackgroundService
    {
        private readonly IHubContext<SignalRHub> _hubContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public StartApplicationService(IHubContext<SignalRHub> hubContext, IWebHostEnvironment webHostEnvironment)
        {
            _hubContext = hubContext;
            _webHostEnvironment = webHostEnvironment;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (_webHostEnvironment.IsProduction())
            {
                await Task.Delay(3000, stoppingToken);
                await _hubContext.Clients.All.SendAsync(SignalRConstants.START_APPLICATION, cancellationToken: stoppingToken);
            }
        }
    }
}