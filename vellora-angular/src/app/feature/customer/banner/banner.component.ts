import { CommonModule } from '@angular/common';
import { Component, ChangeDetectionStrategy } from '@angular/core';
import { CarouselModule, OwlOptions } from 'ngx-owl-carousel-o';

interface BannerSlide {
  bgImage: string;
  personImage: string;
  personImageAlt: string;
  title: string;
  description: string;
}

@Component({
  selector: 'app-banner',
  imports: [CommonModule, CarouselModule],
  standalone: true,
  templateUrl: './banner.component.html',
  styleUrls: ['./banner.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class BannerComponent {
  slides: BannerSlide[] = [
    {
      bgImage: 'assets/banner/background-1.png',
      personImage: 'assets/banner/men-1.png',
      personImageAlt: 'Man smiling confidently, wearing a modern blue suit',
      title: 'Discover Your Style',
      description: 'Explore the latest trends and timeless classics for every occasion.',
    },
    {
      bgImage: 'assets/banner/background-2.png',
      personImage: 'assets/banner/women-1.png',
      personImageAlt: 'Woman posing elegantly in a stylish dress',
      title: 'Elevate Your Look',
      description: 'Fashion that speaks to your soul. Bold, elegant, and uniquely you.',
    },
    {
      bgImage: 'assets/banner/background-3.png',
      personImage: 'assets/banner/men-2.png',
      personImageAlt: 'Man in a casual yet fashionable outfit looking thoughtful',
      title: 'Unmatched Quality',
      description: 'Craftsmanship meets style. Dress to impress with Vellora.',
    },
  ];

  currentIndex = 0;
  autoSlideInterval?: number;
  autoSlideDelay = 3000; // 3 seconds

  customOptions: OwlOptions = {
    loop: true, // Loop through the slides
    mouseDrag: true,    // Enable mouse drag (desktop)
    touchDrag: true,    // Enable touch drag (mobile/tablet)
    pullDrag: true,     // Enable pull drag effect
    dots: true, // Enable dots
    navSpeed: 700,
    navText: ['', ''],
    responsive: {
      0: {
        items: 1, // Show 1 slide on mobile
      },
      400: {
        items: 1, // Show 1 slide on small screens
      },
      740: {
        items: 1, // Show 1 slide on tablets
      },
      940: {
        items: 1, // Show 1 slide on larger screens
      },
    },
    nav: true,
    autoplay: true, // Enable auto play
    autoplayTimeout: this.autoSlideDelay, // Set auto slide delay to 3 seconds
    autoplayHoverPause: false
  };

  // Start auto slide when component is initialized
  ngOnInit() {
    if (this.slides.length > 1) {
      this.startAutoSlide();
    }
  }

  ngOnDestroy() {
    this.stopAutoSlide();
  }

  // Slide change logic
  onNextClick(): void {
    this.next();
    this.restartAutoSlide();
  }

  onPrevClick(): void {
    this.prev();
    this.restartAutoSlide();
  }

  next(): void {
    this.currentIndex = (this.currentIndex + 1) % this.slides.length;
  }

  prev(): void {
    this.currentIndex =
      (this.currentIndex - 1 + this.slides.length) % this.slides.length;
  }

  selectSlide(index: number): void {
    if (index >= 0 && index < this.slides.length) {
      this.currentIndex = index;
      this.restartAutoSlide();
    }
  }

  startAutoSlide(): void {
    this.stopAutoSlide();
    this.autoSlideInterval = window.setInterval(() => {
      this.next();
    }, this.autoSlideDelay);
  }

  stopAutoSlide(): void {
    if (this.autoSlideInterval) {
      clearInterval(this.autoSlideInterval);
      this.autoSlideInterval = undefined;
    }
  }

  restartAutoSlide(): void {
    if (this.slides.length > 1) {
      this.stopAutoSlide();
      this.startAutoSlide();
    }
  }
}
