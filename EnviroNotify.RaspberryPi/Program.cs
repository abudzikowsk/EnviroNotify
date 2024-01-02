using System.Device.I2c;
using EnviroNotify.RaspberryPi.Services;
using Iot.Device.Shtc3;

var settings = new I2cConnectionSettings(1, Shtc3.DefaultI2cAddress);
var device = I2cDevice.Create(settings);

using var sensor = new Shtc3(device);
var sendDataService = new SendDataService();

while (true)
{
    if (sensor.TryGetTemperatureAndHumidity(out var temperature, out var humidity))
    {
        await sendDataService.Send(humidity.Percent, temperature.DegreesCelsius);
    }
    
    sensor.Sleep();
    Thread.Sleep(1000 * 60 * 20);
}