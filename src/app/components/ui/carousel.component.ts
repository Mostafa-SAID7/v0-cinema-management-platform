import { Component, input, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface CarouselItem {
  id: string;
  title: string;
  image?: string;
  content?: any;
}

@Component({
  selector: 'app-carousel',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="relative" role="region" aria-roledescription="carousel">
      <!-- Carousel Container -->
      <div class="overflow-hidden rounded-xl border bg-card shadow">
        <div
          class="flex transition-transform duration-500 ease-in-out"
          [style.transform]="'translateX(-' + currentIndex() * 100 + '%)'"
        >
          <ng-content></ng-content>
        </div>
      </div>

      <!-- Previous Button -->
      <button
        (click)="previous()"
        class="absolute left-4 top-1/2 -translate-y-1/2 z-20 inline-flex h-9 w-9 items-center justify-center rounded-md border border-input bg-background shadow-sm hover:bg-accent hover:text-accent-foreground transition-colors"
        aria-label="Previous slide"
      >
        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
          <path d="m15 18-6-6 6-6"/>
        </svg>
      </button>

      <!-- Next Button -->
      <button
        (click)="next()"
        class="absolute right-4 top-1/2 -translate-y-1/2 z-20 inline-flex h-9 w-9 items-center justify-center rounded-md border border-input bg-background shadow-sm hover:bg-accent hover:text-accent-foreground transition-colors"
        aria-label="Next slide"
      >
        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
          <path d="m9 18 6-6-6-6"/>
        </svg>
      </button>

      <!-- Indicators -->
      <div class="flex justify-center gap-1.5 mt-3">
        <button
          *ngFor="let i of getIndicators()"
          (click)="goToSlide(i)"
          [ngClass]="{
            'bg-primary': currentIndex() === i,
            'bg-muted-foreground/30': currentIndex() !== i
          }"
          class="h-1.5 w-1.5 rounded-full transition-colors"
          [attr.aria-label]="'Go to slide ' + (i + 1)"
        ></button>
      </div>
    </div>
  `,
})
export class CarouselComponent implements OnInit {
  currentIndex = signal(0);
  autoPlay = input(true);
  autoPlayInterval = input(5000);
  items = input<CarouselItem[]>([]);

  ngOnInit(): void {
    if (this.autoPlay()) {
      setInterval(() => this.next(), this.autoPlayInterval());
    }
  }

  next(): void {
    const maxIndex = this.items().length - 1;
    this.currentIndex.update((current) => (current === maxIndex ? 0 : current + 1));
  }

  previous(): void {
    const maxIndex = this.items().length - 1;
    this.currentIndex.update((current) => (current === 0 ? maxIndex : current - 1));
  }

  goToSlide(index: number): void {
    this.currentIndex.set(index);
  }

  getIndicators(): number[] {
    return Array.from({ length: this.items().length }, (_, i) => i);
  }
}

@Component({
  selector: 'app-carousel-item',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="w-full h-full flex-shrink-0 flex items-center justify-center" role="group" aria-roledescription="slide">
      <ng-content></ng-content>
    </div>
  `,
})
export class CarouselItemComponent {}
