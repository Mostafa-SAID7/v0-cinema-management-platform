using AutoMapper;
using MoviesAPI.Application.DTOs.Requests.Tickets;
using MoviesAPI.Application.DTOs.Responses.Tickets;
using MoviesAPI.Models;

namespace MoviesAPI.Application.Mappings
{
    public class TicketProfile : Profile
    {
        public TicketProfile()
        {
            // Entity to Response
            CreateMap<Ticket, MoviesAPI.Application.DTOs.Responses.Tickets.TicketResponse>()
                .ForMember(dest => dest.WatchDateTime, opt => opt.MapFrom(src => src.Watch_Movie))
                .ForMember(dest => dest.MovieId, opt => opt.MapFrom(src => src.Movie_Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User_Id))
                .ForMember(dest => dest.MovieName, opt => opt.Ignore())
                .ForMember(dest => dest.PosterPath, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.HallName, opt => opt.Ignore())
                .ForMember(dest => dest.Row, opt => opt.Ignore())
                .ForMember(dest => dest.SeatNumber, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.BookedAt, opt => opt.Ignore());

            CreateMap<Ticket, TicketSummaryResponse>()
                .ForMember(dest => dest.WatchDateTime, opt => opt.MapFrom(src => src.Watch_Movie))
                .ForMember(dest => dest.MovieName, opt => opt.Ignore())
                .ForMember(dest => dest.PosterPath, opt => opt.Ignore())
                .ForMember(dest => dest.HallName, opt => opt.Ignore())
                .ForMember(dest => dest.Row, opt => opt.Ignore())
                .ForMember(dest => dest.SeatNumber, opt => opt.Ignore());

            // Request to Entity
            CreateMap<CreateTicketRequest, Ticket>()
                .ForMember(dest => dest.Movie_Id, opt => opt.MapFrom(src => src.MovieId))
                .ForMember(dest => dest.User_Id, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Watch_Movie, opt => opt.MapFrom(src => src.WatchDateTime))
                .ForMember(dest => dest.hall_seat_id, opt => opt.MapFrom(src => src.HallSeatId))
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // CreateTicket to Ticket (for backward compatibility)
            CreateMap<CreateTicket, Ticket>();
        }
    }
}
