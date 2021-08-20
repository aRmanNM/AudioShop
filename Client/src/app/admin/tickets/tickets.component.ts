import {Component, OnInit} from '@angular/core';
import {Ticket} from '../../models/ticket';
import {TicketService} from '../../services/ticket.service';
import {MatDialog} from '@angular/material/dialog';
import {SpinnerService} from '../../services/spinner.service';
import {TicketResponsesComponent} from './ticket-responses/ticket-responses.component';

@Component({
  selector: 'app-tickets',
  templateUrl: './tickets.component.html',
  styleUrls: ['./tickets.component.scss']
})
export class TicketsComponent implements OnInit {
  tickets: Ticket[];
  dialogActive = false;
  isFinished = false;
  columnsToDisplay = ['id', 'userName', 'title', 'createdAt', 'ticketStatus', 'actions'];

  constructor(private ticketService: TicketService,
              public dialog: MatDialog,
              public spinnerService: SpinnerService) {
  }

  ngOnInit(): void {
    this.ticketService.updatedEmmiter.subscribe(() => {
      this.getTickets();
    });

    this.ticketService.onUpdate();
  }

  getTickets(): void {
    this.ticketService.getTickets(this.isFinished).subscribe((res) => {
      this.tickets = res;
    });
  }

  openResponseDialog(ticketId: number): void {
    this.dialogActive = true;
    const dialogRef = this.dialog.open(TicketResponsesComponent, {
      width: '700px',
      data: {ticketId}
    });

    dialogRef.afterClosed().subscribe(res => {
      this.dialogActive = false;
    });
  }

  refreshTickets(): void {
    this.ticketService.onUpdate();
  }

}
