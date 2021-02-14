import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';

import {MaterialModule} from '../material/material.module';
import {SidebarComponent} from './sidebar/sidebar.component';
import {TopbarComponent} from './topbar/topbar.component';
import {TestComponent} from './test/test.component';

@NgModule({
  declarations: [SidebarComponent, TopbarComponent, TestComponent],
  imports: [
    CommonModule,
    MaterialModule
  ],
  exports: [
    SidebarComponent,
    TopbarComponent,
    TestComponent
  ]
})
export class SharedModule {
}
