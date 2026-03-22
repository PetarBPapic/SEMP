using Microsoft.AspNetCore.Mvc;
using SemProg.Web.Modeli;
using System.Diagnostics;

namespace SemProg.Web.Kontroleri
{
    public class PocetnaController : Controller
    {
        public IActionResult Index() => View();
        public IActionResult Privatnost() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Greska() => View(new ModelGreske { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
