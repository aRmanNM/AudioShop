import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {MatSnackBar} from '@angular/material/snack-bar';
import {SpinnerService} from '../../../services/spinner.service';
import {Coupon} from '../../../models/coupon';
import {CouponService} from '../../../services/coupon.service';
import {FormControl, FormGroup, Validators} from '@angular/forms';

class DialogData {
  coupon: Coupon;
}

@Component({
  selector: 'app-coupons-create-edit',
  templateUrl: './coupons-create-edit.component.html',
  styleUrls: ['./coupons-create-edit.component.scss']
})
export class CouponsCreateEditComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<CouponsCreateEditComponent>,
              @Inject(MAT_DIALOG_DATA) public data: DialogData,
              private couponService: CouponService,
              private snackBar: MatSnackBar,
              public spinnerService: SpinnerService) {
  }

  couponForm = new FormGroup(
    {
      id: new FormControl(''),
      discountPercentage: new FormControl('', [Validators.required]),
      description: new FormControl('', [Validators.required]),
      code: new FormControl(''),
      isActive: new FormControl('', [Validators.required]),
      userId: new FormControl('')
    }
  );

  ngOnInit(): void {
    if (this.data.coupon) {
      this.couponForm.setValue({
        id: this.data.coupon.id,
        discountPercentage: this.data.coupon.discountPercentage,
        code: this.data.coupon.code,
        userId: this.data.coupon.userId,
        description: this.data.coupon.description,
        isActive: this.data.coupon.isActive,
      });
    }
  }

  createOrEditCoupon(): void {
    if (this.data.coupon) {
      this.couponService.updateCoupon(this.couponForm.value).subscribe((res) => {
        this.snackBar.open('ویرایش کوپن موفقیت آمیز بود', null, {
          duration: 2000,
        });
        this.couponService.onCouponsUpdate();
        this.closeDialog();
      });
    } else {
      this.couponService.createCoupon(this.couponForm.value).subscribe((res) => {
        this.snackBar.open('کوپن جدید با موفقیت ایجاد شد', null, {
          duration: 2000,
        });
        this.couponService.onCouponsUpdate();
        this.closeDialog();
      });
    }
  }

  closeDialog(): void {
    this.dialogRef.close();
  }

}
