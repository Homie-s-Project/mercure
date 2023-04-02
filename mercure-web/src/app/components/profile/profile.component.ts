import { Component } from '@angular/core';
import { UserModel } from 'src/app/models/UserModel';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent {
  currentUser?: UserModel;
  
  constructor(
    //private auth: AuthService,
  ) {
    //auth.getProfile().subscribe(resp => {
      //if (resp.status != 200) {
        //console.log(resp.statusText);
      //}

      //this.currentUser = new UserModel(resp.body);
  }
}

