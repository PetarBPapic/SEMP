using Microsoft.EntityFrameworkCore;
using SemProg.BLL.DtoOvi;
using SemProg.BLL.Interfejsi;
using SemProg.DAL;
using SemProg.DAL.Modeli;

namespace SemProg.BLL.Servisi
{
    public class ServisEpizoda : IServisEpizoda
    {
        private readonly BazaPodatakaKontekst _baza;
        public ServisEpizoda(BazaPodatakaKontekst baza) => _baza = baza;

        public async Task DodajAsync(EpizodaDto dto, int korisnikId)
        {
            var epizoda = new Epizoda
            {
                Naslov = dto.Naslov ?? string.Empty,
                Opis = dto.Opis ?? string.Empty,
                DatumPremijere = dto.DatumPremijere,
                KreiraoId = korisnikId
            };
            _baza.Epizode.Add(epizoda);
            await _baza.SaveChangesAsync();
        }

        public async Task<List<EpizodaDto>> UzmiSveAsync()
        {
            return await _baza.Epizode
                .OrderByDescending(e => e.DatumPremijere)
                .Select(e => new EpizodaDto
                {
                    Id = e.Id,
                    Naslov = e.Naslov ?? string.Empty,
                    Opis = e.Opis ?? string.Empty,
                    DatumPremijere = e.DatumPremijere
                })
                .ToListAsync();
        }
    }
}
