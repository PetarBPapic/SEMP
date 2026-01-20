using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SemProg.BLL.DTOs;
using SemProg.BLL.Interfaces;
using SemProg.DAL;
using System.Threading.Tasks;

namespace SemProg.Web.Controllers
{
    public class EpisodeController : Controller
    {
        private readonly IEpisodeService _es;
        private readonly AppDbContext _ctx;

        public EpisodeController(IEpisodeService es, AppDbContext ctx)
        {
            _es = es;
            _ctx = ctx;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("role") != "admin")
                return RedirectToAction("Login", "Account");

            var list = await _es.GetAllAsync();
            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("role") != "admin")
                return RedirectToAction("Login", "Account");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EpisodeDto dto)
        {
            if (HttpContext.Session.GetString("role") != "admin")
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid) return View(dto);

            var userId = int.Parse(HttpContext.Session.GetString("uid"));
            await _es.AddAsync(dto, userId);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("role") != "admin")
                return RedirectToAction("Login", "Account");

            var ep = await _ctx.Episodes.FindAsync(id);
            if (ep == null) return NotFound();

            var dto = new EpisodeDto
            {
                Id = ep.Id,
                Title = ep.Title,
                Description = ep.Description,
                ReleaseDate = ep.ReleaseDate
            };
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EpisodeDto dto)
        {
            if (HttpContext.Session.GetString("role") != "admin")
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid) return View(dto);

            var ep = await _ctx.Episodes.FindAsync(dto.Id);
            if (ep == null) return NotFound();

            ep.Title = dto.Title;
            ep.Description = dto.Description;
            ep.ReleaseDate = dto.ReleaseDate;

            await _ctx.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // DELETE – samo admin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("role") != "admin")
                return Forbid();

            var ep = await _ctx.Episodes.FindAsync(id);
            if (ep == null) return NotFound();

            // brišemo i sve ocene vezane za epizodu
            var ratings = _ctx.Ratings.Where(r => r.EpisodeId == id);
            _ctx.Ratings.RemoveRange(ratings);

            _ctx.Episodes.Remove(ep);
            await _ctx.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}