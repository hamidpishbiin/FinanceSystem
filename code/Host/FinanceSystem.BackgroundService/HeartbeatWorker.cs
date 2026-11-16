using Shared.Core;

namespace FinanceSystem.BackgroundService;

public class HeartbeatWorker(ILoggerService logger) : Microsoft.Extensions.Hosting.BackgroundService
{
    override protected async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(5));

        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                logger.Information("App is running...");
            }
        }
        catch (OperationCanceledException)
        {
            logger.Information("Heartbeat stopping...");
            throw;
        }
    }
}
