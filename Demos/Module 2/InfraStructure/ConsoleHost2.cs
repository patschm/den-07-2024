using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace InfraStructure;

public class ConsoleHost2(ICounter counter, ILogger<ConsoleHost2> logger) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogCritical("Ooops");
        for (int i = 0; i < 5; i++)
        {
            counter.Increment();
            counter.Show();
        }
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
