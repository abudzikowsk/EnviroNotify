using EnviroNotify.Dashboard.Database.Repositories.Interfaces;
using EnviroNotify.Dashboard.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebPush;

namespace EnviroNotify.Dashboard.Controllers;

[ApiController]
[Route("[controller]")]
public class HumidityController(
    IPersistedClientRepository persistedClientRepository, 
    IOptions<VapidOptions> vapidOptions) : ControllerBase
{
    [Route("[action]")]
    public async Task<IActionResult> TestNotify()
    {
        var subject = vapidOptions.Value.Subject;
        var publicKey = vapidOptions.Value.PublicKey;
        var privateKey = vapidOptions.Value.PrivateKey;

        var vapidDetails = new VapidDetails(subject, publicKey, privateKey);
        var webPushClient = new WebPushClient();

        var clients = await persistedClientRepository.GetAllClients();
        foreach (var client in clients)
        {
            var subscription = new PushSubscription(client.Endpoint, client.P256DH, client.Auth);
            await webPushClient.SendNotificationAsync(subscription, "test", vapidDetails);
        }

        return Ok();
    }
}