import { NgModule } from '@angular/core';
import { DefaultLayoutComponent } from './default-layout/default-layout.component';
import { Routes, RouterModule } from "@angular/router";
import { FullwidthLayoutComponent } from './fullwidth-layout/fullwidth-layout.component';
import { LoginComponent } from '../user/login/login.component';
import { RegistrationComponent } from '../user/registration/registration.component';

const routs:Routes = [
    {path:'', component:DefaultLayoutComponent},
    {path:'layout2', component:FullwidthLayoutComponent, children:[
        {path:'login', component:LoginComponent},
        {path:'registration', component:RegistrationComponent},
    ]},
];

@NgModule({
declarations:[
],
imports:[
    RouterModule.forRoot(routs)
],
exports:[
    RouterModule
]
})

export class LayoutsRoutingModule{}