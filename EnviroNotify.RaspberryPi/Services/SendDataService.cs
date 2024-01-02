using System.Text;
using EnviroNotify.Shared.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace EnviroNotify.RaspberryPi.Services;

public class SendDataService
{
    private const string Url = "http://192.168.53:8383/Humidity/AcceptDataAndNotify";
    
    public void Send(double humidity, double temperature)
    {
        using var client = new HttpClient();
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
        
        client.Send(httpRequest);
        Console.WriteLine("Data has been sent.");
    }
}