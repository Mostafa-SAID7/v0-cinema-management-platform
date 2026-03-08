# CinemaVerse Component Usage Guide

This guide provides practical examples for using all shadcn-aligned components.

## Core Components

### Button

```typescript
// Basic variants
<button appButton>Default</button>
<button appButton variant="primary">Primary</button>
<button appButton variant="secondary">Secondary</button>
<button appButton variant="destructive">Delete</button>
<button appButton variant="outline">Outline</button>
<button appButton variant="ghost">Ghost</button>
<button appButton variant="link">Link</button>

// Sizes
<button appButton size="sm">Small</button>
<button appButton size="md">Medium (default)</button>
<button appButton size="lg">Large</button>

// States
<button appButton [disabled]="isLoading()">Disabled</button>
<button appButton [loading]="isLoading()">Loading...</button>

// Events
<button appButton (clicked)="handleClick($event)">Click me</button>

// Custom styling
<button appButton class="w-full">Full width</button>
```

### Input

```typescript
// Basic input
<input appInput type="text" placeholder="Enter text" />

// With validation
<input
  appInput
  type="email"
  placeholder="user@example.com"
  [hasError]="form.get('email')?.invalid && form.get('email')?.touched"
  [attr.aria-invalid]="form.get('email')?.invalid"
/>

// Form integration
<input
  appInput
  type="password"
  placeholder="Enter password"
  formControlName="password"
/>

// Textarea
<textarea
  appInput
  placeholder="Enter your message"
  formControlName="message"
></textarea>

// With label and error
<div>
  <label for="email-input">Email</label>
  <input
    appInput
    id="email-input"
    type="email"
    placeholder="user@example.com"
    formControlName="email"
    [attr.aria-describedby]="form.get('email')?.invalid ? 'email-error' : undefined"
  />
  <p *ngIf="form.get('email')?.invalid && form.get('email')?.touched" id="email-error">
    Please enter a valid email
  </p>
</div>
```

### Card

```typescript
// Basic card
<div appCard>
  <div appCardHeader>
    <h2 appCardTitle>Card Title</h2>
    <p appCardDescription>Additional context</p>
  </div>
  <div appCardContent>
    Main content here
  </div>
  <div appCardFooter>
    <button appButton variant="outline">Cancel</button>
    <button appButton>Save</button>
  </div>
</div>

// Movie card example
<div appCard>
  <div appCardHeader>
    <h3 appCardTitle>Inception</h3>
    <p appCardDescription>Directed by Christopher Nolan</p>
  </div>
  <div appCardContent>
    <img src="poster.jpg" alt="Movie poster" class="w-full rounded mb-4" />
    <p class="text-sm text-muted-foreground">
      A skilled thief who steals corporate secrets through the use of dream-sharing technology...
    </p>
  </div>
  <div appCardFooter>
    <span class="text-sm text-muted-foreground">Rating: 8.8/10</span>
  </div>
</div>
```

### Dialog

```typescript
export class MyComponent {
  isOpen = signal(false);

  openDialog() {
    this.isOpen.set(true);
  }

  closeDialog() {
    this.isOpen.set(false);
  }

  handleConfirm() {
    // Perform action
    this.closeDialog();
  }
}
```

```html
<!-- Dialog trigger -->
<button appButton (clicked)="openDialog()">Open Dialog</button>

<!-- Dialog -->
<app-dialog [open]="isOpen()" (onOpenChange)="isOpen.set($event)">
  <div appDialogHeader>
    <h2 appDialogTitle>Confirm Action</h2>
  </div>
  <div appDialogContent>
    <p>Are you sure you want to proceed?</p>
  </div>
  <div appDialogFooter>
    <button appButton variant="outline" (clicked)="closeDialog()">Cancel</button>
    <button appButton (clicked)="handleConfirm()">Confirm</button>
  </div>
</app-dialog>
```

## Advanced Components

### Badge

```typescript
// Variants
<span appBadge>Default</span>
<span appBadge variant="primary">Primary</span>
<span appBadge variant="secondary">Secondary</span>
<span appBadge variant="destructive">Destructive</span>
<span appBadge variant="outline">Outline</span>
<span appBadge variant="success">Success</span>
<span appBadge variant="warning">Warning</span>

// In a card
<div appCard>
  <div appCardContent>
    <h3 class="text-lg font-semibold mb-2">Movie Status</h3>
    <div class="space-x-2">
      <span appBadge variant="success">Available</span>
      <span appBadge variant="warning">Limited Time</span>
    </div>
  </div>
</div>
```

### Tabs

```typescript
export class TabsExampleComponent {
  selectedTab = signal('movies');
}
```

