using EnviroNotify.Dashboard.Database.Repositories;
using EnviroNotify.Dashboard.Database.Repositories.Interfaces;
using EnviroNotify.Dashboard.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.Configure<DatabaseSettingsOptions>(builder.Configuration.GetSection("Database"));
builder.Services.AddScoped<IPersistedClientRepository, PersistedClientRepository>();
builder.Services.AddScoped<IEnvironmentDataRepository, EnvironmentDataRepository>();
builder.Services.Configure<VapidOptions>(builder.Configuration.GetSection("VAPID"));
var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
 