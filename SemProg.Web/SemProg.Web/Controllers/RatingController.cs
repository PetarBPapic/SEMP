using Microsoft.AspNetCore.Mvc;
using SemProg.BLL.DTOs;
using SemProg.BLL.Interfaces;

namespace SemProg.Web.Controllers
{
    public class RatingController : Controller
    {
        private readonly IRatingService _rs;
        public RatingController(IRatingService rs) => _rs = rs;

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("user") == null)
                return RedirectToAction("Login", "Account");

            var list = await _rs.GetAllEpisodesWithStatsAsync();
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Rate(int id)
        {
            if (HttpContext.Session.GetString("user") == null)
                return RedirectToAction("Login", "Account");

            var ep = await _rs.GetEpisodeWithStatsAsync(id);
            if (ep == null) return NotFound();

            ViewBag.Title = ep.Title;
            ViewBag.EpisodeId = id;
            ViewBag.Existing = ep.Ratings.FirstOrDefault(r => r.Username == HttpContext.Session.GetString("user"));
            ViewBag.All = ep.Ratings;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Rate(RatingDto dto)
        {
            if (HttpContext.Session.GetString("user") == null)
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid) return View(dto);

            var userId = int.Parse(HttpContext.Session.GetString("uid"));
            await _rs.RateAsync(dto, userId);
            return RedirectToAction("Index");
        }
    }
}
