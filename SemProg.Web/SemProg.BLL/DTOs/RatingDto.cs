namespace SemProg.BLL.DTOs
{
    public class RatingDto
    {
        public int EpisodeId { get; set; }
        public int Score { get; set; } // 1-5
        public string Comment { get; set; }
    }

    public class RatingViewDto
    {
        public string Username { get; set; }
        public int Score { get; set; }
        public string Comment { get; set; }
        public DateTime RatedAt { get; set; }
    }

    public class EpisodeWithStatsDto
    {
        public int EpisodeId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public double AvgScore { get; set; }
        public List<RatingViewDto> Ratings { get; set; }
    }
}
