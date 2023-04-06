import { Component } from '@angular/core';
import { UserModel } from 'src/app/models/UserModel';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent {
  currentUser?: UserModel;
  orderIsShow = false;

  constructor(
    private auth: AuthService,
  ) {
    auth.getProfile().subscribe(resp => {
      if (resp.status != 200) {
        console.log(resp.statusText);
      }

      this.currentUser = new UserModel(resp.body);
    })
  }
  profilShow(){
    this.orderIsShow = false;
  }
  orderShow(){
    this.orderIsShow = true;
  }

}

