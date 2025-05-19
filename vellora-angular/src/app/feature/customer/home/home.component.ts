import { Component } from '@angular/core';
import { BannerComponent } from '../banner/banner.component';
import { CategoryComponent } from '../category/category.component';
import { FeaturedProductsComponent } from '../featured-products/featured-products.component';

@Component({
  selector: 'app-home',
  imports: [BannerComponent,CategoryComponent,FeaturedProductsComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {

}
