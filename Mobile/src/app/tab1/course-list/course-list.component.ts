import { Component, Input, OnInit } from "@angular/core";
import { ModalController } from "@ionic/angular";
import { Course } from "src/app/Models/course";
import { CourseDetailComponent } from "../course-detail/course-detail.component";

@Component({
  selector: "app-course-list",
  templateUrl: "./course-list.component.html",
  styleUrls: ["./course-list.component.scss"],
})
export class CourseListComponent implements OnInit {
  @Input() courses: Course[];

  constructor(public modalController: ModalController) {}

  ngOnInit() {}

  async toggleCourseDetail(courseId: number) {
    const modal = await this.modalController.create({
      component: CourseDetailComponent,
      componentProps: { courseId },
    });
    return await modal.present();
  }
}
