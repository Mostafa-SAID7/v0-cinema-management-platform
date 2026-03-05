import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastService } from '../../services/toast.service';

@Component({
  selector: 'app-toast-container',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="fixed bottom-4 right-4 z-50 flex flex-col gap-2 pointer-events-none">
      <div
        *ngFor="let toast of toastService.toasts()"
        [@slideInOut]
        class="pointer-events-auto animate-slide-in-right"
        [ngClass]="{
          'bg-green-500/20 border border-green-500 text-green-100': toast.type === 'success',
          'bg-red-500/20 border border-red-500 text-red-100': toast.type === 'error',
          'bg-blue-500/20 border border-blue-500 text-blue-100': toast.type === 'info',
          'bg-yellow-500/20 border border-yellow-500 text-yellow-100': toast.type === 'warning',
        }"
        class="rounded-lg px-4 py-3 backdrop-blur-sm max-w-sm shadow-lg flex items-center justify-between gap-4"
      >
        <div class="flex items-center gap-3">
          <span *ngIf="toast.type === 'success'" class="text-xl">✓</span>
          <span *ngIf="toast.type === 'error'" class="text-xl">✕</span>
          <span *ngIf="toast.type === 'info'" class="text-xl">ℹ</span>
          <span *ngIf="toast.type === 'warning'" class="text-xl">⚠</span>
          <p class="text-sm font-medium">{{ toast.message }}</p>
        </div>
        <button
          (click)="toastService.dismiss(toast.id)"
          class="text-current opacity-70 hover:opacity-100 transition-opacity"
        >
          ✕
        </button>
      </div>
    </div>
  `,
  styles: [
    `
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

      .animate-slide-in-right {
        animation: slideInRight 0.3s ease-out;
      }
    `,
  ],
})
export class ToastContainerComponent {
  toastService = inject(ToastService);
}
