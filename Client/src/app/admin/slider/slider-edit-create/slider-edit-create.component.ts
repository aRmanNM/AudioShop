import {Component, ElementRef, Inject, OnInit, ViewChild} from '@angular/core';
import {environment} from '../../../../environments/environment';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {MatSnackBar} from '@angular/material/snack-bar';
import {SpinnerService} from '../../../services/spinner.service';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {SliderItem} from '../../../models/slider-item';
import {SliderService} from '../../../services/slider.service';

class DialogData {
  sliderItem: SliderItem;
}

@Component({
  selector: 'app-slider-edit-create',
  templateUrl: './slider-edit-create.component.html',
  styleUrls: ['./slider-edit-create.component.scss']
})
export class SliderEditCreateComponent implements OnInit {
  baseUrl = environment.apiUrl + 'Slider/';
  imgUrl;
  @ViewChild('fileInput') fileInput: ElementRef;

  constructor(public dialogRef: MatDialogRef<SliderEditCreateComponent>,
              @Inject(MAT_DIALOG_DATA) public data: DialogData,
              private sliderService: SliderService,
              private snackBar: MatSnackBar,
              public spinnerService: SpinnerService) {
  }

  sliderItemForm = new FormGroup(
    {
      id: new FormControl(''),
      title: new FormControl('', [Validators.required]),
      description: new FormControl(''),
      courseId: new FormControl(''),
      link: new FormControl(''),
      isActive: new FormControl('', [Validators.required]),
    }
  );

  ngOnInit(): void {
    if (this.data.sliderItem) {
      this.sliderItemForm.setValue({
        id: this.data.sliderItem.id,
        title: this.data.sliderItem.title,
        description: this.data.sliderItem.description,
        courseId: this.data.sliderItem.courseId,
        isActive: this.data.sliderItem.isActive,
        link: this.data.sliderItem.link
      });

      this.getImage();
    }
  }

  createOrEditSliderItem(): void {
    if (this.data.sliderItem) {
      this.sliderService.updateSliderItem(this.sliderItemForm.value).subscribe((res) => {
        this.snackBar.open('ویرایش اسلایدر موفقیت آمیز بود', null, {
          duration: 2000,
        });
        this.sliderService.onSliderUpdate();
        this.closeDialog();
      });
    } else {
      this.sliderService.createSliderItem(this.sliderItemForm.value).subscribe((res) => {
        this.snackBar.open('اسلایدر جدید با موفقیت ایجاد شد', null, {
          duration: 2000,
        });
        this.sliderService.onSliderUpdate();
        this.closeDialog();
      });
    }
  }

  closeDialog(): void {
    this.dialogRef.close();
  }

  uploadPhoto(): any {
    const nativeElement = this.fileInput.nativeElement;
    this.sliderService.uploadPhoto(this.data.sliderItem.id, nativeElement.files[0]).subscribe((res) => {
      this.data.sliderItem.photoFileName = res.fileName;
      this.getImage();
      this.sliderService.onSliderUpdate();
    }, ((e) => {
      this.snackBar.open(e.error, null, {
        duration: 2000,
      });
    }));
  }

  getImage(): void {
    this.imgUrl = this.baseUrl + this.data.sliderItem.photoFileName;
  }
}
