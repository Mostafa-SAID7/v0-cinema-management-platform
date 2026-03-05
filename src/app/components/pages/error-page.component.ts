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
        <!-- Error Code -->
        <div class="text-8xl font-bold text-primary">{{ statusCode() }}</div>

        <!-- Error Title -->
        <div class="space-y-2">
          <h1 class="text-4xl font-bold text-foreground text-balance">{{ title() }}</h1>
          <p class="text-lg text-muted-foreground">{{ message() }}</p>
        </div>

        <!-- Error Details -->
        <div *ngIf="details()" class="rounded-xl border bg-card text-card-foreground shadow p-4 text-left space-y-2">
          <p class="text-sm font-mono text-destructive break-words">{{ details() }}</p>
        </div>

        <!-- Action Buttons -->
        <div class="flex flex-col sm:flex-row gap-3 justify-center">
          <a
            routerLink="/"
            class="inline-flex items-center justify-center gap-2 whitespace-nowrap rounded-md text-sm font-medium transition-colors focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring bg-primary text-primary-foreground shadow hover:bg-primary/90 h-10 px-8"
          >
            Back to Home
          </a>
          <button
            (click)="goBack()"
            class="inline-flex items-center justify-center gap-2 whitespace-nowrap rounded-md text-sm font-medium transition-colors focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring border border-input bg-background shadow-sm hover:bg-accent hover:text-accent-foreground h-10 px-8"
          >
            Go Back
          </button>
        </div>

        <!-- Separator -->
        <div class="shrink-0 bg-border h-px w-full"></div>

        <!-- Support Info -->
        <div class="text-xs text-muted-foreground">
          <p>Need help? Contact support at <span class="text-primary font-medium">support&#64;cinemaverse.com</span></p>
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
