import { TestBed } from '@angular/core/testing';

import { NewsOverviewService } from './news-overview.service';

describe('NewsOverviewService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: NewsOverviewService = TestBed.get(NewsOverviewService);
    expect(service).toBeTruthy();
  });
});
