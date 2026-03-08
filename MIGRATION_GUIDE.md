# CinemaVerse Component Migration Guide

## Overview

This guide helps you migrate from old custom components to the new shadcn-aligned components.

## Before & After Examples

### Button Component

**Before (Old Pattern):**
```typescript
import { ButtonComponent } from './components/ui/button.component';

<app-button 
  variant="primary" 
  size="md" 
  [disabled]="isLoading"
  (click)="handleClick()">
  Click me
</app-button>
```

**After (shadcn Pattern):**
```typescript
import { ButtonComponent } from './components/ui/button.component';

<button 
  appButton 
  variant="primary" 
  size="md" 
  [disabled]="isLoading()"
  (clicked)="handleClick($event)">
  Click me
</button>
```

**Key Changes:**
- Selector changed from `app-button` to attribute selector `appButton`
- Input signals use modern signal syntax
- Output events renamed to semantic names (`clicked` instead of generic click)
- Event payload includes full PointerEvent

### Card Component

**Before (Old Pattern):**
```typescript
<app-card>
  <app-card-header>
    <app-card-title>Title</app-card-title>
    <app-card-description>Description</app-card-description>
  </app-card-header>
  <app-card-content>
    Content here
  </app-card-content>
  <app-card-footer>
    Footer here
  </app-card-footer>
</app-card>
```

**After (shadcn Pattern):**
```typescript
<div appCard>
  <div appCardHeader>
    <h2 appCardTitle>Title</h2>
    <p appCardDescription>Description</p>
  </div>
  <div appCardContent>
    Content here
  </div>
  <div appCardFooter>
    Footer here
  </div>
</div>
```

**Key Changes:**
- Component selectors replaced with attribute selectors on semantic HTML
- Proper heading hierarchy with `<h2>` tag
- Use of semantic `<p>` tag for descriptions
- More composable and flexible structure

### Input Component

**Before (Old Pattern):**
```typescript
import { InputComponent } from './components/ui/input.component';

<app-input 
  type="email"
  placeholder="Enter email"
  [value]="email"
  (valueChange)="onEmailChange($event)">
</app-input>
```

**After (shadcn Pattern):**
```typescript
import { InputComponent } from './components/ui/input.component';

<input 
  appInput
  type="email"
  placeholder="Enter email"
  [attr.aria-invalid]="hasError()"
  [attr.aria-describedby]="errorId"
  (valueChange)="onEmailChange($event)"
/>
```

**Key Changes:**
- Semantic native `<input>` element with directive
- Full ARIA support for accessibility
- Supports reactive forms out of the box
- Better integration with browser features

### Dialog Component

**Before (Old Pattern):**
```typescript
<app-dialog [open]="isOpen" (openChange)="isOpen = $event">
  <app-dialog-header>
    <app-dialog-title>Title</app-dialog-title>
  </app-dialog-header>
  <app-dialog-content>
    Content
  </app-dialog-content>
  <app-dialog-footer>
    <button>Cancel</button>
    <button>OK</button>
  </app-dialog-footer>
</app-dialog>
```

**After (shadcn Pattern):**
```typescript
<app-dialog [open]="isOpen()" (onOpenChange)="isOpen.set($event)">
  <div appDialogHeader>
    <h2 appDialogTitle>Title</h2>
  </div>
  <div appDialogContent>
    Content
  </div>
  <div appDialogFooter>
    <button appButton variant="outline">Cancel</button>
    <button appButton variant="primary">OK</button>
  </div>
</app-dialog>
```

**Key Changes:**
- Dialog header uses semantic structure
- Title gets automatic ID for ARIA linkage
- All keyboard support (Escape, Tab focus management)
- Backdrop blur and smooth animations
- Better z-index management

## Step-by-Step Migration

### 1. Update Imports

**Old:**
```typescript
import { ButtonComponent } from './components/ui/button.component';
import { InputComponent } from './components/ui/input.component';
import { CardComponent } from './components/ui/card.component';
```

**New:**
```typescript
import { ButtonComponent } from './components/ui/button.component';
import { InputComponent } from './components/ui/input.component';
import { CardComponent, CardHeaderComponent, CardTitleComponent, CardContentComponent, CardFooterComponent } from './components/ui/card.component';
```

### 2. Update Component Declarations

**Old:**
```typescript
@Component({
  selector: 'app-my-component',
  imports: [ButtonComponent, InputComponent, CardComponent]
})
```

**New:**
```typescript
@Component({
  selector: 'app-my-component',
  imports: [
    ButtonComponent, 
    InputComponent, 
    CardComponent,
    CardHeaderComponent,
    CardTitleComponent,
    CardContentComponent,
    CardFooterComponent
  ]
})
```

### 3. Update Templates

**Replace all old component selectors:**

```typescript
// Button
- <app-button>
+ <button appButton>

// Input
- <app-input>
+ <input appInput> or <textarea appInput>

// Card components
- <app-card>
+ <div appCard>

- <app-card-header>
+ <div appCardHeader>

- <app-card-title>
+ <h2 appCardTitle>

- <app-card-content>
+ <div appCardContent>

- <app-card-footer>
+ <div appCardFooter>

// Dialog
- <app-dialog-header>
+ <div appDialogHeader>

- <app-dialog-title>
+ <h2 appDialogTitle>

- <app-dialog-content>
+ <div appDialogContent>

- <app-dialog-footer>
+ <div appDialogFooter>
```

