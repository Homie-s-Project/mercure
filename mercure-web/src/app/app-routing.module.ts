import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {HomeComponent} from './components/home/home.component';
import {ProductComponent} from './components/product/product.component';
import {ProfileComponent} from './components/profile/profile.component';
import {LandingComponent} from './components/landing/landing.component';
import {LoadingComponent} from './components/loading/loading.component';
import {SearchComponent} from "./components/search/search.component";
import {AuthentifiedGuard} from "./guard/authentified/authentified.guard";
import {UnauthentifiedGuard} from "./guard/unauthentified/unauthentified.guard";


const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    pathMatch: 'full'
  },
  {
    path: 'profile',
    component: ProfileComponent,
    canActivate: [
      AuthentifiedGuard
    ]
  },
  {
    path: 'product/:productId',
    component: ProductComponent
  },
  {
    path: 'login',
    component: LandingComponent,
    canActivate: [
      UnauthentifiedGuard
    ]
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
    path: '**',
    redirectTo: ""
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: [
    AuthentifiedGuard,
    UnauthentifiedGuard,
  ]
})
export class AppRoutingModule {
}
