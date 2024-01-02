using EnviroNotify.Dashboard.Database.Entities;
using EnviroNotify.Dashboard.Database.Repositories.Interfaces;
using EnviroNotify.Dashboard.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EnviroNotify.Dashboard.Database.Repositories;

public class EnvironmentDataRepository : IEnvironmentDataRepository
{
    private const string CollectionName = "EnvironmentData";
    private readonly IMongoCollection<EnvironmentData> environmentDataCollection;
    
    public EnvironmentDataRepository(IOptions<DatabaseSettingsOptions> options)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(options.Value.DatabaseName);
        environmentDataCollection = mongoDatabase.GetCollection<EnvironmentData>(CollectionName);
    }
    
    public async Task<List<EnvironmentData>> GetDataByIdAsync(string id)
    {
        return await environmentDataCollection.Find(x => x.Id == id).ToListAsync();
    }
    
    public async Task<List<EnvironmentData>> GetAllDataAsync()
    {
        return await environmentDataCollection.Find(x => true).ToListAsync();
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
    
    public async Task DeleteOldDataAsync()
    {
        await environmentDataCollection.DeleteManyAsync(x => x.DateTime > DateTime.Now.AddDays(-7));
    }

    public async Task<(DateTime MinTime, DateTime MaxTime)> GetMinMaxTimeAsync()
    {
        var minTime = await environmentDataCollection.Find(x => true).SortBy(x => x.DateTime).Limit(1).FirstOrDefaultAsync();
        var maxTime = await environmentDataCollection.Find(x => true).SortByDescending(x => x.DateTime).Limit(1).FirstOrDefaultAsync();
        
        return (minTime.DateTime, maxTime.DateTime);
    }
}