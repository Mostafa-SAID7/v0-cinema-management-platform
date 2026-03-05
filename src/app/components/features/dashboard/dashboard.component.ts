import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { BookingService } from '../../../services/booking.service';
import { ToastService } from '../../../services/toast.service';
import { EmptyStateComponent } from '../../ui/empty-state.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, EmptyStateComponent],
  template: `
    <div class="p-6 md:p-8 space-y-8">
      <!-- Header -->
      <div *ngIf="authService.getCurrentUser(); let user = authService.getCurrentUser()" class="space-y-2">
        <h1 class="text-4xl font-bold text-foreground">Welcome, {{ user.name }}!</h1>
        <p class="text-muted-foreground">Manage your tickets, bookings, and account preferences</p>
      </div>

      <!-- Stats Cards -->
      <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
        <div class="glassmorphism rounded-lg p-6 space-y-2">
          <p class="text-sm text-muted-foreground font-medium">Upcoming Tickets</p>
          <p class="text-4xl font-bold text-primary">{{ upcomingBookings.length }}</p>
          <p class="text-xs text-muted-foreground">Next show in 5 days</p>
        </div>
        <div class="glassmorphism rounded-lg p-6 space-y-2">
          <p class="text-sm text-muted-foreground font-medium">Total Spent</p>
          <p class="text-4xl font-bold text-primary">₹{{ totalSpent }}</p>
          <p class="text-xs text-muted-foreground">This year</p>
        </div>
        <div class="glassmorphism rounded-lg p-6 space-y-2">
          <p class="text-sm text-muted-foreground font-medium">Favorite Movies</p>
          <p class="text-4xl font-bold text-primary">{{ favoriteMovies.length }}</p>
          <p class="text-xs text-muted-foreground">Saved for later</p>
        </div>
      </div>

      <!-- Active Tickets -->
      <section class="space-y-4">
        <h2 class="text-2xl font-bold text-foreground">Active Tickets</h2>
        <div *ngIf="upcomingBookings.length > 0; else noTickets" class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          <div *ngFor="let booking of upcomingBookings" class="glassmorphism rounded-lg overflow-hidden hover:neon-glow transition-smooth">
            <!-- Ticket Header -->
            <div class="bg-primary/20 px-6 py-4 border-b border-border">
              <p class="font-bold text-foreground">{{ booking.title }}</p>
              <p class="text-sm text-muted-foreground">{{ booking.date }} • {{ booking.time }}</p>
            </div>

            <!-- Ticket Body -->
            <div class="p-6 space-y-4">
              <div class="grid grid-cols-2 gap-4 text-sm">
                <div>
                  <p class="text-muted-foreground text-xs font-medium">Seats</p>
                  <p class="font-bold text-foreground">{{ booking.seats }}</p>
                </div>
                <div>
                  <p class="text-muted-foreground text-xs font-medium">Booking ID</p>
                  <p class="font-mono text-sm text-foreground">#{{ booking.id.slice(0, 6) }}</p>
                </div>
              </div>

              <!-- QR Code -->
              <div class="bg-secondary rounded-lg p-4 flex items-center justify-center h-40">
                <div class="text-center space-y-2">
                  <div class="bg-muted/50 rounded w-32 h-32 mx-auto flex items-center justify-center text-muted-foreground font-bold border-2 border-border">
                    {{ generateQRSimulation(booking.id) }}
                  </div>
                  <p class="text-xs text-muted-foreground">Scan at entry</p>
                </div>
              </div>

              <div class="flex gap-2">
                <button class="flex-1 px-4 py-2 rounded-lg bg-primary text-primary-foreground hover:bg-primary/90 transition-smooth text-sm font-medium">
                  Show Ticket
                </button>
                <button class="flex-1 px-4 py-2 rounded-lg border border-border text-foreground hover:bg-secondary transition-smooth text-sm font-medium">
                  Download
                </button>
              </div>
            </div>
          </div>
        </div>

        <ng-template #noTickets>
          <div class="glassmorphism rounded-lg p-12 text-center space-y-4">
            <p class="text-4xl">🎬</p>
            <p class="text-foreground font-medium">No active tickets</p>
            <p class="text-sm text-muted-foreground">Book your next movie to see your tickets here</p>
            <a
              routerLink="/"
              class="inline-block px-6 py-2 rounded-lg bg-primary text-primary-foreground hover:bg-primary/90 transition-smooth text-sm font-medium"
            >
              Browse Movies
            </a>
          </div>
        </ng-template>
      </section>

      <!-- Booking History -->
      <section class="space-y-4">
        <h2 class="text-2xl font-bold text-foreground">Booking History</h2>

        <!-- Tabs -->
        <div class="flex gap-4 border-b border-border overflow-x-auto">
          <button
            (click)="activeTab.set('upcoming')"
            [class.text-primary]="activeTab() === 'upcoming'"
            [class.border-b-2]="activeTab() === 'upcoming'"
            [class.border-primary]="activeTab() === 'upcoming'"
            [class.text-muted-foreground]="activeTab() !== 'upcoming'"
            class="px-4 py-2 font-medium transition-smooth whitespace-nowrap"
          >
            Upcoming
          </button>
          <button
            (click)="activeTab.set('past')"
            [class.text-primary]="activeTab() === 'past'"
            [class.border-b-2]="activeTab() === 'past'"
            [class.border-primary]="activeTab() === 'past'"
            [class.text-muted-foreground]="activeTab() !== 'past'"
            class="px-4 py-2 font-medium transition-smooth whitespace-nowrap"
          >
            Past Bookings
          </button>
        </div>

        <!-- Bookings List -->
        <div class="space-y-3">
          <div *ngFor="let booking of getBookings()" class="glassmorphism rounded-lg p-6 flex items-center justify-between hover:bg-card/80 transition-smooth">
            <div class="space-y-1">
              <p class="font-bold text-foreground">{{ booking.title }}</p>
              <p class="text-sm text-muted-foreground">{{ booking.date }} • {{ booking.time }}</p>
            </div>
            <div class="text-right space-y-1">
              <p class="text-sm font-medium text-foreground">{{ booking.seats }}</p>
              <button
                class="text-xs text-primary hover:text-primary/80 font-medium transition-colors"
                [attr.title]="booking.action"
              >
                {{ booking.action }}
              </button>
            </div>
          </div>
        </div>
      </section>

      <!-- Favorites -->
      <section class="space-y-4">
        <h2 class="text-2xl font-bold text-foreground">Your Favorites</h2>
        <div class="grid grid-cols-2 md:grid-cols-4 lg:grid-cols-5 gap-4">
          <a *ngFor="let movie of favoriteMovies" routerLink="/" class="group cursor-pointer">
            <div class="aspect-video bg-secondary rounded-lg glassmorphism hover:neon-glow group-hover:scale-105 transition-smooth flex items-center justify-center text-muted-foreground font-medium">
              {{ movie }}
            </div>
            <p class="mt-3 font-medium text-foreground group-hover:text-primary transition-colors text-sm line-clamp-2">
              {{ movie }}
            </p>
          </a>
        </div>
      </section>
    </div>
  `,
})
export class DashboardComponent implements OnInit {
  private authService = inject(AuthService);
  private bookingService = inject(BookingService);
  private toastService = inject(ToastService);

