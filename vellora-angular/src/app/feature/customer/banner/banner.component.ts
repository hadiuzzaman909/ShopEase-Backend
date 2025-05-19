import { isPlatformBrowser, CommonModule } from '@angular/common';
import {Component, ChangeDetectionStrategy} from '@angular/core';
import { CarouselModule, OwlOptions} from 'ngx-owl-carousel-o';

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

    customOptions: OwlOptions = {
    loop: true,
    mouseDrag: true,   
    touchDrag: true,    
    pullDrag: true,     
    dots: true, 
    navSpeed: 700,
    navText: ['', ''],
    responsive: {
      0: {
        items: 1, 
      },
      400: {
        items: 1, 
      },
      740: {
        items: 1, 
      },
      940: {
        items: 1, 
      },
    },
    nav: true,
    autoplay: true, 
    autoplayHoverPause: false

  }
}