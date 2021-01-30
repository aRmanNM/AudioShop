import {Component, OnInit} from '@angular/core';
import {SalespersonService} from '../../services/salesperson.service';

@Component({
  selector: 'app-salesperson-orders',
  templateUrl: './salesperson-orders.component.html',
  styleUrls: ['./salesperson-orders.component.scss']
})
export class SalespersonOrdersComponent implements OnInit {
  orders: any[];
  salesAmount: number;
  columnsToDisplay = ['priceToPay', 'date', 'salespersonShareAmount', 'basketItemsNames'];

  constructor(private salespersonService: SalespersonService) {

  }

  ngOnInit(): void {
    this.getSaleAmount();
    this.getOrdersForCheckout();
  }

  getOrdersForCheckout(): void {
    this.salespersonService.getOrdersForCheckout().subscribe((res) => {
      this.orders = res;
    });
  }

  getSaleAmount(): void {
    this.salespersonService.getSaleAmount().subscribe((res) => {
      this.salesAmount = res;
    });
  }

  createCheckout(): void {
    this.salespersonService.createCheckout().subscribe((res) => {
      console.log('checkout created!', res);
      this.getSaleAmount();
    });
  }

}
