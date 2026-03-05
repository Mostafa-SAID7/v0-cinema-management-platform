import { Injectable, signal } from '@angular/core';

export interface Movie {
  id: string;
  title: string;
  rating: number;
  genre: string[];
  year: number;
  duration: number;
  description: string;
  posterUrl?: string;
  backdropUrl?: string;
  releaseDate?: string;
  cast?: string[];
}

@Injectable({
  providedIn: 'root',
})
export class MovieService {
  movies = signal<Movie[]>([]);
  trendingMovies = signal<Movie[]>([]);
  upcomingMovies = signal<Movie[]>([]);

  constructor() {
    this.loadMockData();
  }

  private loadMockData(): void {
    const mockMovies: Movie[] = [
      {
        id: '1',
        title: 'Cosmic Voyage',
        rating: 8.5,
        genre: ['Sci-Fi', 'Adventure'],
        year: 2024,
        duration: 148,
        description: 'An epic journey across distant galaxies with stunning visuals and unforgettable moments.',
        releaseDate: '2024-12-20',
      },
      {
        id: '2',
        title: 'The Last Kingdom',
        rating: 8.2,
        genre: ['Action', 'Drama'],
        year: 2024,
        duration: 156,
        description: 'A gripping tale of power, betrayal, and redemption in a war-torn kingdom.',
        releaseDate: '2024-12-25',
      },
      {
        id: '3',
        title: 'Laughter in Paradise',
        rating: 7.8,
        genre: ['Comedy', 'Romance'],
        year: 2024,
        duration: 118,
        description: 'A heartwarming story of love and laughter in an unexpected paradise.',
        releaseDate: '2024-12-28',
      },
      {
        id: '4',
        title: 'Midnight Horror',
        rating: 8.0,
        genre: ['Horror', 'Thriller'],
        year: 2024,
        duration: 126,
        description: 'A terrifying experience that will keep you on the edge of your seat.',
        releaseDate: '2024-12-22',
      },
      {
        id: '5',
        title: 'Love in the City',
        rating: 7.5,
        genre: ['Romance', 'Drama'],
        year: 2024,
        duration: 124,
        description: 'Two souls find love in the bustling streets of the city.',
        releaseDate: '2025-01-10',
      },
      {
        id: '6',
        title: 'The Mission',
        rating: 8.3,
        genre: ['Action', 'Thriller'],
        year: 2024,
        duration: 138,
        description: 'A high-stakes mission against time and enemies.',
        releaseDate: '2025-01-15',
      },
      {
        id: '7',
        title: 'Cyber Syndrome',
        rating: 7.9,
        genre: ['Sci-Fi', 'Action'],
        year: 2024,
        duration: 142,
        description: 'In a digital world, a hacker fights to expose the truth.',
        releaseDate: '2025-01-20',
      },
      {
        id: '8',
        title: 'Ancient Mysteries',
        rating: 8.1,
        genre: ['Adventure', 'Mystery'],
        year: 2024,
        duration: 152,
        description: 'Archaeologists discover secrets that should have stayed buried.',
        releaseDate: '2025-02-01',
      },
    ];

    this.movies.set(mockMovies);
    this.trendingMovies.set(mockMovies.slice(0, 4));
    this.upcomingMovies.set(mockMovies.slice(4, 8));
  }

  getMovieById(id: string): Movie | undefined {
    return this.movies().find((movie) => movie.id === id);
  }

  searchMovies(query: string): Movie[] {
    const lowerQuery = query.toLowerCase();
    return this.movies().filter((movie) =>
      movie.title.toLowerCase().includes(lowerQuery) ||
      movie.genre.some((g) => g.toLowerCase().includes(lowerQuery))
    );
  }

  getMoviesByGenre(genre: string): Movie[] {
    return this.movies().filter((movie) =>
      movie.genre.map((g) => g.toLowerCase()).includes(genre.toLowerCase())
    );
  }

  getTrendingMovies(): Movie[] {
    return this.trendingMovies();
  }

  getUpcomingMovies(): Movie[] {
    return this.upcomingMovies();
  }

  getAllGenres(): string[] {
    const genresSet = new Set<string>();
    this.movies().forEach((movie) => {
      movie.genre.forEach((genre) => genresSet.add(genre));
    });
    return Array.from(genresSet).sort();
  }
}
