import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';

import {DashboardRoutingModule} from './dashboard-routing.module';
import {DashboardComponent} from './dashboard.component';
import {MaterialModule} from '../material/material.module';
import {SidebarComponent} from './sidebar/sidebar.component';
import {TopbarComponent} from './topbar/topbar.component';
import {TestComponent} from './test/test.component';
import { SalespersonOrdersComponent } from './salesperson-orders/salesperson-orders.component';
import { SalespersonCheckoutsComponent } from './salesperson-checkouts/salesperson-checkouts.component';


@NgModule({
  declarations: [DashboardComponent, SidebarComponent, TopbarComponent, TestComponent, SalespersonOrdersComponent, SalespersonCheckoutsComponent],
  imports: [
    CommonModule,
    DashboardRoutingModule,
    MaterialModule
  ]
})
export class DashboardModule {
}
