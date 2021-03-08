import {Injectable} from '@angular/core';
import {environment} from '../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {Config} from '../models/config';

@Injectable({
  providedIn: 'root'
})
export class AppFileService {
  baseUrl = environment.apiUrl + 'api/';

  constructor(private http: HttpClient) {
  }

  uploadApp(app: any): Observable<any> {
    const formData = new FormData();
    formData.append('file', app);
    return this.http.post(this.baseUrl + 'app/upload', formData);
  }

  getLatest(): Observable<Config> {
    return this.http.get<Config>(this.baseUrl + 'app/latest');
  }
}
