using EnviroNotify.Dashboard.Database.Entities;
using EnviroNotify.Dashboard.Database.Repositories.Interfaces;
using EnviroNotify.Dashboard.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EnviroNotify.Dashboard.Database.Repositories;

public class PersistedClientRepository : IPersistedClientRepository
{
    private const string CollectionName = "PersistedClients";
    private readonly IMongoCollection<PersistedClient> persistedClientsCollection;
    public PersistedClientRepository(IOptions<DatabaseSettingsOptions> options)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(options.Value.DatabaseName);
        persistedClientsCollection = mongoDatabase.GetCollection<PersistedClient>(CollectionName);
    }
    
    public async Task AddClient(string endpoint, string p256dh, string auth)
    {
        var persistedClient = new PersistedClient
        {
            Endpoint = endpoint,
            P256DH = p256dh,
            Auth = auth
        };
        
        await persistedClientsCollection.InsertOneAsync(persistedClient);
    }

    public async Task<List<PersistedClient>> GetAllClients()
    {
        return await persistedClientsCollection.Find(x => true).ToListAsync();
    }
}