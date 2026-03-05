import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  private baseUrl = 'https://api.example.com';

  // Movies endpoints
  getMovies() {
    return `${this.baseUrl}/api/movies`;
  }

  getMovie(id: string) {
    return `${this.baseUrl}/api/movies/${id}`;
  }

  getShowtimes(movieId: string) {
    return `${this.baseUrl}/api/movies/${movieId}/showtimes`;
  }

  // Bookings endpoints
  createBooking(data: any) {
    return `${this.baseUrl}/api/bookings`;
  }

  getUserBookings() {
    return `${this.baseUrl}/api/user/bookings`;
  }

  // Auth endpoints
  login(email: string, password: string) {
    return `${this.baseUrl}/api/auth/login`;
  }

  register(name: string, email: string, password: string) {
    return `${this.baseUrl}/api/auth/register`;
  }

  logout() {
    return `${this.baseUrl}/api/auth/logout`;
  }

  // Reviews endpoints
  getReviews(movieId: string) {
    return `${this.baseUrl}/api/movies/${movieId}/reviews`;
  }

  postReview(movieId: string, review: any) {
    return `${this.baseUrl}/api/movies/${movieId}/reviews`;
  }

  // User endpoints
  getUserProfile() {
    return `${this.baseUrl}/api/user/profile`;
  }

  updateUserProfile(data: any) {
    return `${this.baseUrl}/api/user/profile`;
  }

  getFavorites() {
    return `${this.baseUrl}/api/user/favorites`;
  }

  addFavorite(movieId: string) {
    return `${this.baseUrl}/api/user/favorites`;
  }

  removeFavorite(movieId: string) {
    return `${this.baseUrl}/api/user/favorites/${movieId}`;
  }
}
