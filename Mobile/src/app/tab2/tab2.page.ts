import { Component, OnInit } from '@angular/core';
import { ModalController } from '@ionic/angular';
import { Course } from '../Models/course';
import { ShopService } from '../Services/shop.service';
import { UserCourseDetailComponent } from './user-course-detail/user-course-detail.component';

@Component({
  selector: 'app-tab2',
  templateUrl: 'tab2.page.html',
  styleUrls: ['tab2.page.scss']
})
export class Tab2Page implements OnInit  {
  courses: Course[];

  constructor(private shopService: ShopService, private modalController: ModalController) {}

  ngOnInit() {
    this.courses = this.getUserCourses();
  }

  getUserCourses() {
    return this.shopService.getUserCourses();
  }

  ionViewDidEnter() {
    this.courses = this.getUserCourses();
  }

  async toggleCourseDetail(courseId: number) {
    const modal = await this.modalController.create({
      component: UserCourseDetailComponent,
      componentProps: { courseId },
    });
    return await modal.present();
  }

}
