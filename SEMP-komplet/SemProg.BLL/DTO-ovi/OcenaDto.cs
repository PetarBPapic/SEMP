using System.ComponentModel.DataAnnotations;

namespace SemProg.BLL.DtoOvi
{
    public class OcenaDto
    {
        public int EpizodaId { get; set; }

        [Required(ErrorMessage = "Ocena je obavezna.")]
        [Range(1, 5, ErrorMessage = "Ocena mora biti između 1 i 5.")]
        [Display(Name = "Ocena (1-5)")]
        public int Vrednost { get; set; }

        [Display(Name = "Komentar")]
        public string Komentar { get; set; }
    }

    public class PregledOceneDto
    {
        public string KorisnickoIme { get; set; }
        public int Vrednost { get; set; }
        public string Komentar { get; set; }
        public DateTime OcenjenoNa { get; set; }
    }

    public class EpizodaSaStatistikomDto
    {
        public int EpizodaId { get; set; }
        public string Naslov { get; set; }
        public string Opis { get; set; }
        public DateTime DatumPremijere { get; set; }
        public double ProsecnaOcena { get; set; }
        public List<PregledOceneDto> Ocene { get; set; }
    }
}
