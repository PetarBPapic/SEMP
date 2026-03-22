using Microsoft.AspNetCore.Mvc;
using SemProg.BLL.DtoOvi;
using SemProg.BLL.Interfejsi;

namespace SemProg.Web.Kontroleri
{
    public class NalogController : Controller
    {
        private readonly IServisKorisnika _servis;
        public NalogController(IServisKorisnika servis) => _servis = servis;

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(PrijavaDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            if (!_servis.Provjeri(dto.KorisnickoIme, dto.Lozinka))
            {
                ModelState.AddModelError("", "Pogrešno korisničko ime ili lozinka.");
                return View(dto);
            }
            HttpContext.Session.SetString("korisnik", dto.KorisnickoIme);
            HttpContext.Session.SetString("uloga", _servis.JeAdmin(dto.KorisnickoIme) ? "admin" : "korisnik");
            HttpContext.Session.SetString("uid", _servis.UzmiId(dto.KorisnickoIme).ToString());
            return RedirectToAction("Index", "Pocetna");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
