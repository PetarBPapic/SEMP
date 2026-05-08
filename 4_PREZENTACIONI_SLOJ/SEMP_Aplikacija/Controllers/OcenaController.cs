using Microsoft.AspNetCore.Mvc;
using PoslovnaLogika.PoslovniProcessi;
using SEMP_Aplikacija.ViewModels;

namespace SEMP_Aplikacija.Controllers
{
    public class OcenaController : Controller
    {
        private readonly PoslovniProces _poslovniProces;

        public OcenaController(PoslovniProces poslovniProces)
        {
            _poslovniProces = poslovniProces;
        }

        private bool JeUlogovan() => HttpContext.Session.GetString("korisnik") != null;

        /// <summary>
        /// Lista svih epizoda za ocenjivanje - zahteva prijavu.
        /// </summary>
        [HttpGet]
        public IActionResult Index()
        {
            if (!JeUlogovan())
                return RedirectToAction("Prijava", "Nalog");

            var epizode = _poslovniProces.DajSveEpizode();
            var korisnikIme = HttpContext.Session.GetString("korisnik")!;

            var viewModeli = epizode.Select(e =>
            {
                var ocene = _poslovniProces.DajOceneZaEpizodu(e.Id);
                double prosek = ocene.Any() ? ocene.Average(o => o.Vrednost) : 0;
                return new OcenaViewModel
                {
                    EpizodaId = e.Id,
                    NaslovEpizode = e.Naslov,
                    ProsecnaOcena = prosek,
                    PostojeceOcene = new List<OcenaDetaljiViewModel>()
                };
            }).ToList();

            return View(viewModeli);
        }

        /// <summary>
        /// Prikaz rang liste: Top10 i sve epizode sortirane.
        /// Vidljivo i bez prijave i sa prijavom.
        /// </summary>
        [HttpGet]
        public IActionResult RangLista()
        {
            var viewModel = new EpizodaRangListaViewModel
            {
                Top10 = _poslovniProces.DajTop10IzXml(),
                SveSortirane = _poslovniProces.DajSveEpizodeSortirane(),
                DatumAzuriranja = _poslovniProces.DajDatumAzuriranja()
            };
            return View(viewModel);
        }

        /// <summary>
        /// Forma za ocenjivanje konkretne epizode.
        /// </summary>
        [HttpGet]
        public IActionResult Oceni(int id)
        {
            if (!JeUlogovan())
                return RedirectToAction("Prijava", "Nalog");

            var ep = _poslovniProces.DajEpizodu(id);
            if (ep == null) return NotFound();

            var korisnikId = int.Parse(HttpContext.Session.GetString("uid")!);
            var korisnikIme = HttpContext.Session.GetString("korisnik")!;
            var sveOcene = _poslovniProces.DajOceneZaEpizodu(id);
            var mojaOcena = _poslovniProces.DajOcenuKorisnika(id, korisnikId);

            var viewModel = new OcenaViewModel
            {
                EpizodaId = id,
                NaslovEpizode = ep.Naslov,
                Vrednost = mojaOcena?.Vrednost ?? 3,
                Komentar = mojaOcena?.Komentar ?? "",
                ProsecnaOcena = sveOcene.Any() ? sveOcene.Average(o => o.Vrednost) : 0,
                MojaOcena = mojaOcena,
                PostojeceOcene = sveOcene.Select(o => new OcenaDetaljiViewModel
                {
                    KorisnickoIme = o.KorisnikId.ToString(), // Prikazuje se ID - mozete dopuniti
                    Vrednost = o.Vrednost,
                    Komentar = o.Komentar,
                    OcenjeneNa = o.OcenjeneNa
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Oceni(OcenaViewModel viewModel)
        {
            if (!JeUlogovan())
                return RedirectToAction("Prijava", "Nalog");

            if (!ModelState.IsValid)
            {
                var ep = _poslovniProces.DajEpizodu(viewModel.EpizodaId);
                viewModel.NaslovEpizode = ep?.Naslov ?? "";
                return View(viewModel);
            }

            int korisnikId = int.Parse(HttpContext.Session.GetString("uid")!);

            // Pozivamo poslovni proces - on ce oceniti i automatski azurirati Top liste
            _poslovniProces.OceniEpizodu(
                viewModel.EpizodaId,
                korisnikId,
                viewModel.Vrednost,
                viewModel.Komentar
            );

            TempData["Poruka"] = "Vaša ocena je uspešno sačuvana! Top liste su ažurirane.";
            return RedirectToAction(nameof(Index));
        }
    }
}
