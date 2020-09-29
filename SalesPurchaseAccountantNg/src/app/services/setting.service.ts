import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SettingService {

  constructor(private http:HttpClient) { }

  getThana(districtId:number=-1, thanaId:number=-1){
    return this.http.get(environment.apiUrl+`settings/thana/${districtId}/${thanaId}`);
  }
  getDistrict(districtId:number=-1){
    return this.http.get(environment.apiUrl+`settings/district/${districtId}`);
  }
}
