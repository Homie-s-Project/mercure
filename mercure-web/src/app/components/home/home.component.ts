import { Component } from '@angular/core';
import {AppComponent} from "../../app.component";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {

  constructor(appComponent: AppComponent) {
    appComponent.showNavbar = true;
    appComponent.showFooter = true;
  }
}
