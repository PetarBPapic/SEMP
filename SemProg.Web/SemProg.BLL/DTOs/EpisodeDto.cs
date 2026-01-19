namespace SemProg.BLL.DTOs
{
    public class EpisodeDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; } = DateTime.Today;
    }
}
