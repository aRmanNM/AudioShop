import {Component, Input, OnInit} from '@angular/core';
import {CourseService} from 'src/app/Services/course.service';
import {UserService} from "../../Services/user.service";
import {CourseEpisode} from "../../Models/CourseEpisode";

@Component({
    selector: 'app-user-course-detail',
    templateUrl: './user-course-detail.component.html',
    styleUrls: ['./user-course-detail.component.scss']
})
export class UserCourseDetailComponent implements OnInit {

    @Input() courseId: number;
    courseEpisodes: CourseEpisode[];

    constructor(private userService: UserService, private courseService: CourseService) {
    }

    ngOnInit() {
        this.getCourseEpisodes();
    }

    getCourseEpisodes() {
        this.courseService.getCourseEpisodes(this.courseId).subscribe((res) => {
            this.courseEpisodes = res;
        })
    }

    playEpisode() {

    }

}
