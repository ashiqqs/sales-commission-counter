import { DefaultLayoutComponent } from './layouts/default-layout/default-layout.component';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { SharedModule } from './shared/shared.module';
import { LayoutsModule } from './layouts/layouts.module';
import { UserModule } from './user/user.module';
import { HomeModule } from './home/home.module';
import { HttpClientModule } from '@angular/common/http';
import { ToastrModule, ToastrService } from 'ngx-toastr';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    RouterModule,
    LayoutsModule,
    UserModule,
    HomeModule,
    HttpClientModule,
    ToastrModule.forRoot()
  ],
  providers: [ToastrService],
  bootstrap: [AppComponent]
})
export class AppModule { }
