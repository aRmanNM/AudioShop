import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../environments/environment';
import {Observable} from 'rxjs';
import {Checkout} from '../models/checkout';

@Injectable({
  providedIn: 'root'
})
export class SalespersonService {
  baseUrl = environment.apiUrl + 'salesperson/';

  constructor(private http: HttpClient) {
  }

  getOrdersForCheckout(): Observable<any[]> {
    return this.http.get<any[]>(this.baseUrl + 'orders');
  }

  getSaleAmount(): Observable<number> {
    return this.http.get<number>(this.baseUrl + 'saleamount');
  }

  getTotalSaleAmount(): Observable<number> {
    return this.http.get<number>(this.baseUrl + 'totalsaleamount');
  }

  getCheckouts(): Observable<Checkout[]> {
    return this.http.get<Checkout[]>(this.baseUrl + 'checkouts');
  }

  createCheckout(): Observable<Checkout> {
    return this.http.post<Checkout>(this.baseUrl + 'checkouts', {});
  }

}
