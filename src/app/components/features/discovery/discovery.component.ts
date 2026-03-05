import { Component, inject, signal, computed, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MovieService } from '../../../services/movie.service';
import { ToastService } from '../../../services/toast.service';
import { InputComponent } from '../../ui/input.component';
import { SkeletonCardComponent } from '../../ui/skeleton-card.component';
import { EmptyStateComponent } from '../../ui/empty-state.component';

@Component({
  selector: 'app-discovery',
  standalone: true,
  imports: [CommonModule, RouterModule, InputComponent, SkeletonCardComponent, EmptyStateComponent],
  template: `
    <div class="space-y-8">
      <!-- Hero Section with Search -->
      <section class="bg-gradient-to-b from-secondary to-background px-6 md:px-8 py-12 md:py-16 space-y-6">
        <div class="max-w-3xl space-y-4">
          <h1 class="text-4xl md:text-5xl lg:text-6xl font-bold text-foreground text-balance">
            Discover Your Next <span class="text-primary">Blockbuster</span>
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

          <!-- Search Results Dropdown -->
          <div
            *ngIf="searchResults().length > 0"
            class="absolute top-full left-0 right-0 mt-2 rounded-xl border bg-popover text-popover-foreground shadow-md p-2 max-h-96 overflow-y-auto z-10 space-y-1"
          >
            <a
              *ngFor="let movie of searchResults()"
              [routerLink]="['/movie', movie.id]"
              class="block px-3 py-2 rounded-md hover:bg-accent hover:text-accent-foreground transition-colors text-foreground"
            >
              <p class="font-medium text-sm">{{ movie.title }}</p>
              <p class="text-xs text-muted-foreground">{{ movie.year }} &bull; {{ movie.genre.join(', ') }}</p>
            </a>
          </div>
        </div>
      </section>

      <!-- Trending Now Carousel -->
      <section class="px-6 md:px-8 space-y-4">
        <h2 class="text-2xl font-bold text-foreground">Trending Now</h2>
        <!-- Loading State -->
        <div *ngIf="isLoading()" class="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
          <app-skeleton-card *ngFor="let i of [1,2,3,4,5,6,7,8]"></app-skeleton-card>
        </div>
        <!-- Content -->
        <div *ngIf="!isLoading()" class="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
          <a
            *ngFor="let movie of trendingMovies()"
            [routerLink]="['/movie', movie.id]"
            class="group cursor-pointer"
          >
            <div
              class="aspect-video rounded-xl border bg-card shadow overflow-hidden group-hover:shadow-md group-hover:scale-[1.02] transition-all duration-200 relative"
            >
              <div class="w-full h-full bg-gradient-to-br from-primary/20 to-secondary flex items-center justify-center">
                <div class="text-center space-y-2">
                  <p class="text-sm font-bold text-primary">{{ movie.title }}</p>
                  <p class="text-xs text-muted-foreground">{{ movie.year }}</p>
                </div>
              </div>

              <!-- Rating Badge -->
              <div
                class="absolute top-3 right-3 inline-flex items-center rounded-md border border-transparent bg-primary text-primary-foreground px-2.5 py-0.5 text-xs font-semibold shadow"
              >
                {{ movie.rating }}
              </div>

              <!-- Genre Tags -->
              <div class="absolute bottom-3 left-3 right-3 flex flex-wrap gap-1">
                <span
                  *ngFor="let genre of movie.genre.slice(0, 2)"
                  class="inline-flex items-center rounded-md border border-transparent bg-secondary text-secondary-foreground px-2 py-0.5 text-xs font-semibold"
                >
                  {{ genre }}
                </span>
              </div>
            </div>

            <div class="mt-3 space-y-1">
              <p class="font-medium text-foreground group-hover:text-primary transition-colors text-sm line-clamp-2">
                {{ movie.title }}
              </p>
              <p class="text-xs text-muted-foreground">{{ movie.duration }} min &bull; {{ movie.year }}</p>
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
            [ngClass]="{
              'bg-primary text-primary-foreground shadow hover:bg-primary/90': selectedGenre() === '',
              'border border-input bg-background shadow-sm hover:bg-accent hover:text-accent-foreground': selectedGenre() !== ''
            }"
            class="inline-flex items-center justify-center whitespace-nowrap rounded-md text-sm font-medium transition-colors h-9 px-4 py-2"
          >
            All Movies
          </button>

          <button
            *ngFor="let genre of genres()"
            (click)="setSelectedGenre(genre)"
            [ngClass]="{
              'bg-primary text-primary-foreground shadow hover:bg-primary/90': selectedGenre() === genre,
              'border border-input bg-background shadow-sm hover:bg-accent hover:text-accent-foreground': selectedGenre() !== genre
            }"
            class="inline-flex items-center justify-center whitespace-nowrap rounded-md text-sm font-medium transition-colors h-9 px-4 py-2"
          >
            {{ genre }}
          </button>
        </div>
      </section>

      <!-- Filtered Movies Grid -->
      <section *ngIf="selectedGenre()" class="px-6 md:px-8 space-y-4">
        <h2 class="text-2xl font-bold text-foreground">{{ selectedGenre() }} Movies</h2>
        <div *ngIf="genreMovies().length > 0" class="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
          <a
            *ngFor="let movie of genreMovies()"
            [routerLink]="['/movie', movie.id]"
            class="group cursor-pointer"
          >
            <div
              class="aspect-video rounded-xl border bg-card shadow overflow-hidden group-hover:shadow-md group-hover:scale-[1.02] transition-all duration-200 relative"
            >
              <div class="w-full h-full bg-gradient-to-br from-primary/20 to-secondary flex items-center justify-center">
                <div class="text-center space-y-2">
                  <p class="text-sm font-bold text-primary">{{ movie.title }}</p>
                  <p class="text-xs text-muted-foreground">{{ movie.year }}</p>
                </div>
              </div>

              <div
                class="absolute top-3 right-3 inline-flex items-center rounded-md border border-transparent bg-primary text-primary-foreground px-2.5 py-0.5 text-xs font-semibold shadow"
              >
                {{ movie.rating }}
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
              class="aspect-video rounded-xl border bg-card shadow overflow-hidden group-hover:shadow-md group-hover:scale-[1.02] transition-all duration-200 relative"
            >
              <div class="w-full h-full bg-gradient-to-br from-primary/20 to-secondary flex items-center justify-center">
                <p class="text-sm text-muted-foreground">Coming Soon</p>
              </div>

              <div class="absolute top-3 right-3 inline-flex items-center rounded-md border border-transparent bg-secondary text-secondary-foreground px-2.5 py-0.5 text-xs font-semibold">
                In {{ 45 }} days
              </div>

              <div class="absolute bottom-3 left-3 right-3 inline-flex items-center rounded-md border border-transparent bg-secondary text-secondary-foreground px-2 py-0.5 text-xs font-semibold">
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
export class DiscoveryComponent implements OnInit {
  private movieService = inject(MovieService);
  private toastService = inject(ToastService);

  searchQuery = signal('');
  selectedGenre = signal('');
  isLoading = signal(true);

  ngOnInit(): void {
    setTimeout(() => {
      this.isLoading.set(false);
    }, 800);
  }

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
