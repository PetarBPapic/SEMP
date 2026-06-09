using Microsoft.Data.SqlClient;
using BibliotekaKlasa.KlasePodataka.Modeli;
using BibliotekaKlasa.TehnoloskeKlase;

namespace BibliotekaKlasa.KlasePodataka.Repozitorijumi
{
    public class ZarnRepo : BaseRepo
    {
        public ZarnRepo(KonekcijaKlasa konekcijaObjekat) : base(konekcijaObjekat)
        {
        }

        public List<ZarnModel> DajSve()
        {
            var lista = new List<ZarnModel>();
            _konekcijaObjekat.OtvoriKonekciju();
            string upit = "SELECT Idz, Naziv, Opis FROM Zarn ORDER BY Naziv";
            using var komanda = new SqlCommand(upit, _konekcijaObjekat.DajKonekciju());
            using var citac = komanda.ExecuteReader();
            while (citac.Read())
            {
                lista.Add(new ZarnModel
                {
                    Idz = citac.GetInt32(0),
                    Naziv = citac.GetString(1),
                    Opis = citac.GetString(2)
                });
            }
            _konekcijaObjekat.ZatvoriKonekciju();
            return lista;
        }
    }
}