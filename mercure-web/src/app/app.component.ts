import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  showNavbar: boolean = true;
  showFooter: boolean = true;
  hideCart: boolean = true;

  toggleHideCart(isHidden: boolean) {
    this.hideCart = isHidden;
  }
}
