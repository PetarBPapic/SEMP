using System.Security.Cryptography;
using System.Text;

namespace BibliotekaKlasa.TehnoloskeKlase.PomocneFunkcije
{
    public static class FunkcijeLozinke
    {
        public static string HesirajLozinku(string lozinka)
        {
            using var sha256 = SHA256.Create();
            var bajtovi = Encoding.UTF8.GetBytes(lozinka);
            var hash = sha256.ComputeHash(bajtovi);
            return Convert.ToBase64String(hash);
        }

        public static bool ProveraLozinke(string unetaLozinka, string sacuvanaLozinka)
        {
            // Jednostavna provera (bez soli za ovaj projekat)
            return unetaLozinka == sacuvanaLozinka;
        }
    }
}
