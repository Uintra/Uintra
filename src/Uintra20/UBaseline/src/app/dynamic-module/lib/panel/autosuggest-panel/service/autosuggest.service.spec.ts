import { TestBed } from '@angular/core/testing';

import { AutosuggestService } from './autosuggest.service';

describe('AutosuggestService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AutosuggestService = TestBed.get(AutosuggestService);
    expect(service).toBeTruthy();
  });
});
