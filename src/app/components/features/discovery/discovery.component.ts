import { Component, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MovieService } from '../../../services/movie.service';
import { InputComponent } from '../../ui/input.component';

@Component({
  selector: 'app-discovery',
  standalone: true,
  imports: [CommonModule, RouterModule, InputComponent],
  template: `
    <div class="space-y-8">
      <!-- Hero Section with Search -->
      <section class="bg-gradient-to-b from-secondary to-background px-6 md:px-8 py-12 md:py-16 space-y-6">
        <div class="max-w-3xl space-y-4">
          <h1 class="text-4xl md:text-5xl lg:text-6xl font-bold text-foreground text-balance">
            Discover Your Next <span class="text-primary neon-text">Blockbuster</span>
          </h1>
          <p class="text-lg text-muted-foreground">
            Explore trending movies, upcoming releases, and personalized recommendations all in one place.
          </p>
        </div>

        <!-- Search Bar -->
        <div class="relative max-w-xl">
          <app-input
            type="search"
            placeholder="Search movies..."
            [value]="searchQuery()"
            (valueChange)="onSearch($event)"
          ></app-input>
          <svg
            *ngIf="searchResults().length > 0"
            class="absolute right-3 top-2.5 w-5 h-5 text-muted-foreground"
            fill="none"
            stroke="currentColor"
            viewBox="0 0 24 24"
          >
            <path
              stroke-linecap="round"
              stroke-linejoin="round"
              stroke-width="2"
              d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"
            ></path>
          </svg>

          <!-- Search Results Dropdown -->
          <div
            *ngIf="searchResults().length > 0"
            class="absolute top-full left-0 right-0 mt-2 glassmorphism rounded-lg p-4 max-h-96 overflow-y-auto z-10 space-y-2"
          >
            <a
              *ngFor="let movie of searchResults()"
              [routerLink]="['/movie', movie.id]"
              class="block px-4 py-2 rounded-lg hover:bg-secondary transition-smooth text-foreground"
            >
              <p class="font-medium">{{ movie.title }}</p>
              <p class="text-sm text-muted-foreground">{{ movie.year }} • {{ movie.genre.join(', ') }}</p>
            </a>
          </div>
        </div>
      </section>

      <!-- Trending Now Carousel -->
      <section class="px-6 md:px-8 space-y-4">
        <h2 class="text-2xl font-bold text-foreground">Trending Now</h2>
        <div class="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
          <a
            *ngFor="let movie of trendingMovies()"
            [routerLink]="['/movie', movie.id]"
            class="group cursor-pointer"
          >
            <div
              class="aspect-video bg-secondary rounded-lg overflow-hidden glassmorphism hover:neon-glow group-hover:scale-105 transition-smooth relative"
            >
              <div class="w-full h-full bg-gradient-to-br from-primary/20 to-secondary flex items-center justify-center">
                <div class="text-center space-y-2">
                  <p class="text-sm font-bold text-primary">{{ movie.title }}</p>
                  <p class="text-xs text-muted-foreground">{{ movie.year }}</p>
                </div>
              </div>

              <!-- Rating Badge -->
              <div
                class="absolute top-3 right-3 bg-primary text-primary-foreground px-2 py-1 rounded-lg text-xs font-bold neon-glow"
              >
                ★ {{ movie.rating }}
              </div>

              <!-- Genre Tags -->
              <div class="absolute bottom-3 left-3 right-3 flex flex-wrap gap-1">
                <span *ngFor="let genre of movie.genre.slice(0, 2)" class="text-xs bg-black/50 text-white px-2 py-1 rounded">
                  {{ genre }}
                </span>
              </div>
            </div>

            <div class="mt-3 space-y-1">
              <p class="font-medium text-foreground group-hover:text-primary transition-colors text-sm line-clamp-2">
                {{ movie.title }}
              </p>
              <p class="text-xs text-muted-foreground">{{ movie.duration }} min • {{ movie.year }}</p>
            </div>
          </a>
        </div>
      </section>

      <!-- Genre Filter -->
      <section class="px-6 md:px-8 space-y-4">
        <h2 class="text-2xl font-bold text-foreground">Browse by Genre</h2>
        <div class="flex flex-wrap gap-2">
          <button
            (click)="setSelectedGenre('')"
            [class.bg-primary]="selectedGenre() === ''"
            [class.text-primary-foreground]="selectedGenre() === ''"
            [class.bg-secondary]="selectedGenre() !== ''"
            [class.text-foreground]="selectedGenre() !== ''"
            class="px-6 py-2 rounded-full border border-border transition-smooth font-medium"
          >
            All Movies
          </button>

          <button
            *ngFor="let genre of genres()"
            (click)="setSelectedGenre(genre)"
            [class.bg-primary]="selectedGenre() === genre"
            [class.text-primary-foreground]="selectedGenre() === genre"
            [class.bg-secondary]="selectedGenre() !== genre"
            [class.text-foreground]="selectedGenre() !== genre"
            class="px-6 py-2 rounded-full border border-border transition-smooth hover:border-primary font-medium"
          >
            {{ genre }}
          </button>
        </div>
      </section>

      <!-- Filtered Movies Grid -->
      <section *ngIf="selectedGenre()" class="px-6 md:px-8 space-y-4">
        <h2 class="text-2xl font-bold text-foreground">{{ selectedGenre() }} Movies</h2>
        <div class="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
          <a
            *ngFor="let movie of genreMovies()"
            [routerLink]="['/movie', movie.id]"
            class="group cursor-pointer"
          >
            <div
              class="aspect-video bg-secondary rounded-lg overflow-hidden glassmorphism hover:neon-glow group-hover:scale-105 transition-smooth relative"
            >
              <div class="w-full h-full bg-gradient-to-br from-primary/20 to-secondary flex items-center justify-center">
                <div class="text-center space-y-2">
                  <p class="text-sm font-bold text-primary">{{ movie.title }}</p>
                  <p class="text-xs text-muted-foreground">{{ movie.year }}</p>
                </div>
              </div>

              <div
                class="absolute top-3 right-3 bg-primary text-primary-foreground px-2 py-1 rounded-lg text-xs font-bold neon-glow"
              >
                ★ {{ movie.rating }}
              </div>
            </div>

            <div class="mt-3 space-y-1">
              <p class="font-medium text-foreground group-hover:text-primary transition-colors text-sm line-clamp-2">
                {{ movie.title }}
              </p>
              <p class="text-xs text-muted-foreground">{{ movie.duration }} min</p>
            </div>
          </a>
        </div>
      </section>

      <!-- Coming Soon Section -->
      <section class="px-6 md:px-8 space-y-4">
        <h2 class="text-2xl font-bold text-foreground">Coming Soon</h2>
        <div class="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
          <a
            *ngFor="let movie of upcomingMovies()"
            [routerLink]="['/movie', movie.id]"
            class="group cursor-pointer"
          >
            <div
              class="aspect-video bg-secondary rounded-lg overflow-hidden glassmorphism hover:neon-glow group-hover:scale-105 transition-smooth relative"
            >
              <div class="w-full h-full bg-gradient-to-br from-primary/20 to-secondary flex items-center justify-center">
                <p class="text-sm text-muted-foreground">Coming Soon</p>
              </div>

              <div class="absolute top-3 right-3 bg-primary text-primary-foreground px-2 py-1 rounded text-xs font-bold">
                In {{ 45 }} days
              </div>

              <div class="absolute bottom-3 left-3 right-3 text-xs bg-black/50 text-white px-2 py-1 rounded">
                {{ movie.releaseDate }}
              </div>
            </div>

            <div class="mt-3 space-y-1">
              <p class="font-medium text-foreground group-hover:text-primary transition-colors text-sm line-clamp-2">
                {{ movie.title }}
              </p>
              <p class="text-xs text-muted-foreground">{{ movie.year }}</p>
            </div>
          </a>
        </div>
      </section>
    </div>
  `,
})
export class DiscoveryComponent {
  private movieService = inject(MovieService);

  searchQuery = signal('');
  selectedGenre = signal('');

  trendingMovies = this.movieService.trendingMovies;
  upcomingMovies = this.movieService.upcomingMovies;
  genres = computed(() => this.movieService.getAllGenres());

  searchResults = computed(() => {
    if (!this.searchQuery()) return [];
    return this.movieService.searchMovies(this.searchQuery()).slice(0, 5);
  });

  genreMovies = computed(() => {
    if (!this.selectedGenre()) return [];
    return this.movieService.getMoviesByGenre(this.selectedGenre());
  });

  onSearch(query: string): void {
    this.searchQuery.set(query);
  }

  setSelectedGenre(genre: string): void {
    this.selectedGenre.set(genre);
  }
}
