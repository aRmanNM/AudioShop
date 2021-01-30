import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';

import {DashboardComponent} from './dashboard.component';
import {TestComponent} from './test/test.component';
import {SalespersonCheckoutsComponent} from './salesperson-checkouts/salesperson-checkouts.component';
import {SalespersonAuthGuard} from '../guards/salesperson-auth.guard';
import {SalespersonOrdersComponent} from './salesperson-orders/salesperson-orders.component';

const routes: Routes = [
  {
    path: '', component: DashboardComponent, children: [
      {path: 'test', component: TestComponent},
      {path: 'salesperson-orders', component: SalespersonOrdersComponent, canActivate: [SalespersonAuthGuard]},
      {path: 'salesperson-checkouts', component: SalespersonCheckoutsComponent, canActivate: [SalespersonAuthGuard]}
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DashboardRoutingModule {
}
