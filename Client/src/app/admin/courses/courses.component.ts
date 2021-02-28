import {Component, Input, OnInit} from '@angular/core';
import {CoursesAndEpisodesService} from '../../services/courses-and-episodes.service';
import {Course} from '../../models/course';
import {environment} from '../../../environments/environment';
import {MatDialog} from '@angular/material/dialog';
import {CreateOrEditComponent} from './create-or-edit/create-or-edit.component';
import {SpinnerService} from '../../services/spinner.service';

@Component({
  selector: 'app-admin-courses',
  templateUrl: './courses.component.html',
  styleUrls: ['./courses.component.scss']
})
export class CoursesComponent implements OnInit {
  courses: Course[];
  baseUrl = environment.apiUrl + 'Files/';
  columnsToDisplay = ['id', 'name', 'price', 'watingTime', 'isActive', 'photo', 'actions'];
  searchString = '';
  dialogActive = false;

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
    this.coursesAndEpisodesService.getCourses(this.searchString).subscribe((res) => {
      this.courses = res;
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
}
