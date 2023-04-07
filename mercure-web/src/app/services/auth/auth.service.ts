import {HttpClient, HttpHeaders, HttpResponse} from '@angular/common/http';
import {Injectable, OnInit} from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import {catchError, Observable, throwError} from 'rxjs';
import { UserModel } from 'src/app/models/UserModel';
import {environment} from "../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class AuthService implements OnInit {

  private token?: string;

  constructor(private http: HttpClient) {
   }

  ngOnInit(): void {
    let token = sessionStorage.getItem("token");
    if (token && token.length > 1) {
      this.token = token;
    }
  }

  isLogged(): boolean{
    return sessionStorage.getItem("token") !== null;
  }

  getSessionToken(): string | null {
    return sessionStorage.getItem("token");
  }

  getUserToken(state: string): Promise<{token: string}> {
    return new Promise((resolve, reject) => {
      this.http.get<{token: string}>(environment.apiUrl + "/auth/logged?state="+state)
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
