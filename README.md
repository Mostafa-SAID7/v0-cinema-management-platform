# CinemaVerse - Premium Cinema Discovery & Booking Platform

## Overview

CinemaVerse is a modern, feature-rich cinema discovery and booking platform built with **Angular 19**, showcasing best practices in web development with Standalone Components, Signals for state management, and a beautiful dark-themed UI with neon accents.

## Key Features

### 🎬 Core Features
- **Movie Discovery Hub**: Browse and search movies by title, genre, and ratings
- **Trending & Coming Soon**: Auto-updating sections for trending movies and upcoming releases
- **Movie Details**: Comprehensive movie information with cast, reviews, and ratings
- **Smart Booking System**: Interactive seat selection with real-time availability
- **User Dashboard**: Manage tickets, bookings, and favorite movies
- **Support & FAQ**: Comprehensive help center with CinemaBot AI assistant
- **Authentication**: User sign-in/registration with persistent sessions

### 🎨 Design Highlights
- **Dark Mode Design**: Cinema-inspired dark theme with Cinema Red (#E50914) accents
- **Glassmorphism**: Modern frosted glass effects throughout the UI
- **Neon Glow Effects**: Eye-catching red neon shadows on interactive elements
- **Responsive Layout**: Adaptive design for mobile, tablet, and desktop screens
- **Micro-interactions**: Smooth animations and transitions for better UX
- **Accessibility**: WCAG compliant with keyboard navigation support

### 🏗️ Technical Architecture
- **Angular 19**: Latest Angular framework with Standalone Components
- **Signals**: Modern reactive state management (theme, auth, bookings)
- **Tailwind CSS 4**: Utility-first styling with custom design tokens
- **Responsive Sidebar/Drawer**: Persistent sidebar on desktop, mobile drawer on smaller screens
- **Modular Components**: Reusable UI and feature components
- **Service Layer**: Clean separation of concerns with dedicated services

## Project Structure

```
src/
├── app/
│   ├── app.component.ts           # Root component
│   ├── app.routes.ts              # Route configuration
│   ├── components/
│   │   ├── shared/                # Shared layout components
│   │   │   ├── main-layout/
│   │   │   ├── navbar/
│   │   │   └── sidebar/
│   │   ├── ui/                    # Reusable UI components
│   │   │   ├── button.component.ts
│   │   │   ├── card.component.ts
│   │   │   ├── input.component.ts
│   │   │   ├── badge.component.ts
│   │   │   ├── dialog.component.ts
│   │   │   ├── accordion.component.ts
│   │   │   ├── tabs.component.ts
│   │   │   └── carousel.component.ts
│   │   └── features/              # Feature modules
│   │       ├── discovery/         # Movie discovery
│   │       ├── movie-details/     # Movie information
│   │       ├── booking/           # Seat selection & booking
│   │       ├── dashboard/         # User tickets & profile
│   │       └── support/           # Help & FAQ
│   ├── services/                  # Business logic
│   │   ├── api.service.ts         # API endpoints (ASP.NET Core integration)
│   │   ├── auth.service.ts        # Authentication
│   │   ├── movie.service.ts       # Movie data & caching
│   │   ├── booking.service.ts     # Booking management
│   │   └── theme.service.ts       # Theme management
│   ├── signals/                   # Global state (Signals)
│   │   └── auth.signal.ts         # Auth state
│   └── styles/
│       └── globals.css            # Global styles & design tokens
├── styles/
│   └── globals.css                # Tailwind imports
├── index.html
├── main.ts
└── favicon.ico
```

## Getting Started

### Prerequisites
- Node.js 18+ and npm/pnpm
- Angular CLI 19

### Installation

1. **Install dependencies**:
   ```bash
   pnpm install
   ```

2. **Start the development server**:
   ```bash
   pnpm start
   ```

3. **Open in browser**:
   Navigate to `http://localhost:4200`

### Building for Production

```bash
pnpm build
```

Output will be in `dist/cinemaverse`

## Key Technologies

- **Framework**: Angular 19 with Standalone Components
- **State Management**: Angular Signals
- **Styling**: Tailwind CSS 4 + Custom CSS
- **Icons**: Lucide Angular
- **HTTP**: Angular HttpClient (ready for ASP.NET Core integration)
- **Forms**: Angular Reactive Forms
- **Routing**: Angular Router with lazy loading

## Component Architecture

### Standalone Components
All components are standalone, enabling:
- Better tree-shaking
- Simpler testing
- Faster development
- Easier code organization

### Component Hierarchy
```
AppComponent
└── MainLayoutComponent
    ├── SidebarComponent
    ├── NavbarComponent
    └── Feature Routes
        ├── DiscoveryComponent
        ├── MovieDetailsComponent
        ├── BookingComponent
        ├── DashboardComponent
        └── SupportComponent
```

## State Management with Signals

### Theme Signal
```typescript
theme = signal<'light' | 'dark'>('dark');
```

### Auth Signal
```typescript
authSignal = signal({
  isAuthenticated: boolean;
  user: User | null;
  loading: boolean;
  error: string | null;
});
```

### Booking Signal
```typescript
selectedSeats = signal<string[]>([]);
currentShowtime = signal<string>('');
```

## API Integration

The app is designed to integrate with an **ASP.NET Core backend**. Example endpoints:

```
GET    /api/movies
GET    /api/movies/{id}
GET    /api/movies/{id}/showtimes
POST   /api/bookings
GET    /api/user/bookings
POST   /api/auth/login
POST   /api/auth/register
```

See `ApiService` for endpoint definitions.

## Design System

### Color Palette
- **Primary**: Cinema Red (#E50914) - `bg-primary`
- **Dark Background**: #0A0A0A - `bg-background`
- **Card Background**: #121212 - `bg-card`
- **Text**: #E5E7EB (light mode) - `text-foreground`
- **Muted**: #4B5563 - `text-muted-foreground`

### Typography
- **Font**: Exo (Google Fonts)
- **Body**: 16px, line-height: 1.6
- **Headings**: 24px-60px, font-weight: 700-900

### Effects
- **Neon Glow**: `box-shadow: 0px 0px 20px rgba(239, 68, 68, 0.6)`
- **Glassmorphism**: `backdrop-filter: blur(10px)`
- **Transitions**: 300ms ease-in-out

## Responsive Breakpoints
- **Mobile**: < 768px (full-width drawer navigation)
- **Tablet**: 768px - 1024px (collapsible sidebar)
- **Desktop**: > 1024px (persistent sidebar)

## Performance Optimizations

1. **Lazy Loading**: Feature routes lazy-load their components
2. **Change Detection**: OnPush strategy on most components
3. **Tree-shaking**: Standalone components + modular imports
4. **CSS-in-JS**: Minimal critical CSS in globals.css
5. **Signal Memoization**: Computed signals prevent unnecessary re-renders

## Testing

(Ready for implementation)

```bash
pnpm test
```

Example test files to add:
- `app.component.spec.ts`
- `discovery.component.spec.ts`
- `movie.service.spec.ts`

## Accessibility Features

- Semantic HTML (`<header>`, `<nav>`, `<main>`, etc.)
- ARIA labels on interactive elements
- Focus management and keyboard navigation
- Color contrast compliance (WCAG AA)
- Screen reader support with `.sr-only` class

## Future Enhancements

1. **Payment Integration**: Stripe/Razorpay for ticket payments
2. **Real-time Features**: WebSocket for live seat availability
3. **Advanced Analytics**: User behavior tracking & recommendations
4. **Mobile App**: React Native or Flutter version
5. **Admin Dashboard**: Theater & showtime management
6. **Notifications**: Email/SMS booking confirmations
7. **Social Features**: Movie reviews, ratings, recommendations
8. **VR Experience**: 3D cinema preview with Three.js

## Browser Support

- Chrome/Edge 120+
- Firefox 121+
- Safari 17+
- Mobile browsers (iOS Safari 17+, Chrome Android)

## License

Proprietary - CinemaVerse Experience Platform

## Support

For issues, features, or questions:
- Email: support@cinemaverse.com
- Phone: 1-800-123-456
- Hours: Mon-Sun, 9 AM - 11 PM

---

**Built with ❤️ using Angular 19, Tailwind CSS 4, and modern web standards.**
