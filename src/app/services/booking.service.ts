import { Injectable, signal } from '@angular/core';

export interface Showtime {
  id: string;
  time: string;
  hall: string;
  format: string;
  pricePerSeat: number;
  availableSeats: number;
}

export interface Booking {
  id: string;
  movieId: string;
  showtimeId: string;
  seats: string[];
  totalPrice: number;
  status: 'pending' | 'confirmed' | 'cancelled';
  bookingDate: Date;
}

@Injectable({
  providedIn: 'root',
})
export class BookingService {
  bookings = signal<Booking[]>([]);
  currentBooking = signal<Partial<Booking>>({
    seats: [],
    status: 'pending',
  });

  getShowtimes(movieId: string, date: string): Showtime[] {
    const times = [
      { id: '1', time: '10:00 AM', hall: 'Hall 1', format: '2D', pricePerSeat: 250 },
      { id: '2', time: '1:30 PM', hall: 'Hall 2', format: '3D', pricePerSeat: 300 },
      { id: '3', time: '4:45 PM', hall: 'Hall 3', format: '2D', pricePerSeat: 250 },
      { id: '4', time: '7:30 PM', hall: 'Hall 1', format: '3D', pricePerSeat: 350 },
      { id: '5', time: '10:15 PM', hall: 'Hall 2', format: '2D', pricePerSeat: 250 },
    ];

    return times.map((t) => ({
      ...t,
      availableSeats: Math.floor(Math.random() * 50) + 20,
    }));
  }

  getOccupiedSeats(movieId: string, showtimeId: string): Set<string> {
    // Mock occupied seats
    const occupied = new Set<string>();
    occupied.add('A4');
    occupied.add('A5');
    occupied.add('B3');
    occupied.add('B8');
    occupied.add('C2');
    occupied.add('C9');
    occupied.add('D5');
    occupied.add('E7');
    return occupied;
  }

  addSeat(seat: string): void {
    const seats = [...this.currentBooking().seats!];
    if (!seats.includes(seat)) {
      seats.push(seat);
      this.currentBooking.update((b) => ({ ...b, seats }));
    }
  }

  removeSeat(seat: string): void {
    const seats = this.currentBooking().seats!.filter((s) => s !== seat);
    this.currentBooking.update((b) => ({ ...b, seats }));
  }

  confirmBooking(booking: Booking): void {
    this.bookings.update((bookings) => [...bookings, booking]);
    this.currentBooking.set({ seats: [], status: 'pending' });
  }

  getBookingHistory(): Booking[] {
    return this.bookings();
  }
}
