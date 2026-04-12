# Troubleshooting Guide

## Common Issues & Solutions

### 1. "Port 4200 is not reachable (connect ECONNREFUSED)"

**Cause**: Angular dev server failed to start due to compilation errors.

**Solutions**:
```bash
# Clear node_modules and reinstall
rm -rf node_modules package-lock.json
npm install

# Clear Angular cache
rm -rf .angular/cache

# Start dev server with verbose logging
ng serve --verbose

# Check for TypeScript errors
ng build
```

---

### 2. "Tailwind CSS Classes Not Applying"

**Cause**: `@theme` directive references undefined CSS variables.

**Solution**: Verify `src/styles/globals.css` structure:
```css
/* ✅ CORRECT ORDER */
:root { --background: 0 0% 100%; }
@theme { --color-background: hsl(var(--background)); }

/* ❌ WRONG ORDER (will fail) */
@theme { --color-background: hsl(var(--background)); }
:root { --background: 0 0% 100%; }
```

---

### 3. "Class Not Found" - bg-sidebar-primary, etc.

**Cause**: Sidebar color tokens not registered.

**Check**: Verify all `--color-sidebar-*` variables are defined in both `:root` and `.dark`:
- `--sidebar-background`
- `--sidebar-foreground`
- `--sidebar-primary`
- `--sidebar-primary-foreground`
- `--sidebar-accent`
- `--sidebar-accent-foreground`
- `--sidebar-border`
- `--sidebar-ring`

---

### 4. "Module not found" for UI Components

**Cause**: Component not exported from `src/app/components/ui/index.ts`.

**Solution**: Ensure barrel export includes all 12 components:
```typescript
export { ButtonComponent } from './button.component';
export { CardComponent } from './card.component';
// ... (all 12 components listed)
```

---

### 5. Dark Mode Not Working

**Cause**: Theme service not initializing or theme class not applied.

**Check**:
```typescript
// In app.component.ts, verify ThemeService is injected
constructor(private themeService: ThemeService) {
  this.themeService.initializeTheme();
}
```

**Verify**: `<html class="dark">` is set when dark mode active:
```bash
# Open browser DevTools → Elements
# Should see: <html lang="en" class="dark">
```

---

### 6. Animations Not Playing

**Cause**: Keyframes not registered or `prefers-reduced-motion` media query suppressing animation.

**Solution**:
1. Check `src/styles/globals.css` has `@keyframes` definitions
2. Browser DevTools → Settings → Rendering → Uncheck "Emulate CSS media feature prefers-reduced-motion"

---

### 7. Build Size Too Large

**Cause**: Unused Tailwind utilities included or components not properly imported.

**Solution**:
```bash
# Analyze bundle
ng build --configuration production --stats-json
npm install -g webpack-bundle-analyzer
webpack-bundle-analyzer dist/cinemaverse/stats.json
```

**Actions**:
- Remove unused UI components from imports
- Use `CommonModule` only where needed
- Lazy-load route components

---

### 8. Console Error: "Cannot read property 'classList'"

**Cause**: `ThemeService.initializeTheme()` called on non-DOM (SSR context).

**Solution**: Add null check in theme service:
```typescript
initializeTheme(): void {
  if (!this.isBrowser()) return;
  const htmlElement = document.documentElement;
  // ...
}

private isBrowser(): boolean {
  return typeof document !== 'undefined';
}
```

---

### 9. Image Not Loading - CORS Error

**Cause**: Image URL from external API without proper CORS headers.

**Solution**: In component using images:
```html
<img 
  [src]="movie.posterUrl" 
  alt="{{ movie.title }}"
  crossorigin="anonymous"
  loading="lazy"
/>
```

---

### 10. Search Results Dropdown Hidden Behind Other Elements

**Cause**: `z-index` not high enough or parent has `overflow: hidden`.

**Solution**:
```html
<!-- Add z-10 to dropdown -->
<div class="absolute top-full left-0 right-0 z-10 ...">
  <!-- Results -->
</div>

<!-- Or use absolute positioning outside parent -->
<div class="relative">
  <input/>
  <div class="fixed ..."><!-- Results at fixed position --></div>
</div>
```

---

### 11. Responsive Design Not Working on Mobile

**Cause**: Missing viewport meta tag or CSS breakpoints not configured.

**Check**: `src/index.html` has:
```html
<meta name="viewport" content="width=device-width, initial-scale=1, viewport-fit=cover">
```

**Verify**: Components use Tailwind responsive prefixes:
```html
<!-- ✅ CORRECT -->
<div class="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4">

<!-- ❌ WRONG -->
<div class="grid-cols-4">
```

---

### 12. Font Not Loading - Exo Font Missing

**Cause**: Google Fonts import failed or self-hosted font not found.

**Check**: Browser DevTools → Network tab → search "Exo"

**Fallback**: Add system fonts in CSS:
```css
body {
  font-family: 'Exo', system-ui, -apple-system, 'Segoe UI', Roboto, sans-serif;
}
```

---

## Performance Debugging

### Check Core Web Vitals
```bash
# Use Lighthouse in Chrome DevTools
# Command + Option + I (Mac) or Ctrl + Shift + I (Windows)
# → Lighthouse → Generate Report
```

### Profile Performance
```bash
# DevTools → Performance → Record
# Perform action (e.g., search, click button)
# Analyze: Look for long tasks, jank, layout shifts
```

### Check Bundle Size
```bash
ng build --configuration production
# Check dist/cinemaverse/ size
# Should be <500KB main JS for optimal performance
```

---

## Clean Rebuild Commands

```bash
# Complete clean build
rm -rf node_modules .angular dist
npm install
ng build --configuration production

# Local dev server
ng serve --open

# Run tests
ng test

# Build and analyze
ng build --configuration production --stats-json
```

---

## Getting Help

1. **Check logs**: Look at Terminal output for exact error message
2. **Google error**: Search the exact error message
3. **Check Angular docs**: https://angular.io
4. **Check Tailwind docs**: https://tailwindcss.com
5. **GitHub Issues**: Search repository issues section

---

**Last Updated**: 2026-04-12
