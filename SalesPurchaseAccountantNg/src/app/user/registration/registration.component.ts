import { ToastrService } from 'ngx-toastr';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SettingService } from 'src/app/services/setting.service';
import { UserService } from 'src/app/services/user.service';
import { Router } from '@angular/router';
import { UserModel } from 'src/app/models/user-model';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit {

  title = 'Register New Company'
  registerForm: FormGroup;
  constructor(
    private fb: FormBuilder,
    private settingsService: SettingService,
    private userService: UserService,
    private toasterService:ToastrService,
    private router:Router
  ) { }

  ngOnInit() {
    this.createForm();
    this.getNewCode();
  }
  getNewCode() {
    this.settingsService.getNewCompanyCode().subscribe((response: any) => {
      if (response.status) {
        this.registerForm.patchValue({ code: response.result });
      }
    })
  }
  createForm() {
    this.registerForm = this.fb.group({
      code: [, [Validators.required]],
      name: [, [Validators.required]],
      address: [, []],
    })
  }
onSubmit(){
  if(this.registerForm.invalid){
    this.toasterService.error('Please, Fill all required field', 'Invalid Submission');
    return;
  }
  this.settingsService.registration(this.formVal)
  .subscribe((response:any)=>{
    if (response.status) {
      sessionStorage.setItem('code', response.result.code);
      sessionStorage.setItem('userType', '1');
      sessionStorage.setItem('name', this.formVal.name);
      sessionStorage.setItem('salesAmount', '0');
      sessionStorage.setItem('purchaseAmount', '0');
      sessionStorage.setItem('fullName', this.formVal.name);
      sessionStorage.setItem('companyCode', response.result.code);
      this.router.navigate(['/startup']);
    } else {
      this.toasterService.error(response.result);
    }
  })
}
  get formVal() {
    return this.registerForm.value;
  }

}
