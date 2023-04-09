import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import { faCartShopping, faGlobe, faUser, faMagnifyingGlass } from '@fortawesome/free-solid-svg-icons';
import {AppComponent} from "../../app.component";

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  faCartShopping = faCartShopping
  faGlobe = faGlobe;
  faUser = faUser;
  faMagnifyingGlass = faMagnifyingGlass;
  @Output() hide = new EventEmitter<{ showHide: boolean }>();
  @Output() show = new EventEmitter<{ showHide: boolean }>();
  showHide = true;

  constructor() { }

  ngOnInit(): void {
  }

  toggleshowHide() {

    if (this.showHide === true) {
      this.show.emit({showHide: false});
    } else {
      this.hide.emit({showHide: true});
    }
  }
}
