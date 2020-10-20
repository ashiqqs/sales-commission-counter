import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-salary',
  templateUrl: './salary.component.html',
  styleUrls: ['./salary.component.scss']
})
export class SalaryComponent implements OnInit {

  isProcessing:boolean = false;
  isProcessed:boolean = false;
  isSuccess:boolean = false;
  constructor(
    private accountService:AccountService,
    private toaster:ToastrService) { 
  }

  ngOnInit() {
  }

  process(){
    this.isProcessing = true;
    this.accountService.processSalary(sessionStorage.getItem('companyCode')).subscribe((response:any)=>{
      this.isProcessing = false;
      this.isProcessed = true;
      if(response.status){
        this.isSuccess = true;
        this.toaster.success(response.result)
      }else{
        this.isSuccess=false;
        this.toaster.error(response.result)
      }
    },err=>{
      this.isProcessing=false;
      this.isProcessed = true;
      this.isSuccess = false;
      console.error(err)
    })
  }
}
