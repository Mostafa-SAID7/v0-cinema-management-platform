import { Component, input, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { cva, cn } from '../../lib/utils';

export type BadgeVariant = 'default' | 'primary' | 'secondary' | 'destructive' | 'outline' | 'success' | 'warning';

/**
 * Badge component following shadcn patterns
 * Features:
 * - Compact indicator for status, category, or metadata
 * - Multiple visual variants with semantic colors
 * - Semantic HTML with role="status"
 * - Accessibility-first design
 * - CVA-based variant system
 */
const badgeVariants = cva(
  'inline-flex items-center rounded-full px-2.5 py-0.5 text-xs font-semibold transition-colors focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2',
  {
    variants: {
      variant: {
        default: 'border border-transparent bg-primary text-primary-foreground hover:bg-primary/80',
        primary: 'border border-transparent bg-primary text-primary-foreground hover:bg-primary/80',
        secondary: 'border border-transparent bg-secondary text-secondary-foreground hover:bg-secondary/80',
        destructive:
          'border border-transparent bg-destructive text-destructive-foreground hover:bg-destructive/80',
        outline: 'text-foreground border border-border bg-background',
        success: 'border border-transparent bg-green-500 text-white hover:bg-green-600',
        warning: 'border border-transparent bg-yellow-500 text-white hover:bg-yellow-600',
      },
    },
    defaultVariants: {
      variant: 'default',
    },
  }
);

@Component({
  selector: 'span[appBadge]',
  standalone: true,
  imports: [CommonModule],
  template: `<ng-content></ng-content>`,
  host: {
    '[class]': 'computedClasses()',
    '[attr.role]': '"status"',
  },
})
export class BadgeComponent {
  variant = input<BadgeVariant>('default');
  class = input('');

  computedClasses = computed(() => {
    return cn(badgeVariants({ variant: this.variant() }), this.class());
  });
}
