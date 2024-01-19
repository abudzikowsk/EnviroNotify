using System.Text;
using EnviroNotify.Shared.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace EnviroNotify.RaspberryPi.Services;

public class SendDataService
{
    private const string Url = "http://192.168.50.3:8383/Humidity/AcceptDataAndNotify";
    private HttpClient _httpClient = new();

    public async Task Send(int humidity, double temperature)
    {
        var data = new EnvironmentDataModel
        {
            Humidity = humidity,
            Temperature = temperature
        };
        var json = JsonSerializer.Serialize(data);

        try
        {
            var response = await _httpClient.PostAsync(Url, new StringContent(json, Encoding.UTF8, "application/json"));
            Console.WriteLine($"Response Status: {response.StatusCode}");
            Console.WriteLine("Data has been sent.");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error while sending data.");
        }
    }
}