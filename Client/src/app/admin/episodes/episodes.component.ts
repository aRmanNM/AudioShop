import {Component, OnInit} from '@angular/core';
import {Episode} from '../../models/episode';
import {CoursesAndEpisodesService} from '../../services/courses-and-episodes.service';
import {MatDialog} from '@angular/material/dialog';
import {EpisodeCreateEditComponent} from './episode-create-edit/episode-create-edit.component';
import {Observable} from 'rxjs';

@Component({
  selector: 'app-episodes',
  templateUrl: './episodes.component.html',
  styleUrls: ['./episodes.component.scss']
})
export class EpisodesComponent implements OnInit {
  episodes: Episode[];
  courseId: number;
  addEnabled = false;
  columnsToDisplay = ['id', 'name', 'price', 'order', 'actions'];

  constructor(private coursesAndEpisodesService: CoursesAndEpisodesService, public dialog: MatDialog) {
  }

  ngOnInit(): void {
  }

  getEpisodes(): void {
    this.addEnabled = false;
    this.episodes = null;
    this.coursesAndEpisodesService.getCourseEpisodes(this.courseId).subscribe((res) => {
      this.episodes = res;
      this.addEnabled = true;
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

}
