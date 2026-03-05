import { Component, signal, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ToastService } from '../../../services/toast.service';

@Component({
  selector: 'app-booking',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <div class="p-6 md:p-8">
      <h1 class="text-4xl font-bold text-foreground mb-8">Select Your Seats</h1>

      <div class="grid grid-cols-1 lg:grid-cols-3 gap-8">
        <!-- Seat Map -->
        <div class="lg:col-span-2 space-y-6">
          <!-- Showtime Selection -->
          <div class="rounded-xl border bg-card text-card-foreground shadow p-6 space-y-4">
            <h2 class="font-semibold leading-none tracking-tight text-lg">Select Showtime</h2>
            <div class="grid grid-cols-2 md:grid-cols-3 gap-3">
              <button
                *ngFor="let time of showtimes; let i = index"
                (click)="selectShowtime(i)"
                [ngClass]="{
                  'bg-primary text-primary-foreground shadow hover:bg-primary/90': selectedShowtime() === i,
                  'border border-input bg-background shadow-sm hover:bg-accent hover:text-accent-foreground': selectedShowtime() !== i
                }"
                class="inline-flex items-center justify-center whitespace-nowrap rounded-md text-sm font-medium transition-colors h-10 px-4 py-2"
              >
                {{ time }}
              </button>
            </div>
          </div>

          <!-- Screen -->
          <div class="space-y-4">
            <div class="flex justify-center">
              <div class="relative w-full max-w-md">
                <div class="absolute -top-6 left-0 right-0 text-center text-xs font-bold text-muted-foreground uppercase tracking-widest">
                  Screen
                </div>
                <div class="h-1 bg-gradient-to-r from-transparent via-primary to-transparent rounded-full"></div>
              </div>
            </div>

            <!-- Seat Grid -->
            <div class="rounded-xl border bg-card shadow p-8 space-y-2">
              <div *ngFor="let row of seatGrid; let rowIndex = index" class="flex justify-center gap-2">
                <span class="text-xs text-muted-foreground w-6 flex items-center justify-center font-medium">{{ rowLabels[rowIndex] }}</span>
                <div class="flex gap-2">
                  <button
                    *ngFor="let seat of row; let seatIndex = index"
                    (click)="toggleSeat(rowIndex, seatIndex)"
                    [ngClass]="{
                      'bg-primary text-primary-foreground': isSelected(rowIndex, seatIndex),
                      'bg-muted text-muted-foreground': isOccupied(rowIndex, seatIndex),
                      'bg-secondary text-secondary-foreground hover:bg-accent': !isSelected(rowIndex, seatIndex) && !isOccupied(rowIndex, seatIndex)
                    }"
                    [disabled]="isOccupied(rowIndex, seatIndex)"
                    class="w-8 h-8 rounded-md hover:scale-110 transition-all duration-200 disabled:opacity-50 disabled:cursor-not-allowed border text-xs font-bold"
                  >
                    {{ seatIndex + 1 }}
                  </button>
                </div>
              </div>
            </div>

            <!-- Legend -->
            <div class="flex flex-wrap gap-6 justify-center pt-4">
              <div class="flex items-center gap-2">
                <div class="w-4 h-4 rounded-md bg-secondary border"></div>
                <span class="text-sm text-muted-foreground">Available</span>
              </div>
              <div class="flex items-center gap-2">
                <div class="w-4 h-4 rounded-md bg-primary"></div>
                <span class="text-sm text-muted-foreground">Selected</span>
              </div>
              <div class="flex items-center gap-2">
                <div class="w-4 h-4 rounded-md bg-muted"></div>
                <span class="text-sm text-muted-foreground">Occupied</span>
              </div>
            </div>
          </div>
        </div>

        <!-- Booking Summary -->
        <div class="lg:col-span-1">
          <div class="rounded-xl border bg-card text-card-foreground shadow p-6 space-y-6 sticky top-20">
            <div class="flex flex-col space-y-1.5">
              <h3 class="font-semibold leading-none tracking-tight">Booking Summary</h3>
              <p class="text-sm text-muted-foreground">Review your selection</p>
            </div>

            <!-- Separator -->
            <div class="shrink-0 bg-border h-px w-full"></div>

            <div class="space-y-3">
              <div class="flex justify-between text-sm">
                <span class="text-muted-foreground">Showtime:</span>
                <span class="font-medium text-foreground">{{ selectedShowtime() >= 0 ? showtimes[selectedShowtime()] : 'Select time' }}</span>
              </div>
              <div class="flex justify-between text-sm">
                <span class="text-muted-foreground">Seats:</span>
                <span class="font-medium text-foreground">{{ selectedSeats().length }} seat(s)</span>
              </div>
              <div class="flex justify-between text-sm">
                <span class="text-muted-foreground">Price per seat:</span>
                <span class="font-medium text-foreground">250</span>
              </div>
            </div>

            <!-- Separator -->
            <div class="shrink-0 bg-border h-px w-full"></div>

            <div class="flex justify-between items-center">
              <span class="font-semibold text-foreground">Total:</span>
              <span class="text-2xl font-bold text-primary">{{ selectedSeats().length * 250 }}</span>
            </div>

            <div class="flex flex-col gap-2">
              <button
                [disabled]="selectedSeats().length === 0"
                (click)="confirmBooking()"
                class="inline-flex items-center justify-center gap-2 whitespace-nowrap rounded-md text-sm font-medium transition-colors focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring disabled:pointer-events-none disabled:opacity-50 bg-primary text-primary-foreground shadow hover:bg-primary/90 h-10 px-4 py-2 w-full"
              >
                Proceed to Payment
              </button>

              <button
                (click)="resetBooking()"
                class="inline-flex items-center justify-center gap-2 whitespace-nowrap rounded-md text-sm font-medium transition-colors focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring border border-input bg-background shadow-sm hover:bg-accent hover:text-accent-foreground h-9 px-4 py-2 w-full"
              >
                Clear Selection
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
})
export class BookingComponent implements OnInit {
  private toastService = inject(ToastService);

