using EnviroNotify.Database;
using EnviroNotify.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace EnviroNotify.Repositories;

public class PersistedClientRepository(ApplicationDbContext applicationDbContext)
{
    public async Task AddClient(string clientId, string endpoint, string p256dh, string auth)
    {
        applicationDbContext.PersistedClients.Add(new PersistedClient
        {
            ClientId = clientId, 
            Endpoint = endpoint,
            P256DH = p256dh,
            Auth = auth
        });

        await applicationDbContext.SaveChangesAsync();
    }

    public async Task<bool> ClientExists(string clientId)
    {
        var client = await applicationDbContext.PersistedClients.SingleOrDefaultAsync
            (c => c.ClientId == clientId);

        return client != null;
    }

    public async Task<List<PersistedClient>> GetAllClients()
    {
        return await applicationDbContext.PersistedClients.ToListAsync();
    }
}