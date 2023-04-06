import { TestBed } from '@angular/core/testing';

import { JwtTokenHeaderRequestInterceptor } from './jwt-token-header-request-interceptor.service';

describe('JwtTokenHeaderInterceptor', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      JwtTokenHeaderRequestInterceptor
      ]
  }));

  it('should be created', () => {
    const interceptor: JwtTokenHeaderRequestInterceptor = TestBed.inject(JwtTokenHeaderRequestInterceptor);
    expect(interceptor).toBeTruthy();
  });
});
