import {Component, OnInit} from '@angular/core';
import {AdminService} from '../../services/admin.service';
import {formatDate} from '@angular/common';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {
  user: any;
  userName: string;

  today;
  expDate;
  expired;

  constructor(private adminService: AdminService) {
  }

  ngOnInit(): void {
    this.today = formatDate(new Date(), 'yyyy-MM-dd', 'en_US');
  }

  getUser(): void {
    this.user = null;
    this.adminService.getUserInfo(this.userName).subscribe((res) => {
      this.user = res;
      this.expDate = formatDate(res.subscriptionExpirationDate, 'yyyy-MM-dd', 'en_US');
      this.expired = this.expDate >= this.today ? false : true;
      console.log(res);
    });
  }

}
