import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ColorSketchModule} from 'ngx-color/sketch';
import {ChartModule} from 'angular2-chartjs';
import {SalespersonModule} from '../salesperson/salesperson.module';

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
import {ReceiptInfoComponent} from './checkouts/receipt-info/receipt-info.component';
import {CategoriesComponent} from './courses/categories/categories.component';
import {StatsComponent} from './stats/stats.component';
import {OrdersComponent} from './orders/orders.component';
import {LandingsComponent} from './landings/landings.component';
import {LandingsCreateOrEditComponent} from './landings/landings-create-or-edit/landings-create-or-edit.component';
import {AdsComponent} from './ads/ads.component';
import {AdsCreateEditComponent} from './ads/ads-create-edit/ads-create-edit.component';
import {PlacesComponent} from './ads/places/places.component';
import {MessagesComponent} from './messages/messages.component';
import {MessagesCreateEditComponent} from './messages/messages-create-edit/messages-create-edit.component';
import {UsersComponent} from './users/users.component';
import {UserMessagesComponent} from './messages/user-messages/user-messages.component';
import {TicketsComponent} from './tickets/tickets.component';
import {TicketResponsesComponent} from './tickets/ticket-responses/ticket-responses.component';


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
    CategoriesComponent,
    StatsComponent,
    OrdersComponent,
    LandingsComponent,
    LandingsCreateOrEditComponent,
    AdsComponent,
    AdsCreateEditComponent,
    PlacesComponent,
    MessagesComponent,
    MessagesCreateEditComponent,
    UsersComponent,
    UserMessagesComponent,
    TicketsComponent,
    TicketResponsesComponent
  ],
  imports: [
    CommonModule,
    AdminRoutingModule,
    MaterialModule,
    SharedModule,
    ReactiveFormsModule,
    FormsModule,
    DragDropModule,
    SalespersonModule,
    ChartModule,
    ColorSketchModule
  ],
  exports: [
    DurationPipe
  ],
})
export class AdminModule {
}
