import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable, Subject} from 'rxjs';
import {environment} from '../../environments/environment';
import {Ad} from '../models/ad';
import {Place} from '../models/place';

@Injectable({
  providedIn: 'root'
})
export class AdService {
  updatedEmmiter = new Subject();
  baseUrl = environment.apiUrl + 'api/ads';

  constructor(private http: HttpClient) {
  }

  onUpdate(): any {
    this.updatedEmmiter.next();
  }

  getAds(): Observable<Ad[]> {
    return this.http.get<Ad[]>(this.baseUrl);
  }

  getPlaces(): Observable<Place[]> {
    return this.http.get<Place[]>(this.baseUrl + '/places');
  }

  getAdById(adId: number): Observable<Ad> {
    return this.http.get<Ad>(this.baseUrl + '/' + adId);
  }

  createAd(ad: Ad): Observable<Ad> {
    return this.http.post<Ad>(this.baseUrl, ad);
  }

  editAd(ad: Ad): Observable<Ad> {
    return this.http.put<Ad>(this.baseUrl + '/' + ad.id, ad);
  }

  editPlace(place: Place): Observable<Place> {
    return this.http.put<Place>(this.baseUrl + '/places/' + place.id, place);
  }

  uploadFile(adId: number, file: any): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post(`${this.baseUrl}/${adId}/file`, formData);
  }
}
