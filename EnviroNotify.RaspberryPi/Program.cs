using System.Device.I2c;
using EnviroNotify.RaspberryPi.Services;
using Iot.Device.Shtc3;

Console.WriteLine("Raspberry Pi EnviroNotify has started.");

Shtc3 sensor;
SendDataService sendDataService;

Initialize();

while (true)
{
    if (sensor.TryGetTemperatureAndHumidity(out var temperature, out var humidity))
    {
        Console.WriteLine($"Temperature: {temperature.DegreesCelsius:0.#}\u00B0C");
        Console.WriteLine($"Relative humidity: {humidity.Percent:0.#}%");
        await sendDataService.Send((int)Math.Round(humidity.Percent), temperature.DegreesCelsius);
    }
    
    sensor.Sleep();
    Thread.Sleep(1000 * 60 * 20);
}

void Initialize()
{
    Console.WriteLine("Initialization has started.");

    var settings = new I2cConnectionSettings(1, Shtc3.DefaultI2cAddress);
    var device = I2cDevice.Create(settings);

    sensor = new Shtc3(device);
    Console.WriteLine($"SensorId: {sensor.Id}");
    sendDataService = new SendDataService();
    
    Console.WriteLine("Initialization has finished.");
}