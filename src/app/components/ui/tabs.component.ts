import { Component, input, output, signal, computed, effect, inject, Optional } from '@angular/core';
import { CommonModule } from '@angular/common';
import { cn } from '../../lib/utils';

/**
 * Tabs component following shadcn patterns
 * Features:
 * - Full keyboard navigation (Arrow keys, Home, End)
 * - ARIA tab list implementation (WAI-ARIA Tabs Pattern)
 * - Automatic tab management
 * - Compound component structure
 * - Accessibility-first design with screen reader support
 */

/**
 * Tabs Root - Main container managing tab state
 */
@Component({
  selector: 'div[appTabs]',
  standalone: true,
  imports: [CommonModule],
  template: `<ng-content></ng-content>`,
  host: {
    '[attr.data-tabs]': '"true"',
  },
  providers: [
    {
      provide: TabsComponent,
      useExisting: TabsComponent,
    },
  ],
})
export class TabsComponent {
  value = input<string>('');
  valueChange = output<string>();
  defaultValue = input('');
  orientation = input<'horizontal' | 'vertical'>('horizontal');

  selectedValue = signal('');
  triggers = new Map<string, TabsTriggerComponent>();

  constructor() {
    effect(() => {
      if (this.value()) {
        this.selectedValue.set(this.value());
      } else if (this.defaultValue()) {
        this.selectedValue.set(this.defaultValue());
      }
    });
  }

  selectTab(tabValue: string): void {
    this.selectedValue.set(tabValue);
    this.valueChange.emit(tabValue);
  }

  getSelectedValue(): string {
    return this.selectedValue();
  }

  registerTrigger(trigger: TabsTriggerComponent): void {
    this.triggers.set(trigger.value(), trigger);
  }

  unregisterTrigger(value: string): void {
    this.triggers.delete(value);
  }

  getNextTrigger(current: string): TabsTriggerComponent | undefined {
    const entries = Array.from(this.triggers.entries());
    const index = entries.findIndex(([key]) => key === current);
    if (index === -1) return undefined;
    return entries[(index + 1) % entries.length][1];
  }

  getPreviousTrigger(current: string): TabsTriggerComponent | undefined {
    const entries = Array.from(this.triggers.entries());
    const index = entries.findIndex(([key]) => key === current);
    if (index === -1) return undefined;
    return entries[(index - 1 + entries.length) % entries.length][1];
  }

  getFirstTrigger(): TabsTriggerComponent | undefined {
    return this.triggers.values().next().value;
  }

  getLastTrigger(): TabsTriggerComponent | undefined {
    const entries = Array.from(this.triggers.values());
    return entries[entries.length - 1];
  }
}

/**
 * Tabs List - Container for tab triggers
 */
@Component({
  selector: 'div[appTabsList]',
  standalone: true,
  imports: [CommonModule],
  template: `<ng-content></ng-content>`,
  host: {
    '[class]': 'hostClasses()',
    '[attr.role]': '"tablist"',
    '[attr.aria-orientation]': 'tabsComponent().orientation()',
  },
})
export class TabsListComponent {
  class = input('');
  tabsComponent = inject(TabsComponent);

  hostClasses(): string {
    return cn(
      'inline-flex h-10 items-center justify-center rounded-lg bg-muted p-1',
      this.class()
    );
  }
}

/**
 * Tabs Trigger - Individual tab button with keyboard support
 */
@Component({
  selector: 'button[appTabsTrigger]',
  standalone: true,
  imports: [CommonModule],
  template: `<ng-content></ng-content>`,
  host: {
    '[class]': 'hostClasses()',
    '[attr.role]': '"tab"',
    '[attr.aria-selected]': 'isSelected()',
    '[attr.aria-controls]': 'contentId()',
    '[attr.tabindex]': 'isSelected() ? 0 : -1',
    '[attr.data-state]': 'isSelected() ? "active" : "inactive"',
    '(click)': 'handleClick()',
    '(keydown.arrowRight)': 'handleArrowRight($event)',
    '(keydown.arrowLeft)': 'handleArrowLeft($event)',
    '(keydown.home)': 'handleHome($event)',
    '(keydown.end)': 'handleEnd($event)',
  },
})
export class TabsTriggerComponent {
  value = input.required<string>();
  disabled = input(false);
  class = input('');
  tabsComponent = inject(TabsComponent);

  isSelected = computed(() => this.tabsComponent().getSelectedValue() === this.value());
  contentId = computed(() => `tabs-content-${this.value()}`);

  constructor() {
    effect(() => {
      this.tabsComponent().registerTrigger(this);
      return () => {
        this.tabsComponent().unregisterTrigger(this.value());
      };
    });
  }

  hostClasses(): string {
    return cn(
      'inline-flex items-center justify-center whitespace-nowrap rounded-md px-3 py-1.5 text-sm font-medium transition-all focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2 disabled:pointer-events-none disabled:opacity-50',
      this.isSelected()
        ? 'bg-background text-foreground shadow-sm'
        : 'text-muted-foreground hover:text-foreground'
    );
  }

  handleClick(): void {
    if (!this.disabled()) {
      this.tabsComponent().selectTab(this.value());
    }
  }

  handleArrowRight(event: KeyboardEvent): void {
    event.preventDefault();
    const next = this.tabsComponent().getNextTrigger(this.value());
    if (next) {
      next.focusAndSelect();
    }
  }

  handleArrowLeft(event: KeyboardEvent): void {
    event.preventDefault();
    const prev = this.tabsComponent().getPreviousTrigger(this.value());
    if (prev) {
      prev.focusAndSelect();
    }
  }

  handleHome(event: KeyboardEvent): void {
    event.preventDefault();
    const first = this.tabsComponent().getFirstTrigger();
    if (first) {
      first.focusAndSelect();
    }
  }

  handleEnd(event: KeyboardEvent): void {
    event.preventDefault();
    const last = this.tabsComponent().getLastTrigger();
    if (last) {
      last.focusAndSelect();
    }
  }

  focusAndSelect(): void {
    this.tabsComponent().selectTab(this.value());
  }
}

/**
 * Tabs Content - Content panel for each tab
 */
@Component({
  selector: 'div[appTabsContent]',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div
      *ngIf="isVisible()"
      [@fadeIn]
      [class]="hostClasses()"
      [attr.role]="'tabpanel'"
      [attr.aria-labelledby]="'tabs-trigger-' + value()"
      [attr.data-state]="isVisible() ? 'active' : 'inactive'"
    >
      <ng-content></ng-content>
    </div>
  `,
  host: {
    '[style.display]': 'isVisible() ? "block" : "none"',
  },
})
export class TabsContentComponent {
  value = input.required<string>();
  class = input('');
  tabsComponent = inject(TabsComponent);

  isVisible = computed(() => this.tabsComponent().getSelectedValue() === this.value());

  hostClasses(): string {
    return cn(
      'mt-2 ring-offset-background focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2',
      this.class()
    );
  }
}
