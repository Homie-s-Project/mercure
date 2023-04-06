import { Component } from '@angular/core';
import {faCircleXmark, faSquareXmark, faTrash, faXmark} from "@fortawesome/free-solid-svg-icons";

@Component({
  selector: 'app-item-card-product',
  templateUrl: './item-card-product.component.html',
  styleUrls: ['./item-card-product.component.scss']
})
export class ItemCardProductComponent {
  faTrash = faTrash;
}
