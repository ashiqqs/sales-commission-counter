import { UserType } from '../shared/helper/select-list';

export class UserModel{
    id?:number;
    name:string;
    password:string;
    oldPassword:string;
    userType:UserType;
    code:string;
    salesAmount:number;
    purchaseAmount:number;
    employmentInfo:any;
    company:any;
}