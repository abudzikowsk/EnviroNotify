using EnviroNotify.Dashboard.Database.Repositories;
using Microsoft.AspNetCore.Mvc;
using WebPush;

namespace EnviroNotify.Dashboard.Controllers;

[ApiController]
[Route("[controller]")]
public class HumidityController(PersistedClientRepository persistedClientRepository, IConfiguration configuration) : ControllerBase
{
    [Route("[action]")]
    public async Task<IActionResult> TestNotify()
    {
        var subject = configuration["VAPID:Subject"];
        var publicKey = configuration["VAPID:PublicKey"];
        var privateKey = configuration["VAPID:PrivateKey"];

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