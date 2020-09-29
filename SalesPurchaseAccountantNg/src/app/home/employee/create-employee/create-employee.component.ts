import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { EmployeeService } from 'src/app/services/employee.service';
import { SettingService } from 'src/app/services/setting.service';
import { Designation, UserType } from 'src/app/shared/helper/select-list';

@Component({
  selector: 'app-create-employee',
  templateUrl: './create-employee.component.html',
  styleUrls: ['./create-employee.component.scss']
})
export class CreateEmployeeComponent implements OnInit {

  title: string = 'Create New Employee';
  districts: any[] = [{id:0,name:'---Select District---'}];
  thana: any[] = [{id:0,name:'---Select Thana---'}];
  employeeForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private settingsService: SettingService,
    private employeeService: EmployeeService,
    private toaster: ToastrService
  ) { }

  ngOnInit() {
    this.createForm();
    this.getDistricts();
  }
  getDistricts() {
    this.settingsService.getDistrict()
      .subscribe((response: any) => {
        if (response.status) {
          this.districts=response.result as any[];
          this.districts.push({id:0,name:'---Select District---'});
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
          this.thana.push({id:0,name:'---Select Thana---'});
        } else {
          this.toaster.error(response.result)
        }
      })
  }
  onSubmit(){
    let paramObj = {...this.formVal}
    paramObj.thanaId = Number(this.formVal.thanaId)
    this.employeeService.saveSalesman(paramObj)
    .subscribe((response:any)=>{
      if(response.status){
        this.toaster.success(response.result)
      }else{
        this.toaster.error(response.result)
      }
    })
  }
  createForm() {
    this.employeeForm = this.fb.group({
      referenceCode: [, []],
      code: [, []],
      name: [, []],
      districtId: [0, []],
      thanaId: [0, []],
      joiningDate: [, []],
      email: [, []],
      contactNo: [, []],
      address: [, []],
      designation: [Designation.A, []],
      isAlphaMember: [false, []],
      isBetaMember: [false, []],
    })
  }

  get formVal(){
    return this.employeeForm.value;
  }

}
