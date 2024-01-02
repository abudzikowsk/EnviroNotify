using EnviroNotify.Dashboard.Database.Entities;

namespace EnviroNotify.Dashboard.Database.Repositories.Interfaces;

public interface IEnvironmentDataRepository
{
    Task<List<EnvironmentData>> GetDataByIdAsync(string id);

    Task<List<EnvironmentData>> GetAllDataAsync();

    Task AddData(double humidity, double temperature, DateTime dateTime);

    Task DeleteOldDataAsync();
    
    Task<(DateTime MinTime, DateTime MaxTime)> GetMinMaxTimeAsync();
}