import { TestBed } from '@angular/core/testing';

import { GoogleGeolocationService } from './google-geolocation.service';

describe('GoogleGeolocationService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: GoogleGeolocationService = TestBed.get(GoogleGeolocationService);
    expect(service).toBeTruthy();
  });
});
