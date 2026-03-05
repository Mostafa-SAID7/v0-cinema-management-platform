import { Component, inject, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { MovieService } from '../../../services/movie.service';
import { ToastService } from '../../../services/toast.service';
import { SkeletonCardComponent } from '../../ui/skeleton-card.component';
import { ErrorPageComponent } from '../../pages/error-page.component';

@Component({
  selector: 'app-movie-details',
  standalone: true,
  imports: [CommonModule, RouterModule, SkeletonCardComponent, ErrorPageComponent],
  template: `
    <div class="bg-background">
      <div *ngIf="movie() as m">
        <!-- Hero Section -->
        <div class="relative h-80 md:h-[500px] bg-gradient-to-b from-primary/20 to-background overflow-hidden">
          <div class="absolute inset-0 bg-gradient-to-b from-transparent via-transparent to-background"></div>
          <div class="absolute inset-0 flex items-center justify-center text-muted-foreground text-sm">
            {{ m.title }} - Backdrop Image
          </div>

          <!-- Rating Badge -->
          <div class="absolute top-6 right-6 inline-flex items-center rounded-md border border-transparent bg-primary text-primary-foreground px-3 py-1.5 text-sm font-semibold shadow">
            {{ m.rating }}/10
          </div>
        </div>

        <!-- Content -->
        <div class="relative -mt-16 md:-mt-20 px-4 md:px-8 pb-8 space-y-8">
          <!-- Movie Info Card -->
          <div class="rounded-xl border bg-card text-card-foreground shadow p-6 md:p-8 space-y-6 animate-slide-in-up">
            <div class="flex flex-col md:flex-row gap-6">
              <!-- Poster -->
              <div
                class="w-40 h-56 rounded-lg bg-gradient-to-br from-primary/20 to-secondary flex-shrink-0 flex items-center justify-center text-muted-foreground border shadow-sm"
              >
                <div class="text-center">
                  <p class="font-bold text-primary">{{ m.title }}</p>
                </div>
              </div>

              <!-- Details -->
              <div class="flex-1 space-y-4">
                <div>
                  <h1 class="text-4xl md:text-5xl font-bold text-foreground text-balance">{{ m.title }}</h1>
                  <p class="text-muted-foreground text-lg mt-2">
                    {{ m.year }} &bull; {{ m.duration }} min &bull; {{ m.genre.join(', ') }}
                  </p>
                </div>

                <!-- Rating & Stats -->
                <div class="flex flex-col gap-4">
                  <div class="flex items-center gap-4">
                    <div class="flex items-center gap-3 rounded-lg border bg-card px-4 py-3">
                      <span class="text-4xl font-bold text-primary">{{ m.rating }}</span>
                      <div class="space-y-1">
                        <p class="text-sm font-bold text-primary">Rating</p>
                        <p class="text-xs text-muted-foreground">{{ getVoteCount() }} votes</p>
                      </div>
                    </div>
                  </div>

                  <p class="text-foreground leading-relaxed text-lg">{{ m.description }}</p>

                  <!-- Action Buttons -->
                  <div class="flex gap-3 pt-4">
                    <a
                      [routerLink]="['/booking', m.id]"
                      class="flex-1 inline-flex items-center justify-center gap-2 whitespace-nowrap rounded-md text-sm font-medium transition-colors focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring bg-primary text-primary-foreground shadow hover:bg-primary/90 h-10 px-8 cursor-pointer text-center"
                    >
                      Pick a Showtime
                    </a>
                    <button
                      class="inline-flex items-center justify-center gap-2 whitespace-nowrap rounded-md text-sm font-medium transition-colors focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring border border-input bg-background shadow-sm hover:bg-accent hover:text-accent-foreground h-10 px-4"
                    >
                      <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                        <path d="M19 14c1.49-1.46 3-3.21 3-5.5A5.5 5.5 0 0 0 16.5 3c-1.76 0-3 .5-4.5 2-1.5-1.5-2.74-2-4.5-2A5.5 5.5 0 0 0 2 8.5c0 2.3 1.5 4.05 3 5.5l7 7Z"/>
                      </svg>
                      Favorite
                    </button>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <!-- Separator -->
          <div class="shrink-0 bg-border h-px w-full"></div>

          <!-- Cast Section -->
          <section class="space-y-4">
            <h2 class="text-2xl font-bold text-foreground">Cast & Crew</h2>
            <div class="grid grid-cols-3 md:grid-cols-5 lg:grid-cols-6 gap-4">
              <div *ngFor="let actor of getCastMembers()" class="text-center space-y-2 group cursor-pointer">
                <div
                  class="w-full aspect-square rounded-lg bg-gradient-to-br from-primary/20 to-secondary flex items-center justify-center text-muted-foreground border shadow-sm group-hover:shadow-md transition-all duration-200"
                >
                  <div class="text-center">
                    <p class="text-xs font-bold text-primary">{{ actor }}</p>
                  </div>
                </div>
                <p class="font-medium text-foreground text-xs line-clamp-2">{{ actor }}</p>
                <p class="text-xs text-muted-foreground">Actor</p>
              </div>
            </div>
          </section>

          <!-- Separator -->
          <div class="shrink-0 bg-border h-px w-full"></div>

          <!-- Reviews & Ratings Section -->
          <section class="space-y-4">
            <div class="flex items-center justify-between">
              <h2 class="text-2xl font-bold text-foreground">Reviews & Ratings</h2>
              <button class="inline-flex items-center justify-center gap-2 whitespace-nowrap rounded-md text-sm font-medium transition-colors focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring border border-input bg-background shadow-sm hover:bg-accent hover:text-accent-foreground h-9 px-4 py-2">
                Write a Review
              </button>
            </div>

            <!-- Review List -->
            <div class="space-y-4">
              <div *ngFor="let review of getReviews()" class="rounded-xl border bg-card text-card-foreground shadow p-6 space-y-3 hover:shadow-md transition-all duration-200">
                <div class="flex items-center justify-between">
                  <div class="flex items-center gap-3">
                    <span class="relative flex shrink-0 overflow-hidden rounded-full h-10 w-10 bg-primary/20 items-center justify-center">
                      <span class="text-sm font-medium text-primary">{{ review.author.charAt(0) }}</span>
                    </span>
                    <div>
                      <p class="font-medium text-foreground text-sm">{{ review.author }}</p>
                      <p class="text-xs text-muted-foreground">{{ review.date }}</p>
                    </div>
                  </div>
                  <div class="inline-flex items-center rounded-md border border-transparent bg-primary text-primary-foreground px-2.5 py-0.5 text-xs font-semibold shadow">
                    {{ review.rating }}
                  </div>
                </div>
                <p class="text-foreground leading-relaxed text-sm">{{ review.text }}</p>
                <div class="flex gap-4 pt-2 text-xs text-muted-foreground">
                  <button class="hover:text-foreground transition-colors">Helpful</button>
                  <button class="hover:text-foreground transition-colors">Not Helpful</button>
                </div>
              </div>
            </div>
          </section>

          <!-- Separator -->
          <div class="shrink-0 bg-border h-px w-full"></div>

          <!-- Similar Movies -->
          <section class="space-y-4">
            <h2 class="text-2xl font-bold text-foreground">Similar Movies</h2>
            <div class="grid grid-cols-2 md:grid-cols-4 lg:grid-cols-5 gap-4">
              <a *ngFor="let similar of getSimilarMovies()" [routerLink]="['/movie', similar.id]" class="group cursor-pointer">
                <div class="aspect-video rounded-xl border bg-card shadow overflow-hidden group-hover:shadow-md group-hover:scale-[1.02] transition-all duration-200 relative">
                  <div class="w-full h-full bg-gradient-to-br from-primary/20 to-secondary flex items-center justify-center">
                    <p class="text-xs font-bold text-primary text-center px-2">{{ similar.title }}</p>
                  </div>
                  <div class="absolute top-2 right-2 inline-flex items-center rounded-md border border-transparent bg-primary text-primary-foreground px-2 py-0.5 text-xs font-semibold shadow">
                    {{ similar.rating }}
                  </div>
                </div>
                <p class="mt-2 font-medium text-foreground text-sm group-hover:text-primary transition-colors line-clamp-2">
                  {{ similar.title }}
                </p>
              </a>
            </div>
          </section>
        </div>
      </div>

      <!-- Loading State -->
      <div *ngIf="!movie()" class="h-96 flex items-center justify-center">
        <p class="text-muted-foreground">Loading movie details...</p>
      </div>

      <!-- Error State -->
      <app-error-page
        *ngIf="movieError()"
        [statusCode]="404"
        title="Movie Not Found"
        message="The movie you're looking for doesn't exist or has been removed."
      ></app-error-page>
    </div>
  `,
})
export class MovieDetailsComponent {
  private movieService = inject(MovieService);
  private route = inject(ActivatedRoute);
  private toastService = inject(ToastService);

  movieId = this.route.snapshot.paramMap.get('id');
  movie = computed(() => (this.movieId ? this.movieService.getMovieById(this.movieId) : undefined));
  movieError = computed(() => this.movieId && !this.movie());

  getCastMembers(): string[] {
    return ['Actor 1', 'Actor 2', 'Actor 3', 'Actor 4', 'Actor 5', 'Actor 6'];
  }

  getReviews() {
    return [
      {
        author: 'John Doe',
        rating: 5,
        date: '2 days ago',
        text: 'Absolutely incredible! The cinematography, storytelling, and performances were all top-notch. A must-watch!',
      },
      {
        author: 'Jane Smith',
        rating: 4,
        date: '1 week ago',
        text: 'Great movie overall. Some pacing issues in the middle, but the ending makes up for it. Highly recommended!',
      },
      {
        author: 'Mike Johnson',
        rating: 5,
        date: '2 weeks ago',
        text: 'Masterpiece! This is cinema at its finest. Can\'t wait to watch it again.',
      },
    ];
  }

  getSimilarMovies() {
    return this.movieService.movies().slice(0, 5);
  }

  getVoteCount(): string {
    return (Math.random() * 50000).toFixed(0);
  }
}
