using AutoMapper;
using MoviesAPI.Application.DTOs.Requests.Users;
using MoviesAPI.Application.DTOs.Responses.Users;
using MoviesAPI.Domain.Entities.Users;

namespace MoviesAPI.Application.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Entity to Response
            CreateMap<User, UserResponse>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber ?? src.Phone))
                .ForMember(dest => dest.LastLoginAt, opt => opt.Ignore());

            CreateMap<User, UserSummaryResponse>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber ?? src.Phone));

            CreateMap<User, UserProfileResponse>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber ?? src.Phone))
                .ForMember(dest => dest.MemberSince, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.LastLoginAt, opt => opt.Ignore())
                .ForMember(dest => dest.TotalBookings, opt => opt.Ignore())
                .ForMember(dest => dest.UpcomingMovies, opt => opt.Ignore())
                .ForMember(dest => dest.TotalRatings, opt => opt.Ignore())
                .ForMember(dest => dest.TotalSpent, opt => opt.Ignore())
                .ForMember(dest => dest.FavoriteGenreCount, opt => opt.Ignore())
                .ForMember(dest => dest.FavoriteGenre, opt => opt.Ignore());

            CreateMap<User, UserSecurityResponse>()
                .ForMember(dest => dest.LastPasswordChangedAt, opt => opt.Ignore());

            CreateMap<User, UserProfileUpdateResponse>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber ?? src.Phone))
                .ForMember(dest => dest.Message, opt => opt.Ignore());

            // Request to Entity
            CreateMap<RegisterUserRequest, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.IsAdmin, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                // Ignore Identity properties - they will be set by UserManager
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
                .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore())
                .ForMember(dest => dest.NormalizedUserName, opt => opt.Ignore())
                .ForMember(dest => dest.NormalizedEmail, opt => opt.Ignore())
                .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.Ignore())
                .ForMember(dest => dest.TwoFactorEnabled, opt => opt.Ignore())
                .ForMember(dest => dest.LockoutEnd, opt => opt.Ignore())
                .ForMember(dest => dest.LockoutEnabled, opt => opt.Ignore())
                .ForMember(dest => dest.AccessFailedCount, opt => opt.Ignore())
                .ForMember(dest => dest.MovieRatings, opt => opt.Ignore())
                .ForMember(dest => dest.Tickets, opt => opt.Ignore());

            CreateMap<UpdateUserProfileRequest, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.EmailConfirmed, opt => opt.Ignore())
                .ForMember(dest => dest.IsAdmin, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                // Ignore Identity properties
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
                .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore())
                .ForMember(dest => dest.NormalizedUserName, opt => opt.Ignore())
                .ForMember(dest => dest.NormalizedEmail, opt => opt.Ignore())
                .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.Ignore())
                .ForMember(dest => dest.TwoFactorEnabled, opt => opt.Ignore())
                .ForMember(dest => dest.LockoutEnd, opt => opt.Ignore())
                .ForMember(dest => dest.LockoutEnabled, opt => opt.Ignore())
                .ForMember(dest => dest.AccessFailedCount, opt => opt.Ignore())
                .ForMember(dest => dest.MovieRatings, opt => opt.Ignore())
                .ForMember(dest => dest.Tickets, opt => opt.Ignore());
        }
    }
}
