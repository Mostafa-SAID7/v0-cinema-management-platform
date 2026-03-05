# CinemaVerse Styling & Visual Enhancements

## Design System Overview

### Color Palette
```
Primary:          #E50914 (Cinema Red) + neon-glow
Foreground:       #FFFFFF (White text)
Muted Foreground: #A3A3A3 (Gray text)
Background:       #0A0A0A (Very dark)
Secondary:        #1A1A1A (Dark gray)
Border:           #333333 (Dark border)
Card:             #121212 (Card background)

Success:          #10B981 (Green)
Error:            #EF4444 (Red)
Info:             #3B82F6 (Blue)
Warning:          #F59E0B (Yellow)
```

### Typography
- **Font Family**: Exo (modern, cinematic)
- **Font Weights**: 400 (regular), 500 (medium), 600 (semibold), 700 (bold)
- **Line Heights**: 1.4 (body), 1.5 (heading), 1.6 (comfortable reading)

### Spacing Scale
```
xs: 4px    (0.25rem)
sm: 8px    (0.5rem)
md: 16px   (1rem)
lg: 24px   (1.5rem)
xl: 32px   (2rem)
2xl: 48px  (3rem)
3xl: 64px  (4rem)
```

### Border Radius
- `sm`: 4px   - Small elements
- `md`: 8px   - Standard cards
- `lg`: 12px  - Large components
- `full`: 9999px - Pills, avatars

---

## Component Styling Details

### 1. Toast Container
```css
/* Position */
position: fixed;
bottom: 1rem (16px);
right: 1rem (16px);
z-index: 50;

/* Animation */
animation: slideInRight 0.3s ease-out;

/* Styling */
glassmorphism (backdrop-blur)
rounded-lg (8px)
px-4 py-3 (16px padding)
max-w-sm (448px)
shadow-lg

/* Color Variants */
Success:  bg-green-500/20 border-green-500 text-green-100
Error:    bg-red-500/20 border-red-500 text-red-100
Info:     bg-blue-500/20 border-blue-500 text-blue-100
Warning:  bg-yellow-500/20 border-yellow-500 text-yellow-100
```

### 2. Skeleton Card
```css
/* Layout */
aspect-video (fixed ratio)
rounded-lg (8px)
animate-pulse

/* Components */
- Image placeholder: full width, aspect-video
- Title skeleton: h-6 w-full
- Text skeleton: h-4 w-full (multiple)
- Footer area: flex gap-2

/* Animation */
Gradient shimmer: from-muted via-background to-muted
Pulse effect: opacity 0.5s infinite
```

### 3. Empty State Card
```css
/* Container */
glassmorphism
rounded-lg (8px)
p-12 (48px padding)
my-8 (32px margin top/bottom)

/* Content */
text-center
space-y-4 (gaps between elements)

/* Icon */
text-6xl
emoji or symbol

/* Text */
Title: text-xl font-bold
Message: text-muted-foreground

/* Action Button */
bg-primary text-primary-foreground
hover:bg-primary/90
px-6 py-2 (padding)
rounded-lg
transition-smooth
```

### 4. Error Page
```css
/* Layout */
h-screen (full viewport height)
flex items-center justify-center
bg-background

/* Container */
max-w-md (448px)
mx-auto
text-center
space-y-6 (gaps)

/* Status Code */
text-8xl font-bold
text-primary
neon-text (glow effect)

/* Content */
Title: text-4xl font-bold
Message: text-lg text-muted-foreground

/* Technical Details */
glassmorphism
p-4 (16px)
text-sm font-mono
text-red-400
break-words

/* Buttons */
flex gap-3
sm:flex-row
Primary: bg-primary text-primary-foreground
Secondary: border border-border hover:bg-secondary
rounded-lg px-6 py-3
```

