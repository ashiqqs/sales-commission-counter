import { Injectable } from '@angular/core';
import { UserModel } from '../models/user-model';
import { UserType } from '../shared/helper/select-list';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor() { }
  public static getLoggedUser(): UserModel {
    const user = new UserModel();
    user.code = sessionStorage.getItem('code');
    user.name = sessionStorage.getItem('name');
    user.userType = Number(sessionStorage.getItem('userType')) as UserType,
      user.salesAmount = Number(sessionStorage.getItem('salesAmount'));
    user.purchaseAmount = Number(sessionStorage.getItem('purchaseAmount'));
    user.employmentInfo = {
      name: sessionStorage.getItem('fullName'),
      IsAlphaMember: sessionStorage.getItem('isAlphaMember') == "True" ? true : false,
      IsBetaMember: sessionStorage.getItem('isBetaMember') == "True" ? true : false,
      membership: {
        sidc: sessionStorage.getItem('sidc')
      },
    };
    user.company = {
      code : sessionStorage.getItem('companyCode'),
      name : sessionStorage.getItem('name')
    }
    return user;
  }
}
