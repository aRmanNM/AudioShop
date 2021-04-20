import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {Checkout} from '../../../models/checkout';
import {AdminService} from '../../../services/admin.service';
import {environment} from '../../../../environments/environment';

interface DialogData {
  checkoutId: number;
}

@Component({
  selector: 'app-receipt-info',
  templateUrl: './receipt-info.component.html',
  styleUrls: ['./receipt-info.component.scss']
})
export class ReceiptInfoComponent implements OnInit {
  checkout: Checkout;
  receiptImgUrl;
  baseChecksUrl = environment.apiUrl + 'Checkouts/';

  constructor(public dialogRef: MatDialogRef<ReceiptInfoComponent>,
              @Inject(MAT_DIALOG_DATA) public data: DialogData,
              private adminService: AdminService) {
  }

  ngOnInit(): void {
    this.getCheckoutWithInfo();
  }

  getCheckoutWithInfo(): any {
    this.adminService.getCheckoutWithInfo(this.data.checkoutId).subscribe((res) => {
      this.checkout = res;
      if (this.checkout.receiptPhoto) {
        this.getCheckoutImage();
      }
    });
  }

  getCheckoutImage(): void {
    this.receiptImgUrl = this.baseChecksUrl + this.checkout.userName + '/' + this.checkout.receiptPhoto.fileName;
  }
}
