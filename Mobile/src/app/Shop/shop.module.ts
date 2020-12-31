import {IonicModule} from '@ionic/angular';
import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormsModule} from '@angular/forms';

import {CourseDetailComponent} from './course-detail/course-detail.component';
import {CourseListComponent} from './course-list/course-list.component';
import {ShopPage} from "./shop.page";
import {ShopRoutingModule} from "./shop-routing.module";

@NgModule({
    imports: [
        IonicModule,
        CommonModule,
        FormsModule,
        ShopRoutingModule
    ],
    declarations: [ShopPage, CourseDetailComponent, CourseListComponent]
})
export class ShopPageModule {
}
