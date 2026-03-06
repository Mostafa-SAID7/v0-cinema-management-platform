using MoviesAPI.Application.DTOs.Requests.Email;

namespace MoviesAPI.Service.Interface
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailMessageRequest message);
    }
}