### 4. Update TypeScript Code

**Input signals:**

```typescript
// Before
@Component({...})
export class MyComponent {
  disabled = input(false);
  onClick = output<void>();
}

// After
@Component({...})
export class MyComponent {
  disabled = input(false);
  clicked = output<PointerEvent>();

  handleClick(): void {
    if (!this.disabled()) {
      this.clicked.emit(new PointerEvent('click'));
    }
  }
}
```

### 5. Update Form Integration

**Before:**
```typescript
<app-input 
  [value]="form.get('email')?.value"
  (valueChange)="form.get('email')?.setValue($event)">
</app-input>
```

**After:**
```typescript
<input 
  appInput
  type="email"
  formControlName="email"
  [attr.aria-invalid]="form.get('email')?.invalid"
/>
```

### 6. Add Accessibility Attributes

For all interactive elements, add ARIA attributes:

```typescript
// Buttons
<button appButton [attr.aria-disabled]="disabled()">

// Form fields
<input appInput [attr.aria-invalid]="hasError()" [attr.aria-describedby]="errorId" />

// Dialogs
<app-dialog [attr.aria-modal]="'true'" [attr.aria-labelledby]="'dialog-title'">
```

## Common Patterns

### Using Variants

```typescript
// Button variants
<button appButton variant="primary">Primary</button>
<button appButton variant="secondary">Secondary</button>
<button appButton variant="destructive">Delete</button>
<button appButton variant="outline">Outline</button>
<button appButton variant="ghost">Ghost</button>
<button appButton variant="link">Link</button>

// Button sizes
<button appButton size="sm">Small</button>
<button appButton size="md">Medium</button>
<button appButton size="lg">Large</button>
```

### Form Field with Validation

```typescript
export class MyComponent {
  form = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email])
  });

  emailControl = computed(() => this.form.get('email'));
  emailState = computed(() => useFormField(this.emailControl(), {
    label: 'Email',
    placeholder: 'user@example.com',
    required: true
  }));
}
```

```html
<div [formGroup]="form">
  <label [for]="'email'">{{ emailState().config.label }}</label>
  <input 
    appInput
    id="email"
    type="email"
    formControlName="email"
    [attr.aria-invalid]="emailState().hasError"
    [attr.aria-describedby]="emailState().hasError ? 'email-error' : undefined"
  />
  <p 
    *ngIf="emailState().hasError" 
    id="email-error" 
    class="text-sm text-destructive mt-1">
    {{ emailState().errorMessage }}
  </p>
</div>
```

### Compound Components

```typescript
// Card with content
<div appCard>
  <div appCardHeader>
    <h2 appCardTitle>Movie Card</h2>
    <p appCardDescription>Click to view details</p>
  </div>
  <div appCardContent>
    <img src="poster.jpg" alt="Movie poster" />
    <p>Movie synopsis here...</p>
  </div>
  <div appCardFooter>
    <button appButton variant="outline">Cancel</button>
    <button appButton>Watch Now</button>
  </div>
</div>
```

## Testing Your Migration

### 1. Check Rendering
- Verify all components render correctly
- Check spacing and alignment
- Verify colors match design system

### 2. Test Keyboard Navigation
- Tab through all interactive elements
- Test Escape key on modals
- Test Enter/Space on buttons

### 3. Test with Screen Reader
- Use NVDA, JAWS, or VoiceOver
- Verify all labels are read
- Verify form errors are announced

### 4. Run Accessibility Audit
```bash
npm run test:a11y
```

### 5. Run Unit Tests
```bash
npm run test
```

## Troubleshooting

### Issue: Components not rendering

**Solution:** Make sure all required sub-components are imported.

### Issue: Styles not applied

**Solution:** Check that Tailwind CSS is configured correctly and that `globals.css` is imported.

### Issue: Keyboard navigation not working

**Solution:** Add keyboard event handlers to host binding and implement proper focus management.

### Issue: ARIA attributes missing

**Solution:** Use the `getFormFieldAriaAttrs()` helper from form-utils to automatically generate ARIA attributes.

## Rollback Procedure

If issues arise during migration:

1. Revert component files to previous version
2. Keep new utility files (they're backward compatible)
3. Remove feature flag if used
4. Redeploy
5. Investigate issue and retry migration

## Success Metrics

After migration, verify:

- [ ] 100% TypeScript strict mode compliance
- [ ] 95%+ accessibility compliance (axe-core)
- [ ] All keyboard navigation working
- [ ] Screen reader compatibility verified
- [ ] Bundle size increase < 5%
- [ ] Performance metrics unchanged
- [ ] All tests passing
- [ ] Zero console errors/warnings

## Next Steps

1. Gradually migrate one feature at a time
2. Test thoroughly at each step
3. Get accessibility review
4. Deploy to staging first
5. Gather user feedback
6. Deploy to production
