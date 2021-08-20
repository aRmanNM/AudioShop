export interface TicketResponse {
  id: number;
  ticketId: number;
  body: string;
  createdAt: Date;
  issuedByAdmin: boolean;
}
