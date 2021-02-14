import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';

import {SalespersonRoutingModule} from './salesperson-routing.module';
import {CheckoutsComponent} from './checkouts/checkouts.component';
import {OrdersComponent} from './orders/orders.component';
import {MaterialModule} from '../material/material.module';
import {SharedModule} from '../shared/shared.module';
import {SalespersonComponent} from './salesperson.component';


@NgModule({
  declarations: [CheckoutsComponent, OrdersComponent, SalespersonComponent],
  imports: [
    CommonModule,
    SalespersonRoutingModule,
    MaterialModule,
    SharedModule
  ]
})
export class SalespersonModule {
}
