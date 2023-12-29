using EnviroNotify.Dashboard.Database.Repositories;
using EnviroNotify.Dashboard.Models;
using Microsoft.AspNetCore.Mvc;

namespace EnviroNotify.Dashboard.Controllers;

public class HomeController(IConfiguration configuration, PersistedClientRepository persistedClientRepository, EnvironmentDataRepository environmentDataRepository) : Controller
{
    [HttpGet]
    public ActionResult Subscribe()
    {
        ViewBag.ApplicationServerKey = configuration["VAPID:PublicKey"];
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Subscribe(string endpoint, string p256dh, string auth)
    {
        await persistedClientRepository.AddClient(endpoint, p256dh, auth);

        return View();
    }
    
    [HttpGet]
    public async Task<ViewResult> Index()
    {
        var allData = await environmentDataRepository.GetAllDataAsync();
        var viewModel = new List<IndexViewModel>();
        foreach (var data in allData)
        {
            viewModel.Add(new IndexViewModel
            {
                Humidity = data.Humidity,
                Temperature = data.Temperature,
                DateTime = data.DateTime
            });
        }
        return View(viewModel);
    }
}