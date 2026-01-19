using Microsoft.EntityFrameworkCore;
using SemProg.BLL.DTOs;
using SemProg.BLL.Interfaces;
using SemProg.DAL;
using SemProg.DAL.Models;

namespace SemProg.BLL.Services
{
    public class RatingService : IRatingService
    {
        private readonly AppDbContext _ctx;
        public RatingService(AppDbContext ctx) => _ctx = ctx;

        public async Task<bool> RateAsync(RatingDto dto, int userId)
        {
            var existing = await _ctx.Ratings
                .FirstOrDefaultAsync(r => r.EpisodeId == dto.EpisodeId && r.UserId == userId);

            if (existing != null)
            {
                existing.Score = dto.Score;
                existing.Comment = dto.Comment;
                existing.RatedAt = DateTime.Now;
            }
            else
            {
                _ctx.Ratings.Add(new Rating
                {
                    EpisodeId = dto.EpisodeId,
                    UserId = userId,
                    Score = dto.Score,
                    Comment = dto.Comment,
                    RatedAt = DateTime.Now
                });
            }

            await _ctx.SaveChangesAsync();
            return true;
        }

        public async Task<EpisodeWithStatsDto> GetEpisodeWithStatsAsync(int episodeId)
        {
            var ep = await _ctx.Episodes.FindAsync(episodeId);
            if (ep == null) return null;

            var ratings = await _ctx.Ratings
                .Where(r => r.EpisodeId == episodeId)
                .Include(r => r.User)
                .Select(r => new RatingViewDto
                {
                    Username = r.User.Username,
                    Score = r.Score,
                    Comment = r.Comment,
                    RatedAt = r.RatedAt
                })
                .ToListAsync();

            return new EpisodeWithStatsDto
            {
                EpisodeId = ep.Id,
                Title = ep.Title,
                Description = ep.Description,
                ReleaseDate = ep.ReleaseDate,
                AvgScore = ratings.Any() ? ratings.Average(r => r.Score) : 0,
                Ratings = ratings
            };
        }

        public async Task<List<EpisodeWithStatsDto>> GetAllEpisodesWithStatsAsync()
        {
            var episodes = await _ctx.Episodes
                .OrderByDescending(e => e.ReleaseDate)
                .ToListAsync();

            var result = new List<EpisodeWithStatsDto>();

            foreach (var ep in episodes)
            {
                var dto = await GetEpisodeWithStatsAsync(ep.Id);
                if (dto != null)
                {
                    dto.EpisodeId = ep.Id; // <-- dodato
                    result.Add(dto);
                }
            }

            return result;
        }
    }
}