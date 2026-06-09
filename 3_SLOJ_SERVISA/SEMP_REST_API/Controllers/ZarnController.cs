using Microsoft.AspNetCore.Mvc;
using BibliotekaKlasa.Servisi;
using PoslovnaLogika.PoslovniProcessi;

namespace SEMP_REST_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ZarnController : ControllerBase
    {
        private readonly IConfiguration _konfiguracija;
        private readonly IWebHostEnvironment _okruzenje;

        public ZarnController(IConfiguration konfiguracija, IWebHostEnvironment okruzenje)
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
            return Ok(pp.DajSveZarne());
        }
    }
}