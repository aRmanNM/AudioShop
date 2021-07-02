import {Injectable} from '@angular/core';
import {environment} from '../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {Stat} from '../models/stat';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StatService {
  baseUrl = environment.apiUrl + 'api/stats/get/all/';

  constructor(private http: HttpClient) {
  }

  getAllStatsInRange(start: string, end: string): Observable<Stat[]> {
    return this.http.post<Stat[]>(this.baseUrl + 'daily', {start, end});
  }

  getAllStatsTotal(): Observable<Stat[]> {
    return this.http.post<Stat[]>(this.baseUrl + 'total', {});
  }
}

