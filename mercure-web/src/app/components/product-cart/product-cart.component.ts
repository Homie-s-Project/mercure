import {Component, OnInit} from '@angular/core';
import {faCreditCard} from "@fortawesome/free-solid-svg-icons";
import {CartService} from "../../services/cart/cart.service";
import {ICartProductModel} from "../../models/ICartProductModel";
import {environment} from "../../../environments/environment";
import {ICartModel} from "../../models/ICartModel";

@Component({
  selector: 'app-product-cart',
  templateUrl: './product-cart.component.html',
  styleUrls: ['./product-cart.component.scss']
})
export class ProductCartComponent implements OnInit {
  faCreditCard = faCreditCard;

  cartProducts: ICartProductModel[] = []
  isLoading: boolean = true;

  constructor(private cartService: CartService) {
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
      });
  }
}
