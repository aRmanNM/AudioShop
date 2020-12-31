import {Component, OnInit} from '@angular/core';
import {Course} from '../Models/course';
import {CourseService} from '../Services/course.service';

@Component({
    selector: 'app-shop',
    templateUrl: 'shop.page.html',
    styleUrls: ['shop.page.scss']
})
export class ShopPage implements OnInit {
    searchActivated: boolean = false;
    filter: string = "";
    courses: Course[];

    constructor(private shopService: CourseService) {
    }

    ngOnInit() {
        this.getCourses();
    }

    toggleSearch() {
        this.searchActivated = !this.searchActivated;
        if (!this.searchActivated) {
            this.filter = "";
            this.getCourses();
        }
    }

    filterResult() {
        this.getCourses();
    }

    getCourses() {
        this.courses = [];
        this.shopService.getCourses(this.filter).subscribe((res) => {
            this.courses = res;
        })
    }
}
