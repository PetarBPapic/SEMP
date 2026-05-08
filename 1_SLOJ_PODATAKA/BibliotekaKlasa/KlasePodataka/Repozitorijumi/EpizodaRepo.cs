using Microsoft.Data.SqlClient;
using BibliotekaKlasa.KlasePodataka.Modeli;
using BibliotekaKlasa.TehnoloskeKlase;

namespace BibliotekaKlasa.KlasePodataka.Repozitorijumi
{
    public class EpizodaRepo
    {
        public KonekcijaKlasa _konekcijaObjekat { get; set; }

        public EpizodaRepo(KonekcijaKlasa konekcijaObjekat)
        {
            _konekcijaObjekat = konekcijaObjekat;
        }

        public void Dodaj(EpizodaModel epizodaModelObjekat)
        {
            _konekcijaObjekat.OtvoriKonekciju();
            string upit = @"INSERT INTO Epizode (Naslov, Opis, DatumPremijere, KreiraoId)
                            VALUES (@Naslov, @Opis, @DatumPremijere, @KreiraoId)";
            using var komanda = new SqlCommand(upit, _konekcijaObjekat.DajKonekciju());
            komanda.Parameters.AddWithValue("@Naslov", epizodaModelObjekat.Naslov);
            komanda.Parameters.AddWithValue("@Opis", epizodaModelObjekat.Opis);
            komanda.Parameters.AddWithValue("@DatumPremijere", epizodaModelObjekat.DatumPremijere);
            komanda.Parameters.AddWithValue("@KreiraoId", epizodaModelObjekat.KreiraoId);
            komanda.ExecuteNonQuery();
            _konekcijaObjekat.ZatvoriKonekciju();
        }

        public void Izmeni(EpizodaModel epizodaModelObjekat)
        {
            _konekcijaObjekat.OtvoriKonekciju();
            string upit = @"UPDATE Epizode SET 
                            Naslov=@Naslov, Opis=@Opis, DatumPremijere=@DatumPremijere
                            WHERE Id=@Id";
            using var komanda = new SqlCommand(upit, _konekcijaObjekat.DajKonekciju());
            komanda.Parameters.AddWithValue("@Naslov", epizodaModelObjekat.Naslov);
            komanda.Parameters.AddWithValue("@Opis", epizodaModelObjekat.Opis);
            komanda.Parameters.AddWithValue("@DatumPremijere", epizodaModelObjekat.DatumPremijere);
            komanda.Parameters.AddWithValue("@Id", epizodaModelObjekat.Id);
            komanda.ExecuteNonQuery();
            _konekcijaObjekat.ZatvoriKonekciju();
        }

        public void Obrisi(int id)
        {
            _konekcijaObjekat.OtvoriKonekciju();
            string upit = "DELETE FROM Epizode WHERE Id=@Id";
            using var komanda = new SqlCommand(upit, _konekcijaObjekat.DajKonekciju());
            komanda.Parameters.AddWithValue("@Id", id);
            komanda.ExecuteNonQuery();
            _konekcijaObjekat.ZatvoriKonekciju();
        }

        /// <summary>
        /// Resetuje IDENTITY brojac tabele Epizode na najveci postojeci ID.
        /// Na taj nacin sledeca dodata epizoda dobija prvi slobodan broj
        /// umesto da nastavlja od poslednjeg ikad koriscenog ID-a.
        /// </summary>
        public void ResetuiIdBrojac()
        {
            _konekcijaObjekat.OtvoriKonekciju();
            string upit = @"
                DECLARE @MaxId INT = ISNULL((SELECT MAX(Id) FROM Epizode), 0);
                DBCC CHECKIDENT ('Epizode', RESEED, @MaxId);";
            using var komanda = new SqlCommand(upit, _konekcijaObjekat.DajKonekciju());
            komanda.ExecuteNonQuery();
            _konekcijaObjekat.ZatvoriKonekciju();
        }

        public List<EpizodaModel> DajSve()
        {
            var lista = new List<EpizodaModel>();
            _konekcijaObjekat.OtvoriKonekciju();
            string upit = @"SELECT Id, Naslov, Opis, DatumPremijere, KreiraoId 
                            FROM Epizode 
                            ORDER BY DatumPremijere DESC";
            using var komanda = new SqlCommand(upit, _konekcijaObjekat.DajKonekciju());
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
            string upit = "SELECT Id, Naslov, Opis, DatumPremijere, KreiraoId FROM Epizode WHERE Id=@Id";
            using var komanda = new SqlCommand(upit, _konekcijaObjekat.DajKonekciju());
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
    }
}