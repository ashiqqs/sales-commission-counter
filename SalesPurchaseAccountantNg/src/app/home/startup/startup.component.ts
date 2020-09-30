import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserModel } from 'src/app/models/user-model';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-startup',
  templateUrl: './startup.component.html',
  styleUrls: ['./startup.component.scss']
})
export class StartupComponent implements OnInit {

  user:UserModel;
  constructor(private router:Router){
    this.user = AuthService.getLoggedUser();
    if(this.user.userType){
      this.router.navigate[('/')]
    }
    else{
      this.router.navigate[('/user/login')]
    }
  }
  ngOnInit() {
  }

}