### 5. Button Component
```css
/* Base Styles */
rounded-lg (8px)
font-medium
transition-smooth (200ms)
focus:outline-none focus:ring-2 focus:ring-primary

/* Variants */
Primary:
  bg-primary text-primary-foreground
  hover:bg-primary/90
  active:bg-primary/80
  disabled:opacity-50

Secondary:
  bg-secondary text-foreground
  hover:bg-secondary/80
  active:bg-secondary/70

Outline:
  border-2 border-border
  text-foreground
  hover:bg-secondary

Ghost:
  text-foreground
  hover:bg-secondary/50

/* Sizes */
sm: px-3 py-1.5 text-sm
md: px-4 py-2 text-base
lg: px-6 py-3 text-lg
```

### 6. Card Component
```css
/* Container */
glassmorphism (semi-transparent + blur)
rounded-lg (8px)
border border-border (1px)
p-6 (24px padding)

/* Hover Effect */
hover:bg-card/80
transition-smooth

/* Shadow */
shadow-lg
```

### 7. Input Component
```css
/* Container */
w-full
px-4 py-2 (padding)
rounded-lg (8px)
bg-input text-foreground
placeholder-muted-foreground
border border-border

/* Focus State */
focus:outline-none
focus:ring-2 focus:ring-primary
focus:border-transparent
transition-smooth

/* Disabled State */
disabled:opacity-50
disabled:cursor-not-allowed
```

### 8. Badge Component
```css
/* Base */
rounded-full (9999px)
px-3 py-1 (padding)
text-xs font-semibold
inline-block

/* Variants */
Primary:   bg-primary text-primary-foreground
Secondary: bg-secondary text-foreground
Outline:   border border-border text-foreground
Destructive: bg-red-500/20 text-red-100 border border-red-500
Success:   bg-green-500/20 text-green-100 border border-green-500
```

---

## Animation Details

### Slide In (Toast)
```css
@keyframes slideInRight {
  from {
    transform: translateX(400px);
    opacity: 0;
  }
  to {
    transform: translateX(0);
    opacity: 1;
  }
}

Duration: 0.3s (300ms)
Easing: ease-out
```

### Pulse (Skeleton)
```css
@keyframes pulse {
  0%, 100% {
    opacity: 1;
  }
  50% {
    opacity: 0.5;
  }
}

Duration: 2s (2000ms)
Iteration: infinite
Easing: cubic-bezier(0.4, 0, 0.6, 1)
```

### Fade In (Empty State)
```css
@keyframes fadeIn {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}

Duration: 0.3s (300ms)
Easing: ease-out
```

### Slide Out (Drawer)
```css
@keyframes slideInLeft {
  from {
    transform: translateX(-256px);
    opacity: 0;
  }
  to {
    transform: translateX(0);
    opacity: 1;
  }
}

Duration: 0.3s (300ms)
Easing: ease-out
```

### Neon Glow (Primary Buttons)
```css
text-shadow: 0 0 10px currentColor,
             0 0 20px rgba(229, 9, 20, 0.5);
filter: brightness(1.1);
```

---

## Glassmorphism Effect

### CSS Implementation
```css
.glassmorphism {
  background: rgba(255, 255, 255, 0.05);
  backdrop-filter: blur(10px);
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: 8px;
}

/* Tailwind Classes Used */
class: "glassmorphism"
class: "bg-card/80"
class: "border border-border"
class: "backdrop-blur"
```

### Usage
- Cards and panels
- Toast backgrounds
- Modals and dialogs
- Dropdown menus
- Sidebar on mobile

---

## Responsive Design

### Breakpoints
```
sm: 640px   - Mobile
md: 768px   - Tablet
lg: 1024px  - Desktop
xl: 1280px  - Large desktop
```

### Examples
```html
<!-- Text responsive -->
<h1 class="text-2xl md:text-3xl lg:text-4xl">
  Title

<!-- Grid responsive -->
<div class="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4">

<!-- Padding responsive -->
<div class="p-4 md:p-6 lg:p-8">

<!-- Display responsive -->
<aside class="hidden md:flex md:w-64">
```

