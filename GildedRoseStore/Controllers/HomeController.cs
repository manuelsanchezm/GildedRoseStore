using GildedRoseStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace GildedRoseStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(Repository.Items);
        }

        [HttpGet]
        public IActionResult GetItemDetails(int id)
        {
            return View("ItemDetail", Repository.GetItemById(id));
        }

        [HttpPost]
        public IActionResult PurchaseItem(int id)
        {
            var item = Repository.GetItemById(id);
            if (item != null && item.inStock > 0)
            {
                Repository.PurchaseItem(id);
            }

            return View("index", Repository.Items);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
