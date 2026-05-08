using System.Text.Json;

namespace PoslovnaLogika.Klase
{
    /// <summary>
    /// Klasa Ogranicenja - ucitava i proverava poslovna pravila iz JSON fajla.
    /// Cuva ogranicenja kao sto je maksimalan broj ocena po korisniku dnevno.
    /// </summary>
    public class Ogranicenja
    {
        public int MaksOcenaPoKorisniku { get; set; } = 50;

        public int UzmiMaksOcenaPoKorisnikuIzJSON(string putanja)
        {
            if (!File.Exists(putanja))
                throw new Exception("JSON fajl sa ograničenjem nije pronađen.");

            string json = File.ReadAllText(putanja);
            var podaci = JsonSerializer.Deserialize<Dictionary<string, int>>(json);

            if (podaci == null || !podaci.ContainsKey("MaksOcenaPoKorisniku"))
                throw new Exception("MaksOcenaPoKorisniku nije definisano u JSON-u.");

            return podaci["MaksOcenaPoKorisniku"];
        }
    }
}
