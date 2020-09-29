import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StartupComponent } from './startup/startup.component';
import { HomeRoutingModule } from './home-routing.module';
import { EmployeeModule } from './employee/employee.module';
import { AccountModule } from './account/account.module';

@NgModule({
  declarations: [StartupComponent],
  imports: [
    CommonModule,
    HomeRoutingModule,
    EmployeeModule,
    AccountModule
  ]
})
export class HomeModule { }
