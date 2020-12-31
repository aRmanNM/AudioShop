import {Injectable} from '@angular/core';
import {Observable, Subject} from 'rxjs';
import {Basket} from '../Models/Basket';
import {Course} from '../Models/course';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../environments/environment';
import {AuthService} from './auth.service';
import {Order} from '../Models/Order';
import {UserService} from './user.service';

@Injectable({
    providedIn: 'root',
})
export class OrderService {
    private baseUrl = environment.apiUrl + 'order/';
    basketSub = new Subject<any>();

    constructor(private http: HttpClient, private authService: AuthService, private userService: UserService) {
    }

    addCourseToBasket(course: Course): boolean {
        if (!localStorage.getItem('basket')) {
            const emptyBasket: Basket = {
                courseDtos: [course],
                totalPrice: course.price,
                // userId: this.authService.decodedToken.nameid
            };
            localStorage.setItem('basket', JSON.stringify(emptyBasket));
            this.basketSub.next('changed');
            return true;
        }

        const basket: Basket = JSON.parse(localStorage.getItem('basket'));
        if (basket.courseDtos.find((c) => c.id === course.id)) {
            return false;
        }

        basket.courseDtos.push(course);
        basket.totalPrice = this.calculateBasket();
        localStorage.setItem('basket', JSON.stringify(basket));
        this.basketSub.next('changed');
        return true;
    }

    CheckIfCourseIsRepetitive(courseId): any {
        this.authService.decodeToken();
        return this.userService.CheckIfCourseIsRepetitive(this.authService.decodedToken.nameid, courseId);
    }

    removeCourseFromBasket(course: Course) {
        const basket: Basket = JSON.parse(localStorage.getItem('basket'));
        const index = basket.courseDtos.findIndex((c) => c.id === course.id);
        if (index >= 0) {
            basket.courseDtos.splice(index, 1);
            localStorage.setItem('basket', JSON.stringify(basket));
            this.basketSub.next('changed');
            return;
        } else {
            return null;
        }
    }

    getBasketCount() {
        const basket: Basket = JSON.parse(localStorage.getItem('basket'));
        if (!basket) {
            return 0;
        }
        return basket.courseDtos.length;
    }

    watchStorage(): Observable<any> {
        return this.basketSub.asObservable();
    }

    getBasket(): Basket {
        const basket = localStorage.getItem('basket');
        if (!basket || basket === 'null') {
            return null;
        }
        return JSON.parse(localStorage.getItem('basket'));
    }

    clearBasket() {
        localStorage.removeItem('basket');
        this.basketSub.next('changed');
    }

    calculateBasket() {
        const basket: Basket = this.getBasket();
        let totalPrice = 0;
        basket?.courseDtos.forEach((course) => {
            totalPrice += course.price;
        });

        return totalPrice;
    }

    CreateOrder(basket: Basket) {
        basket.userId = this.authService.decodedToken.nameid;
        return this.http.post<Order>(this.baseUrl, basket);
    }

    PayOrder(order: Order) {
        window.open(environment.apiUrl + 'Payment/PaymentVerification/' + order.id, '_system');
    }

    setBasket(basket: Basket) {
        localStorage.setItem('basket', JSON.stringify(basket));
    }
}
