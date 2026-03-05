# CinemaVerse - Setup & Development Guide

## Quick Start

### 1. Install Dependencies
```bash
pnpm install
```

### 2. Start Development Server
```bash
pnpm start
```

The app will run on `http://localhost:4200` with HMR (Hot Module Replacement) enabled.

### 3. Open in Browser
Navigate to `http://localhost:4200` and you should see the CinemaVerse discovery page.

## Project Commands

### Development
```bash
pnpm start              # Start dev server (ng serve)
pnpm watch              # Build in watch mode (ng build --watch)
```

### Production
```bash
pnpm build              # Production build (ng build)
```

### Testing (Ready to implement)
```bash
pnpm test               # Run unit tests (ng test)
pnpm lint               # Lint code (ng lint)
```

## Environment Setup

### Create `.env` file (optional)
```env
NG_APP_API_BASE_URL=https://api.example.com
NG_APP_ENVIRONMENT=development
```

### Configuration Files

**angular.json** - Angular CLI configuration
- Build options
- Serve options
- Test configuration

**tsconfig.json** - TypeScript configuration
- Compiler options
- Lib and module settings
- Strict mode enabled

**tailwind.config.ts** - Tailwind CSS configuration
- Custom design tokens
- Color palette
- Animations and keyframes

**postcss.config.js** - PostCSS configuration
- Tailwind CSS plugin
- Autoprefixer

## Folder Structure Explained

### `/src/app/`
Main application code organized by feature:
- **components/** - UI and feature components
  - **shared/** - Layout, navbar, sidebar (shared across app)
  - **ui/** - Reusable component library (button, card, input, etc.)
  - **features/** - Feature-specific components (discovery, booking, etc.)
- **services/** - Business logic and API integration
- **signals/** - Global state using Angular Signals

### `/src/styles/`
CSS stylesheets:
- **globals.css** - Global styles, design tokens, animations

### `/src/`
Entry point files:
- **main.ts** - Bootstrap the Angular app
- **index.html** - HTML template

## Adding New Features

### 1. Create a New Feature Component
```bash
ng generate component components/features/feature-name --skip-tests
```

This creates a standalone component in the features folder.

### 2. Create a Feature Service
```bash
ng generate service services/feature-name --skip-tests
```

### 3. Add Route
Update `app.routes.ts`:
```typescript
{
  path: 'feature-path',
  loadComponent: () =>
    import('./components/features/feature-name/feature-name.component').then(
      (m) => m.FeatureNameComponent
    ),
},
```

### 4. Update Navigation
Add link in `sidebar.component.ts`:
```typescript
<a routerLink="/feature-path" routerLinkActive="bg-primary">
  Feature Name
</a>
```

## Working with Signals

### Creating a Signal
```typescript
import { signal } from '@angular/core';

// Basic signal
const count = signal(0);

// Update signal
count.set(5);
count.update(val => val + 1);

// Read signal value
console.log(count());
```

### Creating a Computed Signal
```typescript
import { signal, computed } from '@angular/core';

const count = signal(0);
const doubled = computed(() => count() * 2);
```

### Using Signals in Templates
```typescript
// Component
totalPrice = signal(0);

// Template
<p>{{ totalPrice() }}</p>
```

## Working with Tailwind CSS

### Design Tokens
Custom colors defined in CSS variables (globals.css):
```css
:root {
  --primary: 16 87% 54%;       /* Cinema Red */
  --background: 0 0% 6%;       /* Dark background */
  --border: 0 0% 20%;
}
```

Usage in Tailwind:
```html
<button class="bg-primary text-primary-foreground">
  Click me
</button>
```

### Custom Animations
Available custom animations (defined in tailwind.config.ts):
- `animate-pulse-neon` - Neon pulse effect
- `animate-float` - Floating animation
- `animate-slide-in-left` - Slide from left
- `animate-slide-in-up` - Slide from bottom
- `animate-scale-in` - Scale from small

### Responsive Classes
```html
<div class="text-sm md:text-base lg:text-lg">
  Responsive text
</div>

<div class="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
  Grid that changes columns based on screen size
</div>
```

## API Integration

### Connect to ASP.NET Core Backend

1. Update API base URL in `api.service.ts`:
```typescript
private baseUrl = 'https://your-api.example.com';
```

2. Implement HTTP calls in services:
```typescript
import { HttpClient } from '@angular/common/http';

constructor(private http: HttpClient) {}

getMovies() {
  return this.http.get<Movie[]>(`${this.baseUrl}/api/movies`);
}
```

3. Add HttpClientModule to main.ts (or individual components):
```typescript
import { HttpClientModule } from '@angular/common/http';

bootstrapApplication(AppComponent, {
  providers: [
    provideHttpClient(),
    // other providers
  ]
});
```

## Authentication Flow

1. **User logs in** via login form
2. **AuthService.login()** sends credentials to backend
3. **Backend returns JWT token**
4. **Store token** in localStorage or session storage
5. **Add token to HTTP headers** via interceptor
6. **AuthSignal updates** with user info
7. **UI updates** based on auth state

See `auth.service.ts` and `auth.signal.ts` for implementation.

## Troubleshooting

### Port 4200 Already in Use
```bash
ng serve --port 4300
```

### Tailwind CSS Not Working
1. Check `tailwind.config.ts` has correct `content` paths
2. Rebuild: `pnpm start`
3. Clear cache: `rm -rf node_modules/.vite` (if using Vite)

### Styles Not Updating
1. Check styles are in `globals.css`
2. Restart dev server
3. Clear browser cache (Cmd+Shift+R or Ctrl+Shift+R)

### Components Not Found
1. Verify import paths are correct
2. Check component selector name matches
3. Ensure component is standalone: `standalone: true`

## Performance Tips

1. **Use OnPush Change Detection**: Faster rendering
   ```typescript
   @Component({
     changeDetection: ChangeDetectionStrategy.OnPush,
   })
   ```

2. **Lazy Load Routes**: Reduce initial bundle
   ```typescript
   loadComponent: () => import('./path').then(m => m.Component)
   ```

3. **Use Computed Signals**: Memoize expensive calculations
   ```typescript
   derived = computed(() => calculate(this.source()));
   ```

4. **Unsubscribe from Observables**: Prevent memory leaks
   ```typescript
   private destroy$ = new Subject<void>();
   ngOnDestroy() { this.destroy$.next(); }
   ```

## Common Patterns

### Reactive Form
```typescript
import { ReactiveFormsModule, FormBuilder } from '@angular/forms';

@Component({
  imports: [ReactiveFormsModule],
})
export class MyComponent {
  form = this.fb.group({
    email: ['', Validators.required],
    password: ['', Validators.required],
  });

  constructor(private fb: FormBuilder) {}
}
```

### Async Data Loading
```typescript
movies = computed(() => {
  const genre = this.selectedGenre();
  return this.movieService.getMoviesByGenre(genre);
});
```

### Event Binding
```typescript
<button (click)="handleClick()">Click</button>
<input (input)="handleInput($event)" />
<form (submit)="handleSubmit()">
```

## Resources

- [Angular Docs](https://angular.io/docs)
- [Tailwind CSS Docs](https://tailwindcss.com/docs)
- [TypeScript Docs](https://www.typescriptlang.org/docs)
- [Angular Signals Guide](https://angular.io/guide/signals)

## Support

Need help? Check:
1. Console logs in browser DevTools
2. Network tab for API calls
3. Angular DevTools extension
4. README.md for project overview

---

Happy coding! 🎬
