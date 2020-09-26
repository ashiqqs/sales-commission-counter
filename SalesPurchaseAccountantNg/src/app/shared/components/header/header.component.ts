import { Component, OnInit, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  toggle:boolean=true;
  @Output() toggleSideNav = new EventEmitter<boolean>();
  constructor() { }
  ngOnInit() {
  }

  onClickLogo(){
    this.toggle = !this.toggle;
    this.toggleSideNav.emit(this.toggle);
  }


}
