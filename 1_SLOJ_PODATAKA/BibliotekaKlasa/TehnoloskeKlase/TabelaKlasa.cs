using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace BibliotekaKlasa.TehnoloskeKlase
{
    public class TabelaKlasa
    {
        /* CRC:
          * Responsibility - ODGOVORNOST: Konekcija na celinu baze podataka, SQL server tipa
          * Collaboration - zavisi od standardne klase SqlDataAdapter iz biblioteke Microsoft.Data.SqlClient
          *                 kao i klase DataSet iz standardne biblioteke System.Data */

        #region Atributi

        private string _nazivTabele;
        private KonekcijaKlasa _konekcijaObjekat;
        private SqlDataAdapter _adapterObjekat;
        private DataSet _dataSetObjekat;

        #endregion

        #region Konstruktor

        public TabelaKlasa(KonekcijaKlasa novaKonekcija, string noviNazivTabele)
        {
            _konekcijaObjekat = novaKonekcija;
            _nazivTabele = noviNazivTabele;
        }

        #endregion

        #region Privatne metode

        private void KreirajAdapter(string selectUpit, string insertUpit, string deleteUpit, string updateUpit)
        {
            SqlCommand pomSelectKomanda, pomInsertKomanda, pomDeleteKomanda, pomUpdateKomanda;

            pomSelectKomanda = new SqlCommand();
            pomSelectKomanda.CommandText = selectUpit;
            pomSelectKomanda.Connection = _konekcijaObjekat.DajKonekciju();

            pomInsertKomanda = new SqlCommand();
            pomInsertKomanda.CommandText = insertUpit;
            pomInsertKomanda.Connection = _konekcijaObjekat.DajKonekciju();

            pomDeleteKomanda = new SqlCommand();
            pomDeleteKomanda.CommandText = deleteUpit;
            pomDeleteKomanda.Connection = _konekcijaObjekat.DajKonekciju();

            pomUpdateKomanda = new SqlCommand();
            pomUpdateKomanda.CommandText = updateUpit;
            pomUpdateKomanda.Connection = _konekcijaObjekat.DajKonekciju();

            _adapterObjekat = new SqlDataAdapter();
            _adapterObjekat.SelectCommand = pomSelectKomanda;
            _adapterObjekat.InsertCommand = pomInsertKomanda;
            _adapterObjekat.UpdateCommand = pomUpdateKomanda;
            _adapterObjekat.DeleteCommand = pomDeleteKomanda;
        }

        private void KreirajDataset()
        {
            _dataSetObjekat = new DataSet();
            _adapterObjekat.Fill(_dataSetObjekat, _nazivTabele);
        }

        private void ZatvoriAdapterDataset()
        {
            _adapterObjekat.Dispose();
            _dataSetObjekat.Dispose();
        }

        #endregion

        #region Javne metode

        public DataSet DajPodatke(string selectUpit)
        {
            KreirajAdapter(selectUpit, "", "", "");
            KreirajDataset();
            return _dataSetObjekat;
        }

        public int DajBrojSlogova()
        {
            int brojSlogova = _dataSetObjekat.Tables[0].Rows.Count;
            return brojSlogova;
        }

        public bool IzvrsiAzuriranje(string upit)
        {
            bool uspeh = false;
            SqlConnection pomKonekcija;
            SqlCommand pomKomanda;
            SqlTransaction pomTransakcija = null;
            try
            {
                pomKonekcija = _konekcijaObjekat.DajKonekciju();
                pomKomanda = new SqlCommand();
                pomKomanda.Connection = pomKonekcija;
                pomKomanda = pomKonekcija.CreateCommand();
                pomTransakcija = pomKonekcija.BeginTransaction();
                pomKomanda.Transaction = pomTransakcija;
                pomKomanda.CommandText = upit;
                pomKomanda.ExecuteNonQuery();
                pomTransakcija.Commit();
                uspeh = true;
            }
            catch
            {
                pomTransakcija?.Rollback();
                uspeh = false;
            }
            return uspeh;
        }

        public bool IzvrsiAzuriranje(List<string> listaUpita)
        {
            bool uspeh = false;
            SqlConnection pomKonekcija;
            SqlCommand pomKomanda;
            SqlTransaction pomTransakcija = null;
            try
            {
                pomKonekcija = _konekcijaObjekat.DajKonekciju();
                pomKomanda = new SqlCommand();
                pomKomanda.Connection = pomKonekcija;
                pomKomanda = pomKonekcija.CreateCommand();
                string pomUpit = "";
                pomTransakcija = pomKonekcija.BeginTransaction();
                pomKomanda.Transaction = pomTransakcija;
                for (int i = 0; i < listaUpita.Count; i++)
                {
                    pomUpit = listaUpita[i];
                    pomKomanda.CommandText = pomUpit;
                    pomKomanda.ExecuteNonQuery();
                }
                pomTransakcija.Commit();
                uspeh = true;
            }
            catch
            {
                pomTransakcija?.Rollback();
                uspeh = false;
            }
            return uspeh;
        }

        #endregion
    }
}
