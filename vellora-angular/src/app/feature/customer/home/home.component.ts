import { Component } from '@angular/core';
import { BannerComponent } from '../banner/banner.component';
import { CategoryComponent } from '../category/category.component';

@Component({
  selector: 'app-home',
  imports: [BannerComponent,CategoryComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {

}
