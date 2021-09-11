import {Component, Inject, OnInit} from '@angular/core';
import {Message} from '../../../models/message';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {MatSnackBar} from '@angular/material/snack-bar';
import {SpinnerService} from '../../../services/spinner.service';
import {MessageService} from '../../../services/message.service';
import {FormControl, FormGroup, Validators} from '@angular/forms';

interface DialogData {
  message: Message;
}

@Component({
  selector: 'app-messages-create-edit',
  templateUrl: './messages-create-edit.component.html',
  styleUrls: ['./messages-create-edit.component.scss']
})
export class MessagesCreateEditComponent implements OnInit {


  constructor(public dialogRef: MatDialogRef<MessagesCreateEditComponent>,
              @Inject(MAT_DIALOG_DATA) public data: DialogData,
              private messageService: MessageService,
              private snackBar: MatSnackBar,
              public spinnerService: SpinnerService) {
  }

  messageForm = new FormGroup(
    {
      id: new FormControl(0),
      title: new FormControl('', [Validators.required]),
      body: new FormControl('', [Validators.required]),
      link: new FormControl(''),
      courseId: new FormControl(0),
      userId: new FormControl(''),
      repeatAfterHour: new FormControl(24),
      isRepeatable: new FormControl(false),
      sendPush: new FormControl(false),
      sendSMS: new FormControl(false),
      sendInApp: new FormControl(false),
      messageType: new FormControl('')
    }
  );

  ngOnInit(): void {
    if (this.data.message) {
      this.messageForm.setValue({
        id: this.data.message.id,
        title: this.data.message.title,
        body: this.data.message.body,
        link: this.data.message.link,
        courseId: this.data.message.courseId,
        userId: this.data.message.userId,
        repeatAfterHour: this.data.message.repeatAfterHour,
        isRepeatable: this.data.message.isRepeatable,
        sendPush: this.data.message.sendPush,
        sendSMS: this.data.message.sendSMS,
        sendInApp: this.data.message.sendInApp,
        messageType: this.data.message.messageType.toString()
      });
    }
  }

  createOrEditMessage(): void {
    if (this.data.message) {
      const message: Message = this.messageForm.value;
      if (message.messageType != 1) {
        message.sendSMS = false;
      }
      if (message.messageType == 1) {
        message.isRepeatable = false;
      }
      this.messageService.editMessage(message).subscribe((res) => {
        this.snackBar.open('ویرایش اعلان موفقیت آمیز بود', null, {
          duration: 2000,
        });
        this.messageService.onUpdate();
        this.closeDialog();
      }, error => {
        this.snackBar.open('ویرایش اعلان شکست خورد', null, {
          duration: 2000,
        });
      });
    } else {
      const message: Message = this.messageForm.value;
      if (message.messageType != 1) {
        message.sendSMS = false;
      }
      if (message.messageType == 1) {
        message.isRepeatable = false;
      }
      this.messageService.createMessage(message).subscribe((res) => {
        this.snackBar.open('اعلان جدید با موفقیت ایجاد شد', null, {
          duration: 2000,
        });
        this.messageService.onUpdate();
        this.closeDialog();
      }, error => {
        this.snackBar.open('ساخت اعلان جدید شکست خورد', null, {
          duration: 2000,
        });
      });
    }
  }

  closeDialog(): void {
    this.dialogRef.close();
  }

}
