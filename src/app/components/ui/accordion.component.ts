import { Component, input, signal } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-accordion',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div>
      <ng-content></ng-content>
    </div>
  `,
})
export class AccordionComponent {
  collapsible = input(true);
}

@Component({
  selector: 'app-accordion-item',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="border-b">
      <h3 class="flex">
        <button
          (click)="toggleOpen()"
          class="flex flex-1 items-center justify-between py-4 text-sm font-medium transition-all hover:underline"
          [attr.aria-expanded]="open()"
        >
          <span>{{ title() }}</span>
          <svg
            xmlns="http://www.w3.org/2000/svg"
            width="16"
            height="16"
            viewBox="0 0 24 24"
            fill="none"
            stroke="currentColor"
            stroke-width="2"
            stroke-linecap="round"
            stroke-linejoin="round"
            class="shrink-0 text-muted-foreground transition-transform duration-200"
            [class.rotate-180]="open()"
          >
            <path d="m6 9 6 6 6-6"/>
          </svg>
        </button>
      </h3>

      <div
        *ngIf="open()"
        class="overflow-hidden text-sm animate-accordion-down"
      >
        <div class="pb-4 pt-0">
          <ng-content></ng-content>
        </div>
      </div>
    </div>
  `,
})
export class AccordionItemComponent {
  title = input<string>('');
  open = signal(false);

  toggleOpen(): void {
    this.open.update((val) => !val);
  }
}

@Component({
  selector: 'app-accordion-content',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="text-sm text-muted-foreground leading-relaxed">
      <ng-content></ng-content>
    </div>
  `,
})
export class AccordionContentComponent {}
