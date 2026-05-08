using Microsoft.AspNetCore.Mvc;
using PoslovnaLogika.PoslovniProcessi;
using SEMP_Aplikacija.ViewModels;

namespace SEMP_Aplikacija.Controllers
{
    public class NalogController : Controller
    {
        private readonly PoslovniProces _poslovniProces;

        public NalogController(PoslovniProces poslovniProces)
        {
            _poslovniProces = poslovniProces;
        }

        [HttpGet]
        public IActionResult Prijava() => View();

        [HttpPost]
        public IActionResult Prijava(PrijavaViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            var korisnik = _poslovniProces.ProveraKorisnika(viewModel.KorisnickoIme, viewModel.Lozinka);
            if (korisnik == null)
            {
                ModelState.AddModelError("", "Pogrešno korisničko ime ili lozinka.");
                return View(viewModel);
            }

            HttpContext.Session.SetString("korisnik", korisnik.KorisnickoIme);
            HttpContext.Session.SetString("uloga", korisnik.Uloga);
            HttpContext.Session.SetString("uid", korisnik.Id.ToString());

            return RedirectToAction("Index", "Pocetna");
        }

        public IActionResult Odjava()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Prijava");
        }
    }
}
