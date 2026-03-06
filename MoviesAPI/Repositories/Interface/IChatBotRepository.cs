using MoviesAPI.Domain.Entities.Faqs;

namespace MoviesAPI.Repositories.Interface
{
    public interface IChatBotRepository
    {
        Task<List<Faq>> GetAllFaqAsync();
        Task<Faq?> GetFaqByIdAsync(Guid id);
        Task<Faq> AddFaqAsync(Faq faq);
        Task UpdateFaqAsync(Faq faq);
        Task DeleteFaqAsync(Faq faq);
        Task<Faq?> GetClosestMatchToFaqAsync(string userQuestion);
    }
}
