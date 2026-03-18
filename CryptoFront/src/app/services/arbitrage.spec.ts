import { TestBed } from '@angular/core/testing';

import { Arbitrage } from './arbitrage';

describe('Arbitrage', () => {
  let service: Arbitrage;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Arbitrage);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
