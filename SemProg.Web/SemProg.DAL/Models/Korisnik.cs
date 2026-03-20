using System.ComponentModel.DataAnnotations.Schema;

namespace SemProg.DAL.Modeli
{
    [Table("Users")]
    public class Korisnik
    {
        public int Id { get; set; }

        [Column("Username")]
        public string KorisnickoIme { get; set; }

        [Column("Password")]
        public string Lozinka { get; set; }

        [Column("Role")]
        public string Uloga { get; set; } // "korisnik" ili "admin"
    }
}
