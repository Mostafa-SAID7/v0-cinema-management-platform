import { Routes } from '@angular/router';
import { MainLayoutComponent } from './components/shared/main-layout/main-layout.component';

export const routes: Routes = [
  {
    path: '',
    component: MainLayoutComponent,
    children: [
      {
        path: '',
        loadComponent: () =>
          import('./components/features/discovery/discovery.component').then(
            (m) => m.DiscoveryComponent
          ),
      },
      {
        path: 'movie/:id',
        loadComponent: () =>
          import('./components/features/movie-details/movie-details.component').then(
            (m) => m.MovieDetailsComponent
          ),
      },
      {
        path: 'booking/:movieId',
        loadComponent: () =>
          import('./components/features/booking/booking.component').then(
            (m) => m.BookingComponent
          ),
      },
      {
        path: 'dashboard',
        loadComponent: () =>
          import('./components/features/dashboard/dashboard.component').then(
            (m) => m.DashboardComponent
          ),
      },
      {
        path: 'support',
        loadComponent: () =>
          import('./components/features/support/support.component').then(
            (m) => m.SupportComponent
          ),
      },
    ],
  },
];
