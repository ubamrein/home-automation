using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HomeAutomation.Models;

namespace HomeAutomation.Controllers
{
    public class HomeController : Controller
    {
        public HomeModel MyHome {get;set;} = new HomeModel(){ 
            Controllers = new List<ControllerModel>(){
            new ControllerModel(){
                Name = "Living Room",
                BaseUrl = "http://192.168.179.42:11000",
                Actions = new Dictionary<string, string>() {
                    {"GetServices", "/Services"},
                    {"Play","/Play"},
                    {"PlayNew", "/Play?service=::service::&url=::url::"},
                    {"Pause", "/Pause"},
                    {"Skip", "/Skip"},
                    {"Back", "/Back"},
                    {"Volume", "/Volume?level=::volume::"}
                },
                QuickActions = new List<string>() {
                    "Play",
                    "Pause",
                    "Skip",
                    "Back",
                    "Volume"
                },
                ReadOut = "/Status"
            },
            new ControllerModel(){
                Name = "Kitchen",
                BaseUrl = "http://192.168.179.66:11000",
                Actions = new Dictionary<string, string>() {
                    {"GetServices", "/Services"},
                    {"Play","/Play"},
                    {"PlayNew", "/Play?service=::service::&url=::url::"},
                    {"Pause", "/Pause"},
                    {"Skip", "/Skip"},
                    {"Back", "/Back"},
                    {"Volume", "/Volume?level=::volume::&tell_slaves=::no::"}
                },
                QuickActions = new List<string>() {
                    "Play",
                    "Pause",
                    "Skip",
                    "Back",
                    "Volume"
                },
                ReadOut = "/Status"
            },

        }
        };
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            foreach(var controller in MyHome.Controllers) {
                try {
                    await controller.LoadServices();
                }
                catch(Exception ex) {}
            }
            return View(MyHome);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
