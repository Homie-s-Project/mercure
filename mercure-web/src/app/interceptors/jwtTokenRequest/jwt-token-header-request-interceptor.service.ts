import {Injectable} from '@angular/core';
import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from '@angular/common/http';
import {Observable} from 'rxjs';

@Injectable()
export class JwtTokenHeaderRequestInterceptor implements HttpInterceptor {

  constructor() {
  }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let token = sessionStorage.getItem("token");

    if (token != null) {
      const authRequest = request.clone({
        setHeaders: {Authorization: 'Bearer ' + token}
      });

      return next.handle(authRequest);
    }

    return next.handle(request);
  }
}
