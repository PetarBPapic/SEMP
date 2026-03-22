using System.ComponentModel.DataAnnotations;

namespace SemProg.BLL.DtoOvi
{
    public class EpizodaDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naslov je obavezan.")]
        [Display(Name = "Naslov")]
        public string Naslov { get; set; }

        [Display(Name = "Opis")]
        public string Opis { get; set; }

        [Required(ErrorMessage = "Datum premijere je obavezan.")]
        [Display(Name = "Datum premijere")]
        public DateTime DatumPremijere { get; set; } = DateTime.Today;
    }
}
