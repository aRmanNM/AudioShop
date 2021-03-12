import {Component, OnInit, ViewChild} from '@angular/core';
import {SalespersonService} from '../../services/salesperson.service';
import {SpinnerService} from '../../services/spinner.service';
import {ConfigService} from '../../services/config.service';
import {MatSnackBar} from '@angular/material/snack-bar';
import {Salesperson} from '../../models/salesperson';
import {MatPaginator} from '@angular/material/paginator';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss']
})
export class OrdersComponent implements OnInit {
  orders: any[];
  salesAmount: number;
  salesperson: Salesperson;
  checkoutThreshold: number;
  credentialAccepted;
  columnsToDisplay = ['priceToPay', 'date', 'salespersonShareAmount', 'basketItemsNames'];
  totalItems: number;
  pageSize = 10;
  pageIndex = 0;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(private salespersonService: SalespersonService,
              public spinnerService: SpinnerService,
              private configService: ConfigService,
              private snackBar: MatSnackBar) {
  }

  ngOnInit(): void {

    this.salespersonService.ordersUpdated.subscribe((res) => {
      this.getEverything();
    });

    this.getInfo();
    this.salespersonService.updateOrders();
  }

  getEverything(): void {
    this.getSaleAmount();
    this.getOrdersForCheckout();
    this.getCheckoutThreshold();
    this.checkSalespersonCredetialAccepted();
  }

  getOrdersForCheckout(): void {
    this.salespersonService.getOrdersForCheckout(this.pageIndex, this.pageSize).subscribe((res) => {
      this.orders = res.items;
      this.totalItems = res.totalItems;
    });
  }

  getSaleAmount(): void {
    this.salespersonService.getSaleAmount().subscribe((res) => {
      this.salesAmount = res;
    });
  }

  createCheckout(): void {
    this.salespersonService.createCheckout().subscribe((res) => {
      this.snackBar.open('درخواست ثبت شد. برای بررسی وضعیت به بخش درخواست ها برید', null, {
        duration: 5000,
      });

      this.salespersonService.updateCheckout();
    });
  }

  getCheckoutThreshold(): void {
    this.configService.getConfig('DefaultCheckoutThreshold').subscribe((res) => {
      this.checkoutThreshold = Number(res.value);
    });
  }

  checkSalespersonCredetialAccepted(): void {
    this.salespersonService.checkSalespersonCredetialAccepted().subscribe((res) => {
      this.credentialAccepted = res;
    });
  }

  refresh(): void {
    this.salespersonService.updateOrders();
  }

  getInfo(): void {
    this.salespersonService.getInfo().subscribe((res) => {
      this.salesperson = res;
    });
  }

  changePage(): void {
    this.pageIndex = this.paginator.pageIndex;
    this.pageSize = this.paginator.pageSize;
    this.salespersonService.updateOrders();
  }

}
