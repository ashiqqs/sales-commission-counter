import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-startup',
  templateUrl: './startup.component.html',
  styleUrls: ['./startup.component.scss']
})
export class StartupComponent implements OnInit {

  userType;
  constructor(private router:Router){
    this.userType = sessionStorage.getItem('userType');
    if(this.userType){
      this.router.navigate[('/')]
    }
    else{
      this.router.navigate[('/user/login')]
    }
  }
  ngOnInit() {
  }

}
