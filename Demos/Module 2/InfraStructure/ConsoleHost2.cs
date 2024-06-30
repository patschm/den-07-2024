using Microsoft.Extensions.Hosting;

namespace InfraStructure;

public class ConsoleHost2(ICounter counter) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
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
