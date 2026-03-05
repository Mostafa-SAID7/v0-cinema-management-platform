import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';

export type BadgeVariant = 'default' | 'primary' | 'secondary' | 'outline' | 'destructive';

@Component({
  selector: 'app-badge',
  standalone: true,
  imports: [CommonModule],
  template: `
    <span
      [ngClass]="getVariantClasses()"
      class="inline-flex items-center rounded-full px-3 py-1 text-xs font-semibold transition-smooth"
    >
      <ng-content></ng-content>
    </span>
  `,
})
export class BadgeComponent {
  variant = input<BadgeVariant>('default');

  getVariantClasses(): string {
    switch (this.variant()) {
      case 'primary':
        return 'bg-primary text-primary-foreground';
      case 'secondary':
        return 'bg-secondary text-secondary-foreground';
      case 'outline':
        return 'border border-border text-foreground bg-background';
      case 'destructive':
        return 'bg-destructive text-destructive-foreground';
      case 'default':
      default:
        return 'bg-secondary text-secondary-foreground';
    }
  }
}
