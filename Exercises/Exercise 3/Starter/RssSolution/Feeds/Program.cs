using Feeds;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var host = Host.CreateApplicationBuilder();
host.Services.AddHostedService<ConsoleApp>();
var app = host.Build();
app.Run();


