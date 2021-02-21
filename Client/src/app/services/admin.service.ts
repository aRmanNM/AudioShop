import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Checkout} from '../models/checkout';
import {environment} from '../../environments/environment';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.apiUrl + 'api/admin';

  constructor(private http: HttpClient) {
  }

  getCheckouts(status: boolean, userName: string): Observable<Checkout[]> {
    return this.http.get<Checkout[]>(`${this.baseUrl}/checkouts`, {
      params: new HttpParams().set('status', `${status}`).set('userName', userName)
    });
  }

  editCheckout(checkout: Checkout): Observable<Checkout> {
    return this.http.put<Checkout>(`${this.baseUrl}/checkouts/${checkout.id}`, checkout);
  }
}
