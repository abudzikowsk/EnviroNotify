using System.Text;
using EnviroNotify.Shared.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace EnviroNotify.RaspberryPi.Services;

public class SendDataService
{
    private const string Url = "http://192.168.53:8383/Humidity/AcceptDataAndNotify";
    
    public async Task Send(double humidity, double temperature)
    {
        using var client = new HttpClient();
        var data = new EnvironmentDataModel
        {
            Humidity = humidity,
            Temperature = temperature
        };

        var json = JsonSerializer.Serialize(data);
        await client.PostAsync(Url, new StringContent(json, Encoding.UTF8, "application/json"));
    }
}