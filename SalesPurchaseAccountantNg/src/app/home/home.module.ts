import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StartupComponent } from './startup/startup.component';
import { HomeRoutingModule } from './home-routing.module';

@NgModule({
  declarations: [StartupComponent],
  imports: [
    CommonModule,
    HomeRoutingModule
  ]
})
export class HomeModule { }
