import {NgModule} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';
import {MaterialModule} from '../material/material.module';
import {SalespersonAuthGuard} from '../guards/salesperson-auth.guard';
import {CheckoutsComponent} from './checkouts/checkouts.component';
import {OrdersComponent} from './orders/orders.component';
import {SalespersonComponent} from './salesperson.component';
import {CredentialsComponent} from './credentials/credentials.component';

const routes: Routes = [
  {
    path: '', component: SalespersonComponent, children: [
      {path: 'checkouts', component: CheckoutsComponent, canActivate: [SalespersonAuthGuard], data: {reuseRoute: true}},
      {path: 'orders', component: OrdersComponent, canActivate: [SalespersonAuthGuard], data: {reuseRoute: true}},
      {path: 'credentials', component: CredentialsComponent, canActivate: [SalespersonAuthGuard], data: {reuseRoute: true}}
    ]
  }
];

@NgModule({
  imports: [
    RouterModule.forChild(routes),
    MaterialModule
  ],
  exports: [RouterModule]
})
export class SalespersonRoutingModule {
}
