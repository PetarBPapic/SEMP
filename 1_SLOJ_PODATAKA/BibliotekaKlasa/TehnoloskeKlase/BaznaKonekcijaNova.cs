using Microsoft.Data.SqlClient;

namespace BibliotekaKlasa.TehnoloskeKlase
{
    public class BaznaKonekcijaNova
    {
        private SqlConnection? konekcija;
        public string KonekcioniString { get; set; }

        public BaznaKonekcijaNova(string konekcioniString)
        {
            this.KonekcioniString = konekcioniString;
        }

        public void OtvoriKonekciju()
        {
            konekcija = new SqlConnection(KonekcioniString);
            konekcija.Open();
        }

        public void ZatvoriKonekciju()
        {
            if (konekcija != null && konekcija.State == System.Data.ConnectionState.Open)
            {
                konekcija.Close();
            }
        }
    }
}
