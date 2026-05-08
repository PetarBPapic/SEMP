using Microsoft.Data.SqlClient;
using BibliotekaKlasa.KlasePodataka.Modeli;
using BibliotekaKlasa.TehnoloskeKlase;

namespace BibliotekaKlasa.KlasePodataka.Repozitorijumi
{
    /// <summary>
    /// Repozitorijum za Ocene - koristi TabelaKlasa (DataSet pristup) kao drugi primer nasledjivanja u DAL-u.
    /// </summary>
    public class OcenaRepo
    {
        public KonekcijaKlasa _konekcijaObjekat { get; set; }
        private TabelaKlasa _tabelaObjekat { get; set; }

        public OcenaRepo(KonekcijaKlasa konekcijaObjekat)
        {
            _konekcijaObjekat = konekcijaObjekat;
            _tabelaObjekat = new TabelaKlasa(konekcijaObjekat, "Ocene");
        }

        public void Dodaj(OcenaModel ocenaModelObjekat)
        {
            _konekcijaObjekat.OtvoriKonekciju();
            string upit = @"INSERT INTO Ocene (EpizodaId, KorisnikId, Vrednost, Komentar, OcenjeneNa)
                            VALUES (@EpizodaId, @KorisnikId, @Vrednost, @Komentar, @OcenjeneNa)";
            using var komanda = new SqlCommand(upit, _konekcijaObjekat.DajKonekciju());
            komanda.Parameters.AddWithValue("@EpizodaId", ocenaModelObjekat.EpizodaId);
            komanda.Parameters.AddWithValue("@KorisnikId", ocenaModelObjekat.KorisnikId);
            komanda.Parameters.AddWithValue("@Vrednost", ocenaModelObjekat.Vrednost);
            komanda.Parameters.AddWithValue("@Komentar", ocenaModelObjekat.Komentar ?? "");
            komanda.Parameters.AddWithValue("@OcenjeneNa", ocenaModelObjekat.OcenjeneNa);
            komanda.ExecuteNonQuery();
            _konekcijaObjekat.ZatvoriKonekciju();
        }

        public void Izmeni(OcenaModel ocenaModelObjekat)
        {
            _konekcijaObjekat.OtvoriKonekciju();
            string upit = @"UPDATE Ocene SET 
                            Vrednost=@Vrednost, Komentar=@Komentar, OcenjeneNa=@OcenjeneNa
                            WHERE Id=@Id";
            using var komanda = new SqlCommand(upit, _konekcijaObjekat.DajKonekciju());
            komanda.Parameters.AddWithValue("@Vrednost", ocenaModelObjekat.Vrednost);
            komanda.Parameters.AddWithValue("@Komentar", ocenaModelObjekat.Komentar ?? "");
            komanda.Parameters.AddWithValue("@OcenjeneNa", ocenaModelObjekat.OcenjeneNa);
            komanda.Parameters.AddWithValue("@Id", ocenaModelObjekat.Id);
            komanda.ExecuteNonQuery();
            _konekcijaObjekat.ZatvoriKonekciju();
        }

        public void IzmeniIliDodaj(OcenaModel ocenaModelObjekat)
        {
            var postojeca = DajPoEpizodaIdIKorisnikId(ocenaModelObjekat.EpizodaId, ocenaModelObjekat.KorisnikId);
            if (postojeca != null)
            {
                ocenaModelObjekat.Id = postojeca.Id;
                Izmeni(ocenaModelObjekat);
            }
            else
            {
                Dodaj(ocenaModelObjekat);
            }
        }

        public void Obrisi(int id)
        {
            _konekcijaObjekat.OtvoriKonekciju();
            string upit = "DELETE FROM Ocene WHERE Id=@Id";
            using var komanda = new SqlCommand(upit, _konekcijaObjekat.DajKonekciju());
            komanda.Parameters.AddWithValue("@Id", id);
            komanda.ExecuteNonQuery();
            _konekcijaObjekat.ZatvoriKonekciju();
        }

        /// <summary>
        /// Brise sve ocene za datu epizodu. Mora se pozvati pre brisanja epizode
        /// zbog FK_Ocene_Epizode constraint-a u bazi.
        /// </summary>
        public void ObrisiZaEpizodu(int epizodaId)
        {
            _konekcijaObjekat.OtvoriKonekciju();
            string upit = "DELETE FROM Ocene WHERE EpizodaId=@EpizodaId";
            using var komanda = new SqlCommand(upit, _konekcijaObjekat.DajKonekciju());
            komanda.Parameters.AddWithValue("@EpizodaId", epizodaId);
            komanda.ExecuteNonQuery();
            _konekcijaObjekat.ZatvoriKonekciju();
        }

