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
        await environmentDataRepository.AddData(environmentDataModel.Humidity, environmentDataModel.Temperature, DateTime.Now);
        if (environmentDataModel.Humidity >= 70)
        {
            await webNotificationService.SendNotificationAsync();
        }
        return Ok();
    }
}