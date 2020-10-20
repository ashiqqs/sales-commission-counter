import { CreateEmployeeComponent } from './employee/create-employee/create-employee.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { StartupComponent } from './startup/startup.component';
import { DefaultLayoutComponent } from '../layouts/default-layout/default-layout.component';
import { SalePurchaseComponent } from './account/sale-purchase/sale-purchase.component';
import { SalaryComponent } from './account/salary/salary.component';
import { ReportComponent } from './account/report/report.component';
import { EmployeeDetailsComponent } from './employee/employee-details/employee-details.component';
import { FullwidthLayoutComponent } from '../layouts/fullwidth-layout/fullwidth-layout.component';
import { LoginComponent } from '../user/login/login.component';
import { RegistrationComponent } from '../user/registration/registration.component';

const routes:Routes = [
  {path: '', component: DefaultLayoutComponent, children:[
    {path: 'startup', component:StartupComponent}
  ]},
  {path:'account', component:DefaultLayoutComponent, children:[
    {path:'sale-purchase', component:SalePurchaseComponent},
    {path:'salary', component:SalaryComponent},
    {path:'report', component:ReportComponent}
  ]},
  {path:'employee', component:DefaultLayoutComponent, children:[
    {path:'create', component:CreateEmployeeComponent},
    {path:'details', component:EmployeeDetailsComponent}
  ]},
  {path:'user', component:FullwidthLayoutComponent, children:[
    {path:'login', component:LoginComponent},
    {path:'register', component:RegistrationComponent}
  ]}
]

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class HomeRoutingModule { }
