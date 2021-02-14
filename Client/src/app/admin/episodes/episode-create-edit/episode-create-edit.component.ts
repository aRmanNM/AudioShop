import {Component, ElementRef, Inject, OnInit, ViewChild} from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {CoursesAndEpisodesService} from '../../../services/courses-and-episodes.service';
import {MatSnackBar} from '@angular/material/snack-bar';
import {Episode} from '../../../models/episode';
import {environment} from '../../../../environments/environment';

interface DialogData {
  episode: Episode;
  courseId: number;
}

@Component({
  selector: 'app-create-or-edit',
  templateUrl: './episode-create-edit.component.html',
  styleUrls: ['./episode-create-edit.component.scss']
})
export class EpisodeCreateEditComponent implements OnInit {
  baseUrl = environment.apiUrl + 'files/';
  showProgressBar = false;
  audioFiles = [];
  @ViewChild('fileInput') fileInput: ElementRef;

  episodeForm = new FormGroup(
    {
      courseId: new FormControl(''),
      id: new FormControl(''),
      name: new FormControl('', [Validators.required]),
      description: new FormControl(''),
      price: new FormControl('', [Validators.required])
    }
  );

  constructor(public dialogRef: MatDialogRef<EpisodeCreateEditComponent>,
              @Inject(MAT_DIALOG_DATA) public data: DialogData,
              private coursesAndEpisodesService: CoursesAndEpisodesService,
              private snackBar: MatSnackBar) {
  }

  ngOnInit(): void {
    if (this.data.episode) {
      this.episodeForm.setValue({
        courseId: this.data.episode.courseId,
        id: this.data.episode.id,
        name: this.data.episode.name,
        description: this.data.episode.description,
        price: this.data.episode.price
      });

      this.getAudios();
    }
  }

  createOrEditEpisode(): void {
    if (this.data.episode) {
      this.coursesAndEpisodesService.updateEpisode(this.episodeForm.value).subscribe((res) => {
        this.snackBar.open('ویرایش اپیزود موفقیت آمیز بود', null, {
          duration: 2000,
        });
        this.closeDialog();
      });
    } else {
      this.episodeForm.patchValue({courseId: this.data.courseId});
      this.coursesAndEpisodesService.createEpisode(this.episodeForm.value).subscribe((res) => {
        this.snackBar.open('اپیزود جدید با موفقیت ایجاد شد', null, {
          duration: 2000,
        });
        this.closeDialog();
      });
    }
  }

  closeDialog(): void {
    this.dialogRef.close();
  }

  uploadAudio(): any {
    this.showProgressBar = true;
    const nativeElement = this.fileInput.nativeElement;
    this.coursesAndEpisodesService.uploadAudio(this.data.episode.id, nativeElement.files[0]).subscribe((res) => {
      this.showProgressBar = false;
      this.data.episode.audios.push(res);
      this.getAudios();
    });
  }

  getAudios(): void {
    this.audioFiles = [];
    for (let a of this.data.episode.audios) {
      this.audioFiles.push(`${this.baseUrl}${this.data.episode.courseId}/${a.fileName}`);
    }
  }

}
