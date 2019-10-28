import { TestBed } from '@angular/core/testing';

import { PageResolverService } from './page-resolver.service';

describe('PageResolverService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: PageResolverService = TestBed.get(PageResolverService);
    expect(service).toBeTruthy();
  });
});
