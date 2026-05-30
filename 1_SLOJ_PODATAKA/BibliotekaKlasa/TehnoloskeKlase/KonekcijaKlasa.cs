using Microsoft.Data.SqlClient;

namespace BibliotekaKlasa.TehnoloskeKlase
{
    /// <summary>
    /// Nasledjuje BaznaKonekcijaNova i prosiruje je metodom DajKonekciju()
    /// koja vraca aktivni SqlConnection objekat za izvrsavanje upita.
    /// </summary>
    public class KonekcijaKlasa : BaznaKonekcijaNova
    {
        public KonekcijaKlasa(string konekcioniString) : base(konekcioniString)
        {
        }

        public SqlConnection DajKonekciju()
        {
            if (_konekcija == null)
                throw new InvalidOperationException("Konekcija nije otvorena. Pozovite OtvoriKonekciju() pre poziva DajKonekciju().");
            return _konekcija;
        }
    }
}