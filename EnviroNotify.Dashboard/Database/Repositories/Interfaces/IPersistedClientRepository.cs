using EnviroNotify.Dashboard.Database.Entities;

namespace EnviroNotify.Dashboard.Database.Repositories.Interfaces;

public interface IPersistedClientRepository
{
    Task AddClient(string endpoint, string p256dh, string auth);

    Task<List<PersistedClient>> GetAllClients();
}