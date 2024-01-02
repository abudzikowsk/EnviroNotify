using Hangfire;

namespace EnviroNotify.Dashboard.Jobs;

public static class StartupConfiguration
{
    public static void UseDeleteOldWeatherDataJob(this WebApplication webApplication)
    {
        RecurringJob.AddOrUpdate<DeleteOldEnvironmentDataJob>("deleteOldDataJob", d => d.Run(), Cron.Daily);
    }
}