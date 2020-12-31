import {IonicModule} from '@ionic/angular';
import {RouterModule} from '@angular/router';
import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormsModule} from '@angular/forms';
import {BasketPage} from './basket.page';

import {BasketRoutingModule} from './basket-routing.module';

@NgModule({
    imports: [
        IonicModule,
        CommonModule,
        FormsModule,
        RouterModule.forChild([{path: '', component: BasketPage}]),
        BasketRoutingModule
    ],
    declarations: [BasketPage]
})
export class BasketPageModule {
}
