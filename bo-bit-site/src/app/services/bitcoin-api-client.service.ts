import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment'
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, timer, switchMap, catchError, of, Subject } from 'rxjs';
import { BitcoinPricesResponse } from '../interfaces/bitcoinPricesResponse'

@Injectable({
  providedIn: 'root',
  
})
export class BitcoinApiClientService {
  private readonly apiUrl = (environment as any).apiUrl;
  private readonly pollingInterval = 30000; // 30 seconds
  


  constructor(private http: HttpClient) { }

  /**
   * Fetch Bitcoin price data for a specified time period.
   * @param from - Start date (ISO string or timestamp)
   * @param to - End date (ISO string or timestamp)
   * @returns Observable of the fetched data
   */
  private fetchBitcoinPrices(from: string, to: string): Observable<BitcoinPricesResponse | never[]> {
    //const params = new HttpParams().set('from', from).set('to', to);

    return this.http.get<BitcoinPricesResponse>(`${this.apiUrl}/BitcoinPrices`, { }).pipe(
      catchError((error) => {
        console.error('Error fetching Bitcoin prices:', error);
        return of([]); // Return an empty array on error
      })
    );
  }

  /**
   * Poll Bitcoin price data every 30 seconds.
   * @returns Observable of the polled data
   */
  pollBitcoinPrices(): Observable<BitcoinPricesResponse | never[]> {
    let subject = new Subject<BitcoinPricesResponse | never[]>();
    
    return timer(0, this.pollingInterval).pipe(
      switchMap(() => 
        {
          let nowDate = new Date();
          let to = nowDate.toISOString();
          let from = new Date(nowDate.setHours(nowDate.getHours()-1)).toISOString();
          
          return this.fetchBitcoinPrices(from, to);
        })
    );
  }
}
