using EnviroNotify.Dashboard.Database.Entities;
using EnviroNotify.Dashboard.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EnviroNotify.Dashboard.Database.Repositories;

public class EnvironmentDataRepository
{
    private const string CollectionName = "EnvironmentData";
    private readonly IMongoCollection<EnvironmentData> environmentDataCollection;
    
    public EnvironmentDataRepository(IOptions<DatabaseSettingsOptions> options)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(options.Value.DatabaseName);
        environmentDataCollection = mongoDatabase.GetCollection<EnvironmentData>(CollectionName);
    }
    
    public async Task<List<EnvironmentData>> GetDataByIdAsync(int id)
    {
        return await environmentDataCollection.Find(x => x.Id == id).ToListAsync();
    }
    
    public async Task AddData(double humidity, double temperature, DateTime dateTime)
    {
        var environmentData = new EnvironmentData
        {
            Humidity = humidity,
            Temperature = temperature,
            DateTime = dateTime
        };
        
        await environmentDataCollection.InsertOneAsync(environmentData);
    }
    
    public async Task DeleteDataAsync(int id)
    {
        var dataToDelete = await environmentDataCollection
            .FindAsync(r => r.Id == id);

        if (dataToDelete == null)
        {
            return;
        }
        
        environmentDataCollection.DeleteOne(r => r.Id == id);
    }
}