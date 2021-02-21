import {Component, OnInit} from '@angular/core';
import {AdminService} from '../../services/admin.service';
import {Checkout} from '../../models/checkout';
import {SpinnerService} from '../../services/spinner.service';
import {MatDialog} from '@angular/material/dialog';
import {ActivatedRoute} from '@angular/router';
import {CheckoutEditComponent} from './checkout-edit/checkout-edit.component';

@Component({
  selector: 'app-checkouts',
  templateUrl: './checkouts.component.html',
  styleUrls: ['./checkouts.component.scss']
})
export class CheckoutsComponent implements OnInit {
  checkouts: Checkout[];
  dialogActive = false;
  status: boolean;
  userName = '';
  isFiltered = false;
  columnsToDisplay: string[];

  constructor(private adminService: AdminService,
              public spinnerService: SpinnerService,
              public dialog: MatDialog,
              private route: ActivatedRoute) {
  }

  ngOnInit(): void {

    this.route.params.subscribe(routeParams => {
      this.status = routeParams.status;
      if (this.status) {
        this.columnsToDisplay = ['id', 'userName', 'paymentAmount', 'paymentReceipt', 'createdAt', 'paidAt', 'status'];
      } else {
        this.columnsToDisplay = ['id', 'userName', 'paymentAmount', 'paymentReceipt', 'createdAt', 'paidAt', 'status', 'actions'];
      }
      this.getCheckouts();
    });
  }

  getCheckouts(): void {
    console.log(this.status + this.userName);
    this.adminService.getCheckouts(this.status, this.userName).subscribe((res) => {
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
      this.getCheckouts();
      this.dialogActive = false;
    });
  }

  clearUserName(): void {
    this.userName = '';
    this.getCheckouts();
  }

}
