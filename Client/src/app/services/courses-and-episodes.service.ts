import {Injectable} from '@angular/core';
import {environment} from '../../environments/environment';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Course} from '../models/course';
import {Observable} from 'rxjs';
import {Episode} from '../models/episode';
import {delay} from 'rxjs/operators';

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
}
