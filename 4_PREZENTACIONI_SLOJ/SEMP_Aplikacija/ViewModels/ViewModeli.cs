using System.ComponentModel.DataAnnotations;
using BibliotekaKlasa.KlasePodataka.Modeli;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SEMP_Aplikacija.ViewModels
{
    public class GreskaViewModel
    {
        public string? RequestId { get; set; }
        public bool PrikaziRequestId => !string.IsNullOrEmpty(RequestId);
    }

    public class PrijavaViewModel
    {
        [Required(ErrorMessage = "Korisničko ime je obavezno.")]
        [Display(Name = "Korisničko ime")]
        public string KorisnickoIme { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lozinka je obavezna.")]
        [DataType(DataType.Password)]
        [Display(Name = "Lozinka")]
        public string Lozinka { get; set; } = string.Empty;
    }

    public class EpizodaViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naslov je obavezan.")]
        [MaxLength(128)]
        [Display(Name = "Naslov")]
        public string Naslov { get; set; } = string.Empty;

        [Display(Name = "Opis")]
        [MaxLength(512)]
        public string Opis { get; set; } = string.Empty;

        [Required(ErrorMessage = "Datum premijere je obavezan.")]
        [Display(Name = "Datum premijere")]
        [DataType(DataType.Date)]
        public DateTime DatumPremijere { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Žanr je obavezan.")]
        [Display(Name = "Žanr")]
        public int ZarnIdz { get; set; }

        public int KreiraoId { get; set; }

        // Za dropdown u View-u
        public List<SelectListItem> ZarnLista { get; set; } = new();
    }

    public class ZarnViewModel
    {
        public int Idz { get; set; }
        [Required(ErrorMessage = "Naziv je obavezan.")]
        [MaxLength(128)]
        [Display(Name = "Naziv")]
        public string Naziv { get; set; } = string.Empty;

        [Display(Name = "Opis")]
        [MaxLength(512)]
        public string Opis { get; set; } = string.Empty;
    }

    public class OcenaViewModel
    {
        public int EpizodaId { get; set; }
        public string NaslovEpizode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ocena je obavezna.")]
        [Range(1, 5, ErrorMessage = "Ocena mora biti između 1 i 5.")]
        [Display(Name = "Ocena (1-5)")]
        public int Vrednost { get; set; } = 3;

        [Display(Name = "Komentar")]
        [MaxLength(512)]
        public string Komentar { get; set; } = string.Empty;

        // Za prikaz postojecih ocena
        public List<OcenaDetaljiViewModel> PostojeceOcene { get; set; } = new();
        public double ProsecnaOcena { get; set; }
        public OcenaModel? MojaOcena { get; set; }
    }

    public class OcenaDetaljiViewModel
    {
        public string KorisnickoIme { get; set; } = string.Empty;
        public int Vrednost { get; set; }
        public string Komentar { get; set; } = string.Empty;
        public DateTime OcenjeneNa { get; set; }
    }

    public class PocetnaViewModel
    {
        public List<TopEpizodaModel> Top5 { get; set; } = new();
        public string DatumAzuriranja { get; set; } = string.Empty;
        public bool KorisnikUlogovan { get; set; }
    }

    public class EpizodaRangListaViewModel
    {
        public List<TopEpizodaModel> Top10 { get; set; } = new();
        public List<TopEpizodaModel> SveSortirane { get; set; } = new();
        public string DatumAzuriranja { get; set; } = string.Empty;
    }
}
