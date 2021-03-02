import {Component, OnInit} from '@angular/core';
import {Review} from '../../../models/review';
import {SpinnerService} from '../../../services/spinner.service';
import {CoursesAndEpisodesService} from '../../../services/courses-and-episodes.service';
import {MatDialog} from '@angular/material/dialog';
import {ReviewsEditComponent} from '../reviews-edit/reviews-edit.component';

@Component({
  selector: 'app-reviews-pending',
  templateUrl: './reviews-pending.component.html',
  styleUrls: ['./reviews-pending.component.scss']
})
export class ReviewsPendingComponent implements OnInit {
  courseId = '';
  dialogActive = false;
  reviews: Review[];
  columnsToDisplay = ['id', 'text', 'rating', 'date', 'courseName', 'actions'];

  constructor(public spinnerService: SpinnerService,
              private coursesAndEpisodesService: CoursesAndEpisodesService,
              private dialog: MatDialog) {
  }

  ngOnInit(): void {
    this.coursesAndEpisodesService.reviewsUpdatedEmitter.subscribe(() => {
      this.getReviews();
    });

    this.getReviews();
  }

  getReviews(): void {
    if (this.courseId) {
      this.coursesAndEpisodesService.getCourseReviews(Number(this.courseId), false).subscribe((res) => {
        this.reviews = res;
      });
    } else {
      this.coursesAndEpisodesService.getAllReviews(false).subscribe((res) => {
        this.reviews = res;
      });
    }
  }

  openAddOrEditDialog(review: Review): void {
    this.dialogActive = true;
    const dialogRef = this.dialog.open(ReviewsEditComponent, {
      width: '400px',
      data: {review}
    });

    dialogRef.afterClosed().subscribe(res => {
      this.dialogActive = false;
    });
  }

  refreshReviews(): void {
    this.coursesAndEpisodesService.onReviewsUpdate();
  }
}
