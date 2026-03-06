using AutoMapper;
using MoviesAPI.Application.DTOs.Responses.Halls;
using MoviesAPI.Domain.Entities.Halls;

namespace MoviesAPI.Application.Mappings
{
    public class HallProfile : Profile
    {
        public HallProfile()
        {
            // Entity to Response
            CreateMap<Hall, HallResponse>()
                .ForMember(dest => dest.Rows, opt => opt.MapFrom(src => src.HallSeats != null ? src.HallSeats.Max(s => s.RowNumber) : 0))
                .ForMember(dest => dest.Seats_Per_Row, opt => opt.MapFrom(src => src.HallSeats != null ? src.HallSeats.Max(s => s.SeatNumber) : 0))
                .ForMember(dest => dest.Seats, opt => opt.MapFrom(src => src.HallSeats));

            CreateMap<HallSeat, HallSeatResponse>()
                .ForMember(dest => dest.SeatId, opt => opt.Ignore()) // Will be set manually if needed
                .ForMember(dest => dest.RowNumber, opt => opt.MapFrom(src => src.RowNumber))
                .ForMember(dest => dest.SeatNumber, opt => opt.MapFrom(src => src.SeatNumber));
        }
    }
}
