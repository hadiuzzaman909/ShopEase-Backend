import { Component } from '@angular/core';
import { BannerComponent } from '../banner/banner.component';
import { CategoryComponent } from '../category/category.component';
import { FeaturedProductsComponent } from '../featured-products/featured-products.component';
import { FacebookLoginComponent } from '../facebook-login/facebook-login.component';

@Component({
  selector: 'app-home',
  imports: [BannerComponent,CategoryComponent,FeaturedProductsComponent,FacebookLoginComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {

}
