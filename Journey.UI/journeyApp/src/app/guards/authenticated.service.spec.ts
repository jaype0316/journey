import { TestBed } from '@angular/core/testing';

import { AuthenticatedGuard } from './auth-guard.service';

describe('AuthenticatedService', () => {
  let service: AuthenticatedGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AuthenticatedGuard);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