  protected authService_export = this.authService;

  ngOnInit(): void {
    // Show welcome toast
    const user = this.authService.getCurrentUser();
    if (user) {
      this.toastService.success(`Welcome back, ${user.name}!`);
    }
  }

  activeTab = signal<'upcoming' | 'past'>('upcoming');

  upcomingBookings = [
    {
      id: '12345abc',
      title: 'Cosmic Voyage',
      date: 'Dec 25, 2024',
      time: '7:30 PM',
      seats: 'A5, A6, A7',
      totalPrice: 750,
    },
    {
      id: '67890def',
      title: 'The Last Kingdom',
      date: 'Jan 5, 2025',
      time: '4:45 PM',
      seats: 'B3, B4',
      totalPrice: 500,
    },
  ];

  pastBookings = [
    { title: 'Love in Paradise', date: 'Dec 10, 2024', time: '6:00 PM', seats: 'D2, D3', action: 'Rebook' },
    { title: 'Action Adventure', date: 'Nov 28, 2024', time: '8:30 PM', seats: 'E5', action: 'Rate' },
  ];

  favoriteMovies = [
    'Cosmic Voyage',
    'The Last Kingdom',
    'Laughter in Paradise',
    'Midnight Horror',
    'Love in the City',
  ];

  totalSpent = 4250;

  getBookings() {
    return this.activeTab() === 'upcoming' ? this.upcomingBookings : this.pastBookings;
  }

  generateQRSimulation(id: string): string {
    return id.slice(0, 4).toUpperCase();
  }
}