        public List<OcenaModel> DajSve()
        {
            var lista = new List<OcenaModel>();
            _konekcijaObjekat.OtvoriKonekciju();
            string upit = "SELECT Id, EpizodaId, KorisnikId, Vrednost, Komentar, OcenjeneNa FROM Ocene";
            using var komanda = new SqlCommand(upit, _konekcijaObjekat.DajKonekciju());
            using var citac = komanda.ExecuteReader();
            while (citac.Read())
            {
                lista.Add(new OcenaModel
                {
                    Id = citac.GetInt32(0),
                    EpizodaId = citac.GetInt32(1),
                    KorisnikId = citac.GetInt32(2),
                    Vrednost = citac.GetInt32(3),
                    Komentar = citac.IsDBNull(4) ? "" : citac.GetString(4),
                    OcenjeneNa = citac.GetDateTime(5)
                });
            }
            _konekcijaObjekat.ZatvoriKonekciju();
            return lista;
        }

        public List<OcenaModel> DajZaEpizodu(int epizodaId)
        {
            var lista = new List<OcenaModel>();
            _konekcijaObjekat.OtvoriKonekciju();
            string upit = @"SELECT Id, EpizodaId, KorisnikId, Vrednost, Komentar, OcenjeneNa 
                            FROM Ocene WHERE EpizodaId=@EpizodaId";
            using var komanda = new SqlCommand(upit, _konekcijaObjekat.DajKonekciju());
            komanda.Parameters.AddWithValue("@EpizodaId", epizodaId);
            using var citac = komanda.ExecuteReader();
            while (citac.Read())
            {
                lista.Add(new OcenaModel
                {
                    Id = citac.GetInt32(0),
                    EpizodaId = citac.GetInt32(1),
                    KorisnikId = citac.GetInt32(2),
                    Vrednost = citac.GetInt32(3),
                    Komentar = citac.IsDBNull(4) ? "" : citac.GetString(4),
                    OcenjeneNa = citac.GetDateTime(5)
                });
            }
            _konekcijaObjekat.ZatvoriKonekciju();
            return lista;
        }

        public OcenaModel? DajPoEpizodaIdIKorisnikId(int epizodaId, int korisnikId)
        {
            _konekcijaObjekat.OtvoriKonekciju();
            string upit = @"SELECT Id, EpizodaId, KorisnikId, Vrednost, Komentar, OcenjeneNa 
                            FROM Ocene WHERE EpizodaId=@EpizodaId AND KorisnikId=@KorisnikId";
            using var komanda = new SqlCommand(upit, _konekcijaObjekat.DajKonekciju());
            komanda.Parameters.AddWithValue("@EpizodaId", epizodaId);
            komanda.Parameters.AddWithValue("@KorisnikId", korisnikId);
            using var citac = komanda.ExecuteReader();
            OcenaModel? ocena = null;
            if (citac.Read())
            {
                ocena = new OcenaModel
                {
                    Id = citac.GetInt32(0),
                    EpizodaId = citac.GetInt32(1),
                    KorisnikId = citac.GetInt32(2),
                    Vrednost = citac.GetInt32(3),
                    Komentar = citac.IsDBNull(4) ? "" : citac.GetString(4),
                    OcenjeneNa = citac.GetDateTime(5)
                };
            }
            _konekcijaObjekat.ZatvoriKonekciju();
            return ocena;
        }

        /// <summary>
        /// Koristi TabelaKlasa (DataSet pristup) za citanje top epizoda - drugi pristup u DAL-u
        /// </summary>
        public List<TopEpizodaModel> DajTopEpizodeDataSet(int top)
        {
            var lista = new List<TopEpizodaModel>();
            _konekcijaObjekat.OtvoriKonekciju();
            string upit = $@"SELECT TOP {top} 
                                e.Id, e.Naslov, e.Opis, e.DatumPremijere,
                                AVG(CAST(o.Vrednost AS FLOAT)) AS ProsecnaOcena,
                                COUNT(o.Id) AS BrojOcena
                             FROM Epizode e
                             LEFT JOIN Ocene o ON e.Id = o.EpizodaId
                             GROUP BY e.Id, e.Naslov, e.Opis, e.DatumPremijere
                             HAVING COUNT(o.Id) > 0
                             ORDER BY ProsecnaOcena DESC, BrojOcena DESC";

            var dataset = _tabelaObjekat.DajPodatke(upit);
            if (dataset.Tables.Count > 0)
            {
                foreach (System.Data.DataRow red in dataset.Tables[0].Rows)
                {
                    lista.Add(new TopEpizodaModel
                    {
                        EpizodaId = Convert.ToInt32(red["Id"]),
                        Naslov = red["Naslov"].ToString() ?? "",
                        Opis = red["Opis"].ToString() ?? "",
                        DatumPremijere = Convert.ToDateTime(red["DatumPremijere"]),
                        ProsecnaOcena = Convert.ToDouble(red["ProsecnaOcena"]),
                        BrojOcena = Convert.ToInt32(red["BrojOcena"])
                    });
                }
            }
            _konekcijaObjekat.ZatvoriKonekciju();
            return lista;
        }
    }
}