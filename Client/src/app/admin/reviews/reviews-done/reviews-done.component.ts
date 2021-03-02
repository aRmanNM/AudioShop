import {Component, OnInit} from '@angular/core';
import {SpinnerService} from '../../../services/spinner.service';
import {Review} from '../../../models/review';
import {CoursesAndEpisodesService} from '../../../services/courses-and-episodes.service';
import {ReviewsEditComponent} from '../reviews-edit/reviews-edit.component';
import {MatDialog} from '@angular/material/dialog';

@Component({
  selector: 'app-reviews-done',
  templateUrl: './reviews-done.component.html',
  styleUrls: ['./reviews-done.component.scss']
})
export class ReviewsDoneComponent implements OnInit {
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
      this.coursesAndEpisodesService.getCourseReviews(Number(this.courseId), true).subscribe((res) => {
        this.reviews = res;
      });
    } else {
      this.coursesAndEpisodesService.getAllReviews(true).subscribe((res) => {
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
}
