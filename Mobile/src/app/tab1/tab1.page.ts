import { Component, OnInit } from '@angular/core';
import { Course } from '../Models/course';
import { ShopService } from '../Services/shop.service';

@Component({
  selector: 'app-tab1',
  templateUrl: 'tab1.page.html',
  styleUrls: ['tab1.page.scss']
})
export class Tab1Page implements OnInit {
  searchActivated: boolean = false;
  filter: string = "";
  courses: Course[];

  constructor(private shopService: ShopService) {}

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
