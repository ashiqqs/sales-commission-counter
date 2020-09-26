import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DefaultLayoutComponent } from './default-layout/default-layout.component';
import { FullwidthLayoutComponent } from './fullwidth-layout/fullwidth-layout.component';
import { RouterModule } from '@angular/router';
import { LayoutsRoutingModule } from './laouts-routing.module';
import { SharedModule } from '../shared/shared.module';
import { HeaderComponent } from '../shared/components/header/header.component';
import { MaterialModule } from '../shared/material/material.module';

@NgModule({
  declarations: [
    DefaultLayoutComponent,
    FullwidthLayoutComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,
    LayoutsRoutingModule,
    SharedModule,
    MaterialModule
  ],
  exports:[
  ],
  providers:[
  ]
})
export class LayoutsModule { }
