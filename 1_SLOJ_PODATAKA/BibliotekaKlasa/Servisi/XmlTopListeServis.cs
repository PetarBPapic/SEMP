using System.Xml.Linq;
using BibliotekaKlasa.KlasePodataka.Modeli;

namespace BibliotekaKlasa.Servisi
{
    /// <summary>
    /// Servis za upravljanje Top listama epizoda putem XML fajla (strani XML fajl).
    /// Poslovni proces: azurira Top5 i Top10 liste sa svakom novom ocenom i cuva ih trajno.
    /// </summary>
    public class XmlTopListeServis
    {
        private readonly string _putanjaXml;

        public XmlTopListeServis(string webRootPath)
        {
            string folderPodaci = Path.Combine(webRootPath, "podaci");
            if (!Directory.Exists(folderPodaci))
                Directory.CreateDirectory(folderPodaci);

            _putanjaXml = Path.Combine(folderPodaci, "top_epizode.xml");

            if (!File.Exists(_putanjaXml))
                KreirajPrazanXml();
        }

        private void KreirajPrazanXml()
        {
            var xml = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("TopListe",
                    new XAttribute("azuriranaNa", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                    new XElement("Top5"),
                    new XElement("Top10")
                )
            );
            xml.Save(_putanjaXml);
        }

        /// <summary>
        /// Azurira XML fajl sa novim Top5 i Top10 listama. Poziva se nakon svake ocene.
        /// </summary>
        public void AzurirajTopListe(List<TopEpizodaModel> sve)
        {
            var poredane = sve.OrderByDescending(e => e.ProsecnaOcena)
                              .ThenByDescending(e => e.BrojOcena)
                              .ToList();

            var top5 = poredane.Take(5).ToList();
            var top10 = poredane.Take(10).ToList();

            var xml = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("TopListe",
                    new XAttribute("azuriranaNa", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                    new XElement("Top5",
                        top5.Select((e, i) => KreirajEpizodaElement(e, i + 1))
                    ),
                    new XElement("Top10",
                        top10.Select((e, i) => KreirajEpizodaElement(e, i + 1))
                    )
                )
            );

            xml.Save(_putanjaXml);
        }

        private XElement KreirajEpizodaElement(TopEpizodaModel e, int mesto)
        {
            return new XElement("Epizoda",
                new XAttribute("mesto", mesto),
                new XElement("Id", e.EpizodaId),
                new XElement("Naslov", e.Naslov),
                new XElement("Opis", e.Opis),
                new XElement("ProsecnaOcena", e.ProsecnaOcena.ToString("F2")),
                new XElement("BrojOcena", e.BrojOcena),
                new XElement("DatumPremijere", e.DatumPremijere.ToString("dd.MM.yyyy"))
            );
        }

        public List<TopEpizodaModel> UcitajTop5()
        {
            return UcitajListu("Top5");
        }

        public List<TopEpizodaModel> UcitajTop10()
        {
            return UcitajListu("Top10");
        }

        private List<TopEpizodaModel> UcitajListu(string elementNaziv)
        {
            var lista = new List<TopEpizodaModel>();

            if (!File.Exists(_putanjaXml))
                return lista;

            try
            {
                var xml = XDocument.Load(_putanjaXml);
                var elementi = xml.Root?
                    .Element(elementNaziv)?
                    .Elements("Epizoda") ?? Enumerable.Empty<XElement>();

                foreach (var el in elementi)
                {
                    lista.Add(new TopEpizodaModel
                    {
                        EpizodaId = int.Parse(el.Element("Id")?.Value ?? "0"),
                        Naslov = el.Element("Naslov")?.Value ?? "",
                        Opis = el.Element("Opis")?.Value ?? "",
                        ProsecnaOcena = double.Parse(
                            el.Element("ProsecnaOcena")?.Value ?? "0",
                            System.Globalization.CultureInfo.InvariantCulture),
                        BrojOcena = int.Parse(el.Element("BrojOcena")?.Value ?? "0"),
                        DatumPremijere = DateTime.ParseExact(
                            el.Element("DatumPremijere")?.Value ?? "01.01.2000",
                            "dd.MM.yyyy",
                            System.Globalization.CultureInfo.InvariantCulture)
                    });
                }
            }
            catch
            {
                // Ako XML nije ispravan, vracamo praznu listu
            }

            return lista;
        }

        public string DajDatumAzuriranja()
        {
            if (!File.Exists(_putanjaXml))
                return "Nikad";

            try
            {
                var xml = XDocument.Load(_putanjaXml);
                return xml.Root?.Attribute("azuriranaNa")?.Value ?? "Nepoznato";
            }
            catch
            {
                return "Greška";
            }
        }
    }
}
