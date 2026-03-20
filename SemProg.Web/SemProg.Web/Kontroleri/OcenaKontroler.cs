using Microsoft.AspNetCore.Mvc;
using SemProg.BLL.DtoOvi;
using SemProg.BLL.Interfejsi;

namespace SemProg.Web.Kontroleri
{
    public class OcenaController : Controller
    {
        private readonly IServisOcena _servis;
        public OcenaController(IServisOcena servis) => _servis = servis;

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("korisnik") == null)
                return RedirectToAction("Login", "Nalog");
            var lista = await _servis.UzmiSveEpizodeSaStatistikomAsync();
            return View(lista);
        }

        [HttpGet]
        public async Task<IActionResult> Oceni(int id)
        {
            if (HttpContext.Session.GetString("korisnik") == null)
                return RedirectToAction("Login", "Nalog");
            var ep = await _servis.UzmiEpizoduSaStatistikomAsync(id);
            if (ep == null) return NotFound();
            ViewBag.Naslov = ep.Naslov;
            ViewBag.EpizodaId = id;
            ViewBag.Postojeca = ep.Ocene.FirstOrDefault(o => o.KorisnickoIme == HttpContext.Session.GetString("korisnik"));
            ViewBag.Sve = ep.Ocene;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Oceni(OcenaDto dto)
        {
            if (HttpContext.Session.GetString("korisnik") == null)
                return RedirectToAction("Login", "Nalog");
            if (!ModelState.IsValid) return View(dto);
            var korisnikId = int.Parse(HttpContext.Session.GetString("uid"));
            await _servis.OceniAsync(dto, korisnikId);
            return RedirectToAction("Index");
        }
    }
}
