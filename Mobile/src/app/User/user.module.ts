import {IonicModule} from '@ionic/angular';
import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {UserPage} from './user.page';

import {UserRoutingModule} from './user-routing.module';
import {UserCourseDetailComponent} from './user-course-detail/user-course-detail.component';
import {AuthService} from '../Services/auth.service';
import {LoginComponent} from "./login/login.component";
import {RegisterComponent} from "./register/register.component";
import {UserService} from "../Services/user.service";

@NgModule({
    imports: [
        IonicModule,
        CommonModule,
        FormsModule,
        UserRoutingModule,
        ReactiveFormsModule
    ],
    declarations: [UserPage, UserCourseDetailComponent, LoginComponent, RegisterComponent],
    providers: [AuthService, UserService]
})
export class UserPageModule {
}
