using AmberWeatherDashboard.Server.Filters;
using EnviroNotify.Dashboard.Database.Repositories;
using EnviroNotify.Dashboard.Database.Repositories.Interfaces;
using EnviroNotify.Dashboard.Jobs;
using EnviroNotify.Dashboard.Options;
using EnviroNotify.Dashboard.Services;
using EnviroNotify.Dashboard.Services.Interfaces;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.Configure<DatabaseSettingsOptions>(builder.Configuration.GetSection("Database"));
var databaseSettingsOptions = builder.Configuration.GetSection("Database").Get<DatabaseSettingsOptions>();
var mongoUrlBuilder = new MongoUrlBuilder($"{databaseSettingsOptions.ConnectionString}/jobs");
var mongoClient = new MongoClient(mongoUrlBuilder.ToMongoUrl());
builder.Services.AddHangfire(a => a.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseMongoStorage(mongoClient, mongoUrlBuilder.DatabaseName, new MongoStorageOptions
    {
        MigrationOptions = new MongoMigrationOptions
        {
            MigrationStrategy = new MigrateMongoMigrationStrategy(), 
            BackupStrategy = new CollectionMongoBackupStrategy()
        }, 
        Prefix = "hangfire.mongo",
        CheckConnection = true,
        CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.TailNotificationsCollection
    }));
builder.Services.AddHangfireServer();
builder.Services.AddScoped<IPersistedClientRepository, PersistedClientRepository>();
builder.Services.AddScoped<IEnvironmentDataRepository, EnvironmentDataRepository>();
builder.Services.AddScoped<DeleteOldEnvironmentDataJob>();
builder.Services.AddScoped<IWebNotificationService, WebNotificationService>();
builder.Services.AddSwaggerGen();
builder.Services.Configure<VapidOptions>(builder.Configuration.GetSection("VAPID"));
var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new HangfireAuthorizationFilter() },
    IsReadOnlyFunc = (DashboardContext context) => true
});
app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseDeleteOldWeatherDataJob();

app.Run();
 