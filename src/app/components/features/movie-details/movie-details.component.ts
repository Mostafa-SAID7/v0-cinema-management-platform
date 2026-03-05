import { Component, inject, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { MovieService } from '../../../services/movie.service';

@Component({
  selector: 'app-movie-details',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <div class="bg-background">
      <div *ngIf="movie(); let m = movie()">
        <!-- Hero Section -->
        <div class="relative h-80 md:h-[500px] bg-gradient-to-b from-primary/20 to-background overflow-hidden">
          <div class="absolute inset-0 bg-gradient-to-b from-transparent via-transparent to-background"></div>
          <div class="absolute inset-0 flex items-center justify-center text-muted-foreground text-sm">
            {{ m.title }} - Backdrop Image
          </div>

          <!-- Floating Elements -->
          <div class="absolute top-6 right-6 bg-primary text-primary-foreground px-4 py-2 rounded-full font-bold neon-glow">
            ★ {{ m.rating }}/10
          </div>
        </div>

        <!-- Content -->
        <div class="relative -mt-16 md:-mt-20 px-4 md:px-8 pb-8 space-y-8">
          <!-- Movie Info Card -->
          <div class="glassmorphism rounded-lg p-6 md:p-8 space-y-6 animate-slide-in-up">
            <div class="flex flex-col md:flex-row gap-6">
              <!-- Poster -->
              <div
                class="w-40 h-56 rounded-lg bg-gradient-to-br from-primary/20 to-secondary flex-shrink-0 flex items-center justify-center text-muted-foreground neon-glow"
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
                    {{ m.year }} • {{ m.duration }} min • {{ m.genre.join(', ') }}
                  </p>
                </div>

                <!-- Rating & Stats -->
                <div class="flex flex-col gap-4">
                  <div class="flex items-center gap-4">
                    <div class="flex items-center gap-3 bg-primary/20 rounded-lg px-4 py-3">
                      <span class="text-4xl font-bold text-primary">{{ m.rating }}</span>
                      <div class="space-y-1">
                        <p class="text-sm font-bold text-primary">★★★★★</p>
                        <p class="text-xs text-muted-foreground">{{ getVoteCount() }} votes</p>
                      </div>
                    </div>
                  </div>

                  <p class="text-foreground leading-relaxed text-lg">{{ m.description }}</p>

                  <!-- Action Buttons -->
                  <div class="flex gap-3 pt-4">
                    <a
                      [routerLink]="['/booking', m.id]"
                      class="flex-1 px-8 py-3 rounded-lg bg-primary text-primary-foreground font-bold hover:bg-primary/90 neon-glow transition-smooth text-center cursor-pointer"
                    >
                      Pick a Showtime
                    </a>
                    <button
                      class="px-4 py-3 rounded-lg border border-border text-foreground hover:bg-secondary transition-smooth"
                    >
                      ♥ Add to Favorites
                    </button>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <!-- Cast Section -->
          <section class="space-y-4">
            <h2 class="text-2xl font-bold text-foreground">Cast & Crew</h2>
            <div class="grid grid-cols-3 md:grid-cols-5 lg:grid-cols-6 gap-4">
              <div *ngFor="let actor of getCastMembers()" class="text-center space-y-2 group cursor-pointer">
                <div
                  class="w-full aspect-square rounded-lg bg-gradient-to-br from-primary/20 to-secondary flex items-center justify-center text-muted-foreground group-hover:neon-glow transition-smooth"
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

          <!-- Reviews & Ratings Section -->
          <section class="space-y-4">
            <div class="flex items-center justify-between">
              <h2 class="text-2xl font-bold text-foreground">Reviews & Ratings</h2>
              <button class="px-4 py-2 rounded-lg border border-border text-foreground hover:bg-secondary transition-smooth text-sm font-medium">
                Write a Review
              </button>
            </div>

            <!-- Review List -->
            <div class="space-y-4">
              <div *ngFor="let review of getReviews()" class="glassmorphism rounded-lg p-6 space-y-3 hover:bg-card/80 transition-smooth">
                <div class="flex items-center justify-between">
                  <div class="flex items-center gap-3">
                    <div class="w-10 h-10 rounded-full bg-primary/20 flex items-center justify-center font-bold text-primary text-sm">
                      {{ review.author.charAt(0) }}
                    </div>
                    <div>
                      <p class="font-medium text-foreground">{{ review.author }}</p>
                      <p class="text-xs text-muted-foreground">{{ review.date }}</p>
                    </div>
                  </div>
                  <div class="flex items-center gap-1 text-primary font-bold">
                    <span>★</span>
                    <span>{{ review.rating }}</span>
                  </div>
                </div>
                <p class="text-foreground leading-relaxed">{{ review.text }}</p>
                <div class="flex gap-4 pt-2 text-xs text-muted-foreground">
                  <button class="hover:text-primary transition-colors">👍 Helpful</button>
                  <button class="hover:text-primary transition-colors">👎 Not Helpful</button>
                </div>
              </div>
            </div>
          </section>

          <!-- Similar Movies -->
          <section class="space-y-4">
            <h2 class="text-2xl font-bold text-foreground">Similar Movies</h2>
            <div class="grid grid-cols-2 md:grid-cols-4 lg:grid-cols-5 gap-4">
              <a *ngFor="let similar of getSimilarMovies()" [routerLink]="['/movie', similar.id]" class="group cursor-pointer">
                <div class="aspect-video bg-secondary rounded-lg overflow-hidden glassmorphism hover:neon-glow group-hover:scale-105 transition-smooth relative">
                  <div class="w-full h-full bg-gradient-to-br from-primary/20 to-secondary flex items-center justify-center">
                    <p class="text-xs font-bold text-primary text-center px-2">{{ similar.title }}</p>
                  </div>
                  <div class="absolute top-2 right-2 bg-primary text-primary-foreground px-2 py-1 rounded text-xs font-bold">
                    ★ {{ similar.rating }}
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
    </div>
  `,
})
export class MovieDetailsComponent {
  private movieService = inject(MovieService);
  private route = inject(ActivatedRoute);

  movieId = this.route.snapshot.paramMap.get('id');
  movie = computed(() => (this.movieId ? this.movieService.getMovieById(this.movieId) : undefined));

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
