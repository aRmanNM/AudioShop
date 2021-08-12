import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RouterModule, Routes} from '@angular/router';
import {FormsModule} from '@angular/forms';
import {LandingsComponent} from './landings.component';

const routes: Routes = [
  {
    path: ':id', component: LandingsComponent
  }
];


@NgModule({
  declarations: [LandingsComponent],
  imports: [
    FormsModule,
    CommonModule,
    RouterModule.forChild(routes)
  ]
})
export class LandingsModule {
}
