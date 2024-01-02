using System.Text;
using EnviroNotify.Shared.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace EnviroNotify.RaspberryPi.Services;

public class SendDataService
{
    private const string Url = "http://192.168.53:8383/Humidity/AcceptDataAndNotify";
    private HttpClient _httpClient = new HttpClient();
    
    public void Send(double humidity, double temperature)
    {
        Console.WriteLine("Initialization of request.");
        var data = new EnvironmentDataModel
        {
            Humidity = humidity,
            Temperature = temperature
        };

        var json = JsonSerializer.Serialize(data);
        var httpRequest = new HttpRequestMessage
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json"),
            Method = HttpMethod.Post,
            RequestUri = new Uri(Url)
        };
        
        Console.WriteLine("Initialization of request finished.");
        Task.Run(() => _httpClient.SendAsync(httpRequest));
        Console.WriteLine("Data has been sent.");
    }
}