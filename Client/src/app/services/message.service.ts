import {Injectable} from '@angular/core';
import {Observable, Subject} from 'rxjs';
import {environment} from '../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {Message} from '../models/message';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  updatedEmmiter = new Subject();
  baseUrl = environment.apiUrl + 'api/messages';

  constructor(private http: HttpClient) {
  }

  onUpdate(): any {
    this.updatedEmmiter.next();
  }

  getGeneralMessages(): Observable<Message[]> {
    return this.http.get<Message[]>(this.baseUrl);
  }

  createMessage(message: Message): Observable<Message> {
    return this.http.post<Message>(this.baseUrl, message);
  }

  editMessage(message: Message): Observable<any> {
    return this.http.put<any>(this.baseUrl, message);
  }

  deleteMessage(messageId: number): Observable<any> {
    return this.http.delete(this.baseUrl, {
      params: {
        messageId: messageId.toString()
      }
    });
  }

  getUserMessages(userId: string): Observable<Message[]> {
    return this.http.get<Message[]>(this.baseUrl + '/users/' + userId);
  }

}
