using AutoMapper;
using MoviesAPI.Application.DTOs.Responses.Faqs;
using MoviesAPI.Domain.Entities.Faqs;

namespace MoviesAPI.Application.Mappings
{
    public class FaqProfile : Profile
    {
        public FaqProfile()
        {
            // Entity to Response
            CreateMap<Faq, FaqResponse>();
        }
    }
}
