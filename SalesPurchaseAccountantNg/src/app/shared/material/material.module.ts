import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatToolbarModule, MatMenuModule, MatSidenavModule, MatButtonModule, MatButtonToggleModule, MatIconModule, MatExpansionModule, MatSelectModule } from '@angular/material';

const matModules = [
  MatToolbarModule,
  MatMenuModule,
  MatSidenavModule,
  MatButtonModule,
  MatButtonToggleModule,
  MatIconModule,
  MatExpansionModule,
  MatSelectModule
]

@NgModule({
  imports: [
    CommonModule,
    matModules
  ],
  exports:[matModules]
})
export class MaterialModule { }
