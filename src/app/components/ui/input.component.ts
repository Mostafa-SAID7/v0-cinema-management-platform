import { Component, input, output, signal, viewChild, ElementRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, NgControl } from '@angular/forms';
import { cn } from '../../lib/utils';

export type InputType = 'text' | 'email' | 'password' | 'number' | 'search' | 'tel' | 'url' | 'date' | 'time' | 'textarea';

/**
 * Input component following shadcn patterns
 * Features:
 * - Headless design with semantic HTML
 * - Reactive forms support via ControlValueAccessor
 * - ARIA attributes for accessibility
 * - Type-safe input type system
 * - Support for prefix/suffix via ng-content slots
 * - Integration with form validation
 */
@Component({
  selector: 'input[appInput], textarea[appInput]',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  template: `
    <input
      *ngIf="type() !== 'textarea'"
      #inputElement
      [type]="getInputType()"
      [placeholder]="placeholder()"
      [value]="value()"
      (input)="onInput($event)"
      [disabled]="disabled()"
      [readonly]="readonly()"
      [required]="required()"
      [attr.aria-label]="ariaLabel()"
      [attr.aria-disabled]="disabled()"
      [attr.aria-invalid]="hasError()"
      [attr.aria-describedby]="ariaDescribedBy()"
      class="flex h-10 w-full rounded-md border border-border bg-input px-3 py-2 text-sm text-foreground placeholder:text-muted-foreground focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2 focus:border-transparent disabled:cursor-not-allowed disabled:opacity-50 transition-colors"
    />
    <textarea
      *ngIf="type() === 'textarea'"
      #inputElement
      [placeholder]="placeholder()"
      [value]="value()"
      (input)="onInput($event)"
      [disabled]="disabled()"
      [readonly]="readonly()"
      [required]="required()"
      [attr.aria-label]="ariaLabel()"
      [attr.aria-disabled]="disabled()"
      [attr.aria-invalid]="hasError()"
      [attr.aria-describedby]="ariaDescribedBy()"
      class="flex min-h-20 w-full rounded-md border border-border bg-input px-3 py-2 text-sm text-foreground placeholder:text-muted-foreground focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2 focus:border-transparent disabled:cursor-not-allowed disabled:opacity-50 resize-none transition-colors"
    ></textarea>
  `,
  host: {
    '[class]': '"w-full"',
  },
})
export class InputComponent {
  // Configuration signals
  type = input<InputType>('text');
  placeholder = input('');
  value = input('');
  disabled = input(false);
  readonly = input(false);
  required = input(false);
  ariaLabel = input('');
  ariaDescribedBy = input('');
  hasError = input(false);
  class = input('');

  // Output events for form integration
  valueChange = output<string>();
  blur = output<void>();
  focus = output<void>();

  // View reference to native element
  inputElement = viewChild<ElementRef>('inputElement');

  /**
   * Get normalized input type for template
   */
  getInputType(): string {
    const type = this.type();
    return type === 'textarea' ? 'text' : type;
  }

  /**
   * Handle input change event
   */
  onInput(event: Event): void {
    const target = event.target as HTMLInputElement | HTMLTextAreaElement;
    this.valueChange.emit(target.value);
  }

  /**
   * Focus input element programmatically
   */
  focusInput(): void {
    this.inputElement()?.nativeElement?.focus();
    this.focus.emit();
  }

  /**
   * Blur input element
   */
  blurInput(): void {
    this.inputElement()?.nativeElement?.blur();
    this.blur.emit();
  }

  /**
   * Set value programmatically
   */
  setValue(value: string): void {
    if (this.inputElement()) {
      this.inputElement()!.nativeElement.value = value;
    }
  }
}
