using Feeds;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateApplicationBuilder();
    host.Services.AddHttpClient("nu", client =>
        {
            client.BaseAddress = new Uri("https://nu.nl/");
        }).SetHandlerLifetime(TimeSpan.FromMinutes(10));
        host.Services.AddTransient<IProcessStreamStrategy, XmlSerializerStrategy>();
        //host.Services.AddTransient<IProcessStreamStrategy, RegexpStrategy>();
        //host.Services.AddTransient<IProcessStreamStrategy, LinqToXmlStrategy>();
        host.Services.AddTransient<IFeedReader, FeedReader>();
        host.Services.AddHostedService<ConsoleApp>();
var app = host.Build();
 app.Run();
        
