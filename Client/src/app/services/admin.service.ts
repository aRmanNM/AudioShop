import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Checkout} from '../models/checkout';
import {environment} from '../../environments/environment';
import {Observable, Subject} from 'rxjs';
import {PaginatedResult} from '../models/paginated-result';
import {Salesperson} from '../models/salesperson';
import {SalespersonCredStatus} from '../models/salespersonCredStatus';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.apiUrl + 'api/admin';
  updatedEmmiter = new Subject();
  salespersonsUpdateEmmiter = new Subject();

  constructor(private http: HttpClient) {
  }

  update(): any {
    this.updatedEmmiter.next();
  }

  onSalespersonsUpdate(): any {
    this.salespersonsUpdateEmmiter.next();
  }

  getCheckouts(status: boolean, userName: string): Observable<Checkout[]> {
    return this.http.get<Checkout[]>(`${this.baseUrl}/checkouts`, {
      params: new HttpParams().set('status', `${status}`).set('userName', userName)
    });
  }

  editCheckout(checkout: Checkout): Observable<Checkout> {
    return this.http.put<Checkout>(`${this.baseUrl}/checkouts/${checkout.id}`, checkout);
  }

  getCheckoutWithInfo(checkoutId: number): Observable<Checkout> {
    return this.http.get<Checkout>(`${this.baseUrl}/checkouts/${checkoutId}`);
  }

  getAllSalespersons(search: string,
                     credSatus: SalespersonCredStatus,
                     pageNumber: number, pageSize: number): Observable<PaginatedResult<Salesperson>> {
    return this.http.get<PaginatedResult<Salesperson>>(`${this.baseUrl}/salespersons`, {
      params: new HttpParams()
        .set('search', search)
        .set('status', `${credSatus}`)
        .set('pageNumber', `${pageNumber + 1}`)
        .set('pageSize', `${pageSize}`)
    });
  }

  getSalesperson(userId: string): Observable<Salesperson> {
    return this.http.get<Salesperson>(`${this.baseUrl}/salespersons/${userId}`);
  }

  updateSalespersonCredential(userId: string, accepted: boolean, message: string): Observable<any> {
    return this.http.put<any>(`${this.baseUrl}/salespersons/${userId}/credential`, null, {
      params: new HttpParams()
        .set('message', message)
        .set('accepted', accepted.toString())
    });
  }

  updateSalesperson(userId: string, salesperson: Salesperson): Observable<any> {
    return this.http.put<any>(`${this.baseUrl}/salespersons/${userId}`, salesperson);
  }
}
