using System.ComponentModel.DataAnnotations.Schema;

namespace SemProg.DAL.Modeli
{
    [Table("Ratings")]
    public class Ocena
    {
        public int Id { get; set; }

        [Column("EpisodeId")]
        public int EpizodaId { get; set; }

        public Epizoda Epizoda { get; set; }

        [Column("UserId")]
        public int KorisnikId { get; set; }

        public Korisnik Korisnik { get; set; }

        [Column("Score")]
        public int Vrednost { get; set; }

        [Column("Comment")]
        public string Komentar { get; set; }

        [Column("RatedAt")]
        public DateTime OcenjeneNa { get; set; } = DateTime.Now;
    }
}
