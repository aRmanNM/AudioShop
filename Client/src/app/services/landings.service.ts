import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../environments/environment';
import {Observable, Subject} from 'rxjs';
import {Landing} from '../models/landing';
import {LandingPhoneNumber} from '../models/landing-phone-number';

@Injectable({
  providedIn: 'root'
})
export class LandingsService {
  updatedEmmiter = new Subject();
  baseUrl = environment.apiUrl + 'api/landing';

  constructor(private http: HttpClient) {
  }

  onUpdate(): any {
    this.updatedEmmiter.next();
  }

  getLandings(): Observable<Landing[]> {
    return this.http.get<Landing[]>(this.baseUrl);
  }

  getLanding(landingId: number): Observable<Landing> {
    return this.http.get<Landing>(`${this.baseUrl}/${landingId}`);
  }

  getLandingByUrlName(urlName: string): Observable<Landing> {
    return this.http.get<Landing>(`${this.baseUrl}/urlname/${urlName}`);
  }

  createLanding(landing: Landing): Observable<Landing> {
    return this.http.post<Landing>(this.baseUrl, landing);
  }

  updateLanding(landing: Landing): Observable<any> {
    return this.http.put<any>(this.baseUrl, landing);
  }

  uploadFile(landingId: number, field: string, file: any): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post(`${this.baseUrl}/${landingId}/files/${field}`, formData);
  }

  createLandingPhoneNumber(landingPhoneNumber: LandingPhoneNumber): Observable<any> {
    return this.http.post(`${this.baseUrl}/${landingPhoneNumber.id}/phonenumber`, landingPhoneNumber);
  }

  updateLandingStat(landingId: number): Observable<any> {
    return this.http.put(`${this.baseUrl}/${landingId}/updatestat`, null);
  }

  exportLandingPhoneNumbers(landingId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/${landingId}/phonenumber`, {
        responseType : 'arraybuffer',
      }
    );
  }
}
