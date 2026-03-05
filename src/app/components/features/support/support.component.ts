import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-support',
  standalone: true,
  imports: [CommonModule],
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

          <div class="space-y-2">
            <div *ngFor="let faq of faqs; let i = index" class="glassmorphism rounded-lg overflow-hidden">
              <button
                (click)="toggleFaq(i)"
                class="w-full px-6 py-4 flex items-center justify-between text-left hover:bg-secondary/50 transition-smooth"
              >
                <span class="font-medium text-foreground">{{ faq.question }}</span>
                <span class="text-muted-foreground transform transition-transform" [class.rotate-180]="expandedFaq() === i">
                  ▼
                </span>
              </button>

              <div *ngIf="expandedFaq() === i" class="border-t border-border px-6 py-4 bg-secondary/20">
                <p class="text-foreground leading-relaxed">{{ faq.answer }}</p>
              </div>
            </div>
          </div>
        </div>

        <!-- Contact Section -->
        <div class="lg:col-span-1 space-y-6">
          <!-- Contact Info -->
          <div class="glassmorphism rounded-lg p-6 space-y-4">
            <h3 class="text-xl font-bold text-foreground">Get in Touch</h3>

            <div class="space-y-4">
              <div class="space-y-1">
                <p class="text-sm text-muted-foreground">Email</p>
                <a href="mailto:support@cinemaverse.com" class="text-primary hover:text-primary/80 font-medium">
                  support@cinemaverse.com
                </a>
              </div>

              <div class="space-y-1">
                <p class="text-sm text-muted-foreground">Phone</p>
                <a href="tel:1800123456" class="text-primary hover:text-primary/80 font-medium">
                  1-800-123-456
                </a>
              </div>

              <div class="space-y-1">
                <p class="text-sm text-muted-foreground">Hours</p>
                <p class="text-foreground">Mon-Sun, 9 AM - 11 PM</p>
              </div>
            </div>

            <button class="w-full px-4 py-3 rounded-lg bg-primary text-primary-foreground font-bold hover:bg-primary/90 neon-glow transition-smooth">
              Start Live Chat
            </button>
          </div>

          <!-- CinemaBot Chat -->
          <div class="relative">
            <button
              (click)="toggleCinemaBot()"
              class="w-full glassmorphism rounded-lg p-4 flex items-center justify-center gap-2 hover:bg-secondary transition-smooth"
            >
              <span class="text-lg">🤖</span>
              <span class="font-medium text-foreground">Ask CinemaBot</span>
            </button>

            <div
              *ngIf="cinemaBotOpen()"
              class="absolute bottom-full right-0 mb-4 w-80 glassmorphism rounded-lg shadow-xl overflow-hidden z-50 animate-slide-in-up"
            >
              <!-- Chat Header -->
              <div class="bg-primary/20 px-4 py-3 border-b border-border flex items-center justify-between">
                <div>
                  <p class="font-bold text-foreground">CinemaBot</p>
                  <p class="text-xs text-muted-foreground animate-pulse-neon">● Online</p>
                </div>
                <button (click)="toggleCinemaBot()" class="text-muted-foreground hover:text-foreground">
                  ✕
                </button>
              </div>

              <!-- Chat Messages -->
              <div class="h-64 overflow-y-auto p-4 space-y-3 bg-background/50">
                <div class="flex gap-2">
                  <span class="text-lg">🤖</span>
                  <div class="bg-secondary rounded-lg px-3 py-2 max-w-xs">
                    <p class="text-sm text-foreground">
                      Hi! I'm CinemaBot. How can I help you today?
                    </p>
                  </div>
                </div>
              </div>

              <!-- Chat Input -->
              <div class="border-t border-border p-4 space-y-2">
                <input
                  type="text"
                  placeholder="Ask me anything..."
                  class="w-full px-3 py-2 rounded-lg bg-secondary text-foreground placeholder-muted-foreground focus:outline-none focus:ring-2 focus:ring-primary text-sm"
                />
                <button class="w-full px-3 py-2 rounded-lg bg-primary text-primary-foreground font-medium text-sm hover:bg-primary/90 transition-smooth">
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
export class SupportComponent {
  expandedFaq = signal(-1);
  cinemaBotOpen = signal(false);

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
}
