import {Component, OnInit} from '@angular/core';
import {UserModel} from 'src/app/models/UserModel';
import {UserService} from "../../services/user/user.service";
import { ParameterModel } from 'src/app/models/ParameterModel';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  currentUser: UserModel = {
    userId: 0,
    firstName: 'Loading...',
    lastName: 'Loading...',
    email: 'Loading...',
    role: {roleId: 0, roleName: "Loading...", roleNumber: -1}
  };

  currentParameter: ParameterModel = {
    langue: 'fr',
    monnaie: 'chf',
    shipmentAdress: 'Rue de Genève 63, 1004 Lausanne'
  }

  isUserLoading: boolean = true;
  isParametersLoading: boolean = false;
  orderIsShow: boolean = false;
  parameterIsShow: boolean = false;

  constructor(private userService: UserService) {
  }

  profilShow() {
    this.orderIsShow = false;
    this.parameterIsShow = false;
  }

  orderShow() {
    this.orderIsShow = true;
    this.parameterIsShow = false;
  }
  parameterShow(){
    this.orderIsShow = false;
    this.parameterIsShow = true;
  }

  ngOnInit(): void {
    this.userService.getUser()
      .then(u => this.currentUser = u)
      .finally(() => this.isUserLoading = false);
  }
}


