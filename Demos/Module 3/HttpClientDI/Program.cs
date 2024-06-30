using System.Net.Http.Headers;
using System.Net;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Polly.Extensions.Http;
using Polly;

namespace HttpClientDI;

internal class Program
{
    static void Main(string[] args)
    {
        DIClient();
        StrongClient();
        AuthicateClient();
        AdvancedClient();
    }

    private static void DIClient()
    {
        var builder = Host.CreateApplicationBuilder();
        builder.Services.AddHttpClient("weather", opts =>
        {
            opts.BaseAddress = new Uri("https://localhost:8001/");
        });
        var host = builder.Build();
        
        var clientFactory = host.Services.GetRequiredService<IHttpClientFactory>();
        var client = clientFactory.CreateClient("weather");
        var response = client.GetAsync("WeatherForecast").Result;
        if (response.IsSuccessStatusCode)
        {
            var strData = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(strData);
        }
    }
    private static void StrongClient()
    {
        var builder = Host.CreateApplicationBuilder();
        builder.Services.AddHttpClient<WeatherForecastService>(opts =>
        {
            opts.BaseAddress = new Uri("https://localhost:8001/");
        });
        var host = builder.Build();
        var service = host.Services.GetRequiredService<WeatherForecastService>();
        var result = service?.GetWeather();
        if (result != null)
            foreach (var item in result)
            {
                Console.WriteLine($"{item.Date}, {item.TemperatureC}, {item.Summary}");
            }
    }
    
    private static void AuthicateClient()
    {
        var builder = Host.CreateApplicationBuilder();

        builder.Services.AddHttpClient("weather", opts =>
        {
            opts.BaseAddress = new Uri("https://localhost:8001/");
            opts.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", "token");
        })
        .ConfigurePrimaryHttpMessageHandler(() =>
        {
            return new HttpClientHandler
            {
                Credentials = new NetworkCredential("user", "pass", "domain"),
                UseCookies = true
            };
        });

        var host = builder.Build();

        var clientFactory = host.Services.GetRequiredService<IHttpClientFactory>();
        var client = clientFactory.CreateClient("weather");
        var response = client.GetAsync("WeatherForecast").Result;
        if (response.IsSuccessStatusCode)
        {
            var strData = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(strData);
        }
    }
    private static void AdvancedClient()
    {
        var builder = Host.CreateApplicationBuilder();
        builder.Services.AddHttpClient("weather", opts =>
        {
            opts.BaseAddress = new Uri("https://localhost:8001/");
        })
            .SetHandlerLifetime(TimeSpan.FromMinutes(5)) // Default is 10 minutes
            .AddPolicyHandler(msg =>
            {
                // Retry mechanisms
                // From Microsoft.Extensions.Http.Polly
                return HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .OrResult(m => m.StatusCode == HttpStatusCode.NotFound)
                    .WaitAndRetryAsync(3, retAttempt => TimeSpan.FromSeconds(5));
            });
        var host = builder.Build();

        var clientFactory = host.Services.GetRequiredService<IHttpClientFactory>();
        var client = clientFactory.CreateClient("weather");
        var response = client.GetAsync("WeatherForecast").Result;
        if (response.IsSuccessStatusCode)
        {
            var strData = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(strData);
        }
    }

}
