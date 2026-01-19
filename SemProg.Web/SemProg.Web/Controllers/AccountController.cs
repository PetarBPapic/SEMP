using Microsoft.AspNetCore.Mvc;
using SemProg.BLL.DTOs;
using SemProg.BLL.Interfaces;

namespace SemProg.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _us;
        public AccountController(IUserService us) => _us = us;

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(LoginDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            if (!_us.Validate(dto.Username, dto.Password))
            {
                ModelState.AddModelError("", "Pogrešan username ili password");
                return View(dto);
            }

            HttpContext.Session.SetString("user", dto.Username);
            HttpContext.Session.SetString("role", _us.IsAdmin(dto.Username) ? "admin" : "user");
            HttpContext.Session.SetString("uid", _us.GetId(dto.Username).ToString());
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
