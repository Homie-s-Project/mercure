import {Component, Input} from '@angular/core';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import {ICartProductModel} from "../../models/ICartProductModel";

@Component({
  selector: 'app-item-cart',
  templateUrl: './item-cart.component.html',
  styleUrls: ['./item-cart.component.scss']
})
export class ItemCartComponent {
  faTrash = faTrash;

  @Input() product!: ICartProductModel;

  constructor() {
  }
}
