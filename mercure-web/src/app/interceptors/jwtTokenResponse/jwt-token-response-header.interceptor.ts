import {Injectable} from '@angular/core';
import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse} from '@angular/common/http';
import {filter, map, Observable, tap} from 'rxjs';

@Injectable()
export class JwtTokenResponseHeaderInterceptor implements HttpInterceptor {

  constructor() {
  }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      tap(
        (event: HttpEvent<any>) => {},
        (error: any) => {
          if (error.status === 401 || error.status === 498 || error.status === 0) {
            sessionStorage.removeItem("token"); // Supprime le sessionStorage
          }
        }
      )
    );

  }
}
