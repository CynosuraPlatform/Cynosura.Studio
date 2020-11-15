import { TestBed } from '@angular/core/testing';

import { PropertiesService } from './properties.service';

describe('PropertiesService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: PropertiesService = TestBed.inject(PropertiesService);
    expect(service).toBeTruthy();
  });
});
