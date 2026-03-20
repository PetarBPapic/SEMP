using SemProg.BLL.DtoOvi;

namespace SemProg.BLL.Interfejsi
{
    public interface IServisEpizoda
    {
        Task DodajAsync(EpizodaDto dto, int korisnikId);
        Task<List<EpizodaDto>> UzmiSveAsync();
    }
}
