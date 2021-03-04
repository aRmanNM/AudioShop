import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Observable, Subject} from 'rxjs';
import {Config} from '../models/config';
import {environment} from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ConfigService {
  baseUrl = environment.apiUrl + 'api/configs';
  configUpdateEmitter = new Subject();

  constructor(private http: HttpClient) {
  }

  onConfigUpdate(): void {
    this.configUpdateEmitter.next();
  }

  getConfig(titleEn: string): Observable<Config> {
    return this.http.get<Config>(this.baseUrl, {
      params: new HttpParams().set('title', titleEn)
    });
  }

  getAllConfigs(): Observable<Config[]> {
    return this.http.get<Config[]>(this.baseUrl + '/all');
  }

  setConfig(config: Config): Observable<Config> {
    return this.http.put<Config>(this.baseUrl, config);
  }
}
