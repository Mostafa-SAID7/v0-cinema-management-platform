# CinemaVerse Quick Reference Card

## 🚀 Start Development
```bash
npm install && npm start
# Navigate to http://localhost:4200
```

---

## 📚 Key Components

| Component | Path | Purpose |
|-----------|------|---------|
| Discovery | `features/discovery` | Movie browsing & search |
| Movie Details | `features/movie-details` | Movie info & booking |
| Booking | `features/booking` | Seat selection |
| Dashboard | `features/dashboard` | User tickets & profile |
| Support | `features/support` | FAQ & contact |
| Toast Container | `ui/toast-container` | Notifications |
| Error Page | `pages/error-page` | 404/500 errors |

---

## 🎯 Common Code Patterns

### Show Toast Notification
```typescript
import { ToastService } from '@/services/toast.service';

constructor(private toastService: ToastService) {}

this.toastService.success('Success!');
this.toastService.error('Error!');
this.toastService.info('Info');
this.toastService.warning('Warning');
```

### Add Loading State
```typescript
import { signal } from '@angular/core';

isLoading = signal(true);

ngOnInit() {
  setTimeout(() => this.isLoading.set(false), 1000);
}

// Template
<div *ngIf="isLoading()">
  <app-skeleton-card *ngFor="let i of [1,2,3,4]"></app-skeleton-card>
</div>
```

### Show Empty State
```html
<app-empty-state
  icon="🎬"
  title="No results"
  message="Try different search"
  actionLabel="Clear"
  (onAction)="clearSearch()"
></app-empty-state>
```

### Use Computed State
```typescript
import { signal, computed } from '@angular/core';

selectedGenre = signal('');

genreMovies = computed(() => 
  this.movieService.getMoviesByGenre(this.selectedGenre())
);
```

### Conditional Rendering with Fallback
```html
<ng-container *ngIf="items().length > 0; else empty">
  <div *ngFor="let item of items()">
    {{ item.name }}
  </div>
</ng-container>

<ng-template #empty>
  <app-empty-state
    icon="📭"
    title="No items"
  ></app-empty-state>
</ng-template>
```

### Show Error Page
```html
<app-error-page
  *ngIf="error()"
  statusCode="404"
  title="Not Found"
  message="Item doesn't exist"
></app-error-page>
```

---

## 🎨 Styling Classes

### Typography
```
text-4xl font-bold text-foreground      - Heading
text-lg text-muted-foreground           - Body text
text-xs font-medium                     - Small text
text-balance                            - Optimize text wrapping
```

### Colors
```
bg-primary text-primary-foreground      - Primary button
bg-secondary text-foreground            - Secondary
text-primary neon-text                  - Neon glow
border-border                           - Borders
```

### Layout
```
flex items-center justify-between       - Horizontal layout
grid grid-cols-3 md:grid-cols-4        - Responsive grid
space-y-4 gap-4                        - Spacing
p-6 md:p-8                             - Padding
```

### Cards & Effects
```
glassmorphism                           - Glass effect
rounded-lg                              - Border radius
hover:neon-glow                         - Hover glow
transition-smooth                       - Smooth animation
```

---

## 🔄 Service Injection

### ToastService
```typescript
import { ToastService } from '@/services/toast.service';
constructor(private toastService: ToastService) {}
```

### MovieService
```typescript
import { MovieService } from '@/services/movie.service';
constructor(private movieService: MovieService) {}
```

### AuthService
```typescript
import { AuthService } from '@/services/auth.service';
constructor(private authService: AuthService) {}
```

### BookingService
```typescript
import { BookingService } from '@/services/booking.service';
constructor(private bookingService: BookingService) {}
```

### ThemeService
```typescript
import { ThemeService } from '@/services/theme.service';
constructor(private themeService: ThemeService) {}
```

---

## 🎯 Signals & Computed

```typescript
// Signal - Mutable state
count = signal(0);
count.set(5);
count.update(v => v + 1);

// Computed - Derived state
doubled = computed(() => this.count() * 2);

// Effect - Side effects
effect(() => {
  console.log('Count changed:', this.count());
});
```

---

## 🔀 Routing

### Routes (app.routes.ts)
```typescript
{
  path: 'movie/:id',
  loadComponent: () => import('./movie-details').then(m => m.MovieDetailsComponent)
}
```

### Navigation
```typescript
constructor(private router: Router) {}

navigateToMovie(id: string) {
  this.router.navigate(['/movie', id]);
}
```

### Route Params
```typescript
constructor(private route: ActivatedRoute) {}

movieId = this.route.snapshot.paramMap.get('id');
```

---

## 🧪 Testing Checklist

- [ ] Toast appears and auto-dismisses
- [ ] Skeleton cards animate while loading
- [ ] Empty states show when no data
- [ ] Error pages display for 404/500
- [ ] Navigation works on all routes
- [ ] Mobile layout is responsive
- [ ] Dark mode applies correctly
- [ ] Keyboard navigation works

---

## 📱 Responsive Classes

