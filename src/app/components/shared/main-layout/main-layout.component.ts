import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { NavbarComponent } from '../navbar/navbar.component';
import { ToastContainerComponent } from '../../ui/toast-container.component';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [CommonModule, RouterOutlet, SidebarComponent, NavbarComponent, ToastContainerComponent],
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

      <!-- Mobile Drawer Overlay -->
      <div
        *ngIf="sidebarOpen()"
        class="fixed inset-0 z-50 md:hidden"
        (click)="closeSidebar()"
      >
        <!-- Backdrop (shadcn Sheet pattern) -->
        <div class="fixed inset-0 z-50 bg-black/80"></div>

        <!-- Drawer Panel -->
        <aside
          class="fixed inset-y-0 left-0 z-50 w-64 border-r bg-background animate-slide-in-left overflow-y-auto"
          (click)="$event.stopPropagation()"
        >
          <app-sidebar></app-sidebar>
        </aside>
      </div>

      <!-- Toast Container -->
      <app-toast-container></app-toast-container>
    </div>
  `,
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
