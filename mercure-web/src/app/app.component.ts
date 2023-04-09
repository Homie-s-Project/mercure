import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  showNavbar: boolean = true;
  showFooter: boolean = true;
  showHide = true;

  onHide(event: boolean) {
  this.showHide = true;
  }

  onShow(event: boolean) {
    this.showHide = false;
  }
}
