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
    CheckoutPendingComponent
  ],
  imports: [
    CommonModule,
    AdminRoutingModule,
    MaterialModule,
    SharedModule,
    ReactiveFormsModule,
    FormsModule,
    DragDropModule
  ]
})
export class AdminModule {
}
