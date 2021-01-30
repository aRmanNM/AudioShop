import {Component, OnInit} from '@angular/core';
import {SalespersonService} from '../../services/salesperson.service';
import {Checkout} from '../../models/checkout';

@Component({
  selector: 'app-salesperson-checkouts',
  templateUrl: './salesperson-checkouts.component.html',
  styleUrls: ['./salesperson-checkouts.component.scss']
})
export class SalespersonCheckoutsComponent implements OnInit {
  checkouts: Checkout[];
  totalSaleAmount: number;
  columnsToDisplay = ['amountToCheckout', 'createdAt', 'status', 'paymentReceipt', 'paidAt'];

  constructor(private salespersonService: SalespersonService) {
  }

  ngOnInit(): void {
    this.getCheckouts();
    this.getTotalSaleAmount();
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
