import {Injectable} from '@angular/core';
import {environment} from '../../environments/environment';
import {Observable, Subject} from 'rxjs';
import {HttpClient} from '@angular/common/http';
import {SliderItem} from '../models/slider-item';

@Injectable({
  providedIn: 'root'
})
export class SliderService {
  baseUrl = environment.apiUrl + 'api/sliders/';
  sliderUpdateEmitter = new Subject();

  constructor(private http: HttpClient) {
  }

  onSliderUpdate(): void {
    this.sliderUpdateEmitter.next();
  }

  getSliderItems(): Observable<SliderItem[]> {
    return this.http.get<SliderItem[]>(this.baseUrl);
  }

  createSliderItem(sliderItem: SliderItem): Observable<SliderItem> {
    return this.http.post<SliderItem>(this.baseUrl, sliderItem);
  }

  updateSliderItem(sliderItem: SliderItem): Observable<SliderItem> {
    return this.http.put<SliderItem>(this.baseUrl, sliderItem);
  }

  deleteSliderItem(sliderItemId: number): Observable<any> {
    return this.http.delete(this.baseUrl + sliderItemId);
  }

  uploadPhoto(sliderItemId: number, photo: any): any {
    const formData = new FormData();
    formData.append('file', photo);
    return this.http.post(this.baseUrl + sliderItemId + '/photo', formData);
  }
}
