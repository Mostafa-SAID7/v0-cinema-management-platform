import { Component, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Menu, X, Moon, Sun, Search } from 'lucide-angular';
import { ThemeService } from '../../../services/theme.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <header class="sticky top-0 z-30 border-b border-border bg-card">
      <div class="flex items-center justify-between h-16 px-4 md:px-6">
        <!-- Mobile Menu Toggle -->
        <button
          (click)="sidebarOpenChange.emit(true)"
          class="md:hidden p-2 hover:bg-secondary rounded-lg transition-smooth"
          aria-label="Open menu"
        >
          <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16"></path>
          </svg>
        </button>

        <!-- Search Bar (Desktop) -->
        <div class="hidden md:flex flex-1 mx-6">
          <div class="relative w-full max-w-sm">
            <input
              type="search"
              placeholder="Search movies..."
              class="w-full px-4 py-2 rounded-lg bg-secondary text-foreground placeholder-muted-foreground focus:outline-none focus:ring-2 focus:ring-primary"
            />
            <svg class="absolute right-3 top-2.5 w-5 h-5 text-muted-foreground" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"></path>
            </svg>
          </div>
        </div>

        <!-- Right Actions -->
        <div class="flex items-center gap-4">
          <!-- Mobile Search -->
          <button class="md:hidden p-2 hover:bg-secondary rounded-lg transition-smooth">
            <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"></path>
            </svg>
          </button>

          <!-- Theme Toggle -->
          <button
            (click)="toggleTheme()"
            class="p-2 hover:bg-secondary rounded-lg transition-smooth"
            [attr.aria-label]="themeService.theme() === 'dark' ? 'Switch to light mode' : 'Switch to dark mode'"
          >
            <svg *ngIf="themeService.theme() === 'dark'" class="w-5 h-5" fill="currentColor" viewBox="0 0 24 24">
              <path d="M12 18a6 6 0 100-12 6 6 0 000 12zM12 2v4m0 12v4M6.22 6.22l2.83 2.83m5.9 5.9l2.83 2.83M2 12h4m12 0h4M6.22 17.78l2.83-2.83m5.9-5.9l2.83-2.83"></path>
            </svg>
            <svg *ngIf="themeService.theme() === 'light'" class="w-5 h-5" fill="currentColor" viewBox="0 0 24 24">
              <path d="M21 12.79A9 9 0 1111.21 3 7 7 0 0021 12.79z"></path>
            </svg>
          </button>

          <!-- User Profile -->
          <button class="p-2 hover:bg-secondary rounded-lg transition-smooth">
            <div class="w-8 h-8 rounded-full bg-primary flex items-center justify-center text-primary-foreground font-bold text-sm">
              JD
            </div>
          </button>
        </div>
      </div>
    </header>
  `,
})
export class NavbarComponent {
  sidebarOpen = input(false);
  sidebarOpenChange = output<boolean>();

  constructor(public themeService: ThemeService) {}

  toggleTheme(): void {
    this.themeService.toggleTheme();
  }
}
