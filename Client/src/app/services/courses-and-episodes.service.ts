import {Injectable} from '@angular/core';
import {environment} from '../../environments/environment';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Course} from '../models/course';
import {Observable} from 'rxjs';
import {Episode} from '../models/episode';

@Injectable({
    providedIn: 'root'
})
export class CoursesAndEpisodesService {
    baseUrl = environment.apiUrl + 'api/courses';

    constructor(private http: HttpClient) {
    }

    getCourses(search: string): Observable<Course[]> {
        return this.http.get<Course[]>(this.baseUrl, {
            params: new HttpParams().set('search', search).set('includeepisodes', 'false')
        });
    }

    getCourseEpisodes(courseId: number): Observable<Episode[]> {
        return this.http.get<Episode[]>(this.baseUrl + '/' + courseId + '/episodes');
    }

    createCourse(course: Course): Observable<Course> {
        return this.http.post<Course>(this.baseUrl, course);
    }

    updateCourse(course: Course): Observable<Course> {
        return this.http.put<Course>(this.baseUrl, course);
    }

    uploadPhoto(courseId: number, photo: any): any {
        const formData = new FormData();
        formData.append('file', photo);
        return this.http.post(this.baseUrl + '/' + courseId + '/photo', formData);
    }
}
