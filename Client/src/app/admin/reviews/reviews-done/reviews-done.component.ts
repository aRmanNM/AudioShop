import {Component, OnInit, ViewChild} from '@angular/core';
import {SpinnerService} from '../../../services/spinner.service';
import {Review} from '../../../models/review';
import {CoursesAndEpisodesService} from '../../../services/courses-and-episodes.service';
import {ReviewsEditComponent} from '../reviews-edit/reviews-edit.component';
import {MatDialog} from '@angular/material/dialog';
import {MatPaginator} from '@angular/material/paginator';

@Component({
  selector: 'app-reviews-done',
  templateUrl: './reviews-done.component.html',
  styleUrls: ['./reviews-done.component.scss']
})
export class ReviewsDoneComponent implements OnInit {
  dialogActive = false;
  reviews: Review[];
  columnsToDisplay = ['id', 'text', 'rating', 'date', 'courseName', 'actions'];
  totalItems: number;
  pageSize = 10;
  pageIndex = 0;
  @ViewChild(MatPaginator) paginator: MatPaginator;

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
    this.coursesAndEpisodesService.getAllReviews(true, this.pageIndex, this.pageSize).subscribe((res) => {
      this.reviews = res.items;
      this.totalItems = res.totalItems;
    });
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

  changePage(): void {
    this.pageIndex = this.paginator.pageIndex;
    this.pageSize = this.paginator.pageSize;
    this.coursesAndEpisodesService.onReviewsUpdate();
  }
}
