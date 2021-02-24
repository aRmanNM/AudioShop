import {Component, OnInit} from '@angular/core';
import {Checkout} from '../../../models/checkout';
import {AdminService} from '../../../services/admin.service';
import {SpinnerService} from '../../../services/spinner.service';
import {MatDialog} from '@angular/material/dialog';
import {ActivatedRoute} from '@angular/router';
import {CheckoutEditComponent} from '../checkout-edit/checkout-edit.component';

@Component({
  selector: 'app-checkout-done',
  templateUrl: './checkout-done.component.html',
  styleUrls: ['./checkout-done.component.scss']
})
export class CheckoutDoneComponent implements OnInit {
  checkouts: Checkout[];
  dialogActive = false;
  status: boolean;
  userName = '';
  isFiltered = false;
  columnsToDisplay = ['userName', 'paymentAmount', 'paymentReceipt', 'createdAt', 'paidAt'];

  constructor(private adminService: AdminService,
              public spinnerService: SpinnerService,
              public dialog: MatDialog,
              private route: ActivatedRoute) {
  }

  ngOnInit(): void {
    this.adminService.updatedEmmiter.subscribe(() => {
      this.getCheckouts();
    });

    this.adminService.update();
  }

  getCheckouts(): void {
    this.adminService.getCheckouts(true, this.userName).subscribe((res) => {
      this.checkouts = res;
    });
  }

  openAddOrEditDialog(checkout: Checkout): void {
    this.dialogActive = true;
    const dialogRef = this.dialog.open(CheckoutEditComponent, {
      width: '400px',
      data: {checkout}
    });

    dialogRef.afterClosed().subscribe(res => {
      // this.getCheckouts();
      this.dialogActive = false;
    });
  }

  clearUserName(): void {
    this.userName = '';
    this.getCheckouts();
  }

}
