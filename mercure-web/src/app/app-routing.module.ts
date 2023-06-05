import { Component, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { ProductComponent } from './components/product/product.component';
import { ProfileComponent } from './components/profile/profile.component';
import { LandingComponent } from './components/landing/landing.component';
import { LoadingComponent } from './components/loading/loading.component';
import {SearchComponent} from "./components/search/search.component";
import {ConfirmationComponent} from "./components/confirmation/confirmation.component";


const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    pathMatch: 'full'
  },
  {
    path: 'profile',
    component: ProfileComponent
  },
  {
    path: 'product/:productId',
    component: ProductComponent
  },
  {
    path: 'login',
    component: LandingComponent
  },
  {
    path: 'loading',
    component: LoadingComponent
  },
  {
    path: 'product/:productId',
    component: ProductComponent
  },
  {
    path: 'search',
    component: SearchComponent
  },
  {
    path: 'confirmation',
    component: ConfirmationComponent
  },
  {
    path: '**',
    redirectTo: ""
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
