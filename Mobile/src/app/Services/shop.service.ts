import { Injectable } from "@angular/core";
import { map } from "rxjs/operators";
import { Course } from "../Models/course";
import { User } from "../Models/User";
import { environment } from "src/environments/environment";
import { HttpClient } from "@angular/common/http";
import { CourseEpisode } from "../Models/CourseEpisode";

@Injectable({
  providedIn: "root",
})
export class ShopService {
  baseUrl = environment.apiUrl + 'course/';

  constructor(private http: HttpClient) {}

  getCourses(filter: string) {
    return this.http.get<Course[]>(this.baseUrl).pipe(
      map((courses: Course[]) => courses.filter(course => course.name.includes(filter)))
    );
  }

  getCourse(courseId: number) {
    return this.http.get<Course>(this.baseUrl + courseId);
  }

  getCourseEpisodes(courseId: number) {
    return this.http.get<CourseEpisode[]>(this.baseUrl + 'episodes/' + courseId);
  }

  getCourseEpisode(episodeId: number) {
    return this.http.get<CourseEpisode>(this.baseUrl + 'episode/' + episodeId);
  }

  getUserCourses() {
    if (localStorage.getItem('user') === null) return null;
    let user = JSON.parse(localStorage.getItem('user'));
    return user.courses;
  }

  // TODO: To be implemented currectly!
  addCoursesToUser(courses: any) {
    if (localStorage.getItem('user') === null) {
      let emptyUser: any = {};
      emptyUser.courses = [];
      localStorage.setItem('user', JSON.stringify(emptyUser));
    }

    let user: User = JSON.parse(localStorage.getItem('user'));
    courses.courses.forEach(element => {
      if (!user.courses.find(c => c.id === element.courseId)) {
        user.courses.push(element);
      }
    });

    localStorage.setItem('user', JSON.stringify(user));
    return;
  }

}
