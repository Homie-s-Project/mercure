import { Component, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { ProfileComponent } from './components/profile/profile.component';
import { LandingComponent } from './components/landing/landing.component';
import { LoadingComponent } from './components/loading/loading.component';


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
    path: 'login',
    component: LandingComponent
  },
  {
    path: 'loading',
    component: LoadingComponent
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
