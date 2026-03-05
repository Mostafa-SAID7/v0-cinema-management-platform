import { signal } from '@angular/core';

export interface User {
  id: string;
  name: string;
  email: string;
  avatar?: string;
}

export const authSignal = signal<{
  isAuthenticated: boolean;
  user: User | null;
  loading: boolean;
  error: string | null;
}>({
  isAuthenticated: false,
  user: null,
  loading: false,
  error: null,
});
