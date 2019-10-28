import { TestBed } from '@angular/core/testing';

import { RouteConfigService } from './route-config.service';

describe('RouteConfigService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: RouteConfigService = TestBed.get(RouteConfigService);
    expect(service).toBeTruthy();
  });
});