```
hidden md:flex                   - Hide mobile, show tablet+
grid-cols-2 md:grid-cols-3      - 2 cols mobile, 3 cols tablet+
text-2xl md:text-4xl            - Responsive text sizes
p-4 md:p-6 lg:p-8               - Responsive padding
```

---

## 🎬 Component Lifecycle

```typescript
import { OnInit, OnDestroy } from '@angular/core';

export class MyComponent implements OnInit, OnDestroy {
  ngOnInit() {
    // Load data, show toast
  }
  
  ngOnDestroy() {
    // Cleanup, unsubscribe
  }
}
```

---

## 🔐 Auth Patterns

```typescript
// Check authentication
isAuthenticated = computed(() => 
  this.authService.isAuthenticated()
);

// Get current user
currentUser = computed(() => 
  this.authService.getCurrentUser()
);

// Logout
logout() {
  this.authService.logout();
}
```

---

## 🎥 Movie Data

```typescript
interface Movie {
  id: string;
  title: string;
  year: number;
  duration: number;
  rating: number;
  genre: string[];
  description: string;
  releaseDate: string;
}
```

---

## 🎫 Booking Data

```typescript
interface Booking {
  id: string;
  movieId: string;
  userId: string;
  showtime: string;
  seats: string[];
  totalPrice: number;
  date: string;
}
```

---

## 👤 User Data

```typescript
interface User {
  id: string;
  name: string;
  email: string;
  phone?: string;
  avatar?: string;
  preferences?: {
    favoriteGenres: string[];
    language: string;
  };
}
```

---

## 📝 Common Patterns

### Conditional with else
```html
<div *ngIf="condition; else fallback">
  Content
</div>
<ng-template #fallback>
  Fallback content
</ng-template>
```

### Loop with index
```html
<div *ngFor="let item of items(); let i = index">
  {{ i + 1 }}. {{ item.name }}
</div>
```

### Two-way binding (with component)
```typescript
// Component
@Input() value = '';
@Output() valueChange = output<string>();

onInput(newValue: string) {
  this.valueChange.emit(newValue);
}

// Parent
<app-input [value]="text" (valueChange)="text = $event"></app-input>
```

---

## 🚨 Error Handling

```typescript
try {
  await this.movieService.loadMovies().toPromise();
  this.toastService.success('Loaded successfully');
} catch (error) {
  this.toastService.error('Failed to load');
  console.error(error);
}
```

---

## 💾 Local Storage (if needed)

```typescript
// Save
localStorage.setItem('key', JSON.stringify(data));

// Load
const data = JSON.parse(localStorage.getItem('key') || '{}');

// Remove
localStorage.removeItem('key');
```

---

## 🔗 API Calls (Template)

```typescript
// In service
getMovies() {
  return fetch('/api/movies').then(r => r.json());
}

// In component
movies = signal<Movie[]>([]);

ngOnInit() {
  this.movieService.getMovies().then(movies => {
    this.movies.set(movies);
  });
}
```

---

## 📊 Data Flow

```
User Input → Component Signal
        ↓
    Computed State
        ↓
    Service Call
        ↓
    API Response
        ↓
    Update Signal
        ↓
    Template Re-render
```

---

## 🎨 Tailwind Utilities

```
m-4    - Margin 16px
p-4    - Padding 16px
gap-4  - Gap 16px
w-full - Width 100%
h-12   - Height 48px
flex   - Display flex
grid   - Display grid
```

---

## 🔔 Toast Examples

```typescript
// Success
this.toastService.success('Booking confirmed!', 4000);

// Error
this.toastService.error('Failed to load', 5000);

// Info
this.toastService.info('Loading...', 2000);

// Warning
this.toastService.warning('Will expire soon', 3000);
```

---

## 📱 Mobile First

Always design mobile first:
```html
<!-- Mobile (base) -->
<div class="text-2xl p-4 grid-cols-1">

<!-- Tablet (md:) -->
<div class="md:text-3xl md:p-6 md:grid-cols-2">

<!-- Desktop (lg:) -->
<div class="lg:text-4xl lg:p-8 lg:grid-cols-4">
```

---

## 🎯 Performance Tips

1. Use `computed()` for derived state
2. Use `OnPush` change detection
3. Lazy load routes
4. Unsubscribe in `ngOnDestroy`
5. Use `trackBy` in loops
6. Avoid memory leaks

---

## 🐛 Debugging

```typescript
// Log signal changes
effect(() => {
  console.log('State changed:', this.signal());
});

// Check component
constructor() {
  console.log('Component initialized');
}

// Inspect in browser
window.ng.getComponent(element);
```

---

## 📖 Documentation Files

- `README.md` - Project overview
- `SETUP.md` - Setup instructions
- `ERROR_HANDLING_GUIDE.md` - Error handling details
- `FIXES_AND_ENHANCEMENTS.md` - All fixes applied
- `STYLING_ENHANCEMENTS.md` - Styling guide
- `COMPREHENSIVE_GUIDE.md` - Full guide
- `QUICK_REFERENCE.md` - This file

---

**Last Updated**: March 2024
**Version**: 1.0.0
