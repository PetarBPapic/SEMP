using SemProg.BLL.DTOs;

namespace SemProg.BLL.Interfaces
{
    public interface IEpisodeService
    {
        Task AddAsync(EpisodeDto dto, int userId);
        Task<List<EpisodeDto>> GetAllAsync();
    }
}
