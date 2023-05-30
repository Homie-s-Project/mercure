import {Component, OnInit} from '@angular/core';
import {faCreditCard} from "@fortawesome/free-solid-svg-icons";
import {CartService} from "../../services/cart/cart.service";
import {ICartProductModel} from "../../models/ICartProductModel";
import {environment} from "../../../environments/environment";

@Component({
  selector: 'app-product-cart',
  templateUrl: './product-cart.component.html',
  styleUrls: ['./product-cart.component.scss']
})
export class ProductCartComponent implements OnInit {
  faCreditCard = faCreditCard;

  isLoading: boolean = true;
  isCheckoutLoading: boolean = false;

  constructor(public cartService: CartService) {
  }

  ngOnInit(): void {
    this.cartService.getCart()
      .catch(e => {
        if (!environment.production) {
          console.log(e);
        }
      })
      .finally(() => {
        this.isLoading = false;
      });
  }

  async checkout() {
    if (this.cartService.cart?.products && this.cartService.cart?.products.length > 0) {
      this.isCheckoutLoading = true;

      await this.cartService.getCheckoutUrl()
        .then((data) => {
          window.location.href = data;
        })
        .finally(() => {
          this.isCheckoutLoading = false;
        });
    } else {
      if (!environment.production) {
        console.log("No products in cart");
      }
    }
  }
}
