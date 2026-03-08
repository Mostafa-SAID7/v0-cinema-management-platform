/**
 * shadcn-aligned Form Integration Utilities
 * Provides reactive form integration patterns consistent with shadcn
 */

import { FormControl, AbstractControl, FormGroup } from '@angular/forms';

/**
 * Form field state interface
 * Unified way to track form field state across components
 */
export interface FormFieldState {
  value: any;
  touched: boolean;
  dirty: boolean;
  valid: boolean;
  invalid: boolean;
  errors: Record<string, any> | null;
  disabled: boolean;
}

/**
 * Get reactive form field state
 * shadcn pattern: Expose field state for binding in templates
 */
export function getFormFieldState(control: AbstractControl | null): FormFieldState {
  if (!control) {
    return {
      value: undefined,
      touched: false,
      dirty: false,
      valid: true,
      invalid: false,
      errors: null,
      disabled: false,
    };
  }

  return {
    value: control.value,
    touched: control.touched ?? false,
    dirty: control.dirty ?? false,
    valid: control.valid ?? true,
    invalid: control.invalid ?? false,
    errors: control.errors,
    disabled: control.disabled ?? false,
  };
}

/**
 * Create error message from form validation errors
 */
export function getErrorMessage(
  errors: Record<string, any> | null | undefined,
  fieldName: string = 'This field'
): string {
  if (!errors) return '';

  const errorKeys = Object.keys(errors);
  const firstError = errorKeys[0];

  switch (firstError) {
    case 'required':
      return `${fieldName} is required`;
    case 'minlength':
      return `${fieldName} must be at least ${errors[firstError].requiredLength} characters`;
    case 'maxlength':
      return `${fieldName} must be at most ${errors[firstError].requiredLength} characters`;
    case 'pattern':
      return `${fieldName} has an invalid format`;
    case 'email':
      return `${fieldName} must be a valid email`;
    case 'min':
      return `${fieldName} must be at least ${errors[firstError].min}`;
    case 'max':
      return `${fieldName} must be at most ${errors[firstError].max}`;
    case 'requiredTrue':
      return `${fieldName} must be checked`;
    default:
      return `${fieldName} is invalid`;
  }
}

/**
 * Check if field should show error
 * Shows error only if field is touched and invalid
 */
export function shouldShowFieldError(control: AbstractControl | null): boolean {
  return !!(control && control.invalid && (control.touched || control.dirty));
}

/**
 * Form field configuration interface
 * Used by form-field component to manage field state
 */
export interface FormFieldConfig {
  label?: string;
  description?: string;
  placeholder?: string;
  required?: boolean;
  disabled?: boolean;
  readonly?: boolean;
  error?: string;
  hint?: string;
}

/**
 * Bind form control to template
 * Returns control state and helpers for template binding
 */
export function useFormField(control: AbstractControl | null, config?: FormFieldConfig) {
  const state = getFormFieldState(control);
  const hasError = shouldShowFieldError(control);
  const errorMessage = hasError ? getErrorMessage(state.errors) : '';

  return {
    control,
    state,
    hasError,
    errorMessage,
    config: config || {},
  };
}

/**
 * Get ARIA attributes for form field
 * shadcn pattern: Automatic a11y attribute generation
 */
export function getFormFieldAriaAttrs(
  control: AbstractControl | null,
  fieldId: string
): Record<string, any> {
  const state = getFormFieldState(control);

  return {
    'aria-invalid': state.invalid,
    'aria-disabled': state.disabled,
    'aria-required': state.invalid && state.errors?.['required'],
    'aria-describedby': state.invalid
      ? `${fieldId}-error`
      : `${fieldId}-description`,
  };
}

/**
 * Form group helpers
 * Simplify working with form groups
 */
export class FormGroupHelper {
  constructor(private formGroup: FormGroup) {}

  /**
   * Get all fields with errors
   */
  getFieldsWithErrors(): string[] {
    const fields: string[] = [];

    Object.keys(this.formGroup.controls).forEach((key) => {
      const control = this.formGroup.get(key);
      if (control && control.invalid && (control.touched || control.dirty)) {
        fields.push(key);
      }
    });

    return fields;
  }

  /**
   * Check if any field has errors
   */
  hasErrors(): boolean {
    return this.getFieldsWithErrors().length > 0;
  }

  /**
   * Get error summary
   */
  getErrorSummary(): Record<string, string> {
    const summary: Record<string, string> = {};

    Object.keys(this.formGroup.controls).forEach((key) => {
      const control = this.formGroup.get(key);
      if (control && control.invalid && (control.touched || control.dirty)) {
        summary[key] = getErrorMessage(control.errors, key);
      }
    });

    return summary;
  }

  /**
   * Reset form and clear validation state
   */
  reset(): void {
    this.formGroup.reset();
    Object.keys(this.formGroup.controls).forEach((key) => {
      const control = this.formGroup.get(key);
      if (control) {
        control.markAsUntouched();
        control.markAsPristine();
      }
    });
  }

  /**
   * Mark all fields as touched
   */
  markAllAsTouched(): void {
    Object.keys(this.formGroup.controls).forEach((key) => {
      const control = this.formGroup.get(key);
      if (control) {
        control.markAsTouched();
      }
    });
  }
}
