import {Component, ElementRef, Inject, OnInit, ViewChild} from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {Checkout} from '../../../models/checkout';
import {AdminService} from '../../../services/admin.service';
import {MatSnackBar} from '@angular/material/snack-bar';

interface DialogData {
  checkout: Checkout;
}

@Component({
  selector: 'app-checkout-edit',
  templateUrl: './checkout-edit.component.html',
  styleUrls: ['./checkout-edit.component.scss']
})
export class CheckoutEditComponent implements OnInit {
  paymentReceipt;

  constructor(public dialogRef: MatDialogRef<CheckoutEditComponent>,
              @Inject(MAT_DIALOG_DATA) public data: DialogData,
              private adminService: AdminService,
              private snackBar: MatSnackBar) {
  }

  ngOnInit(): void {}

  addPaymentReceipt(): void {
    this.data.checkout.paymentReceipt = this.paymentReceipt;
    this.data.checkout.status = true;
    this.adminService.editCheckout(this.data.checkout).subscribe((res) => {
      this.snackBar.open('رسید پرداخت ثبت شد', null, {
        duration: 2000,
      });
      this.closeDialog();
    });
  }


  closeDialog(): void {
    this.dialogRef.close();
  }

}
