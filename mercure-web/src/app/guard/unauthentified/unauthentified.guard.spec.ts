import { TestBed } from '@angular/core/testing';

import { UnauthentifiedGuard } from './unauthentified.guard';
import {AuthService} from "../../services/auth/auth.service";
import {RouterTestingModule} from "@angular/router/testing";
import {HttpClientTestingModule} from "@angular/common/http/testing";

describe('UnauthentifiedGuard', () => {
  let guard: UnauthentifiedGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AuthService],
      imports: [ RouterTestingModule, HttpClientTestingModule ]
    });
    guard = TestBed.inject(UnauthentifiedGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
