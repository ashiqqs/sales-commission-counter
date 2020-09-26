import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LayoutModule } from '@angular/cdk/layout';
import { MaterialModule } from './material/material.module';
import { HeaderComponent } from './components/header/header.component';
import { SidenavComponent } from './components/sidenav/sidenav.component';
import { FooterComponent } from './components/footer/footer.component';
import { RouterModule } from '@angular/router';
import { SidenavMenuItemComponent } from './components/sidenav-menu-item/sidenav-menu-item.component';
import { SidenavMenuComponent } from './components/sidenav-menu/sidenav-menu.component';

@NgModule({
  declarations: [
    HeaderComponent,
    SidenavComponent,
    FooterComponent,
    SidenavMenuItemComponent,
    SidenavMenuComponent
  ],
  imports: [
    CommonModule,
    MaterialModule,
    RouterModule,
  ],
  exports:[
    HeaderComponent,
    SidenavComponent,
    FooterComponent
  ]
})
export class SharedModule { }
