using System.ComponentModel.DataAnnotations.Schema;

namespace SemProg.DAL.Modeli
{
    [Table("Episodes")]
    public class Epizoda
    {
        public int Id { get; set; }

        [Column("Title")]
        public string Naslov { get; set; }

        [Column("Description")]
        public string Opis { get; set; }

        [Column("ReleaseDate")]
        public DateTime DatumPremijere { get; set; }

        [Column("CreatedBy")]
        public int KreiraoId { get; set; }

        public Korisnik Kreator { get; set; }
    }
}
