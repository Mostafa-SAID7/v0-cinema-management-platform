using MoviesAPI.Domain.Common;

namespace MoviesAPI.Domain.Entities.Faqs
{
    /// <summary>
    /// FAQ entity - Aggregate Root
    /// </summary>
    public class Faq : BaseEntity
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Category { get; set; }
    }
}
