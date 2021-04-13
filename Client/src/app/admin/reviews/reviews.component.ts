import {Component, OnInit, ViewChild} from '@angular/core';
import {Review} from '../../models/review';
import {MatPaginator} from '@angular/material/paginator';
import {SpinnerService} from '../../services/spinner.service';
import {CoursesAndEpisodesService} from '../../services/courses-and-episodes.service';
import {MatDialog} from '@angular/material/dialog';
import {MatSnackBar} from '@angular/material/snack-bar';
import {ReviewsEditComponent} from './reviews-edit/reviews-edit.component';

@Component({
  selector: 'app-reviews',
  templateUrl: './reviews.component.html',
  styleUrls: ['./reviews.component.scss']
})
export class ReviewsComponent implements OnInit {
  courseId = '';
  dialogActive = false;
  reviews: Review[];
  columnsToDisplay = ['select', 'text', 'rating', 'date', 'courseName', 'actions'];
  totalItems: number;
  pageSize = 10;
  pageIndex = 0;
  selectedIds: number[] = [];
  accepted: boolean;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(public spinnerService: SpinnerService,
              private coursesAndEpisodesService: CoursesAndEpisodesService,
              private dialog: MatDialog,
              private snackBar: MatSnackBar) {
  }

  ngOnInit(): void {
    this.coursesAndEpisodesService.reviewsUpdatedEmitter.subscribe(() => {
      this.getReviews();
    });

    // this.getReviews();
  }

  getReviews(): void {
    this.coursesAndEpisodesService.getAllReviews(this.accepted, this.pageIndex, this.pageSize).subscribe((res) => {
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
      // this.getReviews();
    });
  }

  refreshReviews(): void {
    this.selectedIds = [];
    this.coursesAndEpisodesService.onReviewsUpdate();
  }

  changePage(): void {
    this.selectedIds = [];
    this.pageIndex = this.paginator.pageIndex;
    this.pageSize = this.paginator.pageSize;
    this.coursesAndEpisodesService.onReviewsUpdate();
  }

  updateList(review, event): void {
    if (event.checked) {
      this.selectedIds.push(review.id);
    } else {
      this.selectedIds.splice(this.selectedIds.indexOf(review.id), 1);
    }
  }

  updateListAll(event): void {
    if (event.checked) {
      this.selectedIds = [];
      for (const review of this.reviews) {
        this.selectedIds.push(review.id);
      }
    } else {
      this.selectedIds = [];
    }
  }

  toggleMultipleReviews(): void {
    this.coursesAndEpisodesService.ToggleMultipleReviews(this.selectedIds).subscribe(() => {
      this.snackBar.open('عملیات با موفقیت انجام شد', null, {
        duration: 2500,
      });

      this.coursesAndEpisodesService.onReviewsUpdate();
      this.selectedIds = [];
    }, error => {
      this.snackBar.open('عملیات با خطا مواجه شد', null, {
        duration: 2500,
      });
    });
  }

  deleteMultipleReviews(): void {
    const result = confirm('آیا از انجام این عملیات اطمینان دارید؟');
    if (result) {
      this.coursesAndEpisodesService.DeleteMultipleReviews(this.selectedIds).subscribe(() => {
        this.snackBar.open('عملیات با موفقیت انجام شد', null, {
          duration: 2500,
        });

        this.coursesAndEpisodesService.onReviewsUpdate();
        this.selectedIds = [];
      }, error => {
        this.snackBar.open('عملیات با خطا مواجه شد', null, {
          duration: 2500,
        });
      });
    }
  }

  isSelected(review): boolean {
    return this.selectedIds.includes(review.id);
  }

  toggleSingleReview(id: number): void {
    this.coursesAndEpisodesService.ToggleMultipleReviews([id]).subscribe(() => {
      this.snackBar.open('عملیات با موفقیت انجام شد', null, {
        duration: 2500,
      });

      this.coursesAndEpisodesService.onReviewsUpdate();
      this.selectedIds.splice(this.selectedIds.indexOf(id), 1);
    }, error => {
      this.snackBar.open('عملیات با خطا مواجه شد', null, {
        duration: 2500,
      });
    });
  }

  deleteSingleReview(id: number): void {
    const result = confirm('آیا از انجام این عملیات اطمینان دارید؟');
    if (result) {
      this.coursesAndEpisodesService.DeleteMultipleReviews([id]).subscribe(() => {
        this.snackBar.open('عملیات با موفقیت انجام شد', null, {
          duration: 2500,
        });

        this.coursesAndEpisodesService.onReviewsUpdate();
        this.selectedIds.splice(this.selectedIds.indexOf(id), 1);
      }, error => {
        this.snackBar.open('عملیات با خطا مواجه شد', null, {
          duration: 2500,
        });
      });
    }
  }
}
