import { UserType } from './../shared/helper/select-list';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  constructor(private http:HttpClient) { }

  getNewCode(companyCode,type:UserType){
    return this.http.get(environment.apiUrl+`employee/get/newCode/${companyCode}/${type}`);
  }
  saveSalesman(salesman){
    return this.http.post(environment.apiUrl+`employee/salesman/save`, salesman);
  }
  getSalesman(companyCode,code:string=null){
    return this.http.get(environment.apiUrl+`employee/salesman/get/${companyCode}/${code}`);
  }
  transactionBySalesman(account){
    if(account.type==1){
      return this.http.post(environment.apiUrl+`employee/salesman/purchase`, account);
    }else{
      return this.http.post(environment.apiUrl+`employee/salesman/sale`, account);
    }
  }

  saveMember(member){
    return this.http.post(environment.apiUrl+`employee/member/save`, member);
  }
  getMember(companyCode,code:string=null){
    return this.http.get(environment.apiUrl+`employee/member/get/${companyCode}/${code}`);
  }
  transactionByMember(account){
    if(account.type==1){
      return this.http.post(environment.apiUrl+`employee/member/purchase`, account);
    }else{
      return this.http.post(environment.apiUrl+`employee/member/sale`, account);
    }
  }
  count(companyCode,type:UserType = UserType.Salesman){
    return this.http.get(environment.apiUrl+`employee/count/${companyCode}/${type}`);
  }
  getEmployees(companyCode){
    return this.http.get(environment.apiUrl+`employee/get/${companyCode}`);
  }
}
