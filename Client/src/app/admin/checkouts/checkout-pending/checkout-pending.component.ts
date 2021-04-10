import {Component, OnInit} from '@angular/core';
import {Checkout} from '../../../models/checkout';
import {AdminService} from '../../../services/admin.service';
import {SpinnerService} from '../../../services/spinner.service';
import {MatDialog} from '@angular/material/dialog';
import {ActivatedRoute} from '@angular/router';
import {CheckoutEditComponent} from '../checkout-edit/checkout-edit.component';

@Component({
  selector: 'app-checkout-pending',
  templateUrl: './checkout-pending.component.html',
  styleUrls: ['./checkout-pending.component.scss']
})
export class CheckoutPendingComponent implements OnInit {
  checkouts: Checkout[];
  dialogActive = false;
  status: boolean;
  userName = '';
  isFiltered = false;
  columnsToDisplay = ['userName', 'paymentAmount', 'createdAt', 'actions'];

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
    this.adminService.getCheckouts(false, this.userName).subscribe((res) => {
      this.checkouts = res;
    });
  }

  openAddOrEditDialog(checkoutId: number): void {
    this.dialogActive = true;
    const dialogRef = this.dialog.open(CheckoutEditComponent, {
      width: '400px',
      data: {checkoutId}
    });

    dialogRef.afterClosed().subscribe(res => {
      // this.getCheckouts();
      this.adminService.update();
      this.dialogActive = false;
    });
  }

  clearUserName(): void {
    this.userName = '';
    // this.getCheckouts();
    this.adminService.update();
  }

  refreshCheckouts(): void {
    this.adminService.update();
  }
}
