import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-booking',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="p-6 md:p-8">
      <h1 class="text-4xl font-bold text-foreground mb-8">Select Your Seats</h1>

      <div class="grid grid-cols-1 lg:grid-cols-3 gap-8">
        <!-- Seat Map -->
        <div class="lg:col-span-2 space-y-6">
          <!-- Showtime Selection -->
          <div class="glassmorphism rounded-lg p-6 space-y-4">
            <h2 class="text-xl font-bold text-foreground">Select Showtime</h2>
            <div class="grid grid-cols-2 md:grid-cols-3 gap-3">
              <button
                *ngFor="let time of showtimes; let i = index"
                (click)="selectShowtime(i)"
                [class.bg-primary]="selectedShowtime() === i"
                [class.text-primary-foreground]="selectedShowtime() === i"
                [class.bg-secondary]="selectedShowtime() !== i"
                [class.text-foreground]="selectedShowtime() !== i"
                class="px-4 py-3 rounded-lg font-medium transition-smooth hover:border-primary"
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
                <div class="h-2 bg-gradient-to-r from-transparent via-primary to-transparent rounded-full"></div>
              </div>
            </div>

            <!-- Seat Grid -->
            <div class="bg-secondary rounded-lg p-8 space-y-2">
              <div *ngFor="let row of seatGrid; let rowIndex = index" class="flex justify-center gap-2">
                <span class="text-xs text-muted-foreground w-6 flex items-center justify-center">{{ rowLabels[rowIndex] }}</span>
                <div class="flex gap-2">
                  <button
                    *ngFor="let seat of row; let seatIndex = index"
                    (click)="toggleSeat(rowIndex, seatIndex)"
                    [class.bg-primary]="isSelected(rowIndex, seatIndex)"
                    [class.bg-muted]="isOccupied(rowIndex, seatIndex)"
                    [class.bg-secondary]="!isSelected(rowIndex, seatIndex) && !isOccupied(rowIndex, seatIndex)"
                    [disabled]="isOccupied(rowIndex, seatIndex)"
                    class="w-8 h-8 rounded hover:scale-110 transition-transform disabled:opacity-50 disabled:cursor-not-allowed border border-border"
                  >
                    <span class="text-xs font-bold">{{ seatIndex + 1 }}</span>
                  </button>
                </div>
              </div>
            </div>

            <!-- Legend -->
            <div class="flex flex-wrap gap-6 justify-center pt-4">
              <div class="flex items-center gap-2">
                <div class="w-4 h-4 rounded bg-secondary border border-border"></div>
                <span class="text-sm text-muted-foreground">Available</span>
              </div>
              <div class="flex items-center gap-2">
                <div class="w-4 h-4 rounded bg-primary"></div>
                <span class="text-sm text-muted-foreground">Selected</span>
              </div>
              <div class="flex items-center gap-2">
                <div class="w-4 h-4 rounded bg-muted"></div>
                <span class="text-sm text-muted-foreground">Occupied</span>
              </div>
            </div>
          </div>
        </div>

        <!-- Booking Summary -->
        <div class="lg:col-span-1">
          <div class="glassmorphism rounded-lg p-6 space-y-6 sticky top-20">
            <div>
              <h3 class="text-lg font-bold text-foreground mb-2">Booking Summary</h3>
              <p class="text-sm text-muted-foreground">Review your selection</p>
            </div>

            <div class="space-y-3 border-t border-border pt-4">
              <div class="flex justify-between">
                <span class="text-muted-foreground">Showtime:</span>
                <span class="font-medium text-foreground">{{ selectedShowtime() >= 0 ? showtimes[selectedShowtime()] : 'Select time' }}</span>
              </div>
              <div class="flex justify-between">
                <span class="text-muted-foreground">Seats:</span>
                <span class="font-medium text-foreground">{{ selectedSeats().length }} seat(s)</span>
              </div>
              <div class="flex justify-between">
                <span class="text-muted-foreground">Price per seat:</span>
                <span class="font-medium text-foreground">₹250</span>
              </div>
            </div>

            <div class="border-t border-border pt-4 space-y-3">
              <div class="flex justify-between">
                <span class="font-bold text-foreground">Total:</span>
                <span class="text-2xl font-bold text-primary neon-text">₹{{ selectedSeats().length * 250 }}</span>
              </div>
            </div>

            <button
              [disabled]="selectedSeats().length === 0"
              class="w-full px-4 py-3 rounded-lg bg-primary text-primary-foreground font-bold hover:bg-primary/90 neon-glow transition-smooth disabled:opacity-50 disabled:cursor-not-allowed"
            >
              Proceed to Payment
            </button>

            <button
              class="w-full px-4 py-2 rounded-lg border border-border text-foreground hover:bg-secondary transition-smooth"
            >
              Continue Shopping
            </button>
          </div>
        </div>
      </div>
    </div>
  `,
})
export class BookingComponent {
  selectedShowtime = signal(-1);
  selectedSeats = signal<string[]>([]);

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
}
