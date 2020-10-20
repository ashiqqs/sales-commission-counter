import { Designation, UserType } from './../../../shared/helper/select-list';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { UserModel } from 'src/app/models/user-model';
import { AuthService } from 'src/app/services/auth.service';
import { EmployeeService } from 'src/app/services/employee.service';
import { SettingService } from 'src/app/services/setting.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-create-employee',
  templateUrl: './create-employee.component.html',
  styleUrls: ['./create-employee.component.scss']
})
export class CreateEmployeeComponent implements OnInit {

  title: string = 'Create New Employee';
  user: UserModel;
  districts: any[] = [];
  thana: any[] = [];
  salesman: any[] = [];
  employeeForm: FormGroup;
  employeeType: number;
  constructor(
    private fb: FormBuilder,
    private settingsService: SettingService,
    private employeeService: EmployeeService,
    private toaster: ToastrService,
    private activateRoute: ActivatedRoute
  ) { }

  ngOnInit() {
    this.user = AuthService.getLoggedUser();
    this.employeeType = this.activateRoute.snapshot.params['userType'] as number;
    this.createForm();
    this.getDistricts();
    this.getNewCode(this.user.company.code, this.employeeType);
    this.getAllSalesman()
  }
  getDistricts() {
    this.settingsService.getDistrict()
      .subscribe((response: any) => {
        if (response.status) {
          this.districts = response.result as any[];
        } else {
          this.toaster.error(response.result)
        }
      })
  }
  getThanaByDistrict(districtId) {
    this.settingsService.getThana(districtId)
      .subscribe((response: any) => {
        if (response.status) {
          this.thana = response.result as any[]
        } else {
          this.toaster.error(response.result)
        }
      })
  }
  getNewCode(companyCode,userType: UserType) {
    this.employeeService.getNewCode(companyCode,userType).subscribe((response: any) => {
      if (response.status) {
        this.employeeForm.patchValue({ code: response.result });
      }
    })
  }
  getAllSalesman() {
    this.employeeService.getSalesman(this.user.company.code)
      .subscribe((response: any) => {
        if (response.status) {
          this.salesman = response.result as any[];
        } else {
          this.toaster.error(response.result)
        }
      })
  }
  onSubmit() {
    if(this.employeeForm.invalid){
      this.toaster.error('Invalid Submission');
      return;
    }
    let paramObj = { ...this.formVal }
    paramObj.thanaId = Number(this.formVal.thanaId)
    if (this.employeeType == 2 || this.employeeType == 3) {
      if(this.formVal.sidc==0){
        this.toaster.error('SIDC is required');
        return;
      }
      paramObj.memberType = Number(this.formVal.memberType)
      this.employeeService.saveMember(paramObj)
        .subscribe((response: any) => {
          if (response.status) {
            this.toaster.success(response.result)
          } else {
            this.toaster.error(response.result)
          }
        },err=>{
          this.toaster.error(err.message)
        })
    }
    else {
      this.employeeService.saveSalesman(paramObj)
        .subscribe((response: any) => {
          if (response.status) {
            this.toaster.success(response.result)
          } else {
            this.toaster.error(response.result)
          }
        },err=>{
          this.toaster.error(err.message)
        })
    }
  }
  createForm() {
    this.employeeForm = this.fb.group({
      referenceCode: [this.user.code, []],
      code: [, []],
      name: [, []],
      districtId: [0, []],
      thanaId: [0, [Validators.required]],
      joiningDate: [, []],
      email: [, []],
      contactNo: [, [Validators.required]],
      address: [, []],
      designation: [Designation.A, []],
      isAlphaMember: [false, []],
      isBetaMember: [false, []],
      memberType: [this.employeeType, []],
      sidc: [0, []],
      companyCode:[this.user.company.code,[]]
    })
  }

  get formVal() {
    return this.employeeForm.value;
  }

}
