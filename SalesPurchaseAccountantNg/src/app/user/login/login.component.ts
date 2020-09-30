import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserModel } from 'src/app/models/user-model';
import { EmployeeService } from 'src/app/services/employee.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss',]
})
export class LoginComponent implements OnInit {

  user: any = {
    name: null,
    password: null,
    userType: 0
  }
  hasCompany:boolean = true;
  errMsg: string;

  constructor(
    private userService: UserService,
    private employeeService: EmployeeService,
    private router: Router,
    private toaster: ToastrService
  ) { }
  ngOnInit() {
    this.countEmployee();
  }
  countEmployee(){
    this.employeeService.count().subscribe((response:any)=>{
      if(response.status){
        this.hasCompany = response.result>0;
      }else{
        this.hasCompany = false;
      }
    },err=>{
      this.hasCompany = false;
    })
  }
  login() {
    if (this.user.name && this.user.password) {
      this.userService.login(this.user).subscribe((response: any) => {
        if (response.status) {
          const loggedUser: UserModel = response.result as UserModel;
          sessionStorage.setItem('code', loggedUser.code);
          sessionStorage.setItem('userType', loggedUser.userType.toString());
          sessionStorage.setItem('name', loggedUser.name);
          sessionStorage.setItem('salesAmount', loggedUser.salesAmount.toString());
          sessionStorage.setItem('purchaseAmount', loggedUser.purchaseAmount.toString());
          sessionStorage.setItem('isAlphaMember', loggedUser.employmentInfo.isAlphaMember?'True':'False');
          sessionStorage.setItem('isBetaMember', loggedUser.employmentInfo.isBetaMember?'True':'False');
          sessionStorage.setItem('fullName', (loggedUser.userType==2 || loggedUser.userType==3)
          ?loggedUser.employmentInfo.membership.name:loggedUser.employmentInfo.name);
          sessionStorage.setItem('sidc', loggedUser.employmentInfo.membership.sidc);
          this.router.navigate(['/startup']);
        } else {
          this.errMsg = response.result;
          this.toaster.error(response.result);
        }
      })
    }
    else {
      this.toaster.error('User name and password is required', 'Invalid Submission')
    }
  }
}
