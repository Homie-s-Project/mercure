import {Injectable, OnInit} from '@angular/core';
import {IProductModel} from 'src/app/models/IProductModel';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {IPaginationProductModel} from "../../models/IPaginationProductModel";
import {catchError, throwError} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ProductService implements OnInit {

  private pageIndex: number = 0;
  private pageSize: number = 10;

  private currentPagination: IPaginationProductModel | any[] = [];
  private productsBestSeller: IProductModel[] | any[] = []

  constructor(private http: HttpClient) {
  }

  ngOnInit(): void {
    this.getProductsPagination()
      .then(p => {
        this.currentPagination = p
      })
      .catch((error) => {
        if (!environment.production) {
          console.error(error)
        }
      })
      .finally(() => {
        if (!environment.production) {
          console.log(this.currentPagination)
        }
      });

    this.getBestSeller()
      .then(p => this.productsBestSeller = p)
      .catch((error) => {
        if (!environment.production) {
          console.error(error)
        }
      })
      .finally(() => {
        if (!environment.production) {
          console.log(this.productsBestSeller)
        }
      })
  }

  getBestSeller(): Promise<IProductModel[]> {

    let url = environment.apiUrl + "/shopping/bestSeller"

    return new Promise((resolve, reject) => {
      this.http.get<IProductModel[]>(url)
        .pipe(
          catchError((error) => {
            reject(error);
            return throwError(error);
          })
        )
        .subscribe((data) => {
          resolve(data);
          return data;
        })
    });
  }

  getProductById(productId: number): Promise<IProductModel> {

    let url = environment.apiUrl + "/products/" + productId;

    return new Promise((resolve, reject) => {
      this.http.get<IProductModel>(url)
        .pipe(
          catchError((error) => {
            reject(error);
            return throwError(error);
          })
        )
        .subscribe((data) => {
          resolve(data);
          return data;
        })
    });
  }

  getProductsPagination(pageIndex: number = this.pageIndex, pageSize: number = this.pageSize): Promise<IPaginationProductModel> {

    let url = environment.apiUrl + `/shopping/home?pageIndex=${pageIndex}&pageSize=${pageSize}`;

    return new Promise((resolve, reject) => {
      this.http.get<IPaginationProductModel>(url)
        .pipe(
          catchError((error) => {
            reject(error);
            return throwError(error);
          })
        )
        .subscribe((data) => {
          resolve(data);
          return data;
        })
    });
  }

  getRandomProducts() : Promise<IProductModel[]> {

    return new Promise((resolve, reject) => {
      this.http.get<IProductModel[]>(environment.apiUrl + "/shopping")
        .pipe(
          catchError((error) => {
            reject(error);
            return throwError(error);
          })
        )
        .subscribe((data) => {
          resolve(data);
          return data;
        });
    });
  }
}
