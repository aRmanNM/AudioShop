import {Component, OnInit} from '@angular/core';
import {MessageService} from '../../../services/message.service';
import {MatSnackBar} from '@angular/material/snack-bar';
import {SpinnerService} from '../../../services/spinner.service';

@Component({
  selector: 'app-user-messages',
  templateUrl: './user-messages.component.html',
  styleUrls: ['./user-messages.component.scss']
})
export class UserMessagesComponent implements OnInit {
  userId: string;
  messages: any[] = [];
  columnsToDisplay = ['id', 'title', 'messageType', 'date', 'sendPush', 'sendSMS', 'sendInApp'];

  constructor(private messageService: MessageService,
              private snackBar: MatSnackBar,
              public spinnerService: SpinnerService) {
  }

  ngOnInit(): void {
  }

  getMessagesForUser(): void {
    this.messageService.getMessagesForUser(this.userId, false, true).subscribe((res) => {
      this.messages = res;
    });
  }

  deleteMessage(messageId: number): void {
    const result = confirm('آیا از انجام این عملیات اطمینان دارید؟');
    if (result) {
      this.messageService.deleteMessage(messageId).subscribe(() => {
        this.snackBar.open('پیام با موفقیت حذف شد', null, {
          duration: 3000,
        });
        this.getMessagesForUser();
      }, error => {
        this.snackBar.open('حذف پیام با خطا روبرو شد', null, {
          duration: 3000,
        });
      });
    }
  }


}
