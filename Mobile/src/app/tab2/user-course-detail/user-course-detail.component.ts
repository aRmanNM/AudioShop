import { Component, Input, OnInit } from '@angular/core';
import { ShopService } from 'src/app/Services/shop.service';

@Component({
  selector: 'app-user-course-detail',
  templateUrl: './user-course-detail.component.html',
  styleUrls: ['./user-course-detail.component.scss']
})
export class UserCourseDetailComponent implements OnInit {

  @Input() courseId: number;
  // courseDetail: CourseDetail = [];

  constructor(private shopService: ShopService) { }

  ngOnInit() {
    // this.getCourseDetail();
  }

  getCourseDetail() {
    // this.shopService.getCourseDetail(this.courseId).subscribe((res) => {
    //   this.courseDetail = res;
    // })
  }

  playEpisode() {

  }

}
