using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace InfraStructure;

// IServiceScopeFactory is always a singleton
// while IServiceProvider can vary based on the lifetime of the containing class.
// IServiceScopeFactory is preferred in IHostedService implementations.
public class ConsoleHost(IServiceScopeFactory scopeFactory) : IHostedService
{
    // These statements are replaced by primary constructors (C# 12).
    // Caveat: field is no longer readonly unless you use record (will create readonly Properties!)
    //private readonly IServiceScopeFactory _scopeFactory;  
    //public ConsoleHost(IServiceScopeFactory scopeFactory)
    //{
    //    _scopeFactory = scopeFactory;
    //}

    public Task StartAsync(CancellationToken cancellationToken)
    {
        using (var scope = scopeFactory.CreateScope())
        {
            for (int i = 0; i < 5; i++)
            {
                var counter = scope.ServiceProvider.GetRequiredService<ICounter>();
                counter.Increment();
                counter.Show();
            }
        }
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
