import { Component, signal, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ToastService } from '../../../services/toast.service';
import { InputComponent } from '../../ui/input.component';

@Component({
  selector: 'app-support',
  standalone: true,
  imports: [CommonModule, FormsModule, InputComponent],
  template: `
    <div class="p-6 md:p-8 space-y-8">
      <!-- Header -->
      <div class="space-y-4">
        <h1 class="text-4xl font-bold text-foreground">Help & Support</h1>
        <p class="text-lg text-muted-foreground max-w-xl">
          Find answers to common questions or get in touch with our support team.
        </p>
      </div>

      <div class="grid grid-cols-1 lg:grid-cols-3 gap-8">
        <!-- FAQ Section -->
        <div class="lg:col-span-2 space-y-4">
          <h2 class="text-2xl font-bold text-foreground">Frequently Asked Questions</h2>

          <div>
            <div *ngFor="let faq of faqs; let i = index" class="border-b">
              <h3 class="flex">
                <button
                  (click)="toggleFaq(i)"
                  class="flex flex-1 items-center justify-between py-4 text-sm font-medium transition-all hover:underline"
                  [attr.aria-expanded]="expandedFaq() === i"
                >
                  <span>{{ faq.question }}</span>
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    width="16"
                    height="16"
                    viewBox="0 0 24 24"
                    fill="none"
                    stroke="currentColor"
                    stroke-width="2"
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    class="shrink-0 text-muted-foreground transition-transform duration-200"
                    [class.rotate-180]="expandedFaq() === i"
                  >
                    <path d="m6 9 6 6 6-6"/>
                  </svg>
                </button>
              </h3>

              <div
                *ngIf="expandedFaq() === i"
                class="overflow-hidden text-sm animate-accordion-down"
              >
                <div class="pb-4 pt-0">
                  <p class="text-muted-foreground leading-relaxed">{{ faq.answer }}</p>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Contact Section -->
        <div class="lg:col-span-1 space-y-6">
          <!-- Contact Info Card -->
          <div class="rounded-xl border bg-card text-card-foreground shadow">
            <div class="flex flex-col space-y-1.5 p-6">
              <h3 class="font-semibold leading-none tracking-tight text-lg">Get in Touch</h3>
            </div>
            <div class="p-6 pt-0 space-y-4">
              <div class="space-y-1">
                <p class="text-sm text-muted-foreground">Email</p>
                <a href="mailto:support@cinemaverse.com" class="text-primary hover:text-primary/80 font-medium text-sm">
                  support&#64;cinemaverse.com
                </a>
              </div>

              <div class="space-y-1">
                <p class="text-sm text-muted-foreground">Phone</p>
                <a href="tel:1800123456" class="text-primary hover:text-primary/80 font-medium text-sm">
                  1-800-123-456
                </a>
              </div>

              <div class="space-y-1">
                <p class="text-sm text-muted-foreground">Hours</p>
                <p class="text-foreground text-sm">Mon-Sun, 9 AM - 11 PM</p>
              </div>

              <button class="inline-flex items-center justify-center gap-2 whitespace-nowrap rounded-md text-sm font-medium transition-colors focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring bg-primary text-primary-foreground shadow hover:bg-primary/90 h-10 px-4 py-2 w-full">
                Start Live Chat
              </button>
            </div>
          </div>

          <!-- CinemaBot Chat -->
          <div class="relative">
            <button
              (click)="toggleCinemaBot()"
              class="w-full rounded-xl border bg-card text-card-foreground shadow p-4 flex items-center justify-center gap-2 hover:bg-accent hover:text-accent-foreground transition-colors"
            >
              <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <path d="M12 8V4H8"/>
                <rect width="16" height="12" x="4" y="8" rx="2"/>
                <path d="M2 14h2"/>
                <path d="M20 14h2"/>
                <path d="M15 13v2"/>
                <path d="M9 13v2"/>
              </svg>
              <span class="font-medium text-sm">Ask CinemaBot</span>
            </button>

            <div
              *ngIf="cinemaBotOpen()"
              class="absolute bottom-full right-0 mb-4 w-80 rounded-xl border bg-card text-card-foreground shadow-lg overflow-hidden z-50 animate-slide-in-up"
            >
              <!-- Chat Header -->
              <div class="flex items-center justify-between border-b px-4 py-3">
                <div>
                  <p class="font-semibold text-foreground text-sm">CinemaBot</p>
                  <p class="text-xs text-muted-foreground">Online</p>
                </div>
                <button
                  (click)="toggleCinemaBot()"
                  class="rounded-sm opacity-70 ring-offset-background transition-opacity hover:opacity-100 focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2"
                >
                  <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                    <path d="M18 6 6 18"/>
                    <path d="m6 6 12 12"/>
                  </svg>
                </button>
              </div>

              <!-- Chat Messages -->
              <div class="h-64 overflow-y-auto p-4 space-y-3 bg-background">
                <div class="flex gap-2">
                  <span class="relative flex shrink-0 overflow-hidden rounded-full h-7 w-7 bg-primary/20 items-center justify-center">
                    <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="text-primary">
                      <path d="M12 8V4H8"/>
                      <rect width="16" height="12" x="4" y="8" rx="2"/>
                      <path d="M2 14h2"/>
                      <path d="M20 14h2"/>
                      <path d="M15 13v2"/>
                      <path d="M9 13v2"/>
                    </svg>
                  </span>
                  <div class="rounded-lg border bg-card px-3 py-2 max-w-xs">
                    <p class="text-sm text-foreground">
                      Hi! I'm CinemaBot. How can I help you today?
                    </p>
                  </div>
                </div>
              </div>

              <!-- Chat Input -->
              <div class="border-t p-4 space-y-2">
                <input
                  type="text"
                  placeholder="Ask me anything..."
                  class="flex h-9 w-full rounded-md border border-input bg-transparent px-3 py-1 text-sm shadow-sm transition-colors placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring"
                />
                <button
                  (click)="sendMessage()"
                  class="inline-flex items-center justify-center gap-2 whitespace-nowrap rounded-md text-sm font-medium transition-colors focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring bg-primary text-primary-foreground shadow hover:bg-primary/90 h-9 px-4 py-2 w-full"
                >
                  Send
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
})
export class SupportComponent implements OnInit {
  private toastService = inject(ToastService);

  expandedFaq = signal(-1);
  cinemaBotOpen = signal(false);

  ngOnInit(): void {
    this.toastService.info('Welcome to CinemaVerse Support. How can we help you?');
  }

  faqs = [
    {
      question: 'How do I book tickets?',
      answer:
        'Navigate to the Discover page, select a movie, choose your preferred showtime, and select your seats. Complete the payment to finalize your booking.',
    },
    {
      question: 'Can I cancel my booking?',
      answer:
        'Yes, you can cancel bookings up to 30 minutes before the show. Go to your dashboard, find the booking, and click the Cancel option.',
    },
    {
      question: 'What payment methods do you accept?',
      answer:
        'We accept all major credit/debit cards, digital wallets, and UPI payments. Your payment is secure and encrypted.',
    },
    {
      question: 'How do I get my refund?',
      answer:
        'Refunds are processed within 3-5 business days to your original payment method. You can track the status in your dashboard.',
    },
    {
      question: 'Are there discounts available?',
      answer:
        'Yes! We offer discounts for group bookings, matinee shows, and special promotional events. Check our offers section.',
    },
  ];

  toggleFaq(index: number): void {
    this.expandedFaq.update((current) => (current === index ? -1 : index));
  }

  toggleCinemaBot(): void {
    this.cinemaBotOpen.update((open) => !open);
  }

  sendMessage(): void {
    this.toastService.success('Message sent! We\'ll get back to you within 24 hours.');
  }
}
