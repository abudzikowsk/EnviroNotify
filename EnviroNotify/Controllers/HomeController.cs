using EnviroNotify.Database.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EnviroNotify.Controllers;

public class HomeController(IConfiguration configuration, PersistedClientRepository persistedClientRepository) : Controller
{
    [HttpGet]
    public ActionResult Index()
    {
        ViewBag.ApplicationServerKey = configuration["VAPID:PublicKey"];
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Index(string endpoint, string p256dh, string auth)
    {
        await persistedClientRepository.AddClient(endpoint, p256dh, auth);

        return View();
    }
}