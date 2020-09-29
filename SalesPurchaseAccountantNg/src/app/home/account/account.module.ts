import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SalePurchaseComponent } from './sale-purchase/sale-purchase.component';
import { SalaryComponent } from './salary/salary.component';
import { ReportComponent } from './report/report.component';

@NgModule({
  declarations: [SalePurchaseComponent, SalaryComponent, ReportComponent],
  imports: [
    CommonModule
  ]
})
export class AccountModule { }
