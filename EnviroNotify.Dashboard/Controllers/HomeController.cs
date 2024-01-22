using EnviroNotify.Dashboard.Database.Repositories.Interfaces;
using EnviroNotify.Dashboard.Models;
using EnviroNotify.Dashboard.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EnviroNotify.Dashboard.Controllers;

public class HomeController(
    IPersistedClientRepository persistedClientRepository, 
    IEnvironmentDataRepository environmentDataRepository, 
    IOptions<VapidOptions> vapidOptions) : Controller
{
    [HttpGet]
    public ActionResult Subscribe()
    {
        ViewBag.ApplicationServerKey = vapidOptions.Value.PublicKey;
        return View();
    }
    
    [HttpPost]
    public async Task<ActionResult> Subscribe(string endpoint, string p256dh, string auth)
    {
        await persistedClientRepository.AddClient(endpoint, p256dh, auth);

        return RedirectToAction("Index", "Home");
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
                DateTime = data.DateTime.ToString("dd/MM HH:mm")
            });
        }
        return View(viewModel);
    }
}