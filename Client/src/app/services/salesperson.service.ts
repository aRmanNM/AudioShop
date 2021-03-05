import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {environment} from '../../environments/environment';
import {Observable, Subject} from 'rxjs';
import {Checkout} from '../models/checkout';
import {Credential} from '../models/credential';
import {Salesperson} from '../models/salesperson';

@Injectable({
  providedIn: 'root'
})
export class SalespersonService {
  baseUrl = environment.apiUrl + 'api/salesperson/';
  credentialUpdated = new Subject<any>();
  checkoutUpdated = new Subject<any>();
  ordersUpdated = new Subject<any>();

  updateCredential(): void {
    this.credentialUpdated.next();
  }

  updateCheckout(): void {
    this.checkoutUpdated.next();
  }

  updateOrders(): void {
    this.ordersUpdated.next();
  }

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

  getCredential(): Observable<Credential> {
    return this.http.get<Credential>(this.baseUrl + 'credential');
  }

  updateOrCreateCredential(credential: Credential): Observable<Credential> {
    return this.http.put<Credential>(this.baseUrl + 'credential', credential);
  }

  uploadPhoto(credentialId: number, photo: any, usedAs: string): any {
    console.log();
    const formData = new FormData();
    formData.append('file', photo);
    return this.http.post(this.baseUrl + 'credential/photo', formData, {
      params: new HttpParams().set('usedas', usedAs).set('credentialId', `${credentialId}`)
    });
  }

  checkSalespersonCredetialAccepted(): Observable<boolean> {
    return this.http.get<boolean>(this.baseUrl + 'credential/Accepted');
  }

  getInfo(): Observable<Salesperson> {
    return this.http.get<Salesperson>(this.baseUrl + 'info');
  }

}
