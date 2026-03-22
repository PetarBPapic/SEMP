using Microsoft.EntityFrameworkCore;
using SemProg.DAL.Modeli;

namespace SemProg.DAL
{
    public class BazaPodatakaKontekst : DbContext
    {
        public BazaPodatakaKontekst(DbContextOptions<BazaPodatakaKontekst> opcije) : base(opcije) { }

        public DbSet<Korisnik> Korisnici { get; set; }
        public DbSet<Epizoda> Epizode { get; set; }
        public DbSet<Ocena> Ocene { get; set; }

        protected override void OnModelCreating(ModelBuilder graditelj)
        {
            graditelj.Entity<Korisnik>().HasIndex(k => k.KorisnickoIme).IsUnique();

            graditelj.Entity<Epizoda>()
                .HasOne(e => e.Kreator)
                .WithMany()
                .HasForeignKey(e => e.KreiraoId)
                .OnDelete(DeleteBehavior.Restrict);

            graditelj.Entity<Ocena>()
                .HasOne(o => o.Epizoda)
                .WithMany()
                .HasForeignKey(o => o.EpizodaId)
                .OnDelete(DeleteBehavior.Restrict);

            graditelj.Entity<Ocena>()
                .HasOne(o => o.Korisnik)
                .WithMany()
                .HasForeignKey(o => o.KorisnikId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
