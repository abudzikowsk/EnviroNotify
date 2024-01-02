using EnviroNotify.Dashboard.Database.Repositories.Interfaces;
using EnviroNotify.Dashboard.Options;
using EnviroNotify.Dashboard.Services.Interfaces;
using Microsoft.Extensions.Options;
using WebPush;

namespace EnviroNotify.Dashboard.Services;

public class WebNotificationService(IPersistedClientRepository persistedClientRepository,IOptions<VapidOptions> vapidOptions) : IWebNotificationService
{
    public async Task SendNotificationAsync()
    {
        var subject = vapidOptions.Value.Subject;
        var publicKey = vapidOptions.Value.PublicKey;
        var privateKey = vapidOptions.Value.PrivateKey;

        var vapidDetails = new VapidDetails(subject, publicKey, privateKey);
        var webPushClient = new WebPushClient();

        var clients = await persistedClientRepository.GetAllClients();
        
        var tasks = new List<Task>();
        foreach (var client in clients)
        {
            var subscription = new PushSubscription(client.Endpoint, client.P256DH, client.Auth);
            tasks.Add(webPushClient.SendNotificationAsync(subscription, "Wilgotość powietrza w pomieszczeniu przekroczyła 70%.", vapidDetails));
        }
        
        await Task.WhenAll(tasks);
    }
}