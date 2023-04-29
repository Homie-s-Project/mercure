import { Component, Input, OnInit } from '@angular/core';
import { faTag, faCartPlus } from '@fortawesome/free-solid-svg-icons';
import { IProductModel } from 'src/app/models/IProductModel';
import {CartService} from "../../services/cart/cart.service";

@Component({
  selector: 'app-item-card',
  templateUrl: './item-card.component.html',
  styleUrls: ['./item-card.component.scss']
})
export class ItemCardComponent implements OnInit {
  faTag = faTag;
  faCartPlus = faCartPlus;

  @Input() product?: IProductModel;

  constructor(private cartService: CartService) { }

  ngOnInit(): void {
  }

  addProductCart(productId: number | undefined) {
    if (productId != undefined) {
      this.cartService.addToCart(String(productId), 1).finally(() => {
        console.log("Added to cart");
      });
    }
  }
}
