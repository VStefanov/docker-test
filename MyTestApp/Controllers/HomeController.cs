using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Web.MyTestApp.Models;
using Web.MyTestApp.Services;

namespace Web.MyTestApp.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFibService fibService;

        public HomeController(ILogger<HomeController> logger, IFibService fibService)
        {
            _logger = logger;
            this.fibService = fibService;
        }

        public IActionResult Index(int index)
        {
            IndexViewModel model = null;
            if (index != 0)
            {
                this.fibService.AddIndexToStorage(index.ToString());
                model = new IndexViewModel
                {
                    AvailableIndexes = this.fibService.GetAvailableIndexes(),
                    AvailableIndexesValues = this.fibService.GetAvailableIndexesValues()
                };
            }
            return View(model);
        }

        public IActionResult Refresh()
        {
            return View("Index", new IndexViewModel
            {
                 AvailableIndexes = this.fibService.GetAvailableIndexes(),
                 AvailableIndexesValues = this.fibService.GetAvailableIndexesValues()
            }); 
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
