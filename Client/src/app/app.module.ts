import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {MaterialModule} from './material/material.module';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import {LoginComponent} from './auth/login/login.component';
import {RegisterComponent} from './auth/register/register.component';
import {AuthGuard} from './guards/auth.guard';
import {AdminAuthGuard} from './guards/admin-auth.guard';
import {JwtInterceptor} from './interceptors/jwt.interceptor';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {CustomHttpInterceptor} from './interceptors/http.interceptor';
import {RouteReuseStrategy} from '@angular/router';
import {CustomRouteReuseStrategy} from './extends/reuse-strategy';
import {CustomMatPaginatorIntl} from './extends/paginatorInt';
import {MatPaginatorIntl} from '@angular/material/paginator';
import {LandingComponent} from './landing/landing.component';
import {SharedModule} from './shared/shared.module';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    LandingComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MaterialModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule
  ],
  providers: [
    {provide: MatPaginatorIntl, useClass: CustomMatPaginatorIntl},
    {
      provide: RouteReuseStrategy,
      useClass: CustomRouteReuseStrategy
    },
    AuthGuard,
    AdminAuthGuard, {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true
    }, {
      provide: HTTP_INTERCEPTORS,
      useClass: CustomHttpInterceptor,
      multi: true
    }],
  bootstrap: [AppComponent]
})
export class AppModule {
}
