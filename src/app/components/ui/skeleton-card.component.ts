import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SkeletonComponent } from './skeleton.component';

@Component({
  selector: 'app-skeleton-card',
  standalone: true,
  imports: [CommonModule, SkeletonComponent],
  template: `
    <div class="rounded-xl border bg-card text-card-foreground shadow overflow-hidden animate-pulse">
      <!-- Image Skeleton -->
      <div class="aspect-video bg-primary/10 rounded-t-xl"></div>

      <!-- Content Skeleton -->
      <div class="p-6 space-y-3">
        <!-- Title -->
        <app-skeleton size="lg"></app-skeleton>

        <!-- Description Lines -->
        <div class="space-y-2">
          <app-skeleton size="md"></app-skeleton>
          <app-skeleton size="sm"></app-skeleton>
        </div>

        <!-- Footer -->
        <div class="pt-2 flex gap-2">
          <app-skeleton size="md" class="flex-1"></app-skeleton>
          <app-skeleton size="md" class="w-12"></app-skeleton>
        </div>
      </div>
    </div>
  `,
})
export class SkeletonCardComponent {}
