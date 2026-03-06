namespace MoviesAPI.Application.DTOs.Responses.Faqs
{
    /// <summary>
    /// Response DTO for FAQ
    /// </summary>
    public class FaqResponse
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Category { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
