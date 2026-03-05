import { Injectable, signal } from '@angular/core';

export type Theme = 'light' | 'dark';

@Injectable({
  providedIn: 'root',
})
export class ThemeService {
  theme = signal<Theme>('dark');

  constructor() {
    this.loadThemeFromStorage();
  }

  initializeTheme(): void {
    const htmlElement = document.documentElement;
    if (this.theme() === 'dark') {
      htmlElement.classList.add('dark');
    } else {
      htmlElement.classList.remove('dark');
    }
  }

  toggleTheme(): void {
    const newTheme = this.theme() === 'dark' ? 'light' : 'dark';
    this.theme.set(newTheme);
    this.saveThemeToStorage(newTheme);
    this.initializeTheme();
  }

  setTheme(theme: Theme): void {
    this.theme.set(theme);
    this.saveThemeToStorage(theme);
    this.initializeTheme();
  }

  private loadThemeFromStorage(): void {
    const savedTheme = localStorage.getItem('theme') as Theme | null;
    if (savedTheme) {
      this.theme.set(savedTheme);
    } else {
      // Check system preference
      const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
      this.theme.set(prefersDark ? 'dark' : 'light');
    }
  }

  private saveThemeToStorage(theme: Theme): void {
    localStorage.setItem('theme', theme);
  }
}
