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

  cartProducts: ICartProductModel[] = []
  isLoading: boolean = true;

  totalPrice: number = 0.00;

  constructor(public cartService: CartService) {
  }

  ngOnInit(): void {
    this.cartService.getCart()
      .then((data => {
        this.cartProducts = data.products;
      }))
      .catch(e => {
        if (!environment.production) {
          console.log(e);
        }
      })
      .finally(() => {
        this.isLoading = false;

        this.totalPrice = this.cartProducts.reduce((a, b) => a + (b.product.productPrice * b.quantity), 0);
      });
  }

  checkout() {
    if (this.cartProducts.length > 0) {
      window.location.href = this.cartService.getCheckoutUrl();
    } else {
      if (!environment.production) {
        console.log("No products in cart");
      }
    }
  }
}
