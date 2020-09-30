import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { Router } from '@angular/router';
import { UserModel } from 'src/app/models/user-model';
import { AuthService } from 'src/app/services/auth.service';
import { UserType } from '../../helper/select-list';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  toggle:boolean=true;
  @Output() toggleSideNav = new EventEmitter<boolean>();

  user:UserModel;

  constructor(
    private router:Router
  ) { }
  ngOnInit() {
    this.user = AuthService.getLoggedUser()
  }

  onClickLogo(){
    this.toggle = !this.toggle;
    this.toggleSideNav.emit(this.toggle);
  }
  logout(){
    sessionStorage.clear();
    this.router.navigate(['/user/login']);
  }

}
