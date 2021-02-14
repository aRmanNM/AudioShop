import {Component, Inject, OnInit} from '@angular/core';
import {Course} from '../../../models/course';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {CoursesAndEpisodesService} from '../../../services/courses-and-episodes.service';
import {Episode} from '../../../models/episode';

interface DialogData {
  course: Course;
}

@Component({
  selector: 'app-detail',
  templateUrl: './detail.component.html',
  styleUrls: ['./detail.component.scss']
})
export class DetailComponent implements OnInit {
  episodes: Episode[];
  columnsToDisplay = ['id', 'name', 'price'];

  constructor(public dialogRef: MatDialogRef<DetailComponent>,
              @Inject(MAT_DIALOG_DATA) public data: DialogData,
              private coursesAndEpisodesService: CoursesAndEpisodesService) {
  }

  ngOnInit(): void {
    this.getCourseEpisodes();
  }

  getCourseEpisodes(): void {
    this.coursesAndEpisodesService.getCourseEpisodes(this.data.course.id).subscribe((res) => {
      this.episodes = res;
      console.log(res);
    });
  }

}
