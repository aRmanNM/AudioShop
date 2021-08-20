import {TicketResponse} from './ticket-response';

export interface Ticket {
  id: number;
  userId: string;
  userName: string;
  title: string;
  description: string;
  createdAt: string;
  ticketStatus: number;
  ticketResponses: TicketResponse[];
}
