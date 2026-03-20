using Microsoft.EntityFrameworkCore;
using SemProg.BLL.DtoOvi;
using SemProg.BLL.Interfejsi;
using SemProg.DAL;
using SemProg.DAL.Modeli;

namespace SemProg.BLL.Servisi
{
    public class ServisOcena : IServisOcena
    {
        private readonly BazaPodatakaKontekst _baza;
        public ServisOcena(BazaPodatakaKontekst baza) => _baza = baza;

        public async Task<bool> OceniAsync(OcenaDto dto, int korisnikId)
        {
            var postojeca = await _baza.Ocene
                .FirstOrDefaultAsync(o => o.EpizodaId == dto.EpizodaId && o.KorisnikId == korisnikId);

            if (postojeca != null)
            {
                postojeca.Vrednost = dto.Vrednost;
                postojeca.Komentar = dto.Komentar;
                postojeca.OcenjeneNa = DateTime.Now;
            }
            else
            {
                _baza.Ocene.Add(new Ocena
                {
                    EpizodaId = dto.EpizodaId,
                    KorisnikId = korisnikId,
                    Vrednost = dto.Vrednost,
                    Komentar = dto.Komentar,
                    OcenjeneNa = DateTime.Now
                });
            }

            await _baza.SaveChangesAsync();
            return true;
        }

        public async Task<EpizodaSaStatistikomDto> UzmiEpizoduSaStatistikomAsync(int epizodaId)
        {
            var ep = await _baza.Epizode.FindAsync(epizodaId);
            if (ep == null) return null;

            var ocene = await _baza.Ocene
                .Where(o => o.EpizodaId == epizodaId)
                .Include(o => o.Korisnik)
                .Select(o => new PregledOceneDto
                {
                    KorisnickoIme = o.Korisnik.KorisnickoIme,
                    Vrednost = o.Vrednost,
                    Komentar = o.Komentar,
                    OcenjenoNa = o.OcenjeneNa
                })
                .ToListAsync();

            return new EpizodaSaStatistikomDto
            {
                EpizodaId = ep.Id,
                Naslov = ep.Naslov,
                Opis = ep.Opis,
                DatumPremijere = ep.DatumPremijere,
                ProsecnaOcena = ocene.Any() ? ocene.Average(o => o.Vrednost) : 0,
                Ocene = ocene
            };
        }

        public async Task<List<EpizodaSaStatistikomDto>> UzmiSveEpizodeSaStatistikomAsync()
        {
            var epizode = await _baza.Epizode
                .OrderByDescending(e => e.DatumPremijere)
                .ToListAsync();

            var rezultat = new List<EpizodaSaStatistikomDto>();
            foreach (var ep in epizode)
            {
                var dto = await UzmiEpizoduSaStatistikomAsync(ep.Id);
                if (dto != null)
                {
                    dto.EpizodaId = ep.Id;
                    rezultat.Add(dto);
                }
            }
            return rezultat;
        }
    }
}
