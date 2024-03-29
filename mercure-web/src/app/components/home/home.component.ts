import {Component, OnInit} from '@angular/core';
import {AppComponent} from "../../app.component";
import {ProductService} from 'src/app/services/product/product.service';
import {IProductModel} from 'src/app/models/IProductModel';
import {environment} from "../../../environments/environment";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  bestSellerProducts?: IProductModel[];
  randomProducts?: IProductModel[];

  hasBestSellerError: boolean = false;
  isLoading = true;

  constructor(appComponent: AppComponent, private productService: ProductService) {
    appComponent.showNavbar = true;
    appComponent.showFooter = true;
  }

  async ngOnInit(): Promise<void> {
    this.productService.getBestSeller()
      .then(data => {
        this.bestSellerProducts = data;
      })
      .catch((error) => {
        this.hasBestSellerError = true;

        this.productService.getRandomProducts()
          .then(data => {
            this.randomProducts = data;
          })
          .catch((error) => {
            if (!environment.production) {
              console.error(error)
            }
          })
          .finally(() => {
            this.isLoading = false;
          });

        if (!environment.production) {
          console.error(error)
        }

        // Afficher une erreur avec un modal
        // this.modalService.open(error)
      })
      .finally(() => {
        this.isLoading = false;
      });
  }
}
