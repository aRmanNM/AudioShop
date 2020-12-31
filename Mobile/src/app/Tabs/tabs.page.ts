import {Component, OnInit} from '@angular/core';
import {OrderService} from '../Services/order.service';

@Component({
    selector: 'app-tabs',
    templateUrl: 'tabs.page.html',
    styleUrls: ['tabs.page.scss'],
})
export class TabsPage implements OnInit {
    courseCount: number;

    constructor(private basketService: OrderService) {
    }

    ngOnInit() {
        this.courseCount = this.basketService.getBasketCount();
        this.basketService.watchStorage().subscribe((data: any) => {
            this.courseCount = this.basketService.getBasketCount();
        });
    }
}
