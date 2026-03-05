using AutoMapper;
using MoviesAPI.Application.DTOs.Requests.Screenings;
using MoviesAPI.Application.DTOs.Responses.Screenings;
using MoviesAPI.Models;

namespace MoviesAPI.Application.Mappings
{
    public class ScreeningProfile : Profile
    {
        public ScreeningProfile()
        {
            // Entity to Response
            CreateMap<Screening, MoviesAPI.Application.DTOs.Responses.Screenings.ScreeningResponse>()
                .ForMember(dest => dest.ScreeningDateTime, opt => opt.MapFrom(src => src.Screening_Date_Time))
                .ForMember(dest => dest.MovieId, opt => opt.MapFrom(src => src.Movie_Id))
                .ForMember(dest => dest.HallId, opt => opt.MapFrom(src => src.Hall_Id))
                .ForMember(dest => dest.TotalTickets, opt => opt.MapFrom(src => src.Total_Tickets))
                .ForMember(dest => dest.AvailableTickets, opt => opt.MapFrom(src => src.Available_Tickets))
                .ForMember(dest => dest.BookedTickets, opt => opt.MapFrom(src => src.Total_Tickets - src.Available_Tickets))
                .ForMember(dest => dest.OccupancyPercentage, opt => opt.MapFrom(src => 
                    src.Total_Tickets > 0 ? ((src.Total_Tickets - src.Available_Tickets) * 100.0m / src.Total_Tickets) : 0))
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.Available_Tickets > 0))
                .ForMember(dest => dest.Movie, opt => opt.Ignore())
                .ForMember(dest => dest.Hall, opt => opt.Ignore());

            CreateMap<Screening, ScreeningSummaryResponse>()
                .ForMember(dest => dest.ScreeningDateTime, opt => opt.MapFrom(src => src.Screening_Date_Time))
                .ForMember(dest => dest.AvailableTickets, opt => opt.MapFrom(src => src.Available_Tickets))
                .ForMember(dest => dest.MovieName, opt => opt.Ignore())
                .ForMember(dest => dest.PosterPath, opt => opt.Ignore())
                .ForMember(dest => dest.HallName, opt => opt.Ignore())
                .ForMember(dest => dest.Price, opt => opt.Ignore());

            // Request to Entity
            CreateMap<CreateScreeningRequest, Screening>()
                .ForMember(dest => dest.Movie_Id, opt => opt.MapFrom(src => src.MovieId))
                .ForMember(dest => dest.Hall_Id, opt => opt.MapFrom(src => src.HallId))
                .ForMember(dest => dest.Screening_Date_Time, opt => opt.MapFrom(src => src.ScreeningDateTime))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Total_Tickets, opt => opt.Ignore())
                .ForMember(dest => dest.Available_Tickets, opt => opt.Ignore());

            CreateMap<UpdateScreeningRequest, Screening>()
                .ForMember(dest => dest.Movie_Id, opt => opt.MapFrom(src => src.MovieId))
                .ForMember(dest => dest.Hall_Id, opt => opt.MapFrom(src => src.HallId))
                .ForMember(dest => dest.Screening_Date_Time, opt => opt.MapFrom(src => src.ScreeningDateTime))
                .ForMember(dest => dest.Total_Tickets, opt => opt.MapFrom(src => src.TotalTickets))
                .ForMember(dest => dest.Available_Tickets, opt => opt.MapFrom(src => src.AvailableTickets))
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // CreateScreening to Screening (for backward compatibility)
            CreateMap<CreateScreening, Screening>()
                .ForMember(dest => dest.Screening_Date_Time, opt => opt.MapFrom(src => src.Screening_Date_Time));

            // Movie to MovieInfo
            CreateMap<Movie, MovieInfo>()
                .ForMember(dest => dest.PosterPath, opt => opt.MapFrom(src => src.Poster_Path));

            // Hall to HallInfo
            CreateMap<Hall, HallInfo>()
                .ForMember(dest => dest.TotalSeats, opt => opt.MapFrom(src => src.HallSeats.Count));

            // SeatForScreeningDto to SeatInfo
            CreateMap<SeatForScreeningDto, SeatInfo>()
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => !src.UserId.HasValue))
                .ForMember(dest => dest.BookedByUserId, opt => opt.MapFrom(src => src.UserId));
        }
    }
}
