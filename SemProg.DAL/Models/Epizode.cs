namespace SemProg.DAL.Models
{
    public class Episode
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int CreatedBy { get; set; }
        public User Creator { get; set; }
    }
}
