using Microsoft.AspNetCore.Mvc;
using BibliotekaKlasa.KlasePodataka.Modeli;
using PoslovnaLogika.PoslovniProcessi;
using SEMP_Aplikacija.ViewModels;

namespace SEMP_Aplikacija.Controllers
{
    public class EpizodaController : Controller
    {
        private readonly PoslovniProces _poslovniProces;

        public EpizodaController(PoslovniProces poslovniProces)
        {
            _poslovniProces = poslovniProces;
        }

        private bool JeAdmin() => HttpContext.Session.GetString("uloga") == "admin";

        [HttpGet]
        public IActionResult Index()
        {
            if (!JeAdmin())
                return RedirectToAction("Prijava", "Nalog");

            var epizode = _poslovniProces.DajSveEpizode();
            var viewModeli = epizode.Select(e => new EpizodaViewModel
            {
                Id = e.Id,
                Naslov = e.Naslov,
                Opis = e.Opis,
                DatumPremijere = e.DatumPremijere,
                KreiraoId = e.KreiraoId
            }).ToList();

            return View(viewModeli);
        }

        [HttpGet]
        public IActionResult Dodaj()
        {
            if (!JeAdmin())
                return RedirectToAction("Prijava", "Nalog");

            return View(new EpizodaViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Dodaj(EpizodaViewModel viewModel)
        {
            if (!JeAdmin())
                return RedirectToAction("Prijava", "Nalog");

            if (!ModelState.IsValid) return View(viewModel);

            int korisnikId = int.Parse(HttpContext.Session.GetString("uid")!);
            var epizodaModelObjekat = new EpizodaModel
            {
                Naslov = viewModel.Naslov,
                Opis = viewModel.Opis,
                DatumPremijere = viewModel.DatumPremijere,
                KreiraoId = korisnikId
            };

            _poslovniProces.DodajEpizodu(epizodaModelObjekat);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Izmeni(int id)
        {
            if (!JeAdmin())
                return RedirectToAction("Prijava", "Nalog");

            var ep = _poslovniProces.DajEpizodu(id);
            if (ep == null) return NotFound();

            return View(new EpizodaViewModel
            {
                Id = ep.Id,
                Naslov = ep.Naslov,
                Opis = ep.Opis,
                DatumPremijere = ep.DatumPremijere,
                KreiraoId = ep.KreiraoId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Izmeni(EpizodaViewModel viewModel)
        {
            if (!JeAdmin())
                return RedirectToAction("Prijava", "Nalog");

            if (!ModelState.IsValid) return View(viewModel);

            var epizodaModelObjekat = new EpizodaModel
            {
                Id = viewModel.Id,
                Naslov = viewModel.Naslov,
                Opis = viewModel.Opis,
                DatumPremijere = viewModel.DatumPremijere,
                KreiraoId = viewModel.KreiraoId
            };

            _poslovniProces.IzmeniEpizodu(epizodaModelObjekat);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Obrisi(int id)
        {
            if (!JeAdmin())
                return Forbid();

            _poslovniProces.ObrisiEpizodu(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
