using System.ComponentModel.DataAnnotations;

namespace SemProg.BLL.DtoOvi
{
    public class PrijavaDto
    {
        [Required(ErrorMessage = "Korisničko ime je obavezno.")]
        [Display(Name = "Korisničko ime")]
        public string KorisnickoIme { get; set; }

        [Required(ErrorMessage = "Lozinka je obavezna.")]
        [Display(Name = "Lozinka")]
        public string Lozinka { get; set; }
    }
}
