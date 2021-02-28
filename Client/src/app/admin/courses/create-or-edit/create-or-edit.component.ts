import {Component, ElementRef, Inject, Input, OnInit, ViewChild} from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {Course} from '../../../models/course';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {CoursesAndEpisodesService} from '../../../services/courses-and-episodes.service';
import {MatSnackBar} from '@angular/material/snack-bar';
import {environment} from '../../../../environments/environment';
import {SpinnerService} from '../../../services/spinner.service';

interface DialogData {
  course: Course;
}

@Component({
  selector: 'app-create-or-edit',
  templateUrl: './create-or-edit.component.html',
  styleUrls: ['./create-or-edit.component.scss']
})
export class CreateOrEditComponent implements OnInit {
  baseUrl = environment.apiUrl + 'Files/';
  imgUrl;
  @ViewChild('fileInput') fileInput: ElementRef;

  constructor(public dialogRef: MatDialogRef<CreateOrEditComponent>,
              @Inject(MAT_DIALOG_DATA) public data: DialogData,
              private coursesAndEpisodesService: CoursesAndEpisodesService,
              private snackBar: MatSnackBar,
              public spinnerService: SpinnerService) {
  }

  courseForm = new FormGroup(
    {
      id: new FormControl(''),
      name: new FormControl('', [Validators.required]),
      description: new FormControl(''),
      price: new FormControl('', [Validators.required]),
      isActive: new FormControl('', [Validators.required]),
      waitingTimeBetweenEpisodes: new FormControl('', [Validators.required])
    }
  );

  ngOnInit(): void {
    if (this.data.course) {
      this.courseForm.setValue({
        id: this.data.course.id,
        name: this.data.course.name,
        description: this.data.course.description,
        price: this.data.course.price,
        isActive: this.data.course.isActive,
        waitingTimeBetweenEpisodes: this.data.course.waitingTimeBetweenEpisodes
      });

      this.getImage();
    }
  }

  createOrEditCourse(): void {
    if (this.data.course) {
      this.coursesAndEpisodesService.updateCourse(this.courseForm.value).subscribe((res) => {
        this.snackBar.open('ویرایش دوره موفقیت آمیز بود', null, {
          duration: 2000,
        });
        this.coursesAndEpisodesService.onUpdate();
        this.closeDialog();
      });
    } else {
      this.coursesAndEpisodesService.createCourse(this.courseForm.value).subscribe((res) => {
        this.snackBar.open('دوره جدید با موفقیت ایجاد شد', null, {
          duration: 2000,
        });
        this.coursesAndEpisodesService.onUpdate();
        this.closeDialog();
      });
    }
  }

  closeDialog(): void {
    this.dialogRef.close();
  }

  uploadPhoto(): any {
    const nativeElement = this.fileInput.nativeElement;
    this.coursesAndEpisodesService.uploadPhoto(this.data.course.id, nativeElement.files[0]).subscribe((res) => {
      this.data.course.photoFileName = res.fileName;
      this.getImage();
      this.coursesAndEpisodesService.onUpdate();
    });
  }

  getImage(): void {
    this.imgUrl = this.baseUrl + this.data.course.id + '/' + this.data.course.photoFileName;
  }
}
