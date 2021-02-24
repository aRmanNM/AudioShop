import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Observable} from 'rxjs';
import {Config} from '../models/config';
import {environment} from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ConfigService {
  baseUrl = environment.apiUrl + 'api/configs';

  constructor(private http: HttpClient) {
  }

  getConfig(titleEn: string): Observable<Config> {
    return this.http.get<Config>(this.baseUrl, {
      params: new HttpParams().set('title', titleEn)
    });
  }
}
