import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {Checkout} from '../../../models/checkout';
import {environment} from '../../../../environments/environment';

interface DialogData {
  checkout: Checkout;
}

@Component({
  selector: 'app-receipt-info',
  templateUrl: './receipt-info.component.html',
  styleUrls: ['./receipt-info.component.scss']
})
export class ReceiptInfoComponent implements OnInit {
  receiptImgUrl;
  baseChecksUrl = environment.apiUrl + 'Checkouts/';

  constructor(public dialogRef: MatDialogRef<ReceiptInfoComponent>,
              @Inject(MAT_DIALOG_DATA) public data: DialogData) {
  }

  ngOnInit(): void {
    this.getReceiptImagegetCheckoutImage();
  }

  getReceiptImagegetCheckoutImage(): void {
    this.receiptImgUrl = this.baseChecksUrl +
      this.data.checkout.userName + '/' + this.data.checkout.receiptPhoto.fileName;
  }

}
