import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {AuthService} from "../../services/auth/auth.service";
import {UserService} from "../../services/user/user.service";
import {Subscription} from "rxjs";
import {OrderService} from "../../services/order/order.service";
import {environment} from "../../../environments/environment";

@Component({
  selector: 'app-order-success',
  templateUrl: './order-success.component.html',
  styleUrls: ['./order-success.component.scss']
})
export class OrderSuccessComponent implements OnInit {

  subscriptions: Subscription[] = []

  orderNumber: number = 0;

  isLoading: boolean = true;
  private orderStatus: boolean = false;

  constructor(private router: Router,
              private route: ActivatedRoute,
              private authService: AuthService,
              private userService: UserService,
              private orderService: OrderService
  ) {
  }

  ngOnInit(): void {
    this.subscriptions.push(
      this.route.queryParams
        .subscribe(params => {
          let hasParamOrderNumber = params["orderNumber"] !== null;
          let orderNumber = params["orderNumber"];

          if (hasParamOrderNumber) {
            this.orderNumber = orderNumber;
          }

          this.getOrderStatus();
        })
    );
  }

  getOrderStatus() {
    this.orderService.getOrderStatus(this.orderNumber)
      .then((orderStatus): any => {
        if (!orderStatus) {
          this.router.navigate(['/order/cancel'], {queryParams: {orderNumber: this.orderNumber}});
        }

        if (orderStatus) {
          this.orderStatus = orderStatus;
          return orderStatus;
        }
      })
      .catch((error) => {
        if (!environment.production) {
          console.log(error);
        }

        this.router.navigate(['/']);
      })
      .finally(() => {
        this.isLoading = false;
      });
  }
}
