import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';
import {AuthService} from 'src/app/Services/auth.service';
import {UserService} from '../../Services/user.service';
import {OrderService} from '../../Services/order.service';
import {ToastService} from '../../Services/toast.service';
import {Basket} from '../../Models/Basket';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
    error: string;
    returnUrl: string;
    private loginForm: FormGroup;

    constructor(
        private authService: AuthService,
        private router: Router,
        private activatedRoute: ActivatedRoute,
        private formBuilder: FormBuilder,
        private orderService: OrderService,
        private userService: UserService,
        private toast: ToastService,
    ) {
        this.loginForm = this.formBuilder.group({
            email: new FormControl('', [Validators.required]),
            password: new FormControl('', [Validators.required]),
        });
    }

    ngOnInit() {
        this.returnUrl =
            this.activatedRoute.snapshot.queryParams.returnUrl || 'Tabs/Shop';
    }

    login() {
        this.error = null;
        this.authService.login(this.loginForm.value).subscribe(
            () => {
                const basket: Basket = this.orderService.getBasket();
                if (basket !== null) {
                    basket.userId = this.authService.decodedToken.nameid;
                    console.log(basket);
                    this.userService.RefineRepetitiveCourses(basket).subscribe((res) => {
                        console.log(res);
                        this.orderService.setBasket(res);
                        this.toast.presentToast('سبد خرید بروز رسانی شد');
                        this.orderService.basketSub.next('changed');
                    });
                }

                this.router.navigateByUrl(this.returnUrl);
            },
            (er) => {
                this.error = er.error;
                this.loginForm.reset();
                console.log('login failed!');
            }
        );
    }
}
