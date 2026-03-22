namespace SemProg.Web.Modeli
{
    public class ModelGreske
    {
        public string? RequestId { get; set; }
        public bool PrikaziRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
