using Microsoft.AspNetCore.Mvc;
using BibliotekaKlasa.KlasePodataka.Modeli;
using PoslovnaLogika.PoslovniProcessi;
using SEMP_Aplikacija.ViewModels;
using System.Text.Json;

namespace SEMP_Aplikacija.Controllers
{
    /// <summary>
    /// OcenaController - prezentacioni sloj.
    /// Rang lista (Top5/Top10) se ucitava POZIVOM REST SERVISA.
    /// Ocenjivanje ide kroz PoslovniProces koji sam poziva servis za parametar ogranicenja.
    /// </summary>
    public class OcenaController : Controller
    {
        private readonly PoslovniProces _poslovniProces;
        private readonly IHttpClientFactory _httpKlijentFactory;
        private readonly IConfiguration _konfiguracija;

        public OcenaController(PoslovniProces poslovniProces,
                               IHttpClientFactory httpKlijentFactory,
                               IConfiguration konfiguracija)
        {
            _poslovniProces = poslovniProces;
            _httpKlijentFactory = httpKlijentFactory;
            _konfiguracija = konfiguracija;
        }

        private HttpClient KreirajKlijenta()
        {
            var klijent = _httpKlijentFactory.CreateClient();
            klijent.BaseAddress = new Uri(_konfiguracija["RestApiUrl"] ?? "https://localhost:7001");
            return klijent;
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
        /// Podaci se ucitavaju POZIVOM REST SERVISA - servis je medjusloj.
        /// Vidljivo i bez prijave i sa prijavom.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> RangLista()
        {
            var klijent = KreirajKlijenta();

            var top10Lista = new List<TopEpizodaModel>();
            var sveSortiranaLista = new List<TopEpizodaModel>();
            string datumAzuriranja = "";

            try
            {
                // Poziv REST servisa - GET api/epizode/top10
                var odgovorTop10 = await klijent.GetAsync("api/epizode/top10");
                if (odgovorTop10.IsSuccessStatusCode)
                {
                    var json = await odgovorTop10.Content.ReadAsStringAsync();
                    top10Lista = JsonSerializer.Deserialize<List<TopEpizodaModel>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                        ?? new List<TopEpizodaModel>();
                }

                // Poziv REST servisa - GET api/epizode/sve-sortirane
                var odgovorSve = await klijent.GetAsync("api/epizode/sve-sortirane");
                if (odgovorSve.IsSuccessStatusCode)
                {
                    var json = await odgovorSve.Content.ReadAsStringAsync();
                    sveSortiranaLista = JsonSerializer.Deserialize<List<TopEpizodaModel>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                        ?? new List<TopEpizodaModel>();
                }

                datumAzuriranja = _poslovniProces.DajDatumAzuriranja();
            }
            catch
            {
                // Fallback na direktan poziv ako REST API nije dostupan
                top10Lista = _poslovniProces.DajTop10IzXml();
                sveSortiranaLista = _poslovniProces.DajSveEpizodeSortirane();
                datumAzuriranja = _poslovniProces.DajDatumAzuriranja();
            }

            var viewModel = new EpizodaRangListaViewModel
            {
                Top10 = top10Lista,
                SveSortirane = sveSortiranaLista,
                DatumAzuriranja = datumAzuriranja
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

        /// <summary>
        /// Stampa - printer friendly stranica sa svim epizodama i njihovim ocenama.
        /// Realizacija zahteva za stampom spiska i parametarskom stampom.
        /// </summary>
        [HttpGet]
        public IActionResult Stampa()
        {
            if (!JeUlogovan())
                return RedirectToAction("Prijava", "Nalog");

            var epizode = _poslovniProces.DajSveEpizode();

            var viewModeli = epizode.Select(e =>
            {
                var ocene = _poslovniProces.DajOceneZaEpizodu(e.Id);
                double prosek = ocene.Any() ? ocene.Average(o => o.Vrednost) : 0;
                return new OcenaViewModel
                {
                    EpizodaId = e.Id,
                    NaslovEpizode = e.Naslov,
                    ProsecnaOcena = Math.Round(prosek, 2),
                    PostojeceOcene = ocene.Select(o => new OcenaDetaljiViewModel
                    {
                        KorisnickoIme = o.KorisnikId.ToString(),
                        Vrednost = o.Vrednost,
                        Komentar = o.Komentar,
                        OcenjeneNa = o.OcenjeneNa
                    }).ToList()
                };
            }).ToList();

            return View(viewModeli);
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

            try
            {
                // Poziva PoslovniProces koji interno:
                //   1) cita parametar MaksOcenaPoKorisniku putem REST servisa (iz JSON)
                //   2) proverava broj danasanjih ocena putem stored procedure
                //   3) primenjuje poslovno pravilo (AKO prekoracen limit ONDA odbija)
                //   4) snima ocenu i azurira Top liste u XML-u
                _poslovniProces.OceniEpizodu(
                    viewModel.EpizodaId,
                    korisnikId,
                    viewModel.Vrednost,
                    viewModel.Komentar
                );
                TempData["Poruka"] = "Vaša ocena je uspešno sačuvana! Top liste su ažurirane.";
            }
            catch (InvalidOperationException ex)
            {
                // Poslovno pravilo odbilo akciju - prikazujemo poruku korisniku
                TempData["Greska"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}