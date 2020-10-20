import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private http:HttpClient) { }

  processSalary(companyCode,month=null){
    return this.http.get(environment.apiUrl+`account/salary/process/${companyCode}/${month}`);
  }
  getSalary(companyCode,code:string=null,month:string=null){
    return this.http.get(environment.apiUrl+`account/salary/get/${companyCode}/${code}/${month}`);
  }
}
