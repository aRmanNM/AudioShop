import {NgModule} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';
import {AdminAuthGuard} from '../guards/admin-auth.guard';
import {OverviewComponent} from './overview/overview.component';
import {CoursesComponent} from './courses/courses.component';
import {AdminComponent} from './admin.component';

const routes: Routes = [
  {
    path: '', component: AdminComponent, children: [
      {path: 'overview', component: OverviewComponent, canActivate: [AdminAuthGuard]},
      {path: 'courses', component: CoursesComponent, canActivate: [AdminAuthGuard]}
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule {
}
