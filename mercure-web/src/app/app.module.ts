import {APP_INITIALIZER, NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {NavbarComponent} from './components/navbar/navbar.component';
import {ProfileComponent} from './components/profile/profile.component';
import {HomeComponent} from './components/home/home.component';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';
import {OrdersPanelComponent} from './components/orders-panel/orders-panel.component';
import {OrderItemComponent} from './components/order-item/order-item.component';
import {ItemCardComponent} from './components/item-card/item-card.component';
import {LandingComponent} from './components/landing/landing.component';
import {AuthService} from "./services/auth/auth.service";
import {ShareModule} from "ngx-sharebuttons";
import {HTTP_INTERCEPTORS, HttpClientModule} from "@angular/common/http";
import {LoadingComponent} from "./components/loading/loading.component";
import {JwtTokenResponseHeaderInterceptor} from "./interceptors/jwtTokenResponse/jwt-token-response-header.interceptor";
import {UserService} from "./services/user/user.service";
import {
  JwtTokenHeaderRequestInterceptor
} from "./interceptors/jwtTokenRequest/jwt-token-header-request-interceptor.service";
import {ProductComponent} from './components/product/product.component';
import {TagsComponent} from './components/tags/tags.component';
import {ProductCartComponent} from "./components/product-cart/product-cart.component";
import {ItemCartComponent} from "./components/item-cart/item-cart.component";
import {CartService} from "./services/cart/cart.service";
import {ProductService} from "./services/product/product.service";
import {ProductsPaginationComponent} from './components/products-pagination/products-pagination.component';
import { SearchComponent } from './components/search/search.component';
import {FormsModule} from "@angular/forms";
import {NgOptimizedImage} from "@angular/common";
import { FilterComponent } from './components/filter/filter.component';
import { FilterGroupComponent } from './components/filter-group/filter-group.component';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    HomeComponent,
    ProfileComponent,
    OrdersPanelComponent,
    OrderItemComponent,
    ItemCardComponent,
    LandingComponent,
    LoadingComponent,
    ProductComponent,
    ProductCartComponent,
    ItemCartComponent,
    TagsComponent,
    ProductsPaginationComponent,
    SearchComponent,
    FilterComponent,
    FilterGroupComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    HttpClientModule,
    FontAwesomeModule,
    ShareModule,
    FormsModule,
    NgOptimizedImage
  ],
  providers: [
    AuthService,
    ProductService,
    UserService,
    {
      provide: APP_INITIALIZER,
      useFactory: (cartService: CartService) => () => cartService.ngOnInit(),
      deps: [CartService],
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS, useClass: JwtTokenHeaderRequestInterceptor, multi: true
    },
    {
      provide: HTTP_INTERCEPTORS, useClass: JwtTokenResponseHeaderInterceptor, multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
