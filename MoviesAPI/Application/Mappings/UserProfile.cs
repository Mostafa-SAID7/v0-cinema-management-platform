using AutoMapper;
using MoviesAPI.Application.DTOs.Requests.Users;
using MoviesAPI.Application.DTOs.Responses.Users;
using MoviesAPI.Models.System;

namespace MoviesAPI.Application.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Entity to Response
            CreateMap<User, UserResponse>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<User, UserSummaryResponse>();

            CreateMap<User, UserProfileResponse>()
                .ForMember(dest => dest.TotalBookings, opt => opt.Ignore())
                .ForMember(dest => dest.UpcomingMovies, opt => opt.Ignore())
                .ForMember(dest => dest.MemberSince, opt => opt.Ignore());

            // Request to Entity
            CreateMap<RegisterUserRequest, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => "User"));

            CreateMap<RegisterUserRequest, RegisterRequest>()
                .ForMember(dest => dest.isActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => false));

            CreateMap<UpdateUserProfileRequest, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Username, opt => opt.Ignore())
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.EmailConfirmed, opt => opt.Ignore());

            // LoginRequest mapping
            CreateMap<MoviesAPI.Application.DTOs.Requests.Users.LoginRequest, MoviesAPI.Models.System.LoginRequest>();
        }
    }
}
