import {Injectable, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {AuthService} from "../auth/auth.service";
import {UserService} from "../user/user.service";
import {environment} from "../../../environments/environment";
import {catchError, throwError} from "rxjs";
import {ICartModel} from "../../models/ICartModel";
import {Router} from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class CartService implements OnInit {

  // RandomId is generated if user is not logged to identify the cart
  private cartId?: string | null;

  private cart: any;

  // Last time the cart was loaded
  private lastLoadedCart?: Date;

  constructor(private authService: AuthService, private userService: UserService, private http: HttpClient, private router: Router) {
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
        if (!environment.production) {
          console.log(e);
        }

        if (e.status === 404) {
          this.cart = [];
        }
      })
      .finally(() => {
        if (!environment.production) {
          console.log("Cart loaded");
        }
      });
  }

  getCart(byPassCheck: boolean = false): Promise<ICartModel> {
    // If cart was loaded less than 5 minutes ago, don't load it again
    if (!byPassCheck && this.lastLoadedCart && this.lastLoadedCart.getTime() + 1000 * 60 * 5 > (new Date()).getTime()) {
      if (!environment.production) {
        console.log("Cart loaded less than 5 minutes ago, returning cached cart, refresh page to load again");
      }

      return new Promise((resolve, reject) => {
        resolve(this.cart);
      });
    }

    let isLogged = this.authService.isLogged();
    let hasCartId = this.hasCartId();

    let url = environment.apiUrl + `/cart${!isLogged && hasCartId ? `?randomId=${this.cartId}` : ''}`;

    return new Promise((resolve, reject) => {
      this.http.get<ICartModel>(url)
        .pipe(
          catchError((error) => {
            reject(error);
            return throwError(error);
          })
        )
        .subscribe((data) => {
          this.lastLoadedCart = new Date();
          resolve(data);
          return data;
        })
    });
  }

  addToCart(productId: string, quantity = 1): Promise<any> {
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

    let url = environment.apiUrl + this.router.createUrlTree(['/cart/add', productId], {queryParams: {quantity: quantity, randomId: !isLogged && hasCartId ? this.cartId : ''}}).toString();

    return new Promise((resolve, reject) => {
      this.http.post(url, {})
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

  removeFromCart(productId: string): Promise<any> {
    let isLogged = this.authService.isLogged();
    let hasCartId = this.hasCartId();

    if (productId === undefined || productId === null) {
      throw new Error("Product id is required");
    }

    let productIdConverted = Number(productId);

    if (isNaN(productIdConverted)) {
      throw new Error("Product id is not a number");
    }

    let url = environment.apiUrl + this.router.createUrlTree(['/cart/remove', productId], {queryParams: {randomId: !isLogged && hasCartId ? this.cartId : ''}}).toString();

    return new Promise((resolve, reject) => {
      this.http.delete(url)
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

  getCheckoutUrl() {
    return environment.apiUrl + this.router.createUrlTree(['/order/buy'], {queryParams: {randomId: this.cartId}}).toString();
  }

  checkoutBuyAgain(orderId: number) {
    return environment.apiUrl + `/order/buy-again${orderId ? '?orderId=' + orderId : ''}`;
  }

  forceReloadCart(): void {
    this.getCart(true)
      .then(r => {
        this.cart = r;
      })
      .catch(e => {
        if (!environment.production) {
          console.log(e);
        }

        if (e.status === 404) {
          this.cart = [];
        }
      })
      .finally(() => {
        if (!environment.production) {
          console.log("Cart force reloaded");
        }
      });
  }

  private hasCartId(): boolean {
    return sessionStorage.getItem("cartId") !== null;
  }

  private generateCartId(): string {
    let randomId = Math.random().toString(36).substring(2) + (new Date()).getTime().toString(36);
    sessionStorage.setItem("cartId", randomId)

    return randomId;
  }

  private removeCartId(): void {
    sessionStorage.removeItem("cartId");
  }

  private getCartId(): string | null {
    return sessionStorage.getItem("cartId");
  }
}
