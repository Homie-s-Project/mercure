import { Component } from '@angular/core';
import {faCircleXmark, faSquareXmark, faTrash, faXmark} from "@fortawesome/free-solid-svg-icons";

@Component({
  selector: 'app-product-cart',
  templateUrl: './product-cart.component.html',
  styleUrls: ['./product-cart.component.scss']
})
export class ProductCartComponent {
  faXmark = faXmark;
  faTrash = faTrash;

}