import {Component, Input} from '@angular/core';
import {environment} from "../../../environments/environment";
import {IOrderModel} from "../../models/IOrderModel";
import {IProductModel} from "../../models/IProductModel";

@Component({
  selector: 'app-order-item',
  templateUrl: './order-item.component.html',
  styleUrls: ['./order-item.component.scss']
})
export class OrderItemComponent {

  @Input() order?: IOrderModel;
  @Input() product?: IProductModel;

  constructor() {
    if (!environment.production && !this.order) {
      console.error("OrderItemComponent: order is undefined")
    }
  }
}
