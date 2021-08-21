import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {MatSnackBar} from '@angular/material/snack-bar';
import {SpinnerService} from '../../../services/spinner.service';
import {TicketService} from '../../../services/ticket.service';
import {Ticket} from '../../../models/ticket';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {TicketResponse} from '../../../models/ticket-response';

class DialogData {
  ticketId: number;
}

@Component({
  selector: 'app-ticket-responses',
  templateUrl: './ticket-responses.component.html',
  styleUrls: ['./ticket-responses.component.scss']
})
export class TicketResponsesComponent implements OnInit {
  ticketWithResponses: Ticket;

  constructor(public dialogRef: MatDialogRef<TicketResponsesComponent>,
              @Inject(MAT_DIALOG_DATA) public data: DialogData,
              private ticketService: TicketService,
              private snackBar: MatSnackBar,
              public spinnerService: SpinnerService) {
  }

  responseForm = new FormGroup(
    {
      id: new FormControl(0),
      body: new FormControl('', [Validators.required]),
    }
  );

  ngOnInit(): void {
    this.getTicketResponses();
  }

  getTicketResponses(): void {
    this.ticketService.getTicketWithResponses(this.data.ticketId).subscribe((res) => {
      this.ticketWithResponses = res;
    });
  }

  createResponse(): void {
    const ticketResponse: TicketResponse = this.responseForm.value;
    ticketResponse.ticketId = this.data.ticketId;
    ticketResponse.issuedByAdmin = true;
    this.ticketService.createTicketResponse(ticketResponse).subscribe((res) => {
      this.getTicketResponses();
      this.snackBar.open('درج پاسخ موفقیت آمیز بود', null, {
        duration: 2000,
      });
    });
  }

}
