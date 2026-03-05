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
    <div class="relative">
      <!-- Carousel Container -->
      <div class="overflow-hidden rounded-lg bg-secondary relative h-96 md:h-[500px]">
        <div class="flex transition-transform duration-500" [style.transform]="'translateX(-' + currentIndex() * 100 + '%)'">
          <ng-content></ng-content>
        </div>

        <!-- Overlay Gradient -->
        <div class="absolute inset-0 bg-gradient-to-b from-transparent to-background"></div>
      </div>

      <!-- Navigation Buttons -->
      <button
        (click)="previous()"
        class="absolute left-4 top-1/2 -translate-y-1/2 z-20 p-2 rounded-full bg-black/50 hover:bg-black/70 text-white transition-smooth"
      >
        <span class="text-2xl">‹</span>
      </button>

      <button
        (click)="next()"
        class="absolute right-4 top-1/2 -translate-y-1/2 z-20 p-2 rounded-full bg-black/50 hover:bg-black/70 text-white transition-smooth"
      >
        <span class="text-2xl">›</span>
      </button>

      <!-- Indicators -->
      <div class="flex justify-center gap-2 mt-4">
        <button
          *ngFor="let i of getIndicators()"
          (click)="goToSlide(i)"
          [class.bg-primary]="currentIndex() === i"
          [class.bg-muted]="currentIndex() !== i"
          class="w-2 h-2 rounded-full transition-smooth"
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
    <div class="w-full h-full flex-shrink-0 flex items-center justify-center bg-secondary">
      <ng-content></ng-content>
    </div>
  `,
})
export class CarouselItemComponent {}
