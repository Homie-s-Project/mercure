import { TestBed } from '@angular/core/testing';

import { JwtTokenResponseHeaderInterceptor } from './jwt-token-response-header.interceptor';

describe('JwtTokenResponseHeaderInterceptor', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      JwtTokenResponseHeaderInterceptor
      ]
  }));

  it('should be created', () => {
    const interceptor: JwtTokenResponseHeaderInterceptor = TestBed.inject(JwtTokenResponseHeaderInterceptor);
    expect(interceptor).toBeTruthy();
  });
});
