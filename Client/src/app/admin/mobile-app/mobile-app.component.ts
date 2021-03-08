import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {AppFileService} from '../../services/app-file.service';
import {MatSnackBar} from '@angular/material/snack-bar';
import {SpinnerService} from '../../services/spinner.service';
import {Config} from '../../models/config';

@Component({
  selector: 'app-mobile-app',
  templateUrl: './mobile-app.component.html',
  styleUrls: ['./mobile-app.component.scss']
})
export class MobileAppComponent implements OnInit {
  @ViewChild('fileInput') fileInput: ElementRef;
  latest: Config;

  constructor(private appFileService: AppFileService,
              private snackBar: MatSnackBar,
              public spinnerService: SpinnerService) {
  }

  ngOnInit(): void {
    this.getLatest();
  }

  uploadApp(): any {
    const nativeElement = this.fileInput.nativeElement;
    this.appFileService.uploadApp(nativeElement.files[0]).subscribe((res) => {
      this.snackBar.open('فایل اپلود شد', null, {
        duration: 2000,
      });
      this.getLatest();
    }, ((e) => {
      this.snackBar.open('خطایی رخ داد', null, {
        duration: 2000,
      });
    }));
  }

  getLatest(): void {
    this.appFileService.getLatest().subscribe((res) => {
      this.latest = res;
    });
  }

}
