import { Component, input, output, signal } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-accordion',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="space-y-2">
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
    <div class="glassmorphism rounded-lg overflow-hidden">
      <button
        (click)="toggleOpen()"
        class="w-full px-6 py-4 flex items-center justify-between text-left hover:bg-secondary/50 transition-smooth"
      >
        <span class="font-medium text-foreground">{{ title() }}</span>
        <span
          class="text-muted-foreground transform transition-transform"
          [class.rotate-180]="open()"
        >
          ▼
        </span>
      </button>

      <div *ngIf="open()" class="border-t border-border px-6 py-4 bg-secondary/20 animate-slide-in-down">
        <ng-content></ng-content>
      </div>
    </div>
  `,
  styles: [
    `
      @keyframes slide-in-down {
        from {
          opacity: 0;
          transform: translateY(-10px);
        }
        to {
          opacity: 1;
          transform: translateY(0);
        }
      }

      .animate-slide-in-down {
        animation: slide-in-down 0.3s ease-out;
      }
    `,
  ],
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
    <p class="text-foreground leading-relaxed">
      <ng-content></ng-content>
    </p>
  `,
})
export class AccordionContentComponent {}
