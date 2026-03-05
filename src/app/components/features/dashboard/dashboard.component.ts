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
      <div *ngIf="authService.getCurrentUser() as user" class="space-y-2">
        <h1 class="text-4xl font-bold text-foreground">Welcome, {{ user.name }}!</h1>
        <p class="text-muted-foreground">Manage your tickets, bookings, and account preferences</p>
      </div>

      <!-- Stats Cards -->
      <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
        <div class="rounded-xl border bg-card text-card-foreground shadow">
          <div class="flex flex-col space-y-1.5 p-6">
            <p class="text-sm text-muted-foreground font-medium">Upcoming Tickets</p>
            <p class="text-4xl font-bold text-primary">{{ upcomingBookings.length }}</p>
            <p class="text-xs text-muted-foreground">Next show in 5 days</p>
          </div>
        </div>
        <div class="rounded-xl border bg-card text-card-foreground shadow">
          <div class="flex flex-col space-y-1.5 p-6">
            <p class="text-sm text-muted-foreground font-medium">Total Spent</p>
            <p class="text-4xl font-bold text-primary">{{ totalSpent }}</p>
            <p class="text-xs text-muted-foreground">This year</p>
          </div>
        </div>
        <div class="rounded-xl border bg-card text-card-foreground shadow">
          <div class="flex flex-col space-y-1.5 p-6">
            <p class="text-sm text-muted-foreground font-medium">Favorite Movies</p>
            <p class="text-4xl font-bold text-primary">{{ favoriteMovies.length }}</p>
            <p class="text-xs text-muted-foreground">Saved for later</p>
          </div>
        </div>
      </div>

      <!-- Active Tickets -->
      <section class="space-y-4">
        <h2 class="text-2xl font-bold text-foreground">Active Tickets</h2>
        <div *ngIf="upcomingBookings.length > 0; else noTickets" class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          <div *ngFor="let booking of upcomingBookings" class="rounded-xl border bg-card text-card-foreground shadow overflow-hidden hover:shadow-md transition-all duration-200">
            <!-- Ticket Header -->
            <div class="bg-primary/10 px-6 py-4 border-b">
              <p class="font-semibold text-foreground">{{ booking.title }}</p>
              <p class="text-sm text-muted-foreground">{{ booking.date }} &bull; {{ booking.time }}</p>
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

              <!-- QR Code Placeholder -->
              <div class="rounded-lg border bg-secondary p-4 flex items-center justify-center h-40">
                <div class="text-center space-y-2">
                  <div class="rounded-md border-2 w-32 h-32 mx-auto flex items-center justify-center text-muted-foreground font-bold bg-background">
                    {{ generateQRSimulation(booking.id) }}
                  </div>
                  <p class="text-xs text-muted-foreground">Scan at entry</p>
                </div>
              </div>

              <div class="flex gap-2">
                <button class="flex-1 inline-flex items-center justify-center gap-2 whitespace-nowrap rounded-md text-sm font-medium transition-colors focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring bg-primary text-primary-foreground shadow hover:bg-primary/90 h-9 px-4 py-2">
                  Show Ticket
                </button>
                <button class="flex-1 inline-flex items-center justify-center gap-2 whitespace-nowrap rounded-md text-sm font-medium transition-colors focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring border border-input bg-background shadow-sm hover:bg-accent hover:text-accent-foreground h-9 px-4 py-2">
                  Download
                </button>
              </div>
            </div>
          </div>
        </div>

        <ng-template #noTickets>
          <div class="rounded-xl border bg-card text-card-foreground shadow p-12 text-center space-y-4">
            <p class="text-4xl" aria-hidden="true">&#127916;</p>
            <p class="text-foreground font-medium">No active tickets</p>
            <p class="text-sm text-muted-foreground">Book your next movie to see your tickets here</p>
            <a
              routerLink="/"
              class="inline-flex items-center justify-center gap-2 whitespace-nowrap rounded-md text-sm font-medium transition-colors focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring bg-primary text-primary-foreground shadow hover:bg-primary/90 h-9 px-4 py-2"
            >
              Browse Movies
            </a>
          </div>
        </ng-template>
      </section>

      <!-- Booking History -->
      <section class="space-y-4">
        <h2 class="text-2xl font-bold text-foreground">Booking History</h2>

        <!-- Tabs (shadcn pattern) -->
        <div class="inline-flex h-9 items-center justify-center rounded-lg bg-muted p-1 text-muted-foreground">
          <button
            (click)="activeTab.set('upcoming')"
            [ngClass]="{
              'bg-background text-foreground shadow': activeTab() === 'upcoming',
              'text-muted-foreground': activeTab() !== 'upcoming'
            }"
            class="inline-flex items-center justify-center whitespace-nowrap rounded-md px-3 py-1 text-sm font-medium ring-offset-background transition-all"
          >
            Upcoming
          </button>
          <button
            (click)="activeTab.set('past')"
            [ngClass]="{
              'bg-background text-foreground shadow': activeTab() === 'past',
              'text-muted-foreground': activeTab() !== 'past'
            }"
            class="inline-flex items-center justify-center whitespace-nowrap rounded-md px-3 py-1 text-sm font-medium ring-offset-background transition-all"
          >
            Past Bookings
          </button>
        </div>

        <!-- Bookings List -->
        <div class="space-y-3">
          <div *ngFor="let booking of getBookings()" class="rounded-xl border bg-card text-card-foreground shadow p-6 flex items-center justify-between hover:shadow-md transition-all duration-200">
            <div class="space-y-1">
              <p class="font-semibold text-foreground text-sm">{{ booking.title }}</p>
              <p class="text-sm text-muted-foreground">{{ booking.date }} &bull; {{ booking.time }}</p>
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
            <div class="aspect-video rounded-xl border bg-card shadow overflow-hidden group-hover:shadow-md group-hover:scale-[1.02] transition-all duration-200 flex items-center justify-center text-muted-foreground font-medium">
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
  protected authService = inject(AuthService);
  private bookingService = inject(BookingService);
  private toastService = inject(ToastService);

  ngOnInit(): void {
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
      action: 'View Ticket',
    },
    {
      id: '67890def',
      title: 'The Last Kingdom',
      date: 'Jan 5, 2025',
      time: '4:45 PM',
      seats: 'B3, B4',
      totalPrice: 500,
      action: 'View Ticket',
    },
  ];

  pastBookings = [
    {
      id: 'past1',
      title: 'Love in Paradise',
      date: 'Dec 10, 2024',
      time: '6:00 PM',
      seats: 'D2, D3',
      totalPrice: 450,
      action: 'Rebook',
    },
    {
      id: 'past2',
      title: 'Action Adventure',
      date: 'Nov 28, 2024',
      time: '8:30 PM',
      seats: 'E5',
      totalPrice: 250,
      action: 'Rate',
    },
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
