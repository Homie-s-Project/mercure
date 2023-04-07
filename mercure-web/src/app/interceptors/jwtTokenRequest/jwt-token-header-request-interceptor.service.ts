import {Injectable} from '@angular/core';
import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from '@angular/common/http';
import {Observable} from 'rxjs';
import {AuthService} from "../../services/auth/auth.service";

@Injectable()
export class JwtTokenHeaderRequestInterceptor implements HttpInterceptor {

  constructor(private authService: AuthService) {
  }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {

    if (this.authService.isLogged()) {
      const authRequest = request.clone({
        setHeaders: {Authorization: 'Bearer ' + this.authService.getSessionToken()}
      });

      return next.handle(authRequest);
    }

    return next.handle(request);
  }
}
