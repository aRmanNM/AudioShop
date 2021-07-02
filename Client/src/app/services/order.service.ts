import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../environments/environment';
import {Observable} from 'rxjs';
import {OrderWithUserDetails} from '../models/order-with-user-details';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  baseUrl = environment.apiUrl + 'api/orders/';

  constructor(private http: HttpClient) {
  }

  getOrderWithUserInfo(orderId: number): Observable<OrderWithUserDetails> {
    return this.http.get<OrderWithUserDetails>(this.baseUrl + orderId);
  }

  updateOrder(orderId: number, order: OrderWithUserDetails): Observable<OrderWithUserDetails> {
    return this.http.put<OrderWithUserDetails>(this.baseUrl + orderId, order);
  }
}
