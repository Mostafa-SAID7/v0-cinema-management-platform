import { Component, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-empty-state',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="glassmorphism rounded-lg p-12 text-center space-y-4 my-8">
      <div class="text-6xl">{{ icon() }}</div>
      <div class="space-y-2">
        <h3 class="text-xl font-bold text-foreground">{{ title() }}</h3>
        <p class="text-muted-foreground">{{ message() }}</p>
      </div>
      <button
        *ngIf="actionLabel()"
        (click)="onAction()"
        class="inline-block px-6 py-2 rounded-lg bg-primary text-primary-foreground hover:bg-primary/90 transition-smooth font-medium"
      >
        {{ actionLabel() }}
      </button>
    </div>
  `,
})
export class EmptyStateComponent {
  icon = input('📭');
  title = input('Nothing here');
  message = input('No items to display');
  actionLabel = input('');
  onAction = output<void>();
}
