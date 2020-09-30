import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SalePurchaseComponent } from './sale-purchase/sale-purchase.component';
import { SalaryComponent } from './salary/salary.component';
import { ReportComponent } from './report/report.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [SalePurchaseComponent, SalaryComponent, ReportComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule
  ]
})
export class AccountModule { }
