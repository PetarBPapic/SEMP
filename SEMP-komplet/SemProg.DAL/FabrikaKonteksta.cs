using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SemProg.DAL
{
    public class FabrikaKonteksta : IDesignTimeDbContextFactory<BazaPodatakaKontekst>
    {
        public BazaPodatakaKontekst CreateDbContext(string[] args)
        {
            var graditelj = new DbContextOptionsBuilder<BazaPodatakaKontekst>();
            graditelj.UseSqlServer("Server=(LocalDB)\\MSSQLLocalDB;Database=SemProgDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True");
            return new BazaPodatakaKontekst(graditelj.Options);
        }
    }
}
