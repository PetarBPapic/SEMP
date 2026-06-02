using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliotekaKlasa.KlasePodatakaEF.ModeliEF
{
    [Table("Korisnici")]
    public class KorisnikEntityModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(64)]
        public string KorisnickoIme { get; set; } = string.Empty;

        [Required]
        [MaxLength(256)]
        public string Lozinka { get; set; } = string.Empty;

        [MaxLength(32)]
        public string Uloga { get; set; } = "korisnik";
    }

    [Table("Zarn")]
    public class ZarnEntityModel
    {
        [Key]
        public int ZarnIdz { get; set; }
        [Required]
        [MaxLength(128)]
        public string Naziv { get; set; } = string.Empty;
    }

    [Table("Epizode")]
    public class EpizodaEntityModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(128)]
        public string Naslov { get; set; } = string.Empty;

        [MaxLength(512)]
        public string Opis { get; set; } = string.Empty;

        public DateTime DatumPremijere { get; set; }

        public int KreiraoId { get; set; }

        [ForeignKey("KreiraoId")]
        public KorisnikEntityModel? Kreator { get; set; }

        public int ZarnIdz { get; set; }

        [ForeignKey("ZarnIdz")]
        public KorisnikEntityModel? Zarn { get; set; }
    }

    [Table("Ocene")]
    public class OcenaEntityModel
    {
        [Key]
        public int Id { get; set; }

        public int EpizodaId { get; set; }

        [ForeignKey("EpizodaId")]
        public EpizodaEntityModel? Epizoda { get; set; }

        public int KorisnikId { get; set; }

        [ForeignKey("KorisnikId")]
        public KorisnikEntityModel? Korisnik { get; set; }

        [Range(1, 5)]
        public int Vrednost { get; set; }

        [MaxLength(512)]
        public string? Komentar { get; set; }

        public DateTime OcenjeneNa { get; set; } = DateTime.Now;
    }
}
