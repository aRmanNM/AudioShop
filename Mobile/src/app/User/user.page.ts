import {Component, OnInit} from '@angular/core';
import {ModalController} from '@ionic/angular';
import {Course} from '../Models/course';
import {CourseService} from '../Services/course.service';
import {UserCourseDetailComponent} from './user-course-detail/user-course-detail.component';
import {AuthService} from "../Services/auth.service";
import {UserService} from "../Services/user.service";

@Component({
    selector: 'app-user',
    templateUrl: 'user.page.html',
    styleUrls: ['user.page.scss']
})
export class UserPage implements OnInit {
    courses: Course[];

    constructor(private userService: UserService, shopService: CourseService, private modalController: ModalController, private authService: AuthService) {
    }

    ngOnInit() {
        this.getUserCourses();
    }

    getUserCourses() {
        return this.userService.getUserCourses(this.authService.decodedToken.nameid).subscribe((res) => {
            this.courses = res;
        });
    }

    ionViewDidEnter() {
        this.getUserCourses();
    }

    async toggleCourseDetail(courseId: number) {
        const modal = await this.modalController.create({
            component: UserCourseDetailComponent,
            componentProps: {courseId},
        });
        return await modal.present();
    }

}
