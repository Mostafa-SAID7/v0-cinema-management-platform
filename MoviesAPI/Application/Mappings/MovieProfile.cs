using AutoMapper;
using MoviesAPI.Application.DTOs.Requests.Movies;
using MoviesAPI.Application.DTOs.Responses.Movies;
using MoviesAPI.Domain.Entities.Movies;

namespace MoviesAPI.Application.Mappings
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            // Entity to Response
            CreateMap<Movie, MovieResponse>()
                .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => src.ReleaseDate))
                .ForMember(dest => dest.PosterPath, opt => opt.MapFrom(src => src.PosterPath))
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => 
                    string.IsNullOrEmpty(src.Genres) ? new List<string>() : src.Genres.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(g => g.Trim()).ToList()))
                .ForMember(dest => dest.TotalRatings, opt => opt.MapFrom(src => 
                    src.Ratings != null ? src.Ratings.Count : 0));

            CreateMap<Movie, MovieSummaryResponse>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => src.ReleaseDate))
                .ForMember(dest => dest.PosterUrl, opt => opt.MapFrom(src => src.PosterPath))
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genres))
                .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => (double?)src.Rating));

            // Request to Entity
            CreateMap<CreateMovieRequest, Movie>()
                .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => src.ReleaseDate))
                .ForMember(dest => dest.PosterPath, opt => opt.MapFrom(src => src.PosterPath))
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => string.Join(",", src.Genres)))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Ratings, opt => opt.Ignore())
                .ForMember(dest => dest.Rating, opt => opt.Ignore())
                .ForMember(dest => dest.Screenings, opt => opt.Ignore());

            CreateMap<UpdateMovieRequest, Movie>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Plot, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => src.ReleaseDate ?? DateTime.Now))
                .ForMember(dest => dest.PosterPath, opt => opt.MapFrom(src => src.PosterUrl))
                .ForMember(dest => dest.Directors, opt => opt.MapFrom(src => src.Director))
                .ForMember(dest => dest.Actors, opt => opt.MapFrom(src => src.Cast))
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Genre))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Amount, opt => opt.Ignore())
                .ForMember(dest => dest.Rating, opt => opt.Ignore())
                .ForMember(dest => dest.Ratings, opt => opt.Ignore())
                .ForMember(dest => dest.Screenings, opt => opt.Ignore());

            // Ratings
            CreateMap<CreateRatingRequest, MovieRating>()
                .ForMember(dest => dest.MovieId, opt => opt.Ignore()) // Will be set manually
                .ForMember(dest => dest.UserId, opt => opt.Ignore()) // Will be set manually
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Comment, opt => opt.Ignore())
                .ForMember(dest => dest.Movie, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<MovieRating, MovieRatingResponse>()
                .ForMember(dest => dest.MovieId, opt => opt.Ignore()) // Will be set manually if needed
                .ForMember(dest => dest.UserId, opt => opt.Ignore()) // Will be set manually if needed
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.Name : null));
        }
    }
}
