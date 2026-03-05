import { Component, inject, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <div class="flex flex-col h-full bg-sidebar text-sidebar-foreground">
      <!-- Logo -->
      <div class="flex items-center gap-2 px-6 py-6 border-b border-sidebar-border">
        <div class="flex items-center justify-center w-8 h-8 rounded-lg bg-sidebar-primary shadow-sm">
          <span class="text-sidebar-primary-foreground font-bold text-sm">CV</span>
        </div>
        <span class="text-xl font-bold tracking-tight">CinemaVerse</span>
      </div>

      <!-- Navigation Links -->
      <nav class="flex-1 px-3 py-4 space-y-1">
        <a
          routerLink="/"
          routerLinkActive="bg-sidebar-accent text-sidebar-accent-foreground font-semibold"
          [routerLinkActiveOptions]="{ exact: true }"
          class="flex items-center gap-3 px-3 py-2 rounded-md text-sm text-sidebar-foreground hover:bg-sidebar-accent hover:text-sidebar-accent-foreground transition-colors cursor-pointer"
        >
          <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <circle cx="12" cy="12" r="10"/>
            <polygon points="10 8 16 12 10 16 10 8"/>
          </svg>
          <span>Discover</span>
        </a>

        <a
          routerLink="/dashboard"
          routerLinkActive="bg-sidebar-accent text-sidebar-accent-foreground font-semibold"
          [routerLinkActiveOptions]="{ exact: false }"
          class="flex items-center gap-3 px-3 py-2 rounded-md text-sm text-sidebar-foreground hover:bg-sidebar-accent hover:text-sidebar-accent-foreground transition-colors cursor-pointer"
        >
          <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <path d="M2 9a3 3 0 0 1 0 6v2a2 2 0 0 0 2 2h16a2 2 0 0 0 2-2v-2a3 3 0 0 1 0-6V7a2 2 0 0 0-2-2H4a2 2 0 0 0-2 2Z"/>
            <path d="M13 5v2"/>
            <path d="M13 17v2"/>
            <path d="M13 11v2"/>
          </svg>
          <span>My Tickets</span>
        </a>

        <a
          routerLink="/support"
          routerLinkActive="bg-sidebar-accent text-sidebar-accent-foreground font-semibold"
          [routerLinkActiveOptions]="{ exact: false }"
          class="flex items-center gap-3 px-3 py-2 rounded-md text-sm text-sidebar-foreground hover:bg-sidebar-accent hover:text-sidebar-accent-foreground transition-colors cursor-pointer"
        >
          <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <circle cx="12" cy="12" r="10"/>
            <path d="M9.09 9a3 3 0 0 1 5.83 1c0 2-3 3-3 3"/>
            <path d="M12 17h.01"/>
          </svg>
          <span>Support</span>
        </a>
      </nav>

      <!-- Separator -->
      <div class="shrink-0 bg-sidebar-border h-px w-full"></div>

      <!-- User Section -->
      <div>
        <!-- User Info (if logged in) -->
        <div *ngIf="isAuthenticated()" class="px-3 py-4 space-y-3">
          <div class="flex items-center gap-3">
            <span class="relative flex shrink-0 overflow-hidden rounded-full h-9 w-9 bg-sidebar-primary items-center justify-center">
              <span class="text-xs font-medium text-sidebar-primary-foreground">
                {{ getInitials(currentUser()?.name || 'User') }}
              </span>
            </span>
            <div class="flex-1 min-w-0">
              <p class="text-sm font-medium truncate">{{ currentUser()?.name || 'User' }}</p>
              <p class="text-xs text-muted-foreground truncate">{{ currentUser()?.email || 'user@example.com' }}</p>
            </div>
          </div>
          <button
            (click)="logout()"
            class="inline-flex items-center justify-center gap-2 whitespace-nowrap rounded-md text-sm font-medium transition-colors focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring border border-input bg-background shadow-sm hover:bg-accent hover:text-accent-foreground h-9 px-4 py-2 w-full"
          >
            Logout
          </button>
        </div>

        <!-- Sign In (if not logged in) -->
        <div *ngIf="!isAuthenticated()" class="px-3 py-4">
          <button
            class="inline-flex items-center justify-center gap-2 whitespace-nowrap rounded-md text-sm font-medium transition-colors focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring bg-primary text-primary-foreground shadow hover:bg-primary/90 h-9 px-4 py-2 w-full"
          >
            Sign In
          </button>
        </div>

        <!-- Footer -->
        <div class="px-3 py-3 border-t border-sidebar-border">
          <p class="text-xs text-muted-foreground">
            &copy; 2024 CinemaVerse. All rights reserved.
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
