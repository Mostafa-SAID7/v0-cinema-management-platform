import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-skeleton',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div
      [ngClass]="{
        'h-4 w-full': size() === 'sm',
        'h-6 w-full': size() === 'md',
        'h-8 w-full': size() === 'lg',
        'h-12 w-full': size() === 'xl',
        'aspect-video w-full': size() === 'video',
        'w-10 h-10 rounded-full': size() === 'avatar',
      }"
      class="animate-pulse rounded-md bg-primary/10"
    ></div>
  `,
})
export class SkeletonComponent {
  size = input<'sm' | 'md' | 'lg' | 'xl' | 'video' | 'avatar'>('md');
}
