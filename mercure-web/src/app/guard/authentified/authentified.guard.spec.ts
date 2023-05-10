import { TestBed } from '@angular/core/testing';

import { AuthentifiedGuard } from './authentified.guard';

describe('AuthentifiedGuard', () => {
  let guard: AuthentifiedGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(AuthentifiedGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
