import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';

export type ButtonVariant = 'default' | 'destructive' | 'outline' | 'secondary' | 'ghost' | 'link';
export type ButtonSize = 'default' | 'sm' | 'lg' | 'icon';

@Component({
  selector: 'app-button',
  standalone: true,
  imports: [CommonModule],
  template: `
    <button
      [ngClass]="getButtonClasses()"
      [disabled]="disabled()"
      [type]="type()"
    >
      <ng-content></ng-content>
    </button>
  `,
})
export class ButtonComponent {
  variant = input<ButtonVariant>('default');
  size = input<ButtonSize>('default');
  disabled = input(false);
  type = input<'button' | 'submit' | 'reset'>('button');

  getButtonClasses(): string {
    const base =
      'inline-flex items-center justify-center gap-2 whitespace-nowrap rounded-md text-sm font-medium transition-colors focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring disabled:pointer-events-none disabled:opacity-50';
    return `${base} ${this.getVariantClasses()} ${this.getSizeClasses()}`;
  }

  private getSizeClasses(): string {
    switch (this.size()) {
      case 'sm':
        return 'h-8 rounded-md px-3 text-xs';
      case 'lg':
        return 'h-10 rounded-md px-8';
      case 'icon':
        return 'h-9 w-9';
      case 'default':
      default:
        return 'h-9 px-4 py-2';
    }
  }

  private getVariantClasses(): string {
    switch (this.variant()) {
      case 'destructive':
        return 'bg-destructive text-destructive-foreground shadow-sm hover:bg-destructive/90';
      case 'outline':
        return 'border border-input bg-background shadow-sm hover:bg-accent hover:text-accent-foreground';
      case 'secondary':
        return 'bg-secondary text-secondary-foreground shadow-sm hover:bg-secondary/80';
      case 'ghost':
        return 'hover:bg-accent hover:text-accent-foreground';
      case 'link':
        return 'text-primary underline-offset-4 hover:underline';
      case 'default':
      default:
        return 'bg-primary text-primary-foreground shadow hover:bg-primary/90';
    }
  }
}
