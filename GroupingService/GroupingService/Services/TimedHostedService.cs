using GroupingService.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

internal class TimedHostedService : IHostedService, IDisposable
{
    private readonly ILogger _logger;
    private readonly IMatchCreatorService _matchCreatorService;
    private readonly IHostingEnvironment _hostingEnvironment;
    private Timer _timer;

    public TimedHostedService(
        ILogger<TimedHostedService> logger,
        IMatchCreatorService matchCreatorService,
        IHostingEnvironment hostingEnvironment)
    {
        _logger = logger;
        _matchCreatorService = matchCreatorService;
        _hostingEnvironment = hostingEnvironment;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Timed Background Service is starting.");

        // Repeat task every N seconds
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(3));

        return Task.CompletedTask;
    }

    private void DoWork(object state)
    {
        _logger.LogInformation("Timed Background Service is working.");

        _matchCreatorService.AssignTopPriorityPlayers();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Timed Background Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}