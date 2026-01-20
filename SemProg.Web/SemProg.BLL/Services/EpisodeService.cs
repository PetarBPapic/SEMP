using Microsoft.EntityFrameworkCore;
using SemProg.BLL.DTOs;
using SemProg.BLL.Interfaces;
using SemProg.DAL;
using SemProg.DAL.Models;

namespace SemProg.BLL.Services
{
    public class EpisodeService : IEpisodeService
    {
        private readonly AppDbContext _ctx;
        public EpisodeService(AppDbContext ctx) => _ctx = ctx;

        public async Task AddAsync(EpisodeDto dto, int userId)
        {
            var ep = new Episode
            {
                Title = dto.Title ?? string.Empty,
                Description = dto.Description ?? string.Empty,
                ReleaseDate = dto.ReleaseDate,
                CreatedBy = userId
            };
            _ctx.Episodes.Add(ep);
            await _ctx.SaveChangesAsync();
        }

        public async Task<List<EpisodeDto>> GetAllAsync()
        {
            return await _ctx.Episodes
                .OrderByDescending(e => e.ReleaseDate)
                .Select(e => new EpisodeDto
                {
                    Id = e.Id,
                    Title = e.Title ?? string.Empty,
                    Description = e.Description ?? string.Empty,
                    ReleaseDate = e.ReleaseDate
                })
                .ToListAsync();
        }
    }
}