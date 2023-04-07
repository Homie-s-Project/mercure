import {TestBed} from '@angular/core/testing';

import {JwtTokenHeaderRequestInterceptor} from './jwt-token-header-request-interceptor.service';
import {HttpClientTestingModule} from "@angular/common/http/testing";

describe('JwtTokenHeaderInterceptor', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [JwtTokenHeaderRequestInterceptor],
    imports: [HttpClientTestingModule],
  }));

  it('should be created', () => {
    const interceptor: JwtTokenHeaderRequestInterceptor = TestBed.inject(JwtTokenHeaderRequestInterceptor);
    expect(interceptor).toBeTruthy();
  });
});
