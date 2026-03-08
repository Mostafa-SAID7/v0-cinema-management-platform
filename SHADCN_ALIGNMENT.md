# CinemaVerse UI - shadcn Alignment Guide

## Overview

This document outlines the comprehensive refactoring of CinemaVerse UI components to align with shadcn design system principles and best practices.

## Key Principles

### 1. **Headless Component Architecture**
- Components are unstyled and flexible
- Styling applied via Tailwind CSS classes
- Focus on HTML semantics and accessibility
- Minimal pre-built styles

### 2. **Compound Component Pattern**
- Use Angular attribute selectors (`selector: 'div[appCard]'`)
- Compose complex UIs from simple, focused components
- Enable composition via `ng-content` slots
- Clear parent-child relationships

### 3. **Accessibility-First Design (WCAG 2.1 Level AA)**
- Full WAI-ARIA support with role attributes
- Keyboard navigation support (Tab, Enter, Escape, Arrow keys)
- Screen reader tested and optimized
- Semantic HTML elements
- Proper heading hierarchy

### 4. **Type Safety**
- Full TypeScript typing with generics
- Variant system using CVA (Class Variance Authority) pattern
- Type-safe form integration
- Input/output validation

### 5. **Reactive Angular Patterns**
- Signal-based state management
- Computed properties for derived state
- Effect hooks for side effects
- Proper lifecycle management

## Component Architecture

### Utility Functions

#### `cn()` - Class Name Merger
```typescript
import { cn } from './lib/utils';

// Intelligently merges class strings
const classes = cn('px-4 py-2', isActive && 'bg-primary', customClass);
```

#### `cva()` - Class Variance Authority
```typescript
import { cva } from './lib/utils';

const buttonVariants = cva('base-classes', {
  variants: {
    size: { sm: 'px-3 py-1', lg: 'px-8 py-3' },
    variant: { primary: 'bg-primary', secondary: 'bg-secondary' }
  },
  defaultVariants: { size: 'md', variant: 'default' }
});

// Usage in component
const buttonClass = buttonVariants({ size: 'lg', variant: 'primary' });
```

### Component Registry

All components are registered in `COMPONENT_REGISTRY` with metadata:
- Component name and description
- Category (input, feedback, layout, overlay, navigation, data)
- Accessibility compliance info
- Keyboard support
- Available variants and slots
- Status (stable, beta, deprecated)

```typescript
import { getComponentMetadata, validateA11y } from './lib/component-registry';

const buttonMeta = getComponentMetadata('button');
const a11yStatus = validateA11y('button');
```

## Core Components

### Button Component

**Selector:** `button[appButton]`

**Features:**
- 7 variants: default, primary, secondary, destructive, outline, ghost, link
- 3 sizes: sm, md, lg
- Loading state support
- Full keyboard support (Enter, Space)
- ARIA attributes for disabled/loading states

**Usage:**
```typescript
<button appButton variant="primary" size="lg" (clicked)="handleClick()">
  Click me
</button>
```

### Input Component

**Selector:** `input[appInput]`, `textarea[appInput]`

**Features:**
- Type-safe input types (text, email, password, number, search, tel, url, date, time, textarea)
- Reactive forms integration
- ARIA attributes for validation
- Error state support
- Focus/blur events

**Usage:**
```typescript
<input 
  appInput 
  type="email" 
  placeholder="Enter email"
  [hasError]="form.get('email')?.invalid && form.get('email')?.touched"
  (valueChange)="onEmailChange($event)"
/>
```

### Card Component

**Selector:** `div[appCard]`

**Sub-components:**
- `div[appCardHeader]` - Header section
- `h2[appCardTitle]` - Title heading
- `p[appCardDescription]` - Description text
- `div[appCardContent]` - Main content
- `div[appCardFooter]` - Footer section

**Features:**
- Compound component structure
- Disabled state support
- Semantic HTML structure
- Consistent spacing and styling

**Usage:**
```typescript
<div appCard>
  <div appCardHeader>
    <h2 appCardTitle>Movie Title</h2>
    <p appCardDescription>Additional info</p>
  </div>
  <div appCardContent>
    Main content here
  </div>
</div>
```

### Dialog Component

**Selector:** `app-dialog`

**Sub-components:**
- `div[appDialogHeader]` - Header section
- `h2[appDialogTitle]` - Modal title with ID
- `p[appDialogDescription]` - Description with ID
- `div[appDialogContent]` - Content area
- `div[appDialogFooter]` - Footer with actions

**Features:**
- Full modal behavior
- Keyboard support (Escape to close)
- Focus management
- ARIA modal attributes
- Backdrop click to close
- Smooth animations

**Usage:**
```typescript
@Component({...})
export class MyComponent {
  isOpen = signal(false);
  
  openDialog() {
    this.isOpen.set(true);
  }
}
```

```html
<button appButton (clicked)="openDialog()">Open</button>

<app-dialog [open]="isOpen()" (openChange)="isOpen.set($event)">
  <div appDialogHeader>
    <h2 appDialogTitle>Confirm Action</h2>
  </div>
  <div appDialogContent>
    Are you sure?
  </div>
</app-dialog>
```

## Form Integration

### Form Field State Management

```typescript
import { useFormField, shouldShowFieldError, getErrorMessage } from './lib/form-utils';

export class MyComponent {
  form = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email])
  });

  emailField = computed(() => {
    const control = this.form.get('email');
    return useFormField(control, {
      label: 'Email',
      placeholder: 'Enter your email',
      required: true
    });
  });
}
```

