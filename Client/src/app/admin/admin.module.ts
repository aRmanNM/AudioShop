import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';

import {AdminRoutingModule} from './admin-routing.module';
import {MaterialModule} from '../material/material.module';
import {CoursesComponent} from './courses/courses.component';
import {AdminComponent} from './admin.component';
import {SharedModule} from '../shared/shared.module';
import {DetailComponent} from './courses/detail/detail.component';
import {CreateOrEditComponent} from './courses/create-or-edit/create-or-edit.component';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {EpisodesComponent} from './episodes/episodes.component';
import {EpisodeCreateEditComponent} from './episodes/episode-create-edit/episode-create-edit.component';
import {DragDropModule} from '@angular/cdk/drag-drop';
import {CheckoutEditComponent} from './checkouts/checkout-edit/checkout-edit.component';
import {CheckoutDoneComponent} from './checkouts/checkout-done/checkout-done.component';
import {CheckoutPendingComponent} from './checkouts/checkout-pending/checkout-pending.component';
import {SalespersonModule} from '../salesperson/salesperson.module';
import {ReviewsEditComponent} from './reviews/reviews-edit/reviews-edit.component';
import {SalespersonsComponent} from './salespersons/salespersons.component';
import {SalespersonCredsComponent} from './salespersons/salesperson-creds/salesperson-creds.component';
import {ConfigsComponent} from './configs/configs.component';
import {SliderComponent} from './slider/slider.component';
import {SliderEditCreateComponent} from './slider/slider-edit-create/slider-edit-create.component';
import {CouponsComponent} from './coupons/coupons.component';
import {CouponsCreateEditComponent} from './coupons/coupons-create-edit/coupons-create-edit.component';
import {MobileAppComponent} from './mobile-app/mobile-app.component';
import {PasswordComponent} from './password/password.component';
import {DurationPipe} from '../pipes/duration.pipe';
import {ReviewsComponent} from './reviews/reviews.component';
import {SalespersonEditComponent} from './salespersons/salesperson-edit/salesperson-edit.component';
import { ReceiptInfoComponent } from './checkouts/receipt-info/receipt-info.component';


@NgModule({
  declarations: [
    CoursesComponent,
    AdminComponent,
    DetailComponent,
    CreateOrEditComponent,
    EpisodesComponent,
    EpisodeCreateEditComponent,
    CheckoutEditComponent,
    CheckoutDoneComponent,
    CheckoutPendingComponent,
    ReviewsEditComponent,
    SalespersonsComponent,
    SalespersonCredsComponent,
    ConfigsComponent,
    SliderComponent,
    SliderEditCreateComponent,
    CouponsComponent,
    CouponsCreateEditComponent,
    MobileAppComponent,
    PasswordComponent,
    DurationPipe,
    ReviewsComponent,
    SalespersonEditComponent,
    ReceiptInfoComponent,
  ],
  imports: [
    CommonModule,
    AdminRoutingModule,
    MaterialModule,
    SharedModule,
    ReactiveFormsModule,
    FormsModule,
    DragDropModule,
    SalespersonModule
  ],
  exports: [
    DurationPipe
  ],
})
export class AdminModule {
}
