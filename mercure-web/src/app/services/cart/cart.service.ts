import {Injectable, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {AuthService} from "../auth/auth.service";
import {UserService} from "../user/user.service";
import {environment} from "../../../environments/environment";
import {catchError, throwError} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class CartService implements OnInit {

  // RandomId is generated if user is not logged to identify the cart
  private cartId?: string | null;

  private cart: any;

  constructor(private authService: AuthService, private userService: UserService, private http: HttpClient) {
  }

  ngOnInit(): void {
    this.cartId = this.getCartId();

    // If user is not logged and has no cartId, generate a cartId
    if (!this.authService.isLogged() && !this.hasCartId()) {
      this.cartId = this.generateCartId();
    }

    // If user is logged and has a cartId, remove the cartId
    if (this.authService.isLogged() && this.hasCartId()) {
      this.removeCartId();
    }

    this.getCart()
      .then(r => {
        this.cart = r;
      })
      .catch(e => {
        console.log(e);
        if (e.status === 404) {
          this.cart = [];
        }
      })
      .finally(() => {
        console.log(this.cart);
      });
  }

  getCart(): Promise<any> {
    let isLogged = this.authService.isLogged();
    let hasCartId = this.hasCartId();

    let url = environment.apiUrl + `/cart${!isLogged && hasCartId ? `?randomId=${this.cartId}` : ''}`;

    return new Promise((resolve, reject) => {
      this.http.get<{ token: string }>(url)
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

  addToCart(productId: string, quantity = 1) : Promise<any> {
    let isLogged = this.authService.isLogged();
    let hasCartId = this.hasCartId();

    if (productId === undefined || productId === null) {
      throw new Error("Product id is required");
    }

    let productIdConverted = Number(productId);
    let quantityConverted = Number(quantity);

    if (isNaN(productIdConverted)) {
      throw new Error("Product id is not a number");
    }

    if (isNaN(quantityConverted) || quantity < 1) {
      throw new Error("Quantity is not valid");
    }

    let url = environment.apiUrl + `/cart/add/${productId}/${quantity != null ? quantity : ""}${!isLogged && hasCartId ? `?randomId=${this.cartId}` : ''}`;

    return new Promise((resolve, reject) => {
      this.http.get<{ token: string }>(url)
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

  removeFromCart(productId: string) : Promise<any> {
    let isLogged = this.authService.isLogged();
    let hasCartId = this.hasCartId();

    if (productId === undefined || productId === null) {
      throw new Error("Product id is required");
    }

    let productIdConverted = Number(productId);

    if (isNaN(productIdConverted)) {
      throw new Error("Product id is not a number");
    }

    let url = environment.apiUrl + `/cart/remove/${productId}/${!isLogged && hasCartId ? `?randomId=${this.cartId}` : ''}`;

    return new Promise((resolve, reject) => {
      this.http.get<{ token: string }>(url)
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

  private hasCartId(): boolean {
    return sessionStorage.getItem("cartId") !== null;
  }

  private generateCartId() : string {
    let randomId = Math.random().toString(36).substring(2) + (new Date()).getTime().toString(36);
    sessionStorage.setItem("cartId", randomId)

    return randomId;
  }

  private removeCartId() : void {
    sessionStorage.removeItem("cartId");
  }

  private getCartId() : string | null {
    return sessionStorage.getItem("cartId");
  }
}