  selectedShowtime = signal(-1);
  selectedSeats = signal<string[]>([]);

  ngOnInit(): void {
    this.toastService.info('Select a showtime and click seats to book your ticket');
  }

  showtimes = ['10:00 AM', '1:30 PM', '4:45 PM', '7:30 PM', '10:15 PM'];

  rowLabels = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H'];

  seatGrid = Array(8)
    .fill(null)
    .map(() => Array(10).fill(false));

  occupiedSeats = new Set(['A4', 'A5', 'B3', 'B8', 'C2', 'C9']);

  selectShowtime(index: number): void {
    this.selectedShowtime.set(index);
  }

  toggleSeat(rowIndex: number, seatIndex: number): void {
    const seatId = `${this.rowLabels[rowIndex]}${seatIndex + 1}`;
    const seats = [...this.selectedSeats()];
    const index = seats.indexOf(seatId);

    if (index >= 0) {
      seats.splice(index, 1);
    } else if (seats.length < 10) {
      seats.push(seatId);
    }

    this.selectedSeats.set(seats);
  }

  isSelected(rowIndex: number, seatIndex: number): boolean {
    const seatId = `${this.rowLabels[rowIndex]}${seatIndex + 1}`;
    return this.selectedSeats().includes(seatId);
  }

  isOccupied(rowIndex: number, seatIndex: number): boolean {
    const seatId = `${this.rowLabels[rowIndex]}${seatIndex + 1}`;
    return this.occupiedSeats.has(seatId);
  }

  confirmBooking(): void {
    const seatCount = this.selectedSeats().length;
    const totalPrice = seatCount * 250;
    const showtime = this.showtimes[this.selectedShowtime()];

    this.toastService.success(
      `Booking confirmed! ${seatCount} seat(s) for ${totalPrice} at ${showtime}. Proceeding to payment...`
    );
  }

  resetBooking(): void {
    this.selectedSeats.set([]);
    this.toastService.info('Selection cleared');
  }
}
