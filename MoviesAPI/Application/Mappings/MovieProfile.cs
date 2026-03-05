using AutoMapper;
using MoviesAPI.Application.DTOs.Requests.Movies;
using MoviesAPI.Application.DTOs.Responses.Movies;
using MoviesAPI.Models;

namespace MoviesAPI.Application.Mappings
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            // Entity to Response
            CreateMap<Movie, MovieResponse>()
                .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => src.Release_Date))
                .ForMember(dest => dest.PosterPath, opt => opt.MapFrom(src => src.Poster_Path))
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => 
                    string.IsNullOrEmpty(src.Genres) ? new List<string>() : src.Genres.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(g => g.Trim()).ToList()))
                .ForMember(dest => dest.TotalRatings, opt => opt.MapFrom(src => 
                    src.Ratings != null ? src.Ratings.Count : 0));

            CreateMap<Movie, MovieSummaryResponse>()
                .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => src.Release_Date))
                .ForMember(dest => dest.PosterPath, opt => opt.MapFrom(src => src.Poster_Path))
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => 
                    string.IsNullOrEmpty(src.Genres) ? new List<string>() : src.Genres.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(g => g.Trim()).ToList()));

            // Request to Entity
            CreateMap<CreateMovieRequest, Movie>()
                .ForMember(dest => dest.Release_Date, opt => opt.MapFrom(src => src.ReleaseDate))
                .ForMember(dest => dest.Poster_Path, opt => opt.MapFrom(src => src.PosterPath))
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => string.Join(",", src.Genres)))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Ratings, opt => opt.Ignore())
                .ForMember(dest => dest.Rating, opt => opt.Ignore());

            CreateMap<UpdateMovieRequest, Movie>()
                .ForMember(dest => dest.Release_Date, opt => opt.MapFrom(src => src.ReleaseDate))
                .ForMember(dest => dest.Poster_Path, opt => opt.MapFrom(src => src.PosterPath))
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => string.Join(",", src.Genres)))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Ratings, opt => opt.Ignore())
                .ForMember(dest => dest.Rating, opt => opt.Ignore());

            // CreateAndUpdateMovie to Movie (for backward compatibility)
            CreateMap<CreateAndUpdateMovie, Movie>()
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => string.Join(",", src.Genres)))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Ratings, opt => opt.Ignore())
                .ForMember(dest => dest.Rating, opt => opt.Ignore());

            // Request DTOs to CreateAndUpdateMovie (for repository compatibility)
            CreateMap<CreateMovieRequest, CreateAndUpdateMovie>();
            CreateMap<UpdateMovieRequest, CreateAndUpdateMovie>();
        }
    }
}
