using EnviroNotify.Database.Repositories;
using EnviroNotify.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.Configure<DatabaseSettingsOptions>(builder.Configuration.GetSection("Database"));
builder.Services.AddScoped<PersistedClientRepository>();
var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
 