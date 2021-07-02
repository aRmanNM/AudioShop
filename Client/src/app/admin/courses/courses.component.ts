import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {CoursesAndEpisodesService} from '../../services/courses-and-episodes.service';
import {Course} from '../../models/course';
import {environment} from '../../../environments/environment';
import {MatDialog} from '@angular/material/dialog';
import {CreateOrEditComponent} from './create-or-edit/create-or-edit.component';
import {SpinnerService} from '../../services/spinner.service';
import {MatPaginator} from '@angular/material/paginator';

@Component({
  selector: 'app-admin-courses',
  templateUrl: './courses.component.html',
  styleUrls: ['./courses.component.scss']
})
export class CoursesComponent implements OnInit {
  courses: Course[];
  totalItems: number;
  pageSize = 10;
  pageIndex = 0;
  baseUrl = environment.apiUrl + 'Files/';
  columnsToDisplay = ['id', 'name', 'visits', 'instructor', 'price', 'watingTime', 'isActive', 'photo', 'actions'];
  searchString = '';
  dialogActive = false;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(private coursesAndEpisodesService: CoursesAndEpisodesService,
              public dialog: MatDialog,
              public spinnerService: SpinnerService) {
  }

  ngOnInit(): void {
    this.coursesAndEpisodesService.updatedEmmiter.subscribe(() => {
      this.getCourses();
    });

    this.coursesAndEpisodesService.onUpdate();
  }

  getCourses(): void {
    this.coursesAndEpisodesService.getCourses(this.searchString, this.pageIndex, this.pageSize).subscribe((res) => {

      console.log(res);

      this.courses = res.items;
      this.totalItems = res.totalItems;
    });
  }


  openAddOrEditDialog(course: Course): void {
    this.dialogActive = true;
    const dialogRef = this.dialog.open(CreateOrEditComponent, {
      width: '400px',
      data: {course}
    });

    dialogRef.afterClosed().subscribe(res => {
      this.dialogActive = false;
    });
  }

  search(): void {
    this.coursesAndEpisodesService.onUpdate();
  }

  clearSearch(): void {
    this.searchString = '';
    this.coursesAndEpisodesService.onUpdate();
  }

  changePage(): void {
    this.pageIndex = this.paginator.pageIndex;
    this.pageSize = this.paginator.pageSize;
    this.coursesAndEpisodesService.onUpdate();
  }
}
