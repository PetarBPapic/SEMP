using SemProg.BLL.DtoOvi;

namespace SemProg.BLL.Interfejsi
{
    public interface IServisOcena
    {
        Task<bool> OceniAsync(OcenaDto dto, int korisnikId);
        Task<EpizodaSaStatistikomDto> UzmiEpizoduSaStatistikomAsync(int epizodaId);
        Task<List<EpizodaSaStatistikomDto>> UzmiSveEpizodeSaStatistikomAsync();
    }
}
