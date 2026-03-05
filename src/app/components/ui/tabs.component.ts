import { Component, input, signal, ContentChildren, QueryList, AfterContentInit } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-tabs',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="space-y-4">
      <div class="flex gap-4 border-b border-border">
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
      [class.text-primary]="isActive()"
      [class.border-b-2]="isActive()"
      [class.border-primary]="isActive()"
      [class.text-muted-foreground]="!isActive()"
      class="px-4 py-2 font-medium transition-smooth"
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
    <div *ngIf="isActive()" class="animate-scale-in">
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
