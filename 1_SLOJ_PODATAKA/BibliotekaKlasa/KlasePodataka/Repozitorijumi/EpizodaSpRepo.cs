using Microsoft.Data.SqlClient;
using BibliotekaKlasa.KlasePodataka.Modeli;
using BibliotekaKlasa.TehnoloskeKlase;
using System.Data;

namespace BibliotekaKlasa.KlasePodataka.Repozitorijumi
{
    public class EpizodaSpRepo
    {
        private readonly KonekcijaKlasa _konekcijaObjekat;

        public EpizodaSpRepo(KonekcijaKlasa konekcijaObjekat)
        {
            _konekcijaObjekat = konekcijaObjekat;
        }

        public List<EpizodaModel> DajSve()
        {
            var lista = new List<EpizodaModel>();
            _konekcijaObjekat.OtvoriKonekciju();

            using var komanda = new SqlCommand("sp_DajSveEpizode", _konekcijaObjekat.DajKonekciju());
            komanda.CommandType = CommandType.StoredProcedure;

            using var citac = komanda.ExecuteReader();
            while (citac.Read())
            {
                lista.Add(new EpizodaModel
                {
                    Id = citac.GetInt32(0),
                    Naslov = citac.GetString(1),
                    Opis = citac.GetString(2),
                    DatumPremijere = citac.GetDateTime(3),
                    KreiraoId = citac.GetInt32(4)
                });
            }

            _konekcijaObjekat.ZatvoriKonekciju();
            return lista;
        }

        public EpizodaModel? DajPoId(int id)
        {
            _konekcijaObjekat.OtvoriKonekciju();

            using var komanda = new SqlCommand("sp_DajEpizodu", _konekcijaObjekat.DajKonekciju());
            komanda.CommandType = CommandType.StoredProcedure;
            komanda.Parameters.AddWithValue("@Id", id);

            using var citac = komanda.ExecuteReader();
            EpizodaModel? epizoda = null;
            if (citac.Read())
            {
                epizoda = new EpizodaModel
                {
                    Id = citac.GetInt32(0),
                    Naslov = citac.GetString(1),
                    Opis = citac.GetString(2),
                    DatumPremijere = citac.GetDateTime(3),
                    KreiraoId = citac.GetInt32(4)
                };
            }

            _konekcijaObjekat.ZatvoriKonekciju();
            return epizoda;
        }

        public void Dodaj(EpizodaModel epizodaModelObjekat)
        {
            _konekcijaObjekat.OtvoriKonekciju();

            using var komanda = new SqlCommand("sp_DodajEpizodu", _konekcijaObjekat.DajKonekciju());
            komanda.CommandType = CommandType.StoredProcedure;
            komanda.Parameters.AddWithValue("@Naslov", epizodaModelObjekat.Naslov);
            komanda.Parameters.AddWithValue("@Opis", epizodaModelObjekat.Opis);
            komanda.Parameters.AddWithValue("@DatumPremijere", epizodaModelObjekat.DatumPremijere);
            komanda.Parameters.AddWithValue("@KreiraoId", epizodaModelObjekat.KreiraoId);
            komanda.Parameters.AddWithValue("@ZarnIdz", epizodaModelObjekat.KreiraoId);
            komanda.ExecuteNonQuery();

            _konekcijaObjekat.ZatvoriKonekciju();
        }

        public int DajBrojOcenaKorisnikaDanas(int korisnikId)
        {
            _konekcijaObjekat.OtvoriKonekciju();

            using var komanda = new SqlCommand("sp_BrojOcenaKorisnikaDanas", _konekcijaObjekat.DajKonekciju());
            komanda.CommandType = CommandType.StoredProcedure;
            komanda.Parameters.AddWithValue("@KorisnikId", korisnikId);

            int broj = Convert.ToInt32(komanda.ExecuteScalar());

            _konekcijaObjekat.ZatvoriKonekciju();
            return broj;
        }
    }
}
