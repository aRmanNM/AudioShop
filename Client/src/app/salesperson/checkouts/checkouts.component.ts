import {Component, OnInit} from '@angular/core';
import {SalespersonService} from '../../services/salesperson.service';
import {Checkout} from '../../models/checkout';

@Component({
  selector: 'app-checkouts',
  templateUrl: './checkouts.component.html',
  styleUrls: ['./checkouts.component.scss']
})
export class CheckoutsComponent implements OnInit {
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
