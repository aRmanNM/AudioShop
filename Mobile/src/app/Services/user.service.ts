import {Injectable} from '@angular/core';
import {Course} from '../Models/Course';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../environments/environment';
import {Basket} from '../Models/Basket';

@Injectable({
    providedIn: 'root'
})
export class UserService {
    baseUrl = environment.apiUrl + 'user/';

    constructor(private http: HttpClient) {
    }

    getUserCourses(userId: string) {
        return this.http.get<Course[]>(this.baseUrl + 'courses/' + userId);
    }

    CheckIfCourseIsRepetitive(userId, courseId) {
        return this.http.get<boolean>(this.baseUrl + 'CheckIfCourseIsRepetitive/' + userId + '/' + courseId);
    }

    RefineRepetitiveCourses(basket: Basket) {
        return this.http.post<Basket>(this.baseUrl + 'RefineRepetitiveCourses', basket);
    }
}
