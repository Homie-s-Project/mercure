import {HttpClient, HttpHeaders, HttpResponse} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { Observable } from 'rxjs';
import { UserModel } from 'src/app/models/UserModel';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private token: string;

  constructor(private http: HttpClient,
    private cookie: CookieService) {
    this.token = this.cookie.get('jwt');
   }

  // Récupèration de l'user selon le Token JWT
  getProfile(): Observable<HttpResponse<UserModel>> {
    const header = new HttpHeaders ({
      "Content-Type": "application/json",
      "Authorization": `${this.getToken()}`
    });

    return this.http.get<UserModel>('http://localhost:5000/auth/currentUser', {headers: header, observe: 'response'});
  }

  // Retourne Bearer Token
  getToken() :string {
    return `Bearer ${this.token}`;
}
}
