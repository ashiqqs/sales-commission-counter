import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { StartupComponent } from './startup/startup.component';
import { DefaultLayoutComponent } from '../layouts/default-layout/default-layout.component';

const routes:Routes = [
  {path: '', component: DefaultLayoutComponent, children:[
    {path: 'startup', component:StartupComponent}
  ]},
  // {path: '', component: DefaultLayoutComponent, children:[
  //   {path: 'layout2/login', component:StartupComponent}
  // ]}
]

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class HomeRoutingModule { }
