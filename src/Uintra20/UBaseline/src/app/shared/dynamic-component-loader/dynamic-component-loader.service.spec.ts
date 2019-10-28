import { TestBed } from '@angular/core/testing';

import { DynamicComponentLoaderService } from './dynamic-component-loader.service';

describe('DynamicComponentLoaderService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DynamicComponentLoaderService = TestBed.get(DynamicComponentLoaderService);
    expect(service).toBeTruthy();
  });
});
