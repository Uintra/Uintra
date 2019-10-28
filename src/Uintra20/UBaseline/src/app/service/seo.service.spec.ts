import { TestBed } from '@angular/core/testing';

import { SEOService } from './seo.service';

describe('SEOService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SEOService = TestBed.get(SEOService);
    expect(service).toBeTruthy();
  });
});