---

## Dark Mode Implementation

### CSS Variables
```css
:root {
  --foreground: #FFFFFF;
  --background: #0A0A0A;
  --card: #121212;
  --primary: #E50914;
  --muted-foreground: #A3A3A3;
  --border: #333333;
}
```

### Tailwind Classes
```
text-foreground   - Primary text
bg-background     - Page background
bg-card          - Card background
text-primary     - Primary color
text-muted-foreground - Secondary text
border-border    - Border color
```

---

## State Indicators

### Loading States
- Skeleton pulse animations
- Shimmer effects
- Grayed out interactions

### Error States
- Red color (#EF4444)
- Error icon
- Descriptive message
- Help/support information

### Success States
- Green color (#10B981)
- Checkmark icon
- Confirmation message
- Auto-dismiss

### Warning States
- Yellow color (#F59E0B)
- Warning icon
- Caution message

### Disabled States
- Opacity: 0.5
- Cursor: not-allowed
- No hover effects

---

## Accessibility Features

### Color Contrast
- Text on background: 7:1+ ratio (WCAG AAA)
- Buttons: 4.5:1+ ratio (WCAG AA)
- Border colors: Sufficient contrast

### Focus Indicators
```css
focus:outline-none
focus:ring-2
focus:ring-primary
focus:ring-offset-2
focus:ring-offset-background
```

### Semantic HTML
```html
<button>      - Interactive elements
<a>           - Navigation
<main>        - Main content
<header>      - Top section
<nav>         - Navigation container
<section>     - Content section
<article>     - Standalone content
```

### ARIA Attributes
```html
aria-label="Close"
aria-expanded="false"
aria-hidden="true"
aria-live="polite"
role="alert"
```

---

## Print Styles

```css
@media print {
  .no-print {
    display: none;
  }
  
  body {
    background: white;
    color: black;
  }
  
  .glassmorphism {
    background: transparent;
    border: 1px solid #ccc;
  }
}
```

---

## Performance Optimizations

### CSS
- Minimal use of gradients
- Efficient transitions (GPU-accelerated)
- Reusable utility classes
- Mobile-first approach

### Images
- Lazy loading with `loading="lazy"`
- Responsive images with `srcset`
- Optimized formats (WebP with fallback)

### Animations
- 60fps target
- Hardware acceleration with `transform`
- Debounced scroll/resize handlers
- Smooth frame rates

---

## Component Showcase

### Toast Example
```
┌─────────────────────────────────┐
│ ✓ Success Message               │ ✕
│   Smooth slide-in animation     │
│   Auto-dismisses after 3 seconds│
└─────────────────────────────────┘
```

### Skeleton Example
```
┌─────────────────────┐
│ ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓ │
│ ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓ │
│                     │
│ ▓▓▓▓▓▓▓ ▓▓▓▓▓▓▓▓ │
│ ▓▓▓▓▓   ▓▓▓▓▓▓  │
└─────────────────────┘
```

### Empty State Example
```
        🎬
        
No movies found

We couldn't find any movies in 
the Action genre.

┌──────────────────────┐
│    Browse All        │
└──────────────────────┘
```

### Error Page Example
```
        404
        
Movie Not Found

The movie you're looking for 
doesn't exist or has been removed.

┌──────────────────┐┌──────────────────┐
│  Back to Home    ││   Go Back        │
└──────────────────┘└──────────────────┘

support@cinemaverse.com
```

---

## Browser Support

- ✅ Chrome 90+
- ✅ Firefox 88+
- ✅ Safari 14+
- ✅ Edge 90+
- ⚠️  IE 11 (graceful degradation)

---

## Future Styling Enhancements

- [ ] Dark mode toggle
- [ ] Custom theme selector
- [ ] Reduced motion preferences
- [ ] Font size scaling
- [ ] High contrast mode
- [ ] Animated illustrations
- [ ] Gradient overlays
- [ ] Particle effects
