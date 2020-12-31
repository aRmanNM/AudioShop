import {Component, OnInit} from '@angular/core';
import {Basket} from '../Models/Basket';
import {OrderService} from '../Services/order.service';
import {CourseService} from '../Services/course.service';
import {ToastService} from '../Services/toast.service';
import {Order} from '../Models/Order';
import {AuthService} from '../Services/auth.service';
import {Router} from '@angular/router';
import {UserService} from '../Services/user.service';

@Component({
    selector: 'app-basket',
    templateUrl: 'basket.page.html',
    styleUrls: ['basket.page.scss'],
})
export class BasketPage implements OnInit {
    basket: Basket;
    totalPrice: number;

    constructor(
        private shopService: CourseService,
        private toast: ToastService,
        private orderService: OrderService,
        private authService: AuthService,
        private router: Router,
        private userService: UserService
    ) {
    }

    ngOnInit() {
        this.updateBasket();
        this.orderService.watchStorage().subscribe((data: any) => {
            this.updateBasket();
        });
    }

    getBasket() {
        this.basket = this.orderService.getBasket();
    }

    getTotalPrice() {
        this.totalPrice = this.orderService.calculateBasket();
    }

    removeCourseFromBasket(course) {
        this.orderService.removeCourseFromBasket(course);
    }

    CreateOrder() {
        if (!this.authService.loggedIn()) {
            this.router.navigate(['tabs/user/login'], {
                // queryParams: { returnUrl: state.url },
            });

            return;
        }

        this.orderService.CreateOrder(this.basket).subscribe((res) => {
            console.log(res);
            this.orderService.clearBasket();
            this.toast.presentToast('انتقال به سایت پذیرنده');
            this.PayOrder(res);
        });
    }

    PayOrder(order: Order) {
        this.orderService.PayOrder(order);
    }

    updateBasket() {
        this.getBasket();
        this.getTotalPrice();
    }


}
