import { UserType } from './../shared/helper/select-list';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  constructor(private http:HttpClient) { }

  getNewCode(type:UserType){
    return this.http.get(environment.apiUrl+`employee/get/newCode/${type}`);
  }
  saveSalesman(salesman){
    return this.http.post(environment.apiUrl+`employee/salesman/save`, salesman);
  }
  getSalesman(code:string=null){
    return this.http.get(environment.apiUrl+`employee/salesman/get/${code}`);
  }
  purchaseBySalesman(account){
    return this.http.post(environment.apiUrl+`employee/salesman/purchase`, account);
  }
  saleBySalesman(account){
    return this.http.post(environment.apiUrl+`employee/salesman/sale`, account);
  }

  saveMember(member){
    return this.http.post(environment.apiUrl+`employee/member/save`, member);
  }
  getMember(code:string=null){
    return this.http.get(environment.apiUrl+`employee/member/get/${code}`);
  }
  purchaseByMember(account){
    return this.http.post(environment.apiUrl+`employee/member/purchase`, account);
  }
  saleByMember(account){
    return this.http.post(environment.apiUrl+`employee/member/sale`, account);
  }
}
