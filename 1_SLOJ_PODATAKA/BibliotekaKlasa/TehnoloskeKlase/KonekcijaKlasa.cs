using Microsoft.Data.SqlClient;

namespace BibliotekaKlasa.TehnoloskeKlase
{
    /// <summary>
    /// CRC:
    /// Responsibility - ODGOVORNOST: Upravljanje konekcijom ka SQL Server bazi podataka
    /// Collaboration - zavisi od standardne klase SqlConnection iz biblioteke Microsoft.Data.SqlClient
    /// </summary>
    public class KonekcijaKlasa
    {
        #region Atributi

        private SqlConnection? _konekcija;
        public string KonekcioniString { get; set; }

        #endregion

        #region Konstruktor

        public KonekcijaKlasa(string konekcioniString)
        {
            KonekcioniString = konekcioniString;
        }

        #endregion

        #region Javne metode

        public void OtvoriKonekciju()
        {
            _konekcija = new SqlConnection(KonekcioniString);
            _konekcija.Open();
        }

        public void ZatvoriKonekciju()
        {
            if (_konekcija != null && _konekcija.State == System.Data.ConnectionState.Open)
            {
                _konekcija.Close();
            }
        }

        public SqlConnection DajKonekciju()
        {
            if (_konekcija == null)
                throw new InvalidOperationException("Konekcija nije otvorena. Pozovite OtvoriKonekciju() pre poziva DajKonekciju().");
            return _konekcija;
        }

        #endregion
    }
}
