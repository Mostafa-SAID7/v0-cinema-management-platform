import { Component, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dialog',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div *ngIf="open()" class="fixed inset-0 z-50">
      <!-- Overlay -->
      <div
        class="fixed inset-0 z-50 bg-black/80"
        (click)="openChange.emit(false)"
      ></div>

      <!-- Dialog Content -->
      <div
        class="fixed left-1/2 top-1/2 z-50 grid w-full max-w-lg -translate-x-1/2 -translate-y-1/2 gap-4 border bg-background p-6 shadow-lg sm:rounded-lg animate-scale-in"
        (click)="$event.stopPropagation()"
      >
        <!-- Close Button -->
        <button
          (click)="openChange.emit(false)"
          class="absolute right-4 top-4 rounded-sm opacity-70 ring-offset-background transition-opacity hover:opacity-100 focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2"
        >
          <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <path d="M18 6 6 18"/>
            <path d="m6 6 12 12"/>
          </svg>
          <span class="sr-only">Close</span>
        </button>

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
    <div class="flex flex-col space-y-1.5 text-center sm:text-left">
      <ng-content></ng-content>
    </div>
  `,
})
export class DialogHeaderComponent {}

@Component({
  selector: 'app-dialog-title',
  standalone: true,
  imports: [CommonModule],
  template: `
    <h2 class="text-lg font-semibold leading-none tracking-tight">
      <ng-content></ng-content>
    </h2>
  `,
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
    <div class="py-4">
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
    <div class="flex flex-col-reverse sm:flex-row sm:justify-end sm:space-x-2">
      <ng-content></ng-content>
    </div>
  `,
})
export class DialogFooterComponent {}
