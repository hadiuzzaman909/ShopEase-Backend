import { CommonModule } from '@angular/common';
import { Component, ChangeDetectionStrategy } from '@angular/core';
import { CarouselModule, OwlOptions } from 'ngx-owl-carousel-o';

interface Category {
  name: string;
  imageUrl: string;
}

@Component({
  selector: 'app-category',
  imports: [CommonModule, CarouselModule],
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CategoryComponent {
  categories: Category[] = [
    { name: 'Clothing', imageUrl: 'assets/categories/man.jpg' },
    { name: 'Sunglasses', imageUrl: 'assets/categories/women.jpg' },
    { name: 'Bags', imageUrl: 'assets/categories/bag.jpg' },
    { name: 'Accessories', imageUrl: 'assets/categories/promotion.jpg' },
    { name: 'Watches', imageUrl: 'assets/categories/new in.jpg' },
    { name: 'Shoes', imageUrl: 'assets/categories/shoes.jpg' },
    { name: 'Jewelry', imageUrl: 'assets/categories/kids.jpg' },
  ];

  customOptions: OwlOptions = {
    loop: false,
    mouseDrag: true,
    touchDrag: true,
    pullDrag: true,
    dots: false,
    nav: true,
    navSpeed: 700,
    navText: ['', ''],
    responsive: {
      0: { items: 1 },
      480: { items: 2 },
      768: { items: 3 },
      992: { items: 4 },
      1200: { items: 5 },
    },
    margin: 10,
    autoplay: false,
  };
}
