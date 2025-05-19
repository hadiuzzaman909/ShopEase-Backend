import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'app-featured-products',
  standalone:true,
  imports: [CommonModule],
  templateUrl: './featured-products.component.html',
  styleUrl: './featured-products.component.scss'
})
export class FeaturedProductsComponent {
products = [

    {
      name: 'Modern Tailored Jacket',
      imageUrl: 'assets/products/men-3.png',
      price: 79.99,
      isNew: false,
      isBestSeller: true,
      discountPercentage: 10
    },
    {
      name: 'Elegant Evening Dress',
      imageUrl: 'assets/products/women-1.png',
      price: 99.99,
      isNew: false,
      isBestSeller: false,
      discountPercentage: 0
    },
        {
      name: 'Luxury Leather Bag',
      imageUrl: 'assets/products/bag-1.png',
      price: 99.99,
      isNew: false,
      isBestSeller: false,
      discountPercentage: 0
    },

        {
      name: 'Royal Blue Suit',
      imageUrl: 'assets/products/men-2.png',
      price: 49.99,
      isNew: true,
      isBestSeller: false,
      discountPercentage: 20
    },
    {
      name: 'Chic Summer Dress',
      imageUrl: 'assets/products/women-2.png',
      price: 79.99,
      isNew: false,
      isBestSeller: true,
      discountPercentage: 10
    },

    {
      name: 'Stylish Tote Bag',
      imageUrl: 'assets/products/bag-3.png',
      price: 79.99,
      isNew: false,
      isBestSeller: true,
      discountPercentage: 10
    },
    {
      name: 'Sleek Aviator Sunglasses',
      imageUrl: 'assets/products/sunglass-1.png',
      price: 99.99,
      isNew: false,
      isBestSeller: false,
      discountPercentage: 0
    },
        {
      name: 'Classic Navy Blazer',
      imageUrl: 'assets/products/men-4.png',
      price: 49.99,
      isNew: true,
      isBestSeller: false,
      discountPercentage: 20
    },
    // Add more products
  ];

  addToWishlist(product:any) {
    console.log('Added to Wishlist:', product);
  }

  compareProduct(product:any) {
    console.log('Comparing product:', product);
  }

  addToCart(product:any) {
    console.log('Added to Cart:', product);
  }

}
