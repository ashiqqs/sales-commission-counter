import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-sidenav-menu-item',
  templateUrl: './sidenav-menu-item.component.html',
  styleUrls: ['./sidenav-menu-item.component.scss']
})
export class SidenavMenuItemComponent implements OnInit {

  @Input() routeUrl:string = '';
  @Input() linkText:string = 'link';
  @Input() faIcon:string = '';
  constructor() { }
  ngOnInit() {
  }

}
