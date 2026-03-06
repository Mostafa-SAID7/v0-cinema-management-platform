using Microsoft.EntityFrameworkCore;
using MoviesAPI.Data;
using MoviesAPI.Domain.Entities.Faqs;
using MoviesAPI.Repositories.Interface;

namespace MoviesAPI.Repositories.Implementation
{
    public class ChatBotRepository : IChatBotRepository
    {
        private readonly ApplicationDbContext _context;

        public ChatBotRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Faq>> GetAllFaqAsync()
        {
            return await _context.Faqs
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Faq?> GetFaqByIdAsync(Guid id)
        {
            return await _context.Faqs
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Faq> AddFaqAsync(Faq faq)
        {
            await _context.Faqs.AddAsync(faq);
            return faq;
        }

        public Task UpdateFaqAsync(Faq faq)
        {
            _context.Faqs.Update(faq);
            return Task.CompletedTask;
        }

        public Task DeleteFaqAsync(Faq faq)
        {
            faq.SoftDelete();
            _context.Faqs.Update(faq);
            return Task.CompletedTask;
        }

        public async Task<Faq?> GetClosestMatchToFaqAsync(string userQuestion)
        {
            if (string.IsNullOrWhiteSpace(userQuestion))
                return null;

            var keywords = userQuestion.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            
            var faqs = await _context.Faqs
                .AsNoTracking()
                .ToListAsync();

            var bestMatch = faqs
                .Select(faq => new
                {
                    Faq = faq,
                    Score = keywords.Count(keyword => 
                        faq.Question.ToLower().Contains(keyword) || 
                        faq.Answer.ToLower().Contains(keyword))
                })
                .Where(x => x.Score > 0)
                .OrderByDescending(x => x.Score)
                .FirstOrDefault();

            return bestMatch?.Faq;
        }
    }
}
