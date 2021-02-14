import {Component, OnInit} from '@angular/core';
import {CoursesAndEpisodesService} from '../../services/courses-and-episodes.service';
import {Course} from '../../models/course';
import {environment} from '../../../environments/environment';
import {MatDialog} from '@angular/material/dialog';
import {CreateOrEditComponent} from './create-or-edit/create-or-edit.component';

@Component({
    selector: 'app-admin-courses',
    templateUrl: './courses.component.html',
    styleUrls: ['./courses.component.scss']
})
export class CoursesComponent implements OnInit {
    courses: Course[];
    baseUrl = environment.apiUrl + 'files/';
    columnsToDisplay = ['id', 'name', 'price', 'photo', 'actions'];
    searchString = '';

    constructor(private coursesAndEpisodesService: CoursesAndEpisodesService, public dialog: MatDialog) {
    }

    ngOnInit(): void {
        this.getCourses();
    }

    getCourses(): void {
        this.coursesAndEpisodesService.getCourses(this.searchString).subscribe((res) => {
            this.courses = res;
        });
    }

    openAddOrEditDialog(course: Course): void {
        const dialogRef = this.dialog.open(CreateOrEditComponent, {
            width: '400px',
            data: {course}
        });

        dialogRef.afterClosed().subscribe(res => {
            this.getCourses();
        });
    }

    search(): void {
        this.getCourses();
    }

    clearSearch(): void {
        this.searchString = '';
        this.getCourses();
    }
}
