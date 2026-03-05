# CinemaVerse Error Handling & UI Enhancements Guide

## Overview
This document outlines all the error handling, empty states, loading states, and toast notifications implemented in the CinemaVerse platform.

---

## Toast Notifications

### ToastService
Located at: `/src/app/services/toast.service.ts`

The `ToastService` provides a centralized notification system with different types of alerts.

#### Methods
- `show(message: string, type: 'success' | 'error' | 'info' | 'warning', duration?: number)`
- `success(message: string, duration?: number)` - Green success toast
- `error(message: string, duration?: number)` - Red error toast
- `info(message: string, duration?: number)` - Blue info toast
- `warning(message: string, duration?: number)` - Yellow warning toast
- `dismiss(id: string)` - Remove a specific toast
- `clear()` - Clear all toasts

#### Usage Example
```typescript
export class MyComponent {
  constructor(private toastService: ToastService) {}

  saveData() {
    try {
      // Save operation
      this.toastService.success('Data saved successfully!');
    } catch (error) {
      this.toastService.error('Failed to save data. Please try again.');
    }
  }
}
```

#### Features
- Auto-dismiss after configurable duration (default: 3000ms)
- Smooth slide-in animation from bottom-right
- Color-coded by type (success, error, info, warning)
- Manual dismiss button on each toast
- Glassmorphism design with backdrop blur

---

## Toast Container Component

Located at: `/src/app/components/ui/toast-container.component.ts`

This component displays all active toasts. It's included in the `MainLayoutComponent` for global availability.

#### Implementation
```typescript
// In main-layout.component.ts
imports: [..., ToastContainerComponent]
template: `
  ...
  <app-toast-container></app-toast-container>
</div>
```

---

## Empty State Component

Located at: `/src/app/components/ui/empty-state.component.ts`

Displays a user-friendly message when no data is available.

#### Inputs
- `icon` - Unicode emoji or icon symbol (default: '📭')
- `title` - Empty state title (default: 'Nothing here')
- `message` - Descriptive message (default: 'No items to display')
- `actionLabel` - Optional action button text

#### Output
- `onAction` - Emitted when action button is clicked

#### Usage Example
```typescript
<app-empty-state
  icon="🎬"
  title="No movies found"
  message="We couldn't find any movies in the Action genre. Try another category."
  actionLabel="Browse All"
  (onAction)="setSelectedGenre('')"
></app-empty-state>
```

#### Styling
- Glassmorphism card with border and backdrop blur
- Centered layout with padding
- Responsive text sizes
- Primary action button with hover effects

---

## Skeleton Loading Component

Located at: `/src/app/components/ui/skeleton.component.ts`

Provides animated skeleton loaders for content placeholders.

#### Input
- `size` - One of: 'sm' | 'md' | 'lg' | 'xl' | 'video' | 'avatar'

#### Usage Example
```typescript
<!-- Show skeleton while loading -->
<div *ngIf="isLoading()" class="grid grid-cols-4 gap-4">
  <app-skeleton *ngFor="let i of [1,2,3,4]" size="video"></app-skeleton>
</div>

<!-- Show content when loaded -->
<div *ngIf="!isLoading()" class="grid grid-cols-4 gap-4">
  <app-movie-card *ngFor="let movie of movies()"></app-movie-card>
</div>
```

#### Sizes
- `sm`: Small text-like skeleton (h-4)
- `md`: Medium text skeleton (h-6)
- `lg`: Large heading skeleton (h-8)
- `xl`: Extra large skeleton (h-12)
- `video`: Video thumbnail size (aspect-video)
- `avatar`: Profile avatar size (10x10, rounded)

---

## Skeleton Card Component

Located at: `/src/app/components/ui/skeleton-card.component.ts`

Pre-built skeleton layout matching movie card structure.

#### Usage Example
```typescript
<!-- Loading state -->
<div *ngIf="isLoading()" class="grid grid-cols-4 gap-4">
  <app-skeleton-card *ngFor="let i of [1,2,3,4]"></app-skeleton-card>
</div>
```

---

## Error Page Component

Located at: `/src/app/components/pages/error-page.component.ts`

Full-page error display for critical failures.

#### Inputs
- `statusCode` - HTTP status code (default: 500)
- `title` - Error title (default: 'Something went wrong')
- `message` - Error description
- `details` - Optional technical details (shown in monospace)

#### Usage Example
```typescript
<!-- In movie-details.component.ts -->
<app-error-page
  *ngIf="movieError()"
  statusCode="404"
  title="Movie Not Found"
  message="The movie you're looking for doesn't exist or has been removed."
></app-error-page>
```

#### Features
- Full-page centered error display
- Support code display
- Action buttons (Back Home, Go Back)
- Support contact information
- Neon-styled error code

---

## Component Integration Examples

### Discovery Component (Trending Movies)
```typescript
export class DiscoveryComponent implements OnInit {
  isLoading = signal(true);

