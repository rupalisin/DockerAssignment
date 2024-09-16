import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable()
export class BidService {
    private apiUrl = environment.baseApiUrl + 'Bid';

constructor(private http: HttpClient) { }

placeBid(productId: number, amount: number): Observable<any> {
    const bid = { productId, amount };
    return this.http.post(`${this.apiUrl}`, bid);
  }

  getBidsByUser(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/user-bids`);
  }

}
