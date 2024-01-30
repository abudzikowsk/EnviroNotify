using EnviroNotify.Dashboard.Database.Repositories.Interfaces;
using EnviroNotify.Dashboard.Services.Interfaces;
using EnviroNotify.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace EnviroNotify.Dashboard.Controllers;

[ApiController]
[Route("[controller]")]
public class HumidityController(
    IWebNotificationService webNotificationService, 
    IEnvironmentDataRepository environmentDataRepository) : ControllerBase
{
    [Route("[action]")]
    [HttpPost]
    public async Task<IActionResult> AcceptDataAndNotify(EnvironmentDataModel environmentDataModel)
    {
        var utcTime = DateTime.UtcNow;
        var warsawZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
        var warsawTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, warsawZone);
        await environmentDataRepository.AddData(environmentDataModel.Humidity, environmentDataModel.Temperature, warsawTime);
        if (environmentDataModel.Humidity >= 70)
        {
            await webNotificationService.SendNotificationAsync();
        }
        return Ok();
    }
}