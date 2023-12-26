using System.Device.I2c;
using Iot.Device.Shtc3;

var settings = new I2cConnectionSettings(1, Shtc3.DefaultI2cAddress);
var device = I2cDevice.Create(settings);

using var sensor = new Shtc3(device);
Console.WriteLine($"SensorId {sensor.Id}");

while (true)
{
    if (sensor.TryGetTemperatureAndHumidity(out var temperature, out var humidity))
    {
        Console.WriteLine($"Temperature {temperature.DegreesCelsius}");
        Console.WriteLine($"Humidity {humidity.Percent}");
    }
    
    sensor.Sleep();
    Thread.Sleep(1000);
}