namespace MoviesAPI.Application.DTOs.Requests.Email
{
    /// <summary>
    /// Request DTO for sending emails
    /// </summary>
    public class EmailMessageRequest
    {
        public string? MailTo { get; set; }
        public string? Subject { get; set; }
        public string? Content { get; set; }
    }
}
