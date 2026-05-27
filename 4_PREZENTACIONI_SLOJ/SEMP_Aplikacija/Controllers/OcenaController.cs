using Microsoft.AspNetCore.Mvc;
using BibliotekaKlasa.KlasePodataka.Modeli;
using PoslovnaLogika.PoslovniProcessi;
using SEMP_Aplikacija.ViewModels;
using System.Text.Json;

namespace SEMP_Aplikacija.Controllers
{
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

        [HttpGet]
        public IActionResult Index()
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
                    ProsecnaOcena = prosek,
                    PostojeceOcene = new List<OcenaDetaljiViewModel>()
                };
            }).ToList();

            return View(viewModeli);
        }

        /// <summary>
        /// Rang lista se ucitava POZIVOM REST SERVISA.
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
                var odgovorTop10 = await klijent.GetAsync("api/epizode/top10");
                if (odgovorTop10.IsSuccessStatusCode)
                {
                    var json = await odgovorTop10.Content.ReadAsStringAsync();
                    top10Lista = JsonSerializer.Deserialize<List<TopEpizodaModel>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                        ?? new List<TopEpizodaModel>();
                }

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
                // Fallback ako REST API nije dostupan
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

        [HttpGet]
        public IActionResult Oceni(int id)
        {
            if (!JeUlogovan())
                return RedirectToAction("Prijava", "Nalog");

            var ep = _poslovniProces.DajEpizodu(id);
            if (ep == null) return NotFound();

            var korisnikId = int.Parse(HttpContext.Session.GetString("uid")!);
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
                    KorisnickoIme = o.KorisnikId.ToString(),
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

            try
            {
                // PoslovniProces interno:
                // 1) cita MaksOcenaPoKorisniku putem REST servisa (iz JSON)
                // 2) proverava broj danasanjih ocena putem stored procedure
                // 3) primenjuje poslovno pravilo (AKO limit ONDA odbija)
                // 4) snima ocenu i azurira Top liste
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
                // Poslovno pravilo odbilo - prikazujemo poruku korisniku
                TempData["Greska"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}