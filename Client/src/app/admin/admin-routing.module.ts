import {NgModule} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';
import {AdminAuthGuard} from '../guards/admin-auth.guard';
import {CoursesComponent} from './courses/courses.component';
import {AdminComponent} from './admin.component';
import {EpisodesComponent} from './episodes/episodes.component';
import {CheckoutDoneComponent} from './checkouts/checkout-done/checkout-done.component';
import {CheckoutPendingComponent} from './checkouts/checkout-pending/checkout-pending.component';
import {ReviewsDoneComponent} from './reviews/reviews-done/reviews-done.component';
import {ReviewsPendingComponent} from './reviews/reviews-pending/reviews-pending.component';
import {SalespersonsComponent} from './salespersons/salespersons.component';
import {ConfigsComponent} from './configs/configs.component';
import {SliderComponent} from './slider/slider.component';
import {CouponsComponent} from './coupons/coupons.component';

const routes: Routes = [
  {
    path: '', component: AdminComponent, children: [
      {path: 'courses', component: CoursesComponent, canActivate: [AdminAuthGuard], data: {reuseRoute: true}},
      {path: 'episodes', component: EpisodesComponent, canActivate: [AdminAuthGuard], data: {reuseRoute: true}},
      {path: 'checkouts/done', component: CheckoutDoneComponent, canActivate: [AdminAuthGuard], data: {reuseRoute: true}},
      {path: 'checkouts/pending', component: CheckoutPendingComponent, canActivate: [AdminAuthGuard], data: {reuseRoute: true}},
      {path: 'reviews/done', component: ReviewsDoneComponent, canActivate: [AdminAuthGuard], data: {reuseRoute: true}},
      {path: 'reviews/pending', component: ReviewsPendingComponent, canActivate: [AdminAuthGuard], data: {reuseRoute: true}},
      {path: 'salespersons', component: SalespersonsComponent, canActivate: [AdminAuthGuard], data: {reuseRoute: true}},
      {path: 'configs', component: ConfigsComponent, canActivate: [AdminAuthGuard], data: {reuseRoute: true}},
      {path: 'slider', component: SliderComponent, canActivate: [AdminAuthGuard], data: {reuseRoute: true}},
      {path: 'coupons', component: CouponsComponent, canActivate: [AdminAuthGuard], data: {reuseRoute: true}},
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule {
}
