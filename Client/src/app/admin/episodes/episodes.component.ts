import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {Episode} from '../../models/episode';
import {CoursesAndEpisodesService} from '../../services/courses-and-episodes.service';
import {MatDialog} from '@angular/material/dialog';
import {EpisodeCreateEditComponent} from './episode-create-edit/episode-create-edit.component';
import {CdkDragDrop, moveItemInArray} from '@angular/cdk/drag-drop';
import * as cloneDeep from 'lodash/cloneDeep';
import {SpinnerService} from '../../services/spinner.service';

@Component({
  selector: 'app-episodes',
  templateUrl: './episodes.component.html',
  styleUrls: ['./episodes.component.scss']
})
export class EpisodesComponent implements OnInit {
  episodes: Episode[];
  courseId: number;
  addEnabled = false;
  columnsToDisplay = ['handle', 'name', 'price', 'duration', 'actions'];
  dialogActive = false;

  constructor(private coursesAndEpisodesService: CoursesAndEpisodesService,
              public dialog: MatDialog,
              public spinnerService: SpinnerService) {
  }

  ngOnInit(): void {
  }

  getEpisodes(): void {
    this.addEnabled = false;
    this.episodes = null;
    this.coursesAndEpisodesService.getCourseEpisodes(this.courseId).subscribe((res) => {
      this.episodes = res;
      this.addEnabled = true;

      this.updateSortIndex();
      this.updateCourseEpisodes();
    }, error => {
    });
  }

  addOrEditDialog(episode: Episode): void {
    if (episode) {
      this.coursesAndEpisodesService.getEpisode(episode.id).subscribe((res) => {
        this.openDialog(res);
      });
    } else {
      this.openDialog(null);
    }

  }

  openDialog(episode: Episode): void {
    const dialogRef = this.dialog.open(EpisodeCreateEditComponent, {
      width: '400px',
      data: {
        episode,
        courseId: this.courseId
      }
    });

    dialogRef.afterClosed().subscribe(res => {
      this.getEpisodes();
    });
  }

  drop(event: CdkDragDrop<Episode[]>): void {
    // console.log(event.previousIndex, event.currentIndex);
    moveItemInArray(this.episodes, event.previousIndex, event.currentIndex);
    this.episodes = cloneDeep(this.episodes);
    this.updateSortIndex();
    this.updateCourseEpisodes();
  }

  updateCourseEpisodes(): void {
    this.coursesAndEpisodesService.updateCourseEpisodes(this.courseId, this.episodes).subscribe(() => {
      // console.log('updated!');
    });
  }

  updateSortIndex(): void {
    for (let i = 0; i < this.episodes.length; i++) {
      this.episodes[i].sort = i;
    }
  }

}
