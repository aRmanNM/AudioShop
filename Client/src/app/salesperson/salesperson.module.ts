import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';

import {SalespersonRoutingModule} from './salesperson-routing.module';
import {CheckoutsComponent} from './checkouts/checkouts.component';
import {OrdersComponent} from './orders/orders.component';
import {MaterialModule} from '../material/material.module';
import {SharedModule} from '../shared/shared.module';
import {SalespersonComponent} from './salesperson.component';
import {CredentialsComponent} from './credentials/credentials.component';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {PersianDatePipe} from '../pipes/persian-date.pipe';


@NgModule({
  declarations: [CheckoutsComponent, OrdersComponent, SalespersonComponent, CredentialsComponent, PersianDatePipe],
  imports: [
    CommonModule,
    SalespersonRoutingModule,
    MaterialModule,
    SharedModule,
    ReactiveFormsModule,
    FormsModule,
  ],
})
export class SalespersonModule {
}
