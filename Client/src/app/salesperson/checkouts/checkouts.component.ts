import {Component, OnInit} from '@angular/core';
import {SalespersonService} from '../../services/salesperson.service';
import {Checkout} from '../../models/checkout';
import {SpinnerService} from '../../services/spinner.service';

@Component({
  selector: 'app-checkouts',
  templateUrl: './checkouts.component.html',
  styleUrls: ['./checkouts.component.scss']
})
export class CheckoutsComponent implements OnInit {
  checkouts: Checkout[];
  totalSaleAmount: number;

  columnsToDisplay = ['amountToCheckout', 'createdAt', 'status', 'paymentReceipt', 'paidAt'];

  constructor(private salespersonService: SalespersonService,
              public spinnerService: SpinnerService) {
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

}
