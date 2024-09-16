import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable()
export class ProductService {

    private apiUrl = environment.baseApiUrl + 'Product';

constructor(private http : HttpClient) { }

getProducts() : Observable<any[]> {
    return this.http.get<any[]> (`${this.apiUrl}`)
}

getProductsAddedByUser(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/user-products`);
  }

  getProductsBoughtByUser(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/bought-products`);
  }

  addProduct(product: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}`, product);
  }

  // In product.service.ts
getProduct(id: string): Observable<any> {
  return this.http.get<any>(`${this.apiUrl}${id}`);
}

}
