namespace SemProg.Web.Modeli
{
    public class ModelGreske
    {
        public string? IdZahteva { get; set; }
        public bool PrikaziIdZahteva => !string.IsNullOrEmpty(IdZahteva);
    }
}