### Template Usage

```html
<div [formGroup]="form">
  <label [for]="'email-input'">{{ emailField().config.label }}</label>
  <input
    appInput
    #emailInput
    id="email-input"
    type="email"
    formControlName="email"
    [placeholder]="emailField().config.placeholder"
    [hasError]="emailField().hasError"
  />
  <p *ngIf="emailField().hasError" class="text-destructive text-sm">
    {{ emailField().errorMessage }}
  </p>
</div>
```

## Migration Strategy

### Phase 1: Foundation (✓ Complete)
- Created `cn()` utility for class merging
- Implemented CVA pattern with `cva()` function
- Built component registry with metadata
- Created form integration utilities

### Phase 2: Core Components (In Progress)
- Button component with variants and keyboard support
- Input component with form integration
- Card component with compound structure
- Dialog component with modal behavior

### Phase 3: Advanced Components
- Tabs component with keyboard navigation
- Accordion component with expand/collapse
- Tooltip component with positioning
- Popover component with floating UI
- Alert Dialog component
- Select component
- Radio Group component
- Checkbox component

### Phase 4: Testing & Documentation
- Unit tests with Jest (>90% coverage)
- Integration tests with Cypress
- Accessibility testing with axe-core
- Storybook documentation
- Migration guide for existing code

### Phase 5: Migration & Deployment
- Feature flags for new components
- Gradual rollout over 2-4 weeks
- Backward compatibility layer
- Rollback procedures
- Success metrics tracking

## Accessibility Requirements

### ARIA Attributes
- All interactive elements have proper roles
- Disabled states properly communicated
- Form validation errors announced
- Modal dialogs have aria-modal="true"
- Buttons have proper aria-pressed for toggles

### Keyboard Support
- Tab navigation through all interactive elements
- Enter/Space for buttons and checkboxes
- Escape for modals and dropdowns
- Arrow keys for navigation components
- Focus management and visual indicators

### Screen Reader Support
- Semantic HTML elements
- Descriptive aria-labels
- Hidden decorative elements with aria-hidden
- Proper heading hierarchy
- Form field associations

## Testing Strategy

### Unit Tests
```typescript
describe('Button Component', () => {
  it('should emit clicked event when clicked', () => {
    // Test implementation
  });

  it('should support all variants', () => {
    // Test all variant combinations
  });

  it('should handle disabled state', () => {
    // Test disabled interactions
  });
});
```

### Accessibility Tests
```typescript
describe('Button Accessibility', () => {
  it('should have proper ARIA attributes', () => {
    // axe-core scanning
  });

  it('should be keyboard navigable', () => {
    // Tab, Enter, Escape key tests
  });

  it('should work with screen readers', () => {
    // Test aria-label, aria-described-by
  });
});
```

### Integration Tests
- Test component composition
- Test form integration
- Test event emission
- Test state management

## Best Practices

### 1. Use Attribute Selectors
```typescript
// Good - Attribute selector
@Component({ selector: 'div[appCard]' })

// Avoid - Element selector
@Component({ selector: 'app-card' })
```

### 2. Composition Over Configuration
```typescript
// Good - Compose from slots
<div appCard>
  <div appCardHeader>...</div>
  <div appCardContent>...</div>
</div>

// Avoid - Over-parameterized component
<app-card title="..." description="..." content="..."></app-card>
```

### 3. Type-Safe Variants
```typescript
// Good - CVA pattern with types
const buttonVariants = cva('...', {
  variants: { size: { sm: '...', lg: '...' } }
});

// Avoid - String-based variants
class = `btn btn-${this.size} btn-${this.variant}`;
```

### 4. Semantic HTML
```typescript
// Good - Semantic elements
<button appButton>Click</button>
<h2 appCardTitle>Title</h2>
<input appInput type="email" />

// Avoid - div-based elements
<div role="button">Click</div>
<div class="title">Title</div>
<div class="input"></div>
```

### 5. Accessibility First
```typescript
// Good - Full a11y support
@Component({
  host: {
    '[attr.aria-disabled]': 'disabled()',
    '[attr.aria-invalid]': 'hasError()',
    '(keydown.escape)': 'close()'
  }
})

// Avoid - No accessibility considerations
@Component({
  host: { '[disabled]': 'disabled()' }
})
```

## Migration Checklist

- [ ] Review existing component usage
- [ ] Update import statements to new components
- [ ] Replace old selectors with new attribute selectors
- [ ] Update template syntax for compound components
- [ ] Add ARIA attributes where needed
- [ ] Test keyboard navigation
- [ ] Test with screen readers
- [ ] Run accessibility audit with axe-core
- [ ] Update component documentation
- [ ] Deploy with feature flags
- [ ] Monitor for issues
- [ ] Collect user feedback

## Resources

- [shadcn/ui Documentation](https://ui.shadcn.com)
- [WAI-ARIA Authoring Practices](https://www.w3.org/WAI/ARIA/apg/)
- [Angular Best Practices](https://angular.io/guide/styleguide)
- [CVA Pattern](https://cva.style/)
- [Tailwind CSS](https://tailwindcss.com)

## Support & Questions

For questions or issues related to the shadcn alignment:
1. Check the component registry metadata
2. Review this documentation
3. Check component stories in Storybook
4. Create an issue with reproduction steps
