import {Component, OnInit} from '@angular/core';
import {Message} from '../../models/message';
import {MatDialog} from '@angular/material/dialog';
import {SpinnerService} from '../../services/spinner.service';
import {MessageService} from '../../services/message.service';
import {MessagesCreateEditComponent} from './messages-create-edit/messages-create-edit.component';
import {MatSnackBar} from '@angular/material/snack-bar';
import {UserMessagesComponent} from './user-messages/user-messages.component';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.scss']
})
export class MessagesComponent implements OnInit {
  messages: Message[];
  columnsToDisplay = ['id', 'title', 'messageType', 'courseId', 'repeatAfterHour', 'sendPush', 'sendInApp', 'isRepeatable', 'actions'];
  dialogActive = false;

  constructor(public dialog: MatDialog,
              public spinnerService: SpinnerService,
              private messageService: MessageService,
              private snackBar: MatSnackBar) {
  }

  ngOnInit(): void {
    this.messageService.updatedEmmiter.subscribe(() => {
      this.getMessages();
    });

    this.messageService.onUpdate();
  }

  getMessages(): void {
    this.messageService.getGeneralMessages().subscribe((res) => {
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
        this.messageService.onUpdate();
      }, error => {
        this.snackBar.open('حذف پیام با خطا روبرو شد', null, {
          duration: 3000,
        });
      });
    }
  }

  openAddOrEditDialog(message: Message): void {
    this.dialogActive = true;
    const dialogRef = this.dialog.open(MessagesCreateEditComponent, {
      width: '700px',
      data: {message}
    });

    dialogRef.afterClosed().subscribe(res => {
      this.dialogActive = false;
    });
  }

  openUserMessagesDialog(): void {
    this.dialogActive = true;
    const dialogRef = this.dialog.open(UserMessagesComponent, {
      width: '1000px',
      data: { }
    });

    dialogRef.afterClosed().subscribe(res => {
      this.dialogActive = false;
    });
  }

}
