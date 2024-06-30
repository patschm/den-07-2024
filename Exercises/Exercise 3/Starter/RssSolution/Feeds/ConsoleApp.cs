using Microsoft.Extensions.Hosting;

namespace Feeds;

public class ConsoleApp : IHostedService
{
    public ConsoleApp()
    {
        
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // TODO 1: Make a call to nu.nl/rss and process the results. Display the following data on screen
        // - title
        // - description
        // - category
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
