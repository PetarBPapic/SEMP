namespace SemProg.DAL.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public int EpisodeId { get; set; }
        public Episode Episode { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int Score { get; set; }
        public string Comment { get; set; }
        public DateTime RatedAt { get; set; } = DateTime.Now;
    }
}
