import {TestBed} from '@angular/core/testing';

import {AuthentifiedGuard} from './authentified.guard';
import {AuthService} from "../../services/auth/auth.service";
import {RouterTestingModule} from "@angular/router/testing";
import {HttpClientTestingModule} from "@angular/common/http/testing";

describe('AuthentifiedGuard', () => {
  let guard: AuthentifiedGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AuthService],
      imports: [ RouterTestingModule, HttpClientTestingModule ]
    });
    guard = TestBed.inject(AuthentifiedGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
