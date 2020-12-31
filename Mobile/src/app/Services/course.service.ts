import { Injectable } from "@angular/core";
import { map } from "rxjs/operators";
import { Course } from "../Models/course";
import { environment } from "src/environments/environment";
import { HttpClient } from "@angular/common/http";
import { CourseEpisode } from "../Models/CourseEpisode";

@Injectable({
  providedIn: "root",
})
export class CourseService {
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
}
