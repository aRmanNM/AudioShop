import {Component, Input, OnInit} from '@angular/core';
import {ModalController} from '@ionic/angular';
import {Course} from 'src/app/Models/course';
import {CourseEpisode} from 'src/app/Models/CourseEpisode';
import {OrderService} from 'src/app/Services/order.service';
import {CourseService} from 'src/app/Services/course.service';
import {ToastService} from 'src/app/Services/toast.service';
import {AuthService} from '../../Services/auth.service';

@Component({
    selector: 'app-course-detail',
    templateUrl: './course-detail.component.html',
    styleUrls: ['./course-detail.component.scss'],
})
export class CourseDetailComponent implements OnInit {
    @Input() courseId: number;
    courseEpisodes: CourseEpisode[];
    course: Course;

    constructor(
        private shopService: CourseService,
        private toast: ToastService,
        private modalController: ModalController,
        private orderService: OrderService,
        private authService: AuthService
    ) {
    }

    ngOnInit() {
        this.getCourse();
        this.getCourseEpisodes();
    }

    getCourseEpisodes() {
        this.shopService.getCourseEpisodes(this.courseId).subscribe((res) => {
            this.courseEpisodes = res;
        });
    }

    getCourse() {
        this.shopService.getCourse(this.courseId).subscribe((res) => {
            this.course = res;
        });
    }

    addCourseToBasket() {
        if (this.authService.loggedIn()) {
            this.orderService.CheckIfCourseIsRepetitive(this.course.id).subscribe((res) => {
                if (res) {
                    this.toast.presentToast('این بسته قبلا خریداری شده است');
                    this.modalController.dismiss();
                    return;
                } else {
                    const addResult: boolean = this.orderService.addCourseToBasket(this.course);
                    this.modalController.dismiss();
                    if (addResult) {
                        this.toast.presentToast('بسته به سبد خرید اضافه گردید');
                    } else {
                        this.toast.presentToast('این بسته قبلا در سبد قرار گرفته است');
                    }

                    return;
                }
            });
        } else {
            console.log('this part also executed!');
            const addResult: boolean = this.orderService.addCourseToBasket(this.course);
            this.modalController.dismiss();
            if (addResult) {
                this.toast.presentToast('بسته به سبد خرید اضافه گردید');
            } else {
                this.toast.presentToast('این بسته قبلا در سبد قرار گرفته است');
            }
        }
    }
}
