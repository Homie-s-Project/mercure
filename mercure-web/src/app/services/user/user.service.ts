import {Injectable, OnInit} from '@angular/core';
import {AuthService} from "../auth/auth.service";
import {UserModel} from "../../models/UserModel";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {catchError, throwError} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class UserService implements OnInit {

  private user?: UserModel;

  constructor(private authService: AuthService, private http: HttpClient) {
  }

  async ngOnInit(): Promise<void> {
    this.user = await this.getUser();
  }

  getUser() : Promise<UserModel> {

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
