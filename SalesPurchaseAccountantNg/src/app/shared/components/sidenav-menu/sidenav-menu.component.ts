import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-sidenav-menu',
  templateUrl: './sidenav-menu.component.html',
  styleUrls: ['./sidenav-menu.component.scss']
})
export class SidenavMenuComponent implements OnInit {

  @Input() subMenuId:string = 'subMenu';
  @Input() subMenuList:SubMenu[]=[];
  @Input() menuText:string='Menu'
  @Input() faIcon:string='';
  constructor() { }
  ngOnInit() {
  }

}

export class SubMenu{
  routeUrl:string;
  linkText:string;
  faIcon:string;
}
