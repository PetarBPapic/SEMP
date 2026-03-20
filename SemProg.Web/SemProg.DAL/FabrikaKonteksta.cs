using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SemProg.DAL
{
    public class FabrikaKonteksta : IDesignTimeDbContextFactory<BazaPodatakaKontekst>
    {
        public BazaPodatakaKontekst CreateDbContext(string[] args)
        {
            var opcije = new DbContextOptionsBuilder<BazaPodatakaKontekst>();
            opcije.UseSqlServer("Server=(LocalDB)\\MSSQLLocalDB;Database=SemProgDB;Trusted_Connection=True;TrustServerCertificate=True;");
            return new BazaPodatakaKontekst(opcije.Options);
        }
    }
}
