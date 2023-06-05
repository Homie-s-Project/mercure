import {Component, OnInit} from '@angular/core';
import {faCreditCard} from "@fortawesome/free-solid-svg-icons";
import {ICartProductModel} from "../../models/ICartProductModel";
import {CartService} from "../../services/cart/cart.service";
import {environment} from "../../../environments/environment";

@Component({
  selector: 'app-confirmation',
  templateUrl: './confirmation.component.html',
  styleUrls: ['./confirmation.component.scss']
})
export class ConfirmationComponent implements OnInit{
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
      window.location.href = this.cartService.getCheckoutUrl();
  }


}
