import {Component, OnInit} from '@angular/core';
import {OrderService} from '../../services/order.service';
import {OrderWithUserDetails} from '../../models/order-with-user-details';
import {MatDialog} from '@angular/material/dialog';
import {SpinnerService} from '../../services/spinner.service';
import {MatSnackBar} from '@angular/material/snack-bar';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss']
})
export class OrdersComponent implements OnInit {
  orderId: number;
  order: OrderWithUserDetails;
  dialogActive = false;
  receipt;

  constructor(private orderService: OrderService, public spinnerService: SpinnerService, private snackBar: MatSnackBar,) {
  }

  ngOnInit(): void {
  }

  getOrder(): void {
    this.orderService.getOrderWithUserInfo(this.orderId).subscribe((res) => {
      this.order = res;
    }, error => {
      this.order = null;
    });
  }

  addReceiptAndToggle(): void {
    this.order.status = !this.order.status;
    this.order.paymentReceipt = this.receipt;
    this.orderService.updateOrder(this.orderId, this.order).subscribe((res) => {
      this.order = res;
      this.snackBar.open('ویرایش انجام شد', null, {
        duration: 2000,
      });
    }, error => {
      console.log(error);
      this.snackBar.open('ویرایش شکست خورد', null, {
        duration: 2000,
      });
    });
  }

}
