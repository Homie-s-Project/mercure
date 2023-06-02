import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {catchError, Observable} from "rxjs";
import {environment} from "../../../environments/environment";
import {IOrderModel} from "../../models/IOrderModel";

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  constructor(private http: HttpClient) {
  }

  getOrderStatus(orderNumber: number): Promise<boolean> {
    return new Promise<boolean>((resolve, reject) => {
      this.http.get<boolean>(environment.apiUrl + "/order/status/" + orderNumber)
        .pipe(
          catchError((error) => {
            reject(error);
            return error;
          })
        )
        .subscribe((orderStatus: any) => {
          resolve(orderStatus);
          return orderStatus;
        });
    });
  }

  getHistoryOrders(): Promise<IOrderModel[]> {
    return new Promise<IOrderModel[]>((resolve, reject) => {
      this.http.get<IOrderModel[]>(environment.apiUrl + "/order/history")
        .pipe(
          catchError((error) => {
            reject(error);
            return error;
          })
        )
        .subscribe((orders: any) => {
          console.log(orders)
          resolve(orders);
          return orders;
        });
    });
  }

  getOrder(orderNumber: number): Observable<IOrderModel> {
    return this.http.get<IOrderModel>(environment.apiUrl + "/order/" + orderNumber);
  }
}
