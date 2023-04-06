import { TestBed } from '@angular/core/testing';
import {HttpClient, HttpHeaders, HttpResponse} from '@angular/common/http';
import { CookieService } from 'ngx-cookie-service';
import { Observable } from 'rxjs';
import { UserModel } from 'src/app/models/UserModel';

import { AuthService } from './auth.service';

describe('AuthService', () => {
  let service: AuthService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ AuthService ],
      providers: [HttpClient, HttpHeaders, HttpResponse, CookieService, Observable, UserModel]
    })
    .compileComponents();
    service = TestBed.inject(AuthService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
