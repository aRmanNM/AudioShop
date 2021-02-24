import {NgModule} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';
import {AdminAuthGuard} from '../guards/admin-auth.guard';
import {CoursesComponent} from './courses/courses.component';
import {AdminComponent} from './admin.component';
import {EpisodesComponent} from './episodes/episodes.component';
import {CheckoutDoneComponent} from './checkouts/checkout-done/checkout-done.component';
import {CheckoutPendingComponent} from './checkouts/checkout-pending/checkout-pending.component';

const routes: Routes = [
  {
    path: '', component: AdminComponent, children: [
      {path: 'courses', component: CoursesComponent, canActivate: [AdminAuthGuard], data: {reuseRoute: true}},
      {path: 'episodes', component: EpisodesComponent, canActivate: [AdminAuthGuard], data: {reuseRoute: true}},
      {path: 'checkouts/done', component: CheckoutDoneComponent, canActivate: [AdminAuthGuard], data: {reuseRoute: true}},
      {path: 'checkouts/pending', component: CheckoutPendingComponent, canActivate: [AdminAuthGuard], data: {reuseRoute: true}}
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule {
}
