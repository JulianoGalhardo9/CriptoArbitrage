import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { ArbitrageService } from './arbitrage';

describe('ArbitrageService', () => {
  let service: ArbitrageService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(),
        ArbitrageService
      ]
    });
    service = TestBed.inject(ArbitrageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});