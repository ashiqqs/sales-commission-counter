import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatToolbarModule, MatMenuModule, MatSidenavModule, MatButtonModule, MatButtonToggleModule, MatIconModule, MatExpansionModule } from '@angular/material';

const matModules = [
  MatToolbarModule,
  MatMenuModule,
  MatSidenavModule,
  MatButtonModule,
  MatButtonToggleModule,
  MatIconModule,
  MatExpansionModule
]

@NgModule({
  imports: [
    CommonModule,
    matModules
  ],
  exports:[matModules]
})
export class MaterialModule { }
