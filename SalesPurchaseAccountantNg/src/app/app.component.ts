import { Component } from '@angular/core';
import { Router, Routes } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'sales-purchase-accountant-ng';
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
}
