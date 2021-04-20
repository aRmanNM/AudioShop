import {Component, OnInit} from '@angular/core';
import {SalespersonService} from '../../services/salesperson.service';
import {Checkout} from '../../models/checkout';
import {SpinnerService} from '../../services/spinner.service';
import {MatDialog} from '@angular/material/dialog';
import {ReceiptInfoComponent} from './receipt-info/receipt-info.component';

@Component({
  selector: 'app-checkouts',
  templateUrl: './checkouts.component.html',
  styleUrls: ['./checkouts.component.scss']
})
export class CheckoutsComponent implements OnInit {
  checkouts: Checkout[];
  totalSaleAmount: number;
  dialogActive = false;
  columnsToDisplay = ['amountToCheckout', 'createdAt', 'status', 'paymentReceipt', 'paidAt', 'actions'];

  constructor(private salespersonService: SalespersonService,
              public spinnerService: SpinnerService,
              public dialog: MatDialog) {
  }

  ngOnInit(): void {
    this.salespersonService.checkoutUpdated.subscribe((res) => {
      this.getCheckouts();
      this.getTotalSaleAmount();
    });

    this.salespersonService.updateCheckout();
  }

  getCheckouts(): void {
    this.salespersonService.getCheckouts().subscribe((res) => {
      this.checkouts = res;
    });
  }

  getTotalSaleAmount(): void {
    this.salespersonService.getTotalSaleAmount().subscribe((res) => {
      this.totalSaleAmount = res;
    });
  }

  refresh(): void {
    this.salespersonService.updateCheckout();
  }

  openReceiptInfoDialog(checkout: Checkout): void {
    this.dialogActive = true;
    const dialogRef = this.dialog.open(ReceiptInfoComponent, {
      width: '400px',
      data: {checkout}
    });

    dialogRef.afterClosed().subscribe(res => {
      this.dialogActive = false;
    });
  }

}
