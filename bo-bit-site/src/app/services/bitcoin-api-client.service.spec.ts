import { TestBed } from '@angular/core/testing';

import { BitcoinApiClientService } from '../services/bitcoin-api-client.service';

describe('BitcoinApiClientService', () => {
  let service: BitcoinApiClientService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BitcoinApiClientService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
