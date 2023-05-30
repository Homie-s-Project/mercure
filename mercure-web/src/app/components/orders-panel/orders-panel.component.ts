import {Component, OnInit} from '@angular/core';
import {OrderService} from "../../services/order/order.service";
import {IOrderModel} from "../../models/IOrderModel";
import {environment} from "../../../environments/environment";

@Component({
  selector: 'app-orders-panel',
  templateUrl: './orders-panel.component.html',
  styleUrls: ['./orders-panel.component.scss']
})
export class OrdersPanelComponent implements OnInit {
  orders: IOrderModel[] = [];
  isLoading: boolean = true;

  constructor(private orderService: OrderService) {
  }

  ngOnInit(): void {
    this.orderService.getHistoryOrders()
      .then((orders) => {
        this.orders = orders;
      })
      .catch((error) => {
        if (!environment.production) {
          console.log(error);
        }
      })
      .finally(() => {
        this.isLoading = false;
      });
  }

}
