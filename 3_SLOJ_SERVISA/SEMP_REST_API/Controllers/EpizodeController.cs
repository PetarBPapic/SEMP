using Microsoft.AspNetCore.Mvc;
using BibliotekaKlasa.KlasePodataka.Modeli;
using BibliotekaKlasa.Servisi;
using PoslovnaLogika.PoslovniProcessi;

namespace SEMP_REST_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EpizodeController : ControllerBase
    {
        private readonly IConfiguration _konfiguracija;
        private readonly IWebHostEnvironment _okruzenje;

        public EpizodeController(IConfiguration konfiguracija, IWebHostEnvironment okruzenje)
        {
            _konfiguracija = konfiguracija;
            _okruzenje = okruzenje;
        }

        private PoslovniProces KreirajPoslovniProces()
        {
            string konekcioniString = _konfiguracija.GetConnectionString("PodrazumevanaKonekcija")!;
            var xmlServis = new XmlTopListeServis(_okruzenje.WebRootPath);
            return new PoslovniProces(konekcioniString, xmlServis);
        }

        [HttpGet]
        public IActionResult DajSve()
        {
            var pp = KreirajPoslovniProces();
            return Ok(pp.DajSveEpizode());
        }

        [HttpGet("{id}")]
        public IActionResult DajPoId(int id)
        {
            var pp = KreirajPoslovniProces();
            var epizoda = pp.DajEpizodu(id);
            if (epizoda == null) return NotFound();
            return Ok(epizoda);
        }

        [HttpGet("top5")]
        public IActionResult DajTop5()
        {
            var xmlServis = new XmlTopListeServis(_okruzenje.WebRootPath);
            return Ok(xmlServis.UcitajTop5());
        }

        [HttpGet("top10")]
        public IActionResult DajTop10()
        {
            var xmlServis = new XmlTopListeServis(_okruzenje.WebRootPath);
            return Ok(xmlServis.UcitajTop10());
        }

        [HttpGet("sve-sortirane")]
        public IActionResult DajSveSortirane()
        {
            var pp = KreirajPoslovniProces();
            return Ok(pp.DajSveEpizodeSortirane());
        }

        [HttpPost]
        public IActionResult Dodaj([FromBody] EpizodaModel epizodaModelObjekat)
        {
            var pp = KreirajPoslovniProces();
            pp.DodajEpizodu(epizodaModelObjekat);
            return Ok(new { poruka = "Epizoda uspešno dodata." });
        }

        [HttpPut("{id}")]
        public IActionResult Izmeni(int id, [FromBody] EpizodaModel epizodaModelObjekat)
        {
            epizodaModelObjekat.Id = id;
            var pp = KreirajPoslovniProces();
            pp.IzmeniEpizodu(epizodaModelObjekat);
            return Ok(new { poruka = "Epizoda uspešno izmenjena." });
        }

        [HttpDelete("{id}")]
        public IActionResult Obrisi(int id)
        {
            var pp = KreirajPoslovniProces();
            pp.ObrisiEpizodu(id);
            return Ok(new { poruka = "Epizoda uspešno obrisana." });
        }
    }
}
