using Microsoft.AspNetCore.Mvc;
using SemProg.BLL.DtoOvi;
using SemProg.BLL.Interfejsi;
using SemProg.DAL;

namespace SemProg.Web.Kontroleri
{
    public class EpizodaController : Controller
    {
        private readonly IServisEpizoda _servis;
        private readonly BazaPodatakaKontekst _baza;

        public EpizodaController(IServisEpizoda servis, BazaPodatakaKontekst baza)
        {
            _servis = servis;
            _baza = baza;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("uloga") != "admin")
                return RedirectToAction("Login", "Nalog");
            var lista = await _servis.UzmiSveAsync();
            return View(lista);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("uloga") != "admin")
                return RedirectToAction("Login", "Nalog");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EpizodaDto dto)
        {
            if (HttpContext.Session.GetString("uloga") != "admin")
                return RedirectToAction("Login", "Nalog");
            if (!ModelState.IsValid) return View(dto);
            var korisnikId = int.Parse(HttpContext.Session.GetString("uid"));
            await _servis.DodajAsync(dto, korisnikId);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("uloga") != "admin")
                return RedirectToAction("Login", "Nalog");
            var ep = await _baza.Epizode.FindAsync(id);
            if (ep == null) return NotFound();
            return View(new EpizodaDto { Id = ep.Id, Naslov = ep.Naslov, Opis = ep.Opis, DatumPremijere = ep.DatumPremijere });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EpizodaDto dto)
        {
            if (HttpContext.Session.GetString("uloga") != "admin")
                return RedirectToAction("Login", "Nalog");
            if (!ModelState.IsValid) return View(dto);
            var ep = await _baza.Epizode.FindAsync(dto.Id);
            if (ep == null) return NotFound();
            ep.Naslov = dto.Naslov;
            ep.Opis = dto.Opis;
            ep.DatumPremijere = dto.DatumPremijere;
            await _baza.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("uloga") != "admin")
                return Forbid();
            var ep = await _baza.Epizode.FindAsync(id);
            if (ep == null) return NotFound();
            var ocene = _baza.Ocene.Where(o => o.EpizodaId == id);
            _baza.Ocene.RemoveRange(ocene);
            _baza.Epizode.Remove(ep);
            await _baza.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
