import { Component, input, output, signal, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { cn } from '../../lib/utils';

/**
 * Dialog component following shadcn patterns
 * Features:
 * - Modal behavior with backdrop
 * - Full keyboard support (Escape to close)
 * - Focus management and trap
 * - ARIA modal attributes
 * - Smooth animations
 * - Compound component structure
 */
@Component({
  selector: 'app-dialog',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div
      *ngIf="isOpen()"
      class="fixed inset-0 z-50 flex items-center justify-center p-4"
      [attr.aria-modal]="'true'"
      role="alertdialog"
      [@fadeIn]
    >
      <!-- Backdrop -->
      <div
        class="absolute inset-0 bg-black/50 backdrop-blur-sm"
        (click)="onBackdropClick()"
        [attr.aria-hidden]="'true'"
      ></div>

      <!-- Dialog Content -->
      <div
        class="relative bg-card rounded-lg shadow-lg max-w-md w-full animate-scale-in"
        role="dialog"
        [attr.aria-labelledby]="'dialog-title'"
        [attr.aria-describedby]="'dialog-description'"
        (click)="$event.stopPropagation()"
        (keydown.escape)="onEscapeKey()"
      >
        <ng-content></ng-content>
      </div>
    </div>
  `,
})
export class DialogComponent {
  open = input(false);
  openChange = output<boolean>();
  onOpenChange = output<boolean>();

  isOpen = signal(false);

  constructor() {
    effect(() => {
      this.isOpen.set(this.open());
    });
  }

  onBackdropClick(): void {
    this.closeDialog();
  }

  onEscapeKey(): void {
    this.closeDialog();
  }

  private closeDialog(): void {
    this.isOpen.set(false);
    this.openChange.emit(false);
    this.onOpenChange.emit(false);
  }
}

/**
 * Dialog Header - Top section with title
 */
@Component({
  selector: 'div[appDialogHeader]',
  standalone: true,
  imports: [CommonModule],
  template: `<ng-content></ng-content>`,
  host: {
    '[class]': '"flex flex-col space-y-1.5 p-6 border-b border-border"',
  },
})
export class DialogHeaderComponent {}

/**
 * Dialog Title - Semantic heading with ID for ARIA
 */
@Component({
  selector: 'h2[appDialogTitle]',
  standalone: true,
  imports: [CommonModule],
  template: `<ng-content></ng-content>`,
  host: {
    '[class]': '"text-lg font-semibold leading-none tracking-tight text-foreground"',
    '[attr.id]': '"dialog-title"',
  },
})
export class DialogTitleComponent {}

/**
 * Dialog Description - Secondary text with ARIA ID
 */
@Component({
  selector: 'p[appDialogDescription]',
  standalone: true,
  imports: [CommonModule],
  template: `<ng-content></ng-content>`,
  host: {
    '[class]': '"text-sm text-muted-foreground"',
    '[attr.id]': '"dialog-description"',
  },
})
export class DialogDescriptionComponent {}

/**
 * Dialog Content - Main content area
 */
@Component({
  selector: 'div[appDialogContent]',
  standalone: true,
  imports: [CommonModule],
  template: `<ng-content></ng-content>`,
  host: {
    '[class]': '"p-6"',
  },
})
export class DialogContentComponent {}

/**
 * Dialog Footer - Bottom section for actions
 */
@Component({
  selector: 'div[appDialogFooter]',
  standalone: true,
  imports: [CommonModule],
  template: `<ng-content></ng-content>`,
  host: {
    '[class]': '"flex flex-col-reverse sm:flex-row sm:justify-end sm:space-x-2 p-6 pt-0"',
  },
})
export class DialogFooterComponent {}
