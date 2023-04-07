import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';

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
    LoadingComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FontAwesomeModule,
    ShareModule
  ],
  providers: [
    AuthService,
    UserService,
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
