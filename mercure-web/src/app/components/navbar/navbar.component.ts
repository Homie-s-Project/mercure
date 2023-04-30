import {Component, EventEmitter, Output} from '@angular/core';
import { faCartShopping, faUser, faMagnifyingGlass } from '@fortawesome/free-solid-svg-icons';
import { faXmark} from "@fortawesome/free-solid-svg-icons";
@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent {
  faCartShopping = faCartShopping
  faUser = faUser;
  faXmark = faXmark;
  actualIcone = this.faXmark;
  faMagnifyingGlass = faMagnifyingGlass;
  @Output() toggleHide = new EventEmitter<boolean>();
  hideCart: boolean = true
  constructor() { }

  toggleShowHide() {
    this.actualIcone = this.actualIcone === this.faXmark ? this.faCartShopping : this.faXmark;
    this.hideCart = !this.hideCart;
    this.toggleHide.emit(this.hideCart);
  }
}
