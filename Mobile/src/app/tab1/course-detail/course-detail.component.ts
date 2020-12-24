import { Component, Input, OnInit } from "@angular/core";
import { ModalController } from "@ionic/angular";
import { Course } from "src/app/Models/course";
import { CourseEpisode } from "src/app/Models/CourseEpisode";
import { BasketService } from "src/app/Services/basket.service";
import { ShopService } from "src/app/Services/shop.service";
import { ToastService } from "src/app/Services/toast.service";

@Component({
  selector: "app-course-detail",
  templateUrl: "./course-detail.component.html",
  styleUrls: ["./course-detail.component.scss"],
})
export class CourseDetailComponent implements OnInit {
  @Input() courseId: number;
  courseEpisodes: CourseEpisode[];
  course: Course;

  constructor(
    private shopService: ShopService,
    private toast: ToastService,
    private modalController: ModalController,
    private basketService: BasketService
  ) {}

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
    this.basketService.addCourseToBasket(this.course);
    this.modalController.dismiss();
    this.toast.presentToast("بسته به سبد خرید اضافه گردید");
  }
}
