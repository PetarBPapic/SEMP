using Microsoft.AspNetCore.Mvc;
using BibliotekaKlasa.KlasePodataka.Modeli;
using SEMP_Aplikacija.ViewModels;
using System.Text;
using System.Text.Json;

namespace SEMP_Aplikacija.Controllers
{
    public class EpizodaController : Controller
    {
        private readonly IHttpClientFactory _httpKlijentFactory;
        private readonly IConfiguration _konfiguracija;

        public EpizodaController(IHttpClientFactory httpKlijentFactory, IConfiguration konfiguracija)
        {
            _httpKlijentFactory = httpKlijentFactory;
            _konfiguracija = konfiguracija;
        }

        private HttpClient KreirajKlijenta()
        {
            var klijent = _httpKlijentFactory.CreateClient();
            klijent.BaseAddress = new Uri(_konfiguracija["RestApiUrl"] ?? "https://localhost:7001");
            return klijent;
        }

        private bool JeAdmin() => HttpContext.Session.GetString("uloga") == "admin";

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!JeAdmin())
                return RedirectToAction("Prijava", "Nalog");

            var klijent = KreirajKlijenta();
            var odgovor = await klijent.GetAsync("api/epizode");
            if (!odgovor.IsSuccessStatusCode)
                return View(new List<EpizodaViewModel>());

            var json = await odgovor.Content.ReadAsStringAsync();
            var epizode = JsonSerializer.Deserialize<List<EpizodaModel>>(json,
                          new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                          ?? new List<EpizodaModel>();

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
        public async Task<IActionResult> Dodaj(EpizodaViewModel viewModel)
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

            var klijent = KreirajKlijenta();
            var sadrzaj = new StringContent(
                JsonSerializer.Serialize(epizodaModelObjekat),
                Encoding.UTF8, "application/json");

            await klijent.PostAsync("api/epizode", sadrzaj);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Izmeni(int id)
        {
            if (!JeAdmin())
                return RedirectToAction("Prijava", "Nalog");

            var klijent = KreirajKlijenta();
            var odgovor = await klijent.GetAsync($"api/epizode/{id}");
            if (!odgovor.IsSuccessStatusCode) return NotFound();

            var json = await odgovor.Content.ReadAsStringAsync();
            var ep = JsonSerializer.Deserialize<EpizodaModel>(json,
                       new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
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
        public async Task<IActionResult> Izmeni(EpizodaViewModel viewModel)
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
                KreiraoId = viewModel.KreiraoId,
                ZarnIdz = viewModel.KreiraoId
            };

            var klijent = KreirajKlijenta();
            var sadrzaj = new StringContent(
                JsonSerializer.Serialize(epizodaModelObjekat),
                Encoding.UTF8, "application/json");

            await klijent.PutAsync($"api/epizode/{viewModel.Id}", sadrzaj);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Obrisi(int id)
        {
            if (!JeAdmin())
                return Forbid();

            var klijent = KreirajKlijenta();
            await klijent.DeleteAsync($"api/epizode/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}