import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';

import {DashboardRoutingModule} from './dashboard-routing.module';
import {DashboardComponent} from './dashboard.component';
import {SidebarComponent} from './sidebar/sidebar.component';
import {TopbarComponent} from './topbar/topbar.component';
import {TestComponent} from './test/test.component';
import {OverviewComponent} from './overview/overview.component';
import {MaterialModule} from '../material/material.module';


@NgModule({
  declarations: [DashboardComponent, SidebarComponent, TopbarComponent, TestComponent, OverviewComponent],
  imports: [
    CommonModule,
    DashboardRoutingModule,
    MaterialModule
  ]
})
export class DashboardModule {
}
