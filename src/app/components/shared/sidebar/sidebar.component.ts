import { Component, inject, computed, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <div class="flex flex-col h-full bg-card">
      <!-- Logo -->
      <div class="flex items-center gap-2 px-6 py-8 border-b border-border">
        <div class="flex items-center justify-center w-8 h-8 rounded-lg bg-primary neon-glow">
          <span class="text-primary-foreground font-bold text-sm">CV</span>
        </div>
        <span class="text-xl font-bold text-foreground">CinemaVerse</span>
      </div>

      <!-- Navigation Links -->
      <nav class="flex-1 px-4 py-6 space-y-2">
        <a
          routerLink="/"
          routerLinkActive="bg-primary text-primary-foreground"
          [routerLinkActiveOptions]="{ exact: true }"
          class="flex items-center gap-3 px-4 py-3 rounded-lg text-foreground hover:bg-secondary transition-smooth cursor-pointer"
        >
          <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M7 21a4 4 0 01-4-4V9a7 7 0 0114 0v8a4 4 0 01-4 4zm0 0h12a2 2 0 002-2v-4.586a1 1 0 00-.293-.707l-2.828-2.829A1 1 0 0017 6h-5m0 16V9m0 16H9m4 0h4"></path>
          </svg>
          <span class="font-medium">Discover</span>
        </a>

        <a
          routerLink="/dashboard"
          routerLinkActive="bg-primary text-primary-foreground"
          [routerLinkActiveOptions]="{ exact: false }"
          class="flex items-center gap-3 px-4 py-3 rounded-lg text-foreground hover:bg-secondary transition-smooth cursor-pointer"
        >
          <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M14 10l-2 1m0 0l-2-1m2 1v2.5M20 7l-2 1m2-1l-2-1m2 1v2.5M14 4l-2 1m2-1l-2-1m2 1v2.5"></path>
          </svg>
          <span class="font-medium">My Tickets</span>
        </a>

        <a
          routerLink="/support"
          routerLinkActive="bg-primary text-primary-foreground"
          [routerLinkActiveOptions]="{ exact: false }"
          class="flex items-center gap-3 px-4 py-3 rounded-lg text-foreground hover:bg-secondary transition-smooth cursor-pointer"
        >
          <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8.228 9c.549-1.165 2.03-2 3.772-2 2.21 0 4 1.343 4 3 0 1.4-1.278 2.575-3.006 2.907-.542.104-.994.54-.994 1.093m0 3h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"></path>
          </svg>
          <span class="font-medium">Support</span>
        </a>
      </nav>

      <!-- User Section -->
      <div class="border-t border-border">
        <!-- User Info (if logged in) -->
        <div *ngIf="isAuthenticated()" class="px-4 py-4 space-y-3">
          <div class="flex items-center gap-3">
            <div class="w-10 h-10 rounded-full bg-primary flex items-center justify-center text-primary-foreground font-bold text-sm">
              {{ getInitials(currentUser()?.name || 'User') }}
            </div>
            <div class="flex-1 min-w-0">
              <p class="font-medium text-foreground truncate">{{ currentUser()?.name || 'User' }}</p>
              <p class="text-xs text-muted-foreground truncate">{{ currentUser()?.email || 'user@example.com' }}</p>
            </div>
          </div>
          <button
            (click)="logout()"
            class="w-full px-4 py-2 rounded-lg border border-border text-foreground hover:bg-secondary transition-smooth text-sm font-medium"
          >
            Logout
          </button>
        </div>

        <!-- Sign In (if not logged in) -->
        <div *ngIf="!isAuthenticated()" class="px-4 py-4">
          <button
            class="w-full px-4 py-2 rounded-lg bg-primary text-primary-foreground font-medium hover:bg-primary/90 transition-smooth neon-glow text-sm"
          >
            Sign In
          </button>
        </div>

        <!-- Footer -->
        <div class="px-4 py-4 border-t border-border">
          <p class="text-xs text-muted-foreground">
            © 2024 CinemaVerse. All rights reserved.
          </p>
        </div>
      </div>
    </div>
  `,
})
export class SidebarComponent {
  private authService = inject(AuthService);

  isAuthenticated = computed(() => this.authService.isAuthenticated());
  currentUser = computed(() => this.authService.getCurrentUser());

  getInitials(name: string): string {
    return name
      .split(' ')
      .map((n) => n[0])
      .join('')
      .toUpperCase()
      .slice(0, 2);
  }

  logout(): void {
    this.authService.logout();
  }
}
