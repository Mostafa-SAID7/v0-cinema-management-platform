import { Component, input, signal } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-tabs',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div>
      <div class="inline-flex h-9 items-center justify-center rounded-lg bg-muted p-1 text-muted-foreground">
        <ng-content select="app-tabs-trigger"></ng-content>
      </div>
      <ng-content select="app-tabs-content"></ng-content>
    </div>
  `,
})
export class TabsComponent {
  activeTab = signal<string>('');
}

@Component({
  selector: 'app-tabs-trigger',
  standalone: true,
  imports: [CommonModule],
  template: `
    <button
      (click)="setActive()"
      [ngClass]="{
        'bg-background text-foreground shadow': isActive(),
        'text-muted-foreground': !isActive()
      }"
      class="inline-flex items-center justify-center whitespace-nowrap rounded-md px-3 py-1 text-sm font-medium ring-offset-background transition-all focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50"
    >
      {{ label() }}
    </button>
  `,
})
export class TabsTriggerComponent {
  label = input<string>('');
  value = input<string>('');
  parent: TabsComponent | null = null;

  constructor(parent: TabsComponent) {
    this.parent = parent;
  }

  isActive(): boolean {
    return this.parent?.activeTab() === this.value();
  }

  setActive(): void {
    if (this.parent) {
      this.parent.activeTab.set(this.value());
    }
  }
}

@Component({
  selector: 'app-tabs-content',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div
      *ngIf="isActive()"
      class="mt-2 ring-offset-background focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2"
    >
      <ng-content></ng-content>
    </div>
  `,
})
export class TabsContentComponent {
  value = input<string>('');
  parent: TabsComponent | null = null;

  constructor(parent: TabsComponent) {
    this.parent = parent;
  }

  isActive(): boolean {
    return this.parent?.activeTab() === this.value();
  }
}
