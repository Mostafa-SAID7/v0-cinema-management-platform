import { Component, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-empty-state',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="rounded-xl border bg-card text-card-foreground shadow p-12 text-center space-y-4 my-8">
      <div class="text-6xl" aria-hidden="true">{{ icon() }}</div>
      <div class="space-y-2">
        <h3 class="text-xl font-semibold leading-none tracking-tight">{{ title() }}</h3>
        <p class="text-sm text-muted-foreground">{{ message() }}</p>
      </div>
      <button
        *ngIf="actionLabel()"
        (click)="onAction.emit()"
        class="inline-flex items-center justify-center gap-2 whitespace-nowrap rounded-md text-sm font-medium transition-colors focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring bg-primary text-primary-foreground shadow hover:bg-primary/90 h-9 px-4 py-2"
      >
        {{ actionLabel() }}
      </button>
    </div>
  `,
})
export class EmptyStateComponent {
  icon = input('');
  title = input('Nothing here');
  message = input('No items to display');
  actionLabel = input('');
  onAction = output<void>();
}
