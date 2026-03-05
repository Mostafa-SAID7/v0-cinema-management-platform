import { Injectable, signal } from '@angular/core';
import { authSignal, User } from '../signals/auth.signal';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  // Inject the auth signal
  auth = authSignal;

  login(email: string, password: string): void {
    this.auth.update((state) => ({
      ...state,
      loading: true,
      error: null,
    }));

    // Simulate API call
    setTimeout(() => {
      const user: User = {
        id: '1',
        name: 'John Doe',
        email: email,
        avatar: 'JD',
      };

      this.auth.update((state) => ({
        isAuthenticated: true,
        user: user,
        loading: false,
        error: null,
      }));
    }, 500);
  }

  register(name: string, email: string, password: string): void {
    this.auth.update((state) => ({
      ...state,
      loading: true,
      error: null,
    }));

    // Simulate API call
    setTimeout(() => {
      const user: User = {
        id: Math.random().toString(36).substr(2, 9),
        name: name,
        email: email,
        avatar: name.charAt(0).toUpperCase(),
      };

      this.auth.update((state) => ({
        isAuthenticated: true,
        user: user,
        loading: false,
        error: null,
      }));
    }, 500);
  }

  logout(): void {
    this.auth.set({
      isAuthenticated: false,
      user: null,
      loading: false,
      error: null,
    });
  }

  getCurrentUser(): User | null {
    return this.auth().user;
  }

  isAuthenticated(): boolean {
    return this.auth().isAuthenticated;
  }

  setError(error: string): void {
    this.auth.update((state) => ({
      ...state,
      error: error,
    }));
  }
}
