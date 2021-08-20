import {Component, OnInit} from '@angular/core';
import {AdminService} from '../../services/admin.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {
  user: any;
  userName: string;
  constructor(private adminService: AdminService) {
  }

  ngOnInit(): void {
  }

  getUser(): void {
    this.user = null;
    this.adminService.getUserInfo(this.userName).subscribe((res) => {
      this.user = res;
    });
  }

}
