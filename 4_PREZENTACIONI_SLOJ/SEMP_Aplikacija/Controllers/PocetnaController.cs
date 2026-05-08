using Microsoft.AspNetCore.Mvc;
using PoslovnaLogika.PoslovniProcessi;
using SEMP_Aplikacija.ViewModels;
using System.Diagnostics;

namespace SEMP_Aplikacija.Controllers
{
    public class PocetnaController : Controller
    {
        private readonly PoslovniProces _poslovniProces;

        public PocetnaController(PoslovniProces poslovniProces)
        {
            _poslovniProces = poslovniProces;
        }

        /// <summary>
        /// Pocetna strana - prikazuje Top5 bez potrebe za prijavom.
        /// Poslovno pravilo: Top liste su vidljive svima.
        /// </summary>
        public IActionResult Index()
        {
            var viewModel = new PocetnaViewModel
            {
                Top5 = _poslovniProces.DajTop5IzXml(),
                DatumAzuriranja = _poslovniProces.DajDatumAzuriranja(),
                KorisnikUlogovan = HttpContext.Session.GetString("korisnik") != null
            };
            return View(viewModel);
        }

        public IActionResult Privatnost() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Greska() =>
            View(new GreskaViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
