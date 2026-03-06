using MoviesAPI.Service.Interface;
using System.Text;

namespace MoviesAPI.Service.Implementation
{
    public class ChatBotRagService : IChatBotRagService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOpenAIService _openAIService;

        public ChatBotRagService(
            IUnitOfWork unitOfWork,
            IOpenAIService openAIService)
        {
            _unitOfWork = unitOfWork;
            _openAIService = openAIService;
        }

        public async Task<string> AskQuestionAsync(string userQuestion)
        {
            var contextBuilder = new StringBuilder();

            var faqs = await _unitOfWork.ChatBot.GetAllFaqAsync();
            var matchedFaq = faqs.FirstOrDefault(f =>
                f.Question.ToLower().Contains(userQuestion.ToLower()));

            if (matchedFaq != null)
            {
                contextBuilder.AppendLine($"FAQ: {matchedFaq.Question}");
                contextBuilder.AppendLine($"Answer: {matchedFaq.Answer}");
                contextBuilder.AppendLine();
            }

            DateOnly date = DateOnly.FromDateTime(DateTime.Now);
            if (userQuestion.ToLower().Contains("tomorrow"))
                date = date.AddDays(1);

            var screenings = await _unitOfWork.Screenings.GetScreeningsAsync();

            if (screenings != null && screenings.Any())
            {
                contextBuilder.AppendLine($"🎬 Screenings for {date}:");

                foreach (var s in screenings.Where(s =>
                    s.ScreeningDateTime.Date == date.ToDateTime(TimeOnly.MinValue).Date))
                {
                    var movie = await _unitOfWork.Movies.GetMovieAsync(s.MovieId);
                    if (movie == null) continue;

                    var hallSeats = await _unitOfWork.Halls.GetSeatsByHallIdAsync(s.HallId);

                    // Count tickets for this screening
                    var tickets = await _unitOfWork.Tickets.GetTicketsAsync();
                    int purchasedTickets = tickets.Count(t => t.MovieId == s.MovieId && t.WatchDateTime.Date == s.ScreeningDateTime.Date);

                    int availableTickets = hallSeats.Count - purchasedTickets;

                    string ticketInfo = availableTickets > 0
                        ? $"{availableTickets}/{hallSeats.Count} tickets available"
                        : "Sold out";

                    contextBuilder.AppendLine(
                        $"- {movie.Name} at {s.ScreeningDateTime:HH:mm}, {ticketInfo}"
                    );
                }

                contextBuilder.AppendLine();
            }
            else
            {
                contextBuilder.AppendLine($"No screenings found for {date}.");
            }

            if (contextBuilder.Length == 0)
            {
                contextBuilder.AppendLine("No relevant FAQ or movie data found.");
            }

            string prompt = $@"
                You are a friendly cinema assistant.
                Use the context below to answer the user's question as helpfully as possible.

                Context:
                {contextBuilder}

                User Question: {userQuestion}
                Answer:";

            var answer = await _openAIService.AskQuestionAsync(prompt);
            return answer;
        }
    }
}
