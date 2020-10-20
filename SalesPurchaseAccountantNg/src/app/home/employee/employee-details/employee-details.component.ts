import { Component, OnInit } from '@angular/core';
import { EmployeeService } from 'src/app/services/employee.service';

@Component({
  selector: 'app-employee-details',
  templateUrl: './employee-details.component.html',
  styleUrls: ['./employee-details.component.scss']
})
export class EmployeeDetailsComponent implements OnInit {

  companyCode = sessionStorage.getItem('companyCode');
  employees:any[]=[]
  constructor(
    private employeeService:EmployeeService
  ) { }

  ngOnInit() {
    this.getAllSalesman()
  }

  getAllSalesman() {
    this.employeeService.getEmployees(this.companyCode)
      .subscribe((response: any) => {
        if (response.status) {
          this.employees = response.result as any[];
        } else {
          this.employees = [];
        }
      })
  }
}
