import {Component, OnInit} from '@angular/core';
import {CouponService} from '../../services/coupon.service';
import {Coupon} from '../../models/coupon';
import {MatDialog} from '@angular/material/dialog';
import {SpinnerService} from '../../services/spinner.service';
import {CouponsCreateEditComponent} from './coupons-create-edit/coupons-create-edit.component';

@Component({
  selector: 'app-coupons',
  templateUrl: './coupons.component.html',
  styleUrls: ['./coupons.component.scss']
})
export class CouponsComponent implements OnInit {
  coupons: Coupon[];
  dialogActive = false;
  columnsToDisplay = ['description', 'discount', 'code', 'isActive', 'actions'];

  constructor(private couponService: CouponService,
              public dialog: MatDialog,
              public spinnerService: SpinnerService) {
  }

  ngOnInit(): void {
    this.couponService.couponsUpdateEmitter.subscribe(() => {
      this.getCoupons();
    });

    this.couponService.onCouponsUpdate();
  }

  getCoupons(): void {
    this.couponService.getCoupons().subscribe((res) => {
      this.coupons = res;
    });
  }

  openAddOrEditCoupon(coupon: Coupon): void {
    this.dialogActive = true;
    const dialogRef = this.dialog.open(CouponsCreateEditComponent, {
      width: '400px',
      data: {coupon}
    });

    dialogRef.afterClosed().subscribe(res => {
      this.dialogActive = false;
    });
  }
}
