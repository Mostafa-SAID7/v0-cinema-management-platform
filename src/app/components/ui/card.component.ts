import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { cn } from '../../lib/utils';

/**
 * Card - Base container component following shadcn patterns
 * Features:
 * - Compound component structure with header, title, content, footer slots
 * - Semantic HTML with proper hierarchy
 * - Flexible styling via CSS variables
 * - Support for disabled/interactive states
 */
@Component({
  selector: 'div[appCard]',
  standalone: true,
  imports: [CommonModule],
  template: `<ng-content></ng-content>`,
  host: {
    '[class]': 'hostClasses()',
    '[attr.data-card]': '"true"',
  },
})
export class CardComponent {
  disabled = input(false);
  class = input('');

  hostClasses(): string {
    return cn(
      'rounded-lg border border-border bg-card text-card-foreground shadow-md transition-colors',
      this.disabled() && 'opacity-50 pointer-events-none',
      this.class()
    );
  }
}

/**
 * Card Header - Top section for title and description
 */
@Component({
  selector: 'div[appCardHeader]',
  standalone: true,
  imports: [CommonModule],
  template: `<ng-content></ng-content>`,
  host: {
    '[class]': '"flex flex-col space-y-1.5 p-6"',
  },
})
export class CardHeaderComponent {}

/**
 * Card Title - Semantic heading with consistent styling
 */
@Component({
  selector: 'h2[appCardTitle], h3[appCardTitle], h4[appCardTitle]',
  standalone: true,
  imports: [CommonModule],
  template: `<ng-content></ng-content>`,
  host: {
    '[class]': '"text-2xl font-semibold leading-none tracking-tight text-foreground"',
  },
})
export class CardTitleComponent {}

/**
 * Card Description - Secondary text for additional context
 */
@Component({
  selector: 'p[appCardDescription]',
  standalone: true,
  imports: [CommonModule],
  template: `<ng-content></ng-content>`,
  host: {
    '[class]': '"text-sm text-muted-foreground"',
  },
})
export class CardDescriptionComponent {}

/**
 * Card Content - Main content area with padding
 */
@Component({
  selector: 'div[appCardContent]',
  standalone: true,
  imports: [CommonModule],
  template: `<ng-content></ng-content>`,
  host: {
    '[class]': '"p-6 pt-0"',
  },
})
export class CardContentComponent {}

/**
 * Card Footer - Bottom section for actions and metadata
 */
@Component({
  selector: 'div[appCardFooter]',
  standalone: true,
  imports: [CommonModule],
  template: `<ng-content></ng-content>`,
  host: {
    '[class]': '"flex items-center p-6 pt-0"',
  },
})
export class CardFooterComponent {}