```html
<div appTabs [defaultValue]="'movies'" (valueChange)="selectedTab.set($event)">
  <!-- Tab List -->
  <div appTabsList>
    <button appTabsTrigger value="movies">Movies</button>
    <button appTabsTrigger value="series">Series</button>
    <button appTabsTrigger value="watchlist">Watchlist</button>
  </div>

  <!-- Tab Content -->
  <div appTabsContent value="movies">
    <div class="grid grid-cols-3 gap-4">
      <!-- Movie cards -->
    </div>
  </div>

  <div appTabsContent value="series">
    <div class="grid grid-cols-3 gap-4">
      <!-- Series cards -->
    </div>
  </div>

  <div appTabsContent value="watchlist">
    <div class="space-y-2">
      <!-- Watchlist items -->
    </div>
  </div>
</div>
```

### Accordion

```typescript
export class AccordionExampleComponent {
  faqItems = [
    { id: 'q1', question: 'What is CinemaVerse?', answer: '...' },
    { id: 'q2', question: 'How do I subscribe?', answer: '...' },
    { id: 'q3', question: 'Can I cancel anytime?', answer: '...' },
  ];
}
```

```html
<div appAccordion type="single" [collapsible]="true" (valueChange)="onAccordionChange($event)">
  <div *ngFor="let item of faqItems" [appAccordionItem]="item.id">
    <button appAccordionTrigger class="group">
      {{ item.question }}
      <svg class="h-4 w-4 shrink-0 transition-transform duration-200" fill="none" stroke="currentColor">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 14l-7 7m0 0l-7-7m7 7V3" />
      </svg>
    </button>
    <div appAccordionContent>
      {{ item.answer }}
    </div>
  </div>
</div>
```

## Form Integration

### Complete Form Example

```typescript
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { useFormField, FormGroupHelper } from './lib/form-utils';

@Component({
  selector: 'app-movie-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    ButtonComponent,
    InputComponent,
    CardComponent,
    // ... other components
  ],
  template: `
    <div appCard class="max-w-md">
      <div appCardHeader>
        <h2 appCardTitle>Add Movie</h2>
        <p appCardDescription>Create a new movie entry</p>
      </div>

      <form [formGroup]="form" (ngSubmit)="onSubmit()" appCardContent>
        <!-- Title Field -->
        <div class="mb-4">
          <label for="title" class="block text-sm font-medium mb-1">
            Title
          </label>
          <input
            appInput
            id="title"
            type="text"
            placeholder="Movie title"
            formControlName="title"
            [attr.aria-invalid]="titleField().hasError"
          />
          <p *ngIf="titleField().hasError" class="text-sm text-destructive mt-1">
            {{ titleField().errorMessage }}
          </p>
        </div>

        <!-- Description Field -->
        <div class="mb-4">
          <label for="description" class="block text-sm font-medium mb-1">
            Description
          </label>
          <textarea
            appInput
            id="description"
            placeholder="Movie description"
            formControlName="description"
          ></textarea>
        </div>

        <!-- Release Year Field -->
        <div class="mb-4">
          <label for="year" class="block text-sm font-medium mb-1">
            Release Year
          </label>
          <input
            appInput
            id="year"
            type="number"
            placeholder="2024"
            formControlName="year"
            [attr.aria-invalid]="yearField().hasError"
          />
          <p *ngIf="yearField().hasError" class="text-sm text-destructive mt-1">
            {{ yearField().errorMessage }}
          </p>
        </div>

        <!-- Submit Button -->
        <div appCardFooter class="mt-6">
          <button appButton variant="outline" type="button" (clicked)="onCancel()">
            Cancel
          </button>
          <button
            appButton
            variant="primary"
            type="submit"
            [disabled]="!form.valid || isSubmitting()"
            [loading]="isSubmitting()"
          >
            Save Movie
          </button>
        </div>
      </form>
    </div>
  `,
})
export class MovieFormComponent {
  form: FormGroup;
  isSubmitting = signal(false);
  formHelper: FormGroupHelper;

  constructor(private fb: FormBuilder) {
    this.form = this.fb.group({
      title: ['', [Validators.required, Validators.minLength(3)]],
      description: [''],
      year: ['', [Validators.required, Validators.min(1895), Validators.max(2100)]],
    });
    this.formHelper = new FormGroupHelper(this.form);
  }

  titleField = computed(() => useFormField(this.form.get('title'), { label: 'Title' }));
  yearField = computed(() => useFormField(this.form.get('year'), { label: 'Year' }));

  onSubmit(): void {
    if (this.form.valid) {
      this.isSubmitting.set(true);
      // Submit logic
    }
  }

  onCancel(): void {
    this.formHelper.reset();
  }
}
```

