import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';

export type ButtonVariant = 'default' | 'primary' | 'secondary' | 'outline' | 'ghost' | 'destructive';
export type ButtonSize = 'sm' | 'md' | 'lg';

@Component({
  selector: 'app-button',
  standalone: true,
  imports: [CommonModule],
  template: `
    <button
      [ngClass]="getButtonClasses()"
      [disabled]="disabled()"
      class="inline-flex items-center justify-center font-medium rounded-lg transition-smooth focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary disabled:opacity-50 disabled:cursor-not-allowed"
    >
      <ng-content></ng-content>
    </button>
  `,
})
export class ButtonComponent {
  variant = input<ButtonVariant>('default');
  size = input<ButtonSize>('md');
  disabled = input(false);
  glow = input(false);

  getButtonClasses(): string {
    const baseClasses = this.getSizeClasses();
    const variantClasses = this.getVariantClasses();
    const glowClass = this.glow() ? 'neon-glow' : '';

    return `${baseClasses} ${variantClasses} ${glowClass}`.trim();
  }

  private getSizeClasses(): string {
    switch (this.size()) {
      case 'sm':
        return 'px-3 py-1.5 text-sm';
      case 'lg':
        return 'px-8 py-3 text-lg';
      case 'md':
      default:
        return 'px-6 py-2 text-base';
    }
  }

  private getVariantClasses(): string {
    switch (this.variant()) {
      case 'primary':
        return 'bg-primary text-primary-foreground hover:bg-primary/90';
      case 'secondary':
        return 'bg-secondary text-foreground hover:bg-secondary/80';
      case 'outline':
        return 'border border-border text-foreground hover:bg-secondary';
      case 'destructive':
        return 'bg-destructive text-destructive-foreground hover:bg-destructive/90';
      case 'ghost':
        return 'text-foreground hover:bg-secondary';
      case 'default':
      default:
        return 'bg-secondary text-foreground hover:bg-secondary/80';
    }
  }
}
