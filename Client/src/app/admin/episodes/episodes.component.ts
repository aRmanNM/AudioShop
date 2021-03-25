import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {Episode} from '../../models/episode';
import {CoursesAndEpisodesService} from '../../services/courses-and-episodes.service';
import {MatDialog} from '@angular/material/dialog';
import {EpisodeCreateEditComponent} from './episode-create-edit/episode-create-edit.component';
import {CdkDragDrop, moveItemInArray} from '@angular/cdk/drag-drop';
import * as cloneDeep from 'lodash/cloneDeep';
import {SpinnerService} from '../../services/spinner.service';
import {ActivatedRoute, Params, Router} from '@angular/router';
import {MatSnackBar} from '@angular/material/snack-bar';

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
              public spinnerService: SpinnerService,
              private route: ActivatedRoute,
              private snackBar: MatSnackBar) {
  }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      if (params.courseId) {
        this.courseId = params.courseId;
        this.getEpisodes();
      }
    });
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
      this.snackBar.open('چنین دوره ای وجود ندارد', null, {
        duration: 3000,
      });
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

  deleteEpisode(episodeId: number): void {
    const result = confirm('آیا از انجام این عملیات اطمینان دارید؟');
    if (result) {
      this.coursesAndEpisodesService.deleteEpisode(episodeId).subscribe(() => {
        this.snackBar.open('اپیزود با موفقیت حذف شد', null, {
          duration: 2000,
        });
        this.getEpisodes();
      }, e => {
        if (e.error === 'already bought') {
          this.snackBar.open('این اپیزود قبلا خریداری شده و امکان حذف آن وجود ندارد', null, {
            duration: 3000,
          });
        }
      });
    }
  }

}