  ngOnInit(): void {
    setTimeout(() => {
      this.isLoading.set(false);
    }, 800);
  }
}
```

Template:
```html
<!-- Loading State -->
<div *ngIf="isLoading()" class="grid grid-cols-4 gap-4">
  <app-skeleton-card *ngFor="let i of [1,2,3,4,5,6,7,8]"></app-skeleton-card>
</div>

<!-- Content -->
<div *ngIf="!isLoading()" class="grid grid-cols-4 gap-4">
  <a *ngFor="let movie of trendingMovies()">
    <!-- Movie card -->
  </a>
</div>
```

### Discovery Component (Genre Filter)
```html
<ng-container *ngIf="genreMovies().length > 0; else noGenreMovies">
  <div class="grid grid-cols-4 gap-4">
    <a *ngFor="let movie of genreMovies()">
      <!-- Movie card -->
    </a>
  </div>
</ng-container>

<ng-template #noGenreMovies>
  <app-empty-state
    icon="🎬"
    title="No movies found"
    message="No movies in the {{ selectedGenre() }} genre"
    (onAction)="setSelectedGenre('')"
  ></app-empty-state>
</ng-template>
```

### Dashboard Component
```typescript
export class DashboardComponent implements OnInit {
  ngOnInit(): void {
    const user = this.authService.getCurrentUser();
    if (user) {
      this.toastService.success(`Welcome back, ${user.name}!`);
    }
  }
}
```

Template (Active Tickets):
```html
<ng-container *ngIf="upcomingBookings.length > 0; else noTickets">
  <!-- Ticket cards -->
</ng-container>

<ng-template #noTickets>
  <app-empty-state
    icon="🎬"
    title="No active tickets"
    message="Book your next movie to see tickets here"
    actionLabel="Browse Movies"
    (onAction)="router.navigate(['/']()"
  ></app-empty-state>
</ng-template>
```

### Booking Component
```typescript
export class BookingComponent implements OnInit {
  ngOnInit(): void {
    this.toastService.info('Select a showtime and click seats to book');
  }

  confirmBooking(): void {
    const seatCount = this.selectedSeats().length;
    const totalPrice = seatCount * 250;
    
    this.toastService.success(
      `Booking confirmed! ${seatCount} seat(s) for ₹${totalPrice}`
    );
  }

  resetBooking(): void {
    this.selectedSeats.set([]);
    this.toastService.info('Selection cleared');
  }
}
```

### Movie Details Component
```typescript
export class MovieDetailsComponent {
  movieError = computed(() => this.movieId && !this.movie());
}
```

Template:
```html
<!-- Content -->
<div *ngIf="movie(); let m = movie()">
  <!-- Movie details -->
</div>

<!-- Loading -->
<div *ngIf="!movie()">
  <p>Loading...</p>
</div>

<!-- Error -->
<app-error-page
  *ngIf="movieError()"
  statusCode="404"
  title="Movie Not Found"
></app-error-page>
```

### Support Component
```typescript
export class SupportComponent implements OnInit {
  ngOnInit(): void {
    this.toastService.info('Welcome to CinemaVerse Support!');
  }

  sendMessage(): void {
    this.toastService.success('Message sent! We\'ll respond within 24 hours.');
  }
}
```

---

## Best Practices

### 1. Loading States
- Use skeleton cards for grid-based content
- Simulate async loading with `setTimeout` for demo purposes
- Replace with actual API calls in production

### 2. Empty States
- Always provide an action or navigation option
- Use contextual icons (emojis) for quick visual recognition
- Keep messages concise and actionable

### 3. Toast Notifications
- Keep messages brief (max 80 characters)
- Use success for positive actions
- Use error for failures with optional recovery action
- Auto-dismiss for info/warning, but keep error longer
- Don't spam multiple toasts simultaneously

### 4. Error Handling
- Show error pages for critical failures (404, 500)
- Use error modals for non-critical issues
- Always provide a way to recover (back button, retry)
- Include support contact info on error pages

### 5. Animations
- All toasts slide in smoothly
- Skeleton cards pulse continuously
- Empty states fade in
- Keep animations subtle (300-500ms)

---

## Styling

All components use:
- **Glassmorphism**: Semi-transparent cards with backdrop blur
- **Neon Glow**: Primary color with text-shadow glow effect
- **Design Tokens**: CSS variables for consistent theming
- **Tailwind CSS 4**: Utility-first responsive styling
- **Dark Mode**: Default dark theme with light text

### Color Coding
- **Success**: Green (#10B981)
- **Error**: Red (#EF4444)
- **Info**: Blue (#3B82F6)
- **Warning**: Yellow (#F59E0B)

---

## Testing

To test error states:
1. **Empty State**: Filter movies by a genre with no results
2. **Error Page**: Navigate to `/movie/invalid-id`
3. **Loading State**: Discovery page shows skeletons for 800ms
4. **Toasts**: Click booking confirm button to see success toast

---

## Future Enhancements

- [ ] Add retry mechanisms for failed API calls
- [ ] Implement error boundary for component errors
- [ ] Add offline detection with appropriate messaging
- [ ] Create animated error illustrations
- [ ] Add progress indicators for long operations
- [ ] Implement undo functionality for actions
