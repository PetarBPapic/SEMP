using Microsoft.Data.SqlClient;

namespace BibliotekaKlasa.TehnoloskeKlase
{
    /// <summary>
    /// Bazna klasa za upravljanje SQL konekcijom.
    /// Sadrzi zajednicki kod koji nasledjuju sve konkretne konekcijske klase.
    /// </summary>
    public class BaznaKonekcijaNova
    {
        protected SqlConnection? _konekcija;
        public string KonekcioniString { get; set; }

        public BaznaKonekcijaNova(string konekcioniString)
        {
            this.KonekcioniString = konekcioniString;
        }

        public virtual void OtvoriKonekciju()
        {
            _konekcija = new SqlConnection(KonekcioniString);
            _konekcija.Open();
        }

        public virtual void ZatvoriKonekciju()
        {
            if (_konekcija != null && _konekcija.State == System.Data.ConnectionState.Open)
            {
                _konekcija.Close();
            }
        }
    }
}