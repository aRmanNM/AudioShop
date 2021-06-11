import {Component, Inject, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {CoursesAndEpisodesService} from '../../../services/courses-and-episodes.service';
import {MatSnackBar} from '@angular/material/snack-bar';
import {SpinnerService} from '../../../services/spinner.service';
import {Review} from '../../../models/review';

class DialogData {
  review: Review;
}

@Component({
  selector: 'app-reviews-edit',
  templateUrl: './reviews-edit.component.html',
  styleUrls: ['./reviews-edit.component.scss']
})
export class ReviewsEditComponent implements OnInit {

  reviewForm = new FormGroup(
    {
      id: new FormControl(''),
      text: new FormControl('', [Validators.required]),
      adminMessage: new FormControl(''),
      accepted: new FormControl('', [Validators.required])
    }
  );

  constructor(public dialogRef: MatDialogRef<ReviewsEditComponent>,
              @Inject(MAT_DIALOG_DATA) public data: DialogData,
              private coursesAndEpisodesService: CoursesAndEpisodesService,
              private snackBar: MatSnackBar,
              public spinnerService: SpinnerService) {
  }

  ngOnInit(): void {
    if (this.data.review) {
      this.reviewForm.setValue({
        id: this.data.review.id,
        text: this.data.review.text,
        accepted: this.data.review.accepted,
        adminMessage: this.data.review.adminMessage
      });
    }
  }

  closeDialog(): void {
    this.dialogRef.close();
    this.coursesAndEpisodesService.onReviewsUpdate();
  }

  updateReview(): void {
    this.data.review.text = this.reviewForm.value.text;
    this.data.review.accepted = this.reviewForm.value.accepted;
    this.data.review.adminMessage = this.reviewForm.value.adminMessage;
    this.coursesAndEpisodesService.updateReview(this.data.review.courseId, this.data.review.id, this.data.review).subscribe((res) => {
      this.snackBar.open('ویرایش نظر انجام شد', null, {
        duration: 2000,
      });
      this.closeDialog();
    });
  }

}
