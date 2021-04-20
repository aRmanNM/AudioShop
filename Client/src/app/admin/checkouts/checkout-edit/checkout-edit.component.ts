import {Component, ElementRef, Inject, OnInit, ViewChild} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {Checkout} from '../../../models/checkout';
import {AdminService} from '../../../services/admin.service';
import {MatSnackBar} from '@angular/material/snack-bar';
import {SpinnerService} from '../../../services/spinner.service';
import {environment} from '../../../../environments/environment';

interface DialogData {
  checkoutId: number;
}

@Component({
  selector: 'app-checkout-edit',
  templateUrl: './checkout-edit.component.html',
  styleUrls: ['./checkout-edit.component.scss']
})
export class CheckoutEditComponent implements OnInit {
  paymentReceipt;
  checkout: Checkout;
  showProgressBar = false;
  receiptImgUrl;
  bankCardImgUrl;
  baseCredsUrl = environment.apiUrl + 'Credentials/';
  baseChecksUrl = environment.apiUrl + 'Checkouts/';
  @ViewChild('fileInput') fileInput: ElementRef;

  constructor(public dialogRef: MatDialogRef<CheckoutEditComponent>,
              @Inject(MAT_DIALOG_DATA) public data: DialogData,
              private adminService: AdminService,
              private snackBar: MatSnackBar,
              public spinnerService: SpinnerService) {
  }

  ngOnInit(): void {
    this.getCheckoutWithInfo();
  }

  addPaymentReceipt(): void {
    this.checkout.paymentReceipt = this.paymentReceipt;
    this.checkout.status = true;
    this.adminService.editCheckout(this.checkout).subscribe((res) => {
      this.snackBar.open('رسید پرداخت ثبت شد', null, {
        duration: 2000,
      });
      this.closeDialog();
    });
  }


  closeDialog(): void {
    this.dialogRef.close();
  }

  getCheckoutWithInfo(): any {
    this.adminService.getCheckoutWithInfo(this.data.checkoutId).subscribe((res) => {
      this.checkout = res;
      if (this.checkout.receiptPhoto) {
        this.getCheckoutImage();
      }

      this.getBankCardImage();
    });
  }

  uploadPhoto(): any {
    const nativeElement = this.fileInput.nativeElement;
    this.adminService.uploadReceipt(this.data.checkoutId, nativeElement.files[0]).subscribe((res) => {
      this.checkout.receiptPhoto = res;
      this.getCheckoutImage();
    }, ((e) => {
      this.snackBar.open(e.error, null, {
        duration: 2000,
      });
    }));
  }

  getCheckoutImage(): void {
    this.receiptImgUrl = this.baseChecksUrl + this.checkout.userName + '/' + this.checkout.receiptPhoto.fileName;
  }

  getBankCardImage(): void {
    this.bankCardImgUrl = this.baseCredsUrl +
      this.checkout.userName + '/' + this.checkout.user.salespersonCredential.bankCardPhoto.fileName;
  }

}