## Accessibility Patterns

### Keyboard Navigation

All components support full keyboard navigation:

- **Buttons**: Enter, Space
- **Inputs**: Tab, Arrow keys (in specialized inputs)
- **Modals**: Escape to close, Tab focus management
- **Tabs**: Arrow keys to navigate, Home/End for first/last
- **Accordion**: Space/Enter to toggle

### Screen Reader Support

```html
<!-- Proper labeling -->
<label for="search-input">Search movies</label>
<input
  appInput
  id="search-input"
  type="search"
  aria-label="Search for movies"
  aria-describedby="search-help"
/>
<small id="search-help">Use keywords, actors, or directors</small>

<!-- Form validation announcements -->
<input
  appInput
  [attr.aria-invalid]="hasError"
  [attr.aria-describedby]="hasError ? 'error-message' : undefined"
/>
<p *ngIf="hasError" id="error-message" role="alert">
  Error message for screen readers
</p>

<!-- Dialog accessibility -->
<app-dialog
  [attr.aria-modal]="'true'"
  [attr.aria-labelledby]="'dialog-title'"
  [attr.aria-describedby]="'dialog-description'"
>
  <h2 id="dialog-title">Confirm Delete</h2>
  <p id="dialog-description">This action cannot be undone.</p>
</app-dialog>
```

## Styling & Customization

### Using Custom Classes

```html
<!-- Add custom margins -->
<button appButton class="mb-4">Button</button>

<!-- Override sizing -->
<button appButton size="lg" class="w-full">Full Width Button</button>

<!-- Combine with utility classes -->
<div appCard class="border-primary shadow-lg">
  <div appCardContent class="p-8">
    Custom styled content
  </div>
</div>
```

### Color Variants

```html
<!-- Using design tokens -->
<button appButton variant="primary">Primary (brand color)</button>
<button appButton variant="secondary">Secondary (muted)</button>
<button appButton variant="destructive">Destructive (red)</button>
<button appButton variant="outline">Outline (bordered)</button>
<button appButton variant="ghost">Ghost (transparent)</button>
<button appButton variant="link">Link (underlined text)</button>
```

## Common Patterns

### Loading State

```typescript
export class LoadingExample {
  isLoading = signal(false);

  async loadData() {
    this.isLoading.set(true);
    try {
      // Load data
    } finally {
      this.isLoading.set(false);
    }
  }
}
```

```html
<button appButton [loading]="isLoading()" (clicked)="loadData()">
  {{ isLoading() ? 'Loading...' : 'Load Data' }}
</button>
```

### Error Handling

```html
<div *ngIf="error()">
  <div appCard class="border-destructive">
    <div appCardHeader>
      <h3 appCardTitle>Error</h3>
    </div>
    <div appCardContent>
      {{ error() }}
    </div>
  </div>
</div>
```

### Empty State

```html
<div *ngIf="items().length === 0">
  <div appCard class="text-center py-12">
    <div appCardContent>
      <p class="text-muted-foreground mb-4">No items found</p>
      <button appButton (clicked)="createNew()">Create First Item</button>
    </div>
  </div>
</div>
```

## Testing Components

### Unit Test Example

```typescript
describe('Button Component', () => {
  it('should emit clicked event when clicked', () => {
    // Component setup
    const component = TestBed.createComponent(ButtonComponent);
    let clickedCount = 0;

    component.componentInstance.clicked.subscribe(() => {
      clickedCount++;
    });

    component.nativeElement.click();
    expect(clickedCount).toBe(1);
  });

  it('should not emit when disabled', () => {
    // Component setup
    const component = TestBed.createComponent(ButtonComponent);
    component.componentInstance.disabled = signal(true);

    let clickedCount = 0;
    component.componentInstance.clicked.subscribe(() => {
      clickedCount++;
    });

    component.nativeElement.click();
    expect(clickedCount).toBe(0);
  });
});
```

## Performance Tips

1. Use signals for reactive state
2. Use `computed` for derived state
3. Minimize template computation
4. Use `trackBy` with `*ngFor`
5. Lazy load dialogs and modals
6. Use `OnPush` change detection

## Troubleshooting

### Styles not applying

Ensure `globals.css` is imported and Tailwind is configured.

### Keyboard navigation not working

Check that `host` bindings are properly defined on the component.

### ARIA attributes missing

Use the component registry to validate a11y compliance:

```typescript
import { validateA11y } from './lib/component-registry';

const status = validateA11y('button');
console.log(status.compliant, status.issues);
```
