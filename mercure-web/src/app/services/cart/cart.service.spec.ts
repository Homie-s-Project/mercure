import {TestBed} from '@angular/core/testing';

import {CartService} from './cart.service';
import {HttpClientTestingModule} from "@angular/common/http/testing";
import {AuthService} from "../auth/auth.service";
import {UserService} from "../user/user.service";

describe('CartService', () => {
  let service: CartService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AuthService, UserService],
      imports: [ HttpClientTestingModule ]
    })
      .compileComponents();

    service = TestBed.inject(CartService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
