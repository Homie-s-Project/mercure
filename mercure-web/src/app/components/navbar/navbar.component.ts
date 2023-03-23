import { Component, OnInit } from '@angular/core';
import { faCartShopping, faGlobe, faUser, faMagnifyingGlass } from '@fortawesome/free-solid-svg-icons';

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

  constructor() { }

  ngOnInit(): void {
  }

}
