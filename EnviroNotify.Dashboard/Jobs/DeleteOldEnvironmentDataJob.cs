using EnviroNotify.Dashboard.Database.Repositories.Interfaces;
using EnviroNotify.Dashboard.Jobs.Interfaces;

namespace EnviroNotify.Dashboard.Jobs;

public class DeleteOldEnvironmentDataJob : IJob
{
    private readonly IEnvironmentDataRepository environmentDataRepository;

    public DeleteOldEnvironmentDataJob(IEnvironmentDataRepository environmentDataRepository)
    {
        this.environmentDataRepository = environmentDataRepository;
    }

    public async Task Run()
    {
        await environmentDataRepository.DeleteOldDataAsync();
    }
}