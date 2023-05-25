import {Injectable, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {IRoleModel} from "../../models/IRoleModel";
import {environment} from "../../../environments/environment";
import {catchError, throwError} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class RoleService implements OnInit {

  roles: IRoleModel[] = [];

  constructor(private http: HttpClient) {
  }

  ngOnInit(): void {
  }

  getRoles(): Promise<IRoleModel[]> {
    return new Promise<IRoleModel[]>((resolve, reject) => {

      if (environment.production) {
        return reject('Not allowed in production');
      }

      this.http.get<IRoleModel[]>(environment.apiUrl + "/dev/roles")
        .pipe(
          catchError((error) => {
            reject(error);
            return throwError(error);
          })
        )
        .subscribe((data) => {
          this.roles = data;
          resolve(data);
          return data;
        })
    });
  }

  setRoles(roleNumber: number) {
    return new Promise((resolve, reject) => {
      this.http.post(environment.apiUrl + `/dev/roles/${roleNumber}`, null)
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
