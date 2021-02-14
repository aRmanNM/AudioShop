import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';

import {AdminRoutingModule} from './admin-routing.module';
import {MaterialModule} from '../material/material.module';
import {CoursesComponent} from './courses/courses.component';
import {OverviewComponent} from './overview/overview.component';
import {AdminComponent} from './admin.component';
import {SharedModule} from '../shared/shared.module';
import {DetailComponent} from './courses/detail/detail.component';
import {CreateOrEditComponent} from './courses/create-or-edit/create-or-edit.component';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {EpisodesComponent} from './episodes/episodes.component';
import {EpisodeCreateEditComponent} from './episodes/episode-create-edit/episode-create-edit.component';


@NgModule({
  declarations: [
    CoursesComponent,
    OverviewComponent,
    AdminComponent,
    DetailComponent,
    CreateOrEditComponent,
    EpisodesComponent,
    EpisodeCreateEditComponent
  ],
  imports: [
    CommonModule,
    AdminRoutingModule,
    MaterialModule,
    SharedModule,
    ReactiveFormsModule,
    FormsModule
  ]
})
export class AdminModule {
}
