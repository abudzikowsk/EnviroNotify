var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles();

app.MapGet("/",  async context => context.Response.Redirect("index.html"));

app.Run();
 