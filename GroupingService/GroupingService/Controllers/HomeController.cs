using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GroupingService.Models;
using GroupingService.Services;

namespace GroupingService.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMatchCreatorService _matchCreatorService;

        public HomeController(IMatchCreatorService matchCreatorService)
        {
            _matchCreatorService = matchCreatorService;
        }

        public IActionResult Index()
        {
            GroupingModel model = new GroupingModel();
            model.NPlayersPerGroup = 2; // Default
            return View(model: model);
        }

        [HttpPost]
        public IActionResult Index(GroupingModel model)
        {
            _matchCreatorService.SetPlayersPerMatch(model.NPlayersPerGroup);
            return View("AddPlayers", model: model);
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
