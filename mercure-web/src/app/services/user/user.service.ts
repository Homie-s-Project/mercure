import {Injectable, OnInit} from '@angular/core';
import {AuthService} from "../auth/auth.service";
import {UserModel} from "../../models/UserModel";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {catchError, throwError} from "rxjs";
import {IRoleModel} from "../../models/IRoleModel";

@Injectable({
  providedIn: 'root'
})
export class UserService implements OnInit {

  private user!: UserModel;

  constructor(private authService: AuthService, private http: HttpClient) {
  }

  async ngOnInit(): Promise<void> {
    this.getUser()
      .then((data) => {
        this.user = data;
      })
      .catch((error) => {
        if (!environment.production) {
          console.log(error);
          this.authService.logOut();
        }
      });
  }

  getRole() : IRoleModel | null {
    if (this.user) {
      return this.user.role;
    }

    return null;
  }

  getUser() : Promise<UserModel> {

    if (this.user) {
      return new Promise((resolve, reject) => {
        resolve(this.user);
      });
    }

    return new Promise((resolve, reject) => {
      this.http.get<UserModel>(environment.apiUrl + "/auth/current-user")
        .pipe(
          catchError((error) => {
            reject(error);
            return throwError(error);
          })
        )
        .subscribe((data) => {
          resolve(data);
          return data;
        })
    });
  }
}
