import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { UserModel } from 'src/app/models/user-model';
import { AccountService } from 'src/app/services/account.service';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.scss']
})
export class ReportComponent implements OnInit {

  user:UserModel;
  code: string;
  salaries: any[] = [];
  totalSalary: any;
  constructor(
    private accountService: AccountService,
    private toasterService: ToastrService
  ) { }

  ngOnInit() {
    this.user = AuthService.getLoggedUser();
    this.code = this.user.code;
    this.getSalary();
  }

  getSalary() {
    const companyCode = (this.user.userType==1 && this.user.code==this.code)?null:this.user.company.code;
    this.accountService.getSalary(companyCode, this.code).subscribe((response: any) => {
      if (response.status) {
        this.salaries = response.result as any[];
        this.totalSalary = {
          salesCommission: 0,
          ordinalCommission: 0,
          inboundCommission: 0,
          outboundCommission: 0,
          gbCommission: 0,
          total: 0
        }
        this.salaries.forEach(s => {
          this.totalSalary.salesCommission += Number(s.salesCommission);
          this.totalSalary.ordinalCommission += Number(s.ordinalCommission);
          this.totalSalary.inboundCommission += Number(s.inboundCommission);
          this.totalSalary.outboundCommission += Number(s.outboundCommission);
          this.totalSalary.gbCommission += Number(s.gbCommission);
          this.totalSalary.total += Number(s.total);
        })
      } else {
        this.salaries = [];
        this.totalSalary = null;
        this.toasterService.error('Salary not processed yet.', 'Not found')
      }
    }, err => {
      this.salaries = [];
      console.error(err)
    })
  }
}
