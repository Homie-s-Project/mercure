import {Component, OnInit} from '@angular/core';
import { UserModel } from 'src/app/models/UserModel';
import { AuthService } from 'src/app/services/auth/auth.service';
import {UserService} from "../../services/user/user.service";

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  currentUser: UserModel = {userId: 0, firstName: 'Loading...', lastName: 'Loading...', email: 'Loading...'};
  isUserLoading: boolean = true;
  orderIsShow = false;

  constructor(private userService: UserService  ) {}

  profilShow(){
    this.orderIsShow = false;
  }
  orderShow(){
    this.orderIsShow = true;
  }

  ngOnInit(): void {
    this.userService.getUser()
      .then(u => this.currentUser = u)
      .finally(() => this.isUserLoading = false);
  }
}


