import {Component, Input} from '@angular/core';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import {ICartProductModel} from "../../models/ICartProductModel";
import {CartService} from "../../services/cart/cart.service";
import {environment} from "../../../environments/environment";

@Component({
  selector: 'app-item-cart',
  templateUrl: './item-cart.component.html',
  styleUrls: ['./item-cart.component.scss']
})
export class ItemCartComponent {
  faTrash = faTrash;

  @Input() product!: ICartProductModel;

  constructor(private cartService: CartService) {
  }

  deleteProduct(productId: number) {
    this.cartService.removeFromCart(productId.toString())
      .finally(() => {
        if (environment.apiUrl) {
          console.log("Product deleted");
        }

        this.cartService.forceReloadCart();
      });
  }
}
