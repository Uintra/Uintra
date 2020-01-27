import { TestBed } from '@angular/core/testing';

import { LeftNavigationService } from './left-navigation.service';

describe('LeftNavigationService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: LeftNavigationService = TestBed.get(LeftNavigationService);
    expect(service).toBeTruthy();
  });
});
