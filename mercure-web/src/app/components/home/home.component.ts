import { Component, OnInit } from '@angular/core';
import {AppComponent} from "../../app.component";
import { ProductService } from 'src/app/services/product/product.service';
import { IProductModel } from 'src/app/models/IProductModel';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  products?: IProductModel[]

  constructor(appComponent: AppComponent, public productService: ProductService) {
    appComponent.showNavbar = true;
    appComponent.showFooter = true;
  }

  ngOnInit(): void {
    this.products = this.productService.getProducts();
  }
}
