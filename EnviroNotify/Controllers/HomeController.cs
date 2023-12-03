using EnviroNotify.Repositories;
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
    public async Task<ActionResult> Index(string clientId, string endpoint, string p256dh, string auth)
    {
        if (await persistedClientRepository.ClientExists(clientId))
        {
            return BadRequest("Client already exists.");
        }

        await persistedClientRepository.AddClient(clientId, endpoint, p256dh, auth);

        return View();
    }
}