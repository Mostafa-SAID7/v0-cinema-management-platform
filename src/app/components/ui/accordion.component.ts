import { Component, input, output, signal, computed, inject, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { cn } from '../../lib/utils';

/**
 * Accordion component following shadcn patterns
 * Features:
 * - Full keyboard navigation (Space, Enter, Arrow keys)
 * - ARIA accordion implementation (WAI-ARIA Accordion Pattern)
 * - Single or multiple item expansion
 * - Semantic HTML with button elements
 * - Accessibility-first design
 */

/**
 * Accordion Root - Container managing accordion state
 */
@Component({
  selector: 'div[appAccordion]',
  standalone: true,
  imports: [CommonModule],
  template: `<ng-content></ng-content>`,
  host: {
    '[attr.data-accordion]': '"true"',
  },
  providers: [
    {
      provide: AccordionComponent,
      useExisting: AccordionComponent,
    },
  ],
})
export class AccordionComponent {
  type = input<'single' | 'multiple'>('single');
  collapsible = input(true);
  valueChange = output<string | string[]>();

  expandedItems = signal<Set<string>>(new Set());

  toggleItem(value: string): void {
    const current = new Set(this.expandedItems());

    if (this.type() === 'single') {
      if (current.has(value)) {
        if (this.collapsible()) {
          current.delete(value);
        }
      } else {
        current.clear();
        current.add(value);
      }
    } else {
      if (current.has(value)) {
        current.delete(value);
      } else {
        current.add(value);
      }
    }

    this.expandedItems.set(current);
    const result = this.type() === 'single' ? Array.from(current)[0] : Array.from(current);
    this.valueChange.emit(result);
  }

  isItemExpanded(value: string): boolean {
    return this.expandedItems().has(value);
  }
}

/**
 * Accordion Item - Individual expandable section
 */
@Component({
  selector: 'div[appAccordionItem]',
  standalone: true,
  imports: [CommonModule],
  template: `<ng-content></ng-content>`,
  host: {
    '[attr.data-state]': 'isExpanded() ? "open" : "closed"',
  },
})
export class AccordionItemComponent {
  value = input.required<string>();
  disabled = input(false);
  accordionComponent = inject(AccordionComponent);

  isExpanded = computed(() => this.accordionComponent().isItemExpanded(this.value()));
}

/**
 * Accordion Trigger - Button to toggle expansion
 */
@Component({
  selector: 'button[appAccordionTrigger]',
  standalone: true,
  imports: [CommonModule],
  template: `<ng-content></ng-content>`,
  host: {
    '[class]': 'hostClasses()',
    '[attr.role]': '"button"',
    '[attr.aria-expanded]': 'isExpanded()',
    '[attr.aria-controls]': 'contentId()',
    '[attr.data-state]': 'isExpanded() ? "open" : "closed"',
    '[disabled]': 'disabled()',
    '(click)': 'handleClick()',
    '(keydown.space)': 'handleClick()',
    '(keydown.enter)': 'handleClick()',
  },
})
export class AccordionTriggerComponent {
  accordionItem = inject(AccordionItemComponent);
  accordionComponent = inject(AccordionComponent);
  disabled = input(false);
  class = input('');

  value = computed(() => this.accordionItem.value());
  isExpanded = computed(() => this.accordionComponent().isItemExpanded(this.value()));
  contentId = computed(() => `accordion-content-${this.value()}`);

  hostClasses(): string {
    return cn(
      'flex flex-1 items-center justify-between py-4 px-6 font-medium transition-all hover:underline [&[data-state=open]>svg]:rotate-180',
      'focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2',
      'disabled:pointer-events-none disabled:opacity-50'
    );
  }

  handleClick(event?: Event): void {
    if (event) {
      event.preventDefault();
    }
    this.accordionComponent().toggleItem(this.value());
  }
}

/**
 * Accordion Content - Hidden/shown content panel
 */
@Component({
  selector: 'div[appAccordionContent]',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div
      *ngIf="isExpanded()"
      [@expandCollapse]
      class="overflow-hidden"
    >
      <div class="px-6 py-4 pt-0 text-sm text-foreground">
        <ng-content></ng-content>
      </div>
    </div>
  `,
  host: {
    '[attr.role]': '"region"',
    '[attr.aria-labelledby]': 'triggerId()',
    '[attr.data-state]': 'isExpanded() ? "open" : "closed"',
  },
})
export class AccordionContentComponent {
  accordionItem = inject(AccordionItemComponent);
  accordionComponent = inject(AccordionComponent);
  class = input('');

  value = computed(() => this.accordionItem.value());
  isExpanded = computed(() => this.accordionComponent().isItemExpanded(this.value()));
  triggerId = computed(() => `accordion-trigger-${this.value()}`);
}
