using SemProg.BLL.DTOs;

namespace SemProg.BLL.Interfaces
{
    public interface IRatingService
    {
        Task<bool> RateAsync(RatingDto dto, int userId);
        Task<EpisodeWithStatsDto> GetEpisodeWithStatsAsync(int episodeId);
        Task<List<EpisodeWithStatsDto>> GetAllEpisodesWithStatsAsync();
    }
}
