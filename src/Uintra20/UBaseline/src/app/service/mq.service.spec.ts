import { TestBed } from '@angular/core/testing';

import { MqService } from './mq.service';

describe('MqService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: MqService = TestBed.get(MqService);
    expect(service).toBeTruthy();
  });
});
