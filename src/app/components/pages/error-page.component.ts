import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-error-page',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <div class="h-screen w-full flex items-center justify-center bg-background">
      <div class="max-w-md mx-auto px-4 text-center space-y-6">
        <!-- Error Icon -->
        <div class="text-8xl font-bold text-primary neon-text">{{ statusCode() }}</div>

        <!-- Error Title -->
        <div class="space-y-2">
          <h1 class="text-4xl font-bold text-foreground">{{ title() }}</h1>
          <p class="text-lg text-muted-foreground">{{ message() }}</p>
        </div>

        <!-- Error Details -->
        <div *ngIf="details()" class="glassmorphism rounded-lg p-4 text-left space-y-2">
          <p class="text-sm font-mono text-red-400 break-words">{{ details() }}</p>
        </div>

        <!-- Action Buttons -->
        <div class="flex flex-col sm:flex-row gap-3 justify-center">
          <a
            routerLink="/"
            class="px-6 py-3 rounded-lg bg-primary text-primary-foreground font-medium hover:bg-primary/90 transition-smooth"
          >
            Back to Home
          </a>
          <button
            (click)="goBack()"
            class="px-6 py-3 rounded-lg border border-border text-foreground hover:bg-secondary transition-smooth font-medium"
          >
            Go Back
          </button>
        </div>

        <!-- Support Info -->
        <div class="text-xs text-muted-foreground pt-4 border-t border-border">
          <p>Need help? Contact support at <span class="text-primary font-medium">support@cinemaverse.com</span></p>
        </div>
      </div>
    </div>
  `,
})
export class ErrorPageComponent {
  statusCode = input(500);
  title = input('Something went wrong');
  message = input('An unexpected error occurred. Please try again later.');
  details = input<string | null>(null);

  goBack(): void {
    window.history.back();
  }
}
