using Microsoft.Extensions.Http.Resilience;
using Newtonsoft.Json;
using Polly;
using Polly.Extensions.Http;
using System.Net;
using System.Net.Http.Headers;

namespace HttpClientApp;

// Used Nuget packages:
// Microsoft.Extensions.Http.Resilience;
// Microsoft.Extensions.Http
// Polly
// Newtonsoft.Json
public class Program
{
    static void Main(string[] args)
    {
        //VeelClients();
        //BasicGetClient();
        //BasicPostClient();
        AdvancedClient();
        //BearerClient();
        //AuthenticateClient();
        //ResilienceClient();
    }

    static HttpClient client = new HttpClient();
    private static void VeelClients()
    {
        client.BaseAddress = new Uri("https://localhost:8001/");
       for(int i = 0; i < 1000; i++)
        {
            var response = client.GetAsync("WeatherForecast").Result;
            Console.WriteLine($"Request {i}");
        }
    }

    private static void BasicGetClient()
    {
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri("https://localhost:8001/");

        var response = client.GetAsync("WeatherForecast").Result;
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine(response.Headers.Server);
            Console.WriteLine(response.Content.Headers.ContentEncoding);
            var strData = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(strData);
        }
    }
    private static void BasicPostClient()
    {
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri("https://localhost:8001/");

        var item = new WeatherForecast { Date = DateTime.Now, Summary = "Mottig", TemperatureC = 31 };
        var content = new StringContent(JsonConvert.SerializeObject(item));
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        var response = client.PostAsync("WeatherForecast", content).Result;
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine(response.Headers.Location);
        }
    }
    private static void AdvancedClient()
    {
        var handler = new SocketsHttpHandler();
        handler.EnableMultipleHttp2Connections = true;
        handler.MaxConnectionsPerServer = 10;
        handler.PooledConnectionLifetime = TimeSpan.FromMinutes(4);

        HttpClient client = new HttpClient(handler);
        client.DefaultRequestVersion = HttpVersion.Version11;
        client.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher;
        client.BaseAddress = new Uri("https://localhost:8001/");


        var response = client.GetAsync("WeatherForecast").Result;
        if (response.IsSuccessStatusCode)
        {
            var strData = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(strData);
        }
    }
    private static void AuthenticateClient()
    {
        var handler = new HttpClientHandler
        {
            Credentials = new NetworkCredential("user", "pass", "domain"),
            UseCookies = true
        };
        var client = new HttpClient(handler);

        var response = client.GetAsync("WeatherForecast").Result;
        if (response.IsSuccessStatusCode)
        {
            var strData = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(strData);
        }
    }
    private static void BearerClient()
    {
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri("https://localhost:8001/");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", "token");

        var response = client.GetAsync("WeatherForecast").Result;
        if (response.IsSuccessStatusCode)
        {
            var strData = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(strData);
        }
    }
    private static void ResilienceClient()
    {
        // Experimental
        var pipeline = new ResiliencePipelineBuilder<HttpResponseMessage>()
            .AddRetry(new HttpRetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                BackoffType = DelayBackoffType.Linear,
               Delay = TimeSpan.FromMinutes(4) 
            }).Build();

        // Experimental
#pragma warning disable
        var handler = new ResilienceHandler(pipeline)
        {
            InnerHandler = new SocketsHttpHandler()
        };

        var client = new HttpClient(handler);
        client.BaseAddress = new Uri("https://localhost:8001/");
        
        var response = client.GetAsync("WeatherForecast").Result;
        if (response.IsSuccessStatusCode)
        {
            var strData = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(strData);
        }

    }
}