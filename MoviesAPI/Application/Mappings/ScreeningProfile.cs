using AutoMapper;
using MoviesAPI.Application.DTOs.Requests.Screenings;
using MoviesAPI.Application.DTOs.Responses.Screenings;
using MoviesAPI.Domain.Entities.Screenings;
using MoviesAPI.Domain.Entities.Movies;
using MoviesAPI.Domain.Entities.Halls;

namespace MoviesAPI.Application.Mappings
{
    public class ScreeningProfile : Profile
    {
        public ScreeningProfile()
        {
            // Entity to Response
            CreateMap<Screening, MoviesAPI.Application.DTOs.Responses.Screenings.ScreeningResponse>()
                .ForMember(dest => dest.ScreeningDateTime, opt => opt.MapFrom(src => src.ScreeningDateTime))
                .ForMember(dest => dest.MovieId, opt => opt.MapFrom(src => src.MovieId))
                .ForMember(dest => dest.HallId, opt => opt.MapFrom(src => src.HallId))
                .ForMember(dest => dest.TotalTickets, opt => opt.MapFrom(src => src.TotalTickets))
                .ForMember(dest => dest.AvailableTickets, opt => opt.MapFrom(src => src.AvailableTickets))
                .ForMember(dest => dest.BookedTickets, opt => opt.MapFrom(src => src.TotalTickets - src.AvailableTickets))
                .ForMember(dest => dest.OccupancyPercentage, opt => opt.MapFrom(src => 
                    src.TotalTickets > 0 ? ((src.TotalTickets - src.AvailableTickets) * 100.0m / src.TotalTickets) : 0))
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.AvailableTickets > 0))
                .ForMember(dest => dest.Movie, opt => opt.MapFrom(src => src.Movie))
                .ForMember(dest => dest.Hall, opt => opt.MapFrom(src => src.Hall));

            CreateMap<Screening, ScreeningSummaryResponse>()
                .ForMember(dest => dest.ScreeningDateTime, opt => opt.MapFrom(src => src.ScreeningDateTime))
                .ForMember(dest => dest.AvailableTickets, opt => opt.MapFrom(src => src.AvailableTickets))
                .ForMember(dest => dest.MovieName, opt => opt.MapFrom(src => src.Movie != null ? src.Movie.Name : null))
                .ForMember(dest => dest.PosterPath, opt => opt.MapFrom(src => src.Movie != null ? src.Movie.PosterPath : null))
                .ForMember(dest => dest.HallName, opt => opt.MapFrom(src => src.Hall != null ? src.Hall.Name : null))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Movie != null ? src.Movie.Amount : 0));

            // Request to Entity
            CreateMap<CreateScreeningRequest, Screening>()
                .ForMember(dest => dest.MovieId, opt => opt.Ignore()) // Will be set manually
                .ForMember(dest => dest.HallId, opt => opt.Ignore()) // Will be set manually
                .ForMember(dest => dest.ScreeningDateTime, opt => opt.MapFrom(src => src.ScreeningDateTime))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.TotalTickets, opt => opt.Ignore())
                .ForMember(dest => dest.AvailableTickets, opt => opt.Ignore())
                .ForMember(dest => dest.Movie, opt => opt.Ignore())
                .ForMember(dest => dest.Hall, opt => opt.Ignore())
                .ForMember(dest => dest.Tickets, opt => opt.Ignore());

            CreateMap<UpdateScreeningRequest, Screening>()
                .ForMember(dest => dest.MovieId, opt => opt.Ignore()) // Will be set manually
                .ForMember(dest => dest.HallId, opt => opt.Ignore()) // Will be set manually
                .ForMember(dest => dest.ScreeningDateTime, opt => opt.MapFrom(src => src.ScreeningDateTime))
                .ForMember(dest => dest.TotalTickets, opt => opt.MapFrom(src => src.TotalTickets))
                .ForMember(dest => dest.AvailableTickets, opt => opt.MapFrom(src => src.AvailableTickets))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Movie, opt => opt.Ignore())
                .ForMember(dest => dest.Hall, opt => opt.Ignore())
                .ForMember(dest => dest.Tickets, opt => opt.Ignore());

            // Movie to MovieInfo
            CreateMap<Movie, MovieInfo>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.PosterPath, opt => opt.MapFrom(src => src.PosterPath))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount));

            // Hall to HallInfo
            CreateMap<Hall, HallInfo>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.TotalSeats, opt => opt.MapFrom(src => src.HallSeats != null ? src.HallSeats.Count : 0));

            // HallSeat to SeatInfo
            CreateMap<HallSeat, SeatInfo>()
                .ForMember(dest => dest.HallSeatId, opt => opt.Ignore()) // Will be set manually
                .ForMember(dest => dest.RowNumber, opt => opt.MapFrom(src => src.RowNumber))
                .ForMember(dest => dest.SeatNumber, opt => opt.MapFrom(src => src.SeatNumber))
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.BookedByUserId, opt => opt.Ignore());
        }
    }
}
