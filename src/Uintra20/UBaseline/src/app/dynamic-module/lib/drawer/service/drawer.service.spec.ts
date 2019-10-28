import { TestBed } from '@angular/core/testing';

import { DrawerService } from './drawer.service';

describe('DrawerService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DrawerService = TestBed.get(DrawerService);
    expect(service).toBeTruthy();
  });
});
