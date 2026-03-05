import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { NavbarComponent } from '../navbar/navbar.component';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [CommonModule, RouterOutlet, SidebarComponent, NavbarComponent],
  template: `
    <div class="flex h-screen bg-background">
      <!-- Desktop Sidebar -->
      <aside class="hidden md:flex md:w-64 md:flex-col border-r border-border">
        <app-sidebar></app-sidebar>
      </aside>

      <!-- Main Content -->
      <div class="flex flex-col flex-1 overflow-hidden">
        <!-- Top Navbar -->
        <app-navbar [sidebarOpen]="sidebarOpen()" (sidebarOpenChange)="toggleSidebar()"></app-navbar>

        <!-- Content Area -->
        <main class="flex-1 overflow-auto">
          <router-outlet></router-outlet>
        </main>
      </div>

      <!-- Mobile Drawer -->
      <div 
        *ngIf="sidebarOpen()" 
        class="fixed inset-0 z-40 md:hidden"
        (click)="closeSidebar()"
      >
        <!-- Backdrop -->
        <div class="absolute inset-0 bg-black/50 animate-fade-in"></div>

        <!-- Drawer -->
        <aside class="absolute inset-y-0 left-0 w-64 bg-card border-r border-border animate-slide-in-left overflow-y-auto">
          <app-sidebar></app-sidebar>
        </aside>
      </div>
    </div>
  `,
  styles: [
    `
      @keyframes fade-in {
        from {
          opacity: 0;
        }
        to {
          opacity: 1;
        }
      }

      .animate-fade-in {
        animation: fade-in 0.3s ease-out;
      }
    `,
  ],
})
export class MainLayoutComponent {
  sidebarOpen = signal(false);

  toggleSidebar(): void {
    this.sidebarOpen.update((open) => !open);
  }

  closeSidebar(): void {
    this.sidebarOpen.set(false);
  }
}
