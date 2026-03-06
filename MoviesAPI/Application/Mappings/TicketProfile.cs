using AutoMapper;
using MoviesAPI.Application.DTOs.Requests.Tickets;
using MoviesAPI.Application.DTOs.Responses.Tickets;
using MoviesAPI.Domain.Entities.Tickets;

namespace MoviesAPI.Application.Mappings
{
    public class TicketProfile : Profile
    {
        public TicketProfile()
        {
            // Entity to Response
            CreateMap<Ticket, MoviesAPI.Application.DTOs.Responses.Tickets.TicketResponse>()
                .ForMember(dest => dest.WatchDateTime, opt => opt.MapFrom(src => src.WatchDateTime))
                .ForMember(dest => dest.MovieId, opt => opt.Ignore()) // Will be set manually if needed
                .ForMember(dest => dest.UserId, opt => opt.Ignore()) // Will be set manually if needed
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.MovieName, opt => opt.MapFrom(src => src.Movie != null ? src.Movie.Name : null))
                .ForMember(dest => dest.PosterPath, opt => opt.MapFrom(src => src.Movie != null ? src.Movie.PosterPath : null))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.Name : null))
                .ForMember(dest => dest.HallName, opt => opt.MapFrom(src => src.HallSeat != null && src.HallSeat.Hall != null ? src.HallSeat.Hall.Name : null))
                .ForMember(dest => dest.Row, opt => opt.MapFrom(src => src.HallSeat != null ? src.HallSeat.RowNumber : 0))
                .ForMember(dest => dest.SeatNumber, opt => opt.MapFrom(src => src.HallSeat != null ? src.HallSeat.SeatNumber : 0))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.BookedAt, opt => opt.MapFrom(src => src.CreatedAt));

            CreateMap<Ticket, TicketSummaryResponse>()
                .ForMember(dest => dest.WatchDateTime, opt => opt.MapFrom(src => src.WatchDateTime))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.MovieName, opt => opt.MapFrom(src => src.Movie != null ? src.Movie.Name : null))
                .ForMember(dest => dest.PosterPath, opt => opt.MapFrom(src => src.Movie != null ? src.Movie.PosterPath : null))
                .ForMember(dest => dest.HallName, opt => opt.MapFrom(src => src.HallSeat != null && src.HallSeat.Hall != null ? src.HallSeat.Hall.Name : null))
                .ForMember(dest => dest.Row, opt => opt.MapFrom(src => src.HallSeat != null ? src.HallSeat.RowNumber : 0))
                .ForMember(dest => dest.SeatNumber, opt => opt.MapFrom(src => src.HallSeat != null ? src.HallSeat.SeatNumber : 0));

            CreateMap<Ticket, TicketDetailsResponse>()
                .ForMember(dest => dest.Watch_Movie, opt => opt.MapFrom(src => src.WatchDateTime))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.MovieName, opt => opt.MapFrom(src => src.Movie != null ? src.Movie.Name : null))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.Name : null))
                .ForMember(dest => dest.PosterPath, opt => opt.MapFrom(src => src.Movie != null ? src.Movie.PosterPath : null))
                .ForMember(dest => dest.HallName, opt => opt.MapFrom(src => src.HallSeat != null && src.HallSeat.Hall != null ? src.HallSeat.Hall.Name : null))
                .ForMember(dest => dest.Row, opt => opt.MapFrom(src => src.HallSeat != null ? src.HallSeat.RowNumber : 0))
                .ForMember(dest => dest.Column, opt => opt.MapFrom(src => src.HallSeat != null ? src.HallSeat.SeatNumber : 0));

            // Request to Entity
            CreateMap<CreateTicketRequest, Ticket>()
                .ForMember(dest => dest.MovieId, opt => opt.Ignore()) // Will be set manually
                .ForMember(dest => dest.UserId, opt => opt.Ignore()) // Will be set manually
                .ForMember(dest => dest.WatchDateTime, opt => opt.MapFrom(src => src.WatchDateTime))
                .ForMember(dest => dest.HallSeatId, opt => opt.Ignore()) // Will be set manually
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Price, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.Movie, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.HallSeat, opt => opt.Ignore());
        }
    }
}
