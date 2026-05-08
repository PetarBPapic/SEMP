using Microsoft.EntityFrameworkCore;
using BibliotekaKlasa.KlasePodatakaEF.ModeliEF;

namespace BibliotekaKlasa.KlasePodatakaEF.KontekstEF
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opcije) : base(opcije) { }

        public DbSet<KorisnikEntityModel> KorisniciEntityModelObjektiDBSet { get; set; }
        public DbSet<EpizodaEntityModel> EpizodeEntityModelObjektiDBSet { get; set; }
        public DbSet<OcenaEntityModel> OceneEntityModelObjektiDBSet { get; set; }

        protected override void OnModelCreating(ModelBuilder graditelj)
        {
            graditelj.Entity<KorisnikEntityModel>()
                .HasIndex(k => k.KorisnickoIme)
                .IsUnique();

            graditelj.Entity<EpizodaEntityModel>()
                .HasOne(e => e.Kreator)
                .WithMany()
                .HasForeignKey(e => e.KreiraoId)
                .OnDelete(DeleteBehavior.Restrict);

            graditelj.Entity<OcenaEntityModel>()
                .HasOne(o => o.Epizoda)
                .WithMany()
                .HasForeignKey(o => o.EpizodaId)
                .OnDelete(DeleteBehavior.Restrict);

            graditelj.Entity<OcenaEntityModel>()
                .HasOne(o => o.Korisnik)
                .WithMany()
                .HasForeignKey(o => o.KorisnikId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
