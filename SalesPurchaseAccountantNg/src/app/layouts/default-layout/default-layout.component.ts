import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-default-layout',
  templateUrl: './default-layout.component.html',
  styleUrls: ['./default-layout.component.scss']
})
export class DefaultLayoutComponent implements OnInit {

  @Input() showSideNav:boolean=true;
  constructor() { }
  ngOnInit() {
  }

}
