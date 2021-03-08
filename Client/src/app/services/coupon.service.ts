import {Injectable} from '@angular/core';
import {environment} from '../../environments/environment';
import {Observable, Subject} from 'rxjs';
import {Coupon} from '../models/coupon';
import {HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CouponService {
  baseUrl = environment.apiUrl + 'api/coupons/';
  couponsUpdateEmitter = new Subject();

  constructor(private http: HttpClient) {
  }

  onCouponsUpdate(): void {
    this.couponsUpdateEmitter.next();
  }

  getCoupons(): Observable<Coupon[]> {
    return this.http.get<Coupon[]>(this.baseUrl);
  }

  createCoupon(coupon: Coupon): Observable<Coupon> {
    const couponToCreate = {
      discountPercentage: coupon.discountPercentage,
      description: coupon.description,
      isActive: coupon.isActive
    };

    return this.http.post<Coupon>(this.baseUrl, couponToCreate);
  }

  updateCoupon(coupon: Coupon): Observable<Coupon> {
    return this.http.put<Coupon>(this.baseUrl, coupon);
  }
}
