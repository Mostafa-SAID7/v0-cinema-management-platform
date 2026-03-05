import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';

export type BadgeVariant = 'default' | 'secondary' | 'destructive' | 'outline';

@Component({
  selector: 'app-badge',
  standalone: true,
  imports: [CommonModule],
  template: `
    <span
      [ngClass]="getVariantClasses()"
      class="inline-flex items-center rounded-md border px-2.5 py-0.5 text-xs font-semibold transition-colors focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2"
    >
      <ng-content></ng-content>
    </span>
  `,
})
export class BadgeComponent {
  variant = input<BadgeVariant>('default');

  getVariantClasses(): string {
    switch (this.variant()) {
      case 'secondary':
        return 'border-transparent bg-secondary text-secondary-foreground hover:bg-secondary/80';
      case 'destructive':
        return 'border-transparent bg-destructive text-destructive-foreground shadow hover:bg-destructive/80';
      case 'outline':
        return 'text-foreground';
      case 'default':
      default:
        return 'border-transparent bg-primary text-primary-foreground shadow hover:bg-primary/80';
    }
  }
}
