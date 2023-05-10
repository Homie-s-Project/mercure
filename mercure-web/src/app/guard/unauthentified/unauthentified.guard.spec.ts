import { TestBed } from '@angular/core/testing';

import { UnauthentifiedGuard } from './unauthentified.guard';

describe('UnauthentifiedGuard', () => {
  let guard: UnauthentifiedGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(UnauthentifiedGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
