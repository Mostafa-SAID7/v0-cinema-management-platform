import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-card',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="bg-card text-card-foreground rounded-lg border border-border overflow-hidden">
      <ng-content></ng-content>
    </div>
  `,
})
export class CardComponent {}

@Component({
  selector: 'app-card-header',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="px-6 py-4 border-b border-border">
      <ng-content></ng-content>
    </div>
  `,
})
export class CardHeaderComponent {}

@Component({
  selector: 'app-card-title',
  standalone: true,
  imports: [CommonModule],
  template: ` <h2 class="text-2xl font-bold tracking-tight"><ng-content></ng-content></h2> `,
})
export class CardTitleComponent {}

@Component({
  selector: 'app-card-description',
  standalone: true,
  imports: [CommonModule],
  template: `
    <p class="text-sm text-muted-foreground mt-1">
      <ng-content></ng-content>
    </p>
  `,
})
export class CardDescriptionComponent {}

@Component({
  selector: 'app-card-content',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="px-6 py-4">
      <ng-content></ng-content>
    </div>
  `,
})
export class CardContentComponent {}

@Component({
  selector: 'app-card-footer',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="px-6 py-4 border-t border-border flex gap-3">
      <ng-content></ng-content>
    </div>
  `,
})
export class CardFooterComponent {}
