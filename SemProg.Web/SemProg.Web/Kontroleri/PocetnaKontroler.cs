using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SemProg.Web.Modeli;

namespace SemProg.Web.Kontroleri
{
    public class PocetnaController : Controller
    {
        private readonly ILogger<PocetnaController> _logger;
        public PocetnaController(ILogger<PocetnaController> logger) => _logger = logger;

        public IActionResult Index() => View();
        public IActionResult Privatnost() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Greska() => View(new ModelGreske { IdZahteva = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
