import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';

import {DashboardComponent} from './dashboard.component';
import {TestComponent} from './test/test.component';
import {OverviewComponent} from './overview/overview.component';
import {AdminAuthGuard} from '../guards/admin-auth.guard';

const routes: Routes = [
  {
    path: '', component: DashboardComponent, children: [
      {path: 'test', component: TestComponent},
      {path: 'overview', component: OverviewComponent, canActivate: [AdminAuthGuard]}
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DashboardRoutingModule {
}
