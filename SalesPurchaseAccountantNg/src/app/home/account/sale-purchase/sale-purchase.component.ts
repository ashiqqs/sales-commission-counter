import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserModel } from 'src/app/models/user-model';
import { AuthService } from 'src/app/services/auth.service';
import { EmployeeService } from 'src/app/services/employee.service';

@Component({
  selector: 'app-sale-purchase',
  templateUrl: './sale-purchase.component.html',
  styleUrls: ['./sale-purchase.component.scss']
})
export class SalePurchaseComponent implements OnInit {

  user: UserModel;
  vendors: any[] = [];
  customers: any[] = [];
  transactionForm: FormGroup;
  companyCode = sessionStorage.getItem('companyCode');
  constructor(
    private employeService: EmployeeService,
    private activateRouter: ActivatedRoute,
    private toaster: ToastrService,
    private fb: FormBuilder
  ) { }

  ngOnInit() {
    this.user = AuthService.getLoggedUser();
    this.createForm();
    this.getCustomers();
    this.getVendors();
  }
  getCustomers() {
    this.employeService.getSalesman(this.companyCode).subscribe((response: any) => {
      if (response.status) {
        this.customers = response.result as any[];
      }
    }, err => {
      this.toaster.error(err.message)
    })
  }
  getVendors() {
    this.employeService.getMember(this.companyCode).subscribe((response: any) => {
      if (response.status) {
        this.vendors = response.result as any[];
        var index = this.vendors.findIndex(c=>c.sidc==this.user.code);
        this.vendors.splice(index,1)
      }
    }, err => {
      this.toaster.error(err.message)
    })
  }
  onSubmit(){
    if(this.transactionForm.invalid){
      this.toaster.error('Invalid submission');
      return;
    }
    const paramObj = {
      ...this.formVal
    }
    paramObj.userType = Number(this.formVal.userType)
    if(this.user.userType==4){
      this.employeService.transactionBySalesman(this.formVal).subscribe((response:any)=>{
        if(response.status){
          if(this.formVal.type==1){
            sessionStorage.setItem('purchaseAmount', response.result)
          }else{
            sessionStorage.setItem('salesAmount', response.result)
          }
          this.toaster.success('Transaction Success')
        }else{
          this.toaster.error(response.result,'Transaction Failed')
        }
      })
    }else{
      this.employeService.transactionByMember(this.formVal).subscribe((response:any)=>{
        if(response.status){
          if(this.formVal.type==1){
            sessionStorage.setItem('purchaseAmount', response.result)
          }else{
            sessionStorage.setItem('salesAmount', response.result)
          }
          this.toaster.success('Transaction Success')
        }else{
          this.toaster.error(response.result,'Transaction Failed')
        }
      },err=>{
      })
    }
  }
  createForm() {
    this.transactionForm = this.fb.group({
      code: [this.user.code,Validators.required],
      amount: [, [Validators.required]],
      userType: [this.user.userType, []],
      vendorCode: ["0", []],
      customerCode: ["0",[]],
      sidc: [this.user.employmentInfo.membership.sidc,[]],
      type: ["0", [Validators.required]],
      companyCode:[this.companyCode,[]]
    })
  }

  get formVal() {
    return this.transactionForm.value;
  }
}
