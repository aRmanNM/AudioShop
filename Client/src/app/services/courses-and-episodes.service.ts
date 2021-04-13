import {Injectable} from '@angular/core';
import {environment} from '../../environments/environment';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Course} from '../models/course';
import {Observable, of, Subject} from 'rxjs';
import {Episode} from '../models/episode';
import {Review} from '../models/review';
import {PaginatedResult} from '../models/paginated-result';

@Injectable({
  providedIn: 'root'
})
export class CoursesAndEpisodesService {
  updatedEmmiter = new Subject();
  reviewsUpdatedEmitter = new Subject();
  baseUrl = environment.apiUrl + 'api/courses';

  constructor(private http: HttpClient) {
  }

  onUpdate(): any {
    this.updatedEmmiter.next();
  }

  onReviewsUpdate(): any {
    this.reviewsUpdatedEmitter.next();
  }

  getCourses(search: string, pageIndex: number, pageSize: number): Observable<PaginatedResult<Course>> {
    return this.http.get<PaginatedResult<Course>>(this.baseUrl, {
      params: new HttpParams()
        .set('search', search)
        .set('includeEpisodes', 'false')
        .set('includeInactive', 'true')
        .set('pageNumber', `${pageIndex + 1}`)
        .set('pageSize', `${pageSize}`)
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

  updateEpisode(episode: Episode): Observable<Episode> {
    return this.http.put<Episode>(this.baseUrl + '/episodes', episode);
  }

  createEpisode(episode: Episode): Observable<Episode> {
    return this.http.post<Episode>(this.baseUrl + '/episodes', episode);
  }

  uploadAudio(episodeId: number, audio: any): Observable<any> {
    const formData = new FormData();
    formData.append('file', audio);
    return this.http.post(`${environment.apiUrl}api/episodes/${episodeId}/audios`, formData);
  }

  deleteEpisodeAudios(episodeId: number): Observable<any> {
    return this.http.delete(`${environment.apiUrl}api/episodes/${episodeId}/audios`);
  }

  getEpisode(episodeId: number): Observable<Episode> {
    return this.http.get<Episode>(`${this.baseUrl}/episodes/${episodeId}`);
  }

  updateCourseEpisodes(courseId: number, episodes: Episode[]): Observable<any> {
    return this.http.put<Episode[]>(this.baseUrl + '/' + courseId + '/episodes', episodes);
  }

  getAllReviews(accepted: boolean, pageIndex: number, pageSize: number): Observable<PaginatedResult<Review>> {
    return this.http.get<PaginatedResult<Review>>(this.baseUrl + '/reviews', {
      params: new HttpParams()
        .set('accepted', `${accepted}`)
        .set('pageNumber', `${pageIndex + 1}`)
        .set('pageSize', `${pageSize}`)
    });
  }

  getCourseReviews(courseId: number, accepted: boolean): Observable<Review[]> {
    return this.http.get<Review[]>(`${this.baseUrl}/${courseId}/reviews`, {
      params: new HttpParams().set('accepted', `${accepted}`)
    });
  }

  updateReview(courseId: number = 0, reviewId: number, review: Review): Observable<Review> {
    return this.http.put<Review>(`${this.baseUrl}/${courseId}/reviews/${reviewId}`, review);
  }

  deleteEpisode(episodeId: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/episodes/${episodeId}`);
  }

  ToggleMultipleReviews(Ids: number[]): Observable<any> {
    return this.http.put(this.baseUrl + '/reviews', Ids, {
      params: new HttpParams().set('action', 'toggle')
    });
  }

  DeleteMultipleReviews(Ids: number[]): Observable<any> {
    return this.http.put(this.baseUrl + '/reviews', Ids, {
      params: new HttpParams().set('action', 'delete')
    });
  }
}
