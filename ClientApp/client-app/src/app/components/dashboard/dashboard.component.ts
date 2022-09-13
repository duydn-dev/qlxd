import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user/user.service';


@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  customers: any = [];
  pageSize: number = 10;
  pageIndex: number = 1;
  chartOptions: any;
  data: any;
  option: any = {
    legend: {
      labels: {
        fontColor: 'white'
      }
    }
  }
  staticalData:any = {};
  constructor(
    private _userService: UserService
  ) { }

  ngOnInit(): void {
    this.getStatistical();
  }
  getUserRole(userId: any) {
    this._userService.getRoles(userId).subscribe(n => {
    })
  }
  getStatistical() {
  }
}
