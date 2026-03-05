import { Component, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dialog',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div *ngIf="open()" class="fixed inset-0 z-50 flex items-center justify-center">
      <!-- Backdrop -->
      <div
        class="absolute inset-0 bg-black/50"
        (click)="openChange.emit(false)"
        [@fadeIn]
      ></div>

      <!-- Dialog Content -->
      <div
        class="relative bg-card rounded-lg shadow-lg max-w-md w-full mx-4 animate-scale-in"
        (click)="$event.stopPropagation()"
      >
        <ng-content></ng-content>
      </div>
    </div>
  `,
})
export class DialogComponent {
  open = input(false);
  openChange = output<boolean>();
}

@Component({
  selector: 'app-dialog-header',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="px-6 py-4 border-b border-border">
      <ng-content></ng-content>
    </div>
  `,
})
export class DialogHeaderComponent {}

@Component({
  selector: 'app-dialog-title',
  standalone: true,
  imports: [CommonModule],
  template: ` <h2 class="text-xl font-bold text-foreground"><ng-content></ng-content></h2> `,
})
export class DialogTitleComponent {}

@Component({
  selector: 'app-dialog-description',
  standalone: true,
  imports: [CommonModule],
  template: `
    <p class="text-sm text-muted-foreground">
      <ng-content></ng-content>
    </p>
  `,
})
export class DialogDescriptionComponent {}

@Component({
  selector: 'app-dialog-content',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="px-6 py-4">
      <ng-content></ng-content>
    </div>
  `,
})
export class DialogContentComponent {}

@Component({
  selector: 'app-dialog-footer',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="px-6 py-4 border-t border-border flex gap-3 justify-end">
      <ng-content></ng-content>
    </div>
  `,
})
export class DialogFooterComponent {}
