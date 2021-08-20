import {Injectable} from '@angular/core';
import {Observable, Subject} from 'rxjs';
import {environment} from '../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {TicketResponse} from '../models/ticket-response';
import {Ticket} from '../models/ticket';

@Injectable({
  providedIn: 'root'
})
export class TicketService {
  updatedEmmiter = new Subject();
  baseUrl = environment.apiUrl + 'api/tickets';

  constructor(private http: HttpClient) {
  }

  onUpdate(): any {
    this.updatedEmmiter.next();
  }

  createTicketResponse(ticketResponse: TicketResponse): Observable<TicketResponse> {
    return this.http.post<TicketResponse>(this.baseUrl + '/response', ticketResponse);
  }

  getTickets(isFinished: boolean): Observable<Ticket[]> {
    return this.http.get<Ticket[]>(this.baseUrl, {
      params: {
        isFinished: isFinished.toString()
      }
    });
  }

  getTicketWithResponses(ticketId: number): Observable<Ticket> {
    return this.http.get<Ticket>(this.baseUrl + '/' + ticketId);
  }
}
