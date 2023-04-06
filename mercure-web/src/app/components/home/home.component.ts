import { Component } from '@angular/core';
import {AppComponent} from "../../app.component";
import { ProductService } from 'src/app/services/product/product.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {

  constructor(appComponent: AppComponent, public product: ProductService) {
    appComponent.showNavbar = true;
    appComponent.showFooter = true;
  }
}
