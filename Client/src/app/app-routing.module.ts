import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {RegisterComponent} from './auth/register/register.component';
import {LoginComponent} from './auth/login/login.component';
import {AuthGuard} from './guards/auth.guard';
import {AdminAuthGuard} from './guards/admin-auth.guard';
import {SalespersonAuthGuard} from './guards/salesperson-auth.guard';

const routes: Routes = [
  {path: '', component: LoginComponent},
  {path: 'login', component: LoginComponent},
  {path: 'register', component: RegisterComponent},
  {path: 'admin', loadChildren: () => import ('./admin/admin.module').then(m => m.AdminModule), canActivate: [AuthGuard, AdminAuthGuard]},
  {
    path: 'salesperson',
    loadChildren: () => import ('./salesperson/salesperson.module').then(m => m.SalespersonModule),
    canActivate: [AuthGuard, SalespersonAuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
