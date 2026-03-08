import { Component, input, output, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { cva, cn } from '../../lib/utils';

export type ButtonVariant = 'default' | 'primary' | 'secondary' | 'outline' | 'ghost' | 'destructive' | 'link';
export type ButtonSize = 'sm' | 'md' | 'lg';

/**
 * Button component following shadcn patterns
 * Features:
 * - Compound variant system using CVA
 * - Full accessibility support (ARIA attributes, keyboard navigation)
 * - Type-safe variant management
 * - Composition-first design with ng-content
 * - Semantic HTML
 */
const buttonVariants = cva(
  'inline-flex items-center justify-center font-medium rounded-lg transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary disabled:opacity-50 disabled:cursor-not-allowed disabled:pointer-events-none',
  {
    variants: {
      variant: {
        default: 'bg-secondary text-foreground hover:bg-secondary/80 active:bg-secondary/70',
        primary: 'bg-primary text-primary-foreground hover:bg-primary/90 active:bg-primary/80',
        secondary: 'bg-secondary text-secondary-foreground hover:bg-secondary/80 active:bg-secondary/70',
        destructive:
          'bg-destructive text-destructive-foreground hover:bg-destructive/90 active:bg-destructive/80',
        outline:
          'border border-border text-foreground hover:bg-secondary/50 active:bg-secondary/70 bg-background',
        ghost: 'text-foreground hover:bg-secondary/50 active:bg-secondary/70',
        link: 'text-primary underline-offset-4 hover:underline active:text-primary/80',
      },
      size: {
        sm: 'h-8 px-3 py-1 text-xs',
        md: 'h-10 px-4 py-2 text-sm',
        lg: 'h-12 px-8 py-3 text-base',
      },
    },
    defaultVariants: {
      variant: 'default',
      size: 'md',
    },
  }
);

@Component({
  selector: 'button[appButton]',
  standalone: true,
  imports: [CommonModule],
  template: `
    <span class="inline-flex items-center justify-center gap-2">
      <ng-content></ng-content>
    </span>
  `,
  host: {
    '[class]': 'computedClasses()',
    '[disabled]': 'disabled()',
    '[attr.type]': 'type()',
    '[attr.aria-disabled]': 'disabled()',
    '[attr.aria-busy]': 'loading()',
    '(click)': 'handleClick()',
  },
})
export class ButtonComponent {
  // Input signals for variant configuration
  variant = input<ButtonVariant>('default');
  size = input<ButtonSize>('md');
  disabled = input(false);
  loading = input(false);
  type = input<'button' | 'submit' | 'reset'>('button');
  class = input('');

  // Output events
  clicked = output<PointerEvent>();

  // Computed class based on variants
  computedClasses = computed(() => {
    return cn(
      buttonVariants({
        variant: this.variant(),
        size: this.size(),
      }),
      this.loading() && 'opacity-70 pointer-events-none',
      this.class()
    );
  });

  /**
   * Handle button click with keyboard support
   */
  handleClick(): void {
    if (!this.disabled() && !this.loading()) {
      this.clicked.emit(new PointerEvent('click'));
    }
  }
}
