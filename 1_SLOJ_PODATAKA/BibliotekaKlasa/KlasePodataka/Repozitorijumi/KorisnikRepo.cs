using Microsoft.Data.SqlClient;
using BibliotekaKlasa.KlasePodataka.Modeli;
using BibliotekaKlasa.TehnoloskeKlase;

namespace BibliotekaKlasa.KlasePodataka.Repozitorijumi
{
    public class KorisnikRepo : BaseRepo
    {
        public KorisnikRepo(KonekcijaKlasa konekcijaObjekat) : base(konekcijaObjekat)
        {
        }

        public void Dodaj(KorisnikModel korisnikModelObjekat)
        {
            _konekcijaObjekat.OtvoriKonekciju();
            string upit = @"INSERT INTO Korisnici (KorisnickoIme, Lozinka, Uloga) 
                            VALUES (@KorisnickoIme, @Lozinka, @Uloga)";
            using var komanda = new SqlCommand(upit, _konekcijaObjekat.DajKonekciju());
            komanda.Parameters.AddWithValue("@KorisnickoIme", korisnikModelObjekat.KorisnickoIme);
            komanda.Parameters.AddWithValue("@Lozinka", korisnikModelObjekat.Lozinka);
            komanda.Parameters.AddWithValue("@Uloga", korisnikModelObjekat.Uloga);
            komanda.ExecuteNonQuery();
            _konekcijaObjekat.ZatvoriKonekciju();
        }

        public void Izmeni(KorisnikModel korisnikModelObjekat)
        {
            _konekcijaObjekat.OtvoriKonekciju();
            string upit = @"UPDATE Korisnici SET 
                            KorisnickoIme=@KorisnickoIme, Lozinka=@Lozinka, Uloga=@Uloga 
                            WHERE Id=@Id";
            using var komanda = new SqlCommand(upit, _konekcijaObjekat.DajKonekciju());
            komanda.Parameters.AddWithValue("@KorisnickoIme", korisnikModelObjekat.KorisnickoIme);
            komanda.Parameters.AddWithValue("@Lozinka", korisnikModelObjekat.Lozinka);
            komanda.Parameters.AddWithValue("@Uloga", korisnikModelObjekat.Uloga);
            komanda.Parameters.AddWithValue("@Id", korisnikModelObjekat.Id);
            komanda.ExecuteNonQuery();
            _konekcijaObjekat.ZatvoriKonekciju();
        }

        public void Obrisi(int id)
        {
            _konekcijaObjekat.OtvoriKonekciju();
            string upit = "DELETE FROM Korisnici WHERE Id=@Id";
            using var komanda = new SqlCommand(upit, _konekcijaObjekat.DajKonekciju());
            komanda.Parameters.AddWithValue("@Id", id);
            komanda.ExecuteNonQuery();
            _konekcijaObjekat.ZatvoriKonekciju();
        }

        public List<KorisnikModel> DajSve()
        {
            var lista = new List<KorisnikModel>();
            _konekcijaObjekat.OtvoriKonekciju();
            string upit = "SELECT Id, KorisnickoIme, Lozinka, Uloga FROM Korisnici";
            using var komanda = new SqlCommand(upit, _konekcijaObjekat.DajKonekciju());
            using var citac = komanda.ExecuteReader();
            while (citac.Read())
            {
                lista.Add(new KorisnikModel
                {
                    Id = citac.GetInt32(0),
                    KorisnickoIme = citac.GetString(1),
                    Lozinka = citac.GetString(2),
                    Uloga = citac.GetString(3)
                });
            }
            _konekcijaObjekat.ZatvoriKonekciju();
            return lista;
        }

        public KorisnikModel? DajPoKorisnickomImenu(string korisnickoIme)
        {
            _konekcijaObjekat.OtvoriKonekciju();
            string upit = "SELECT Id, KorisnickoIme, Lozinka, Uloga FROM Korisnici WHERE KorisnickoIme=@KorisnickoIme";
            using var komanda = new SqlCommand(upit, _konekcijaObjekat.DajKonekciju());
            komanda.Parameters.AddWithValue("@KorisnickoIme", korisnickoIme);
            using var citac = komanda.ExecuteReader();
            KorisnikModel? korisnik = null;
            if (citac.Read())
            {
                korisnik = new KorisnikModel
                {
                    Id = citac.GetInt32(0),
                    KorisnickoIme = citac.GetString(1),
                    Lozinka = citac.GetString(2),
                    Uloga = citac.GetString(3)
                };
            }
            _konekcijaObjekat.ZatvoriKonekciju();
            return korisnik;
        }
    }
}
