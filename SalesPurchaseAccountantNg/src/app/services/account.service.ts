import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private http:HttpClient) { }

  processSalary(month=null){
    return this.http.get(environment.apiUrl+`account/salary/process`);
  }
  getSalary(code:string=null,month:string=null){
    return this.http.get(environment.apiUrl+`account/salary/get/${code}/${month}`);
  }
}
