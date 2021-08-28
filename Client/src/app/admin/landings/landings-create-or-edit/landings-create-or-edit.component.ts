import {Component, ElementRef, Inject, OnInit, ViewChild} from '@angular/core';
import {Landing} from '../../../models/landing';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {MatSnackBar} from '@angular/material/snack-bar';
import {SpinnerService} from '../../../services/spinner.service';
import {LandingsService} from '../../../services/landings.service';
import {FormControl, FormGroup} from '@angular/forms';
import {environment} from '../../../../environments/environment';
import {Color} from 'ngx-color';

interface DialogData {
  landing: Landing;
}

@Component({
  selector: 'app-create-or-edit',
  templateUrl: './landings-create-or-edit.component.html',
  styleUrls: ['./landings-create-or-edit.component.scss']
})
export class LandingsCreateOrEditComponent implements OnInit {
  baseUrl = environment.apiUrl + 'Landings/';
  logoImgUrl;
  mediaUrl;
  backgroundImgUrl;
  uploadSub;
  // themeColor: Color;

  @ViewChild('logoInput') logoInput: ElementRef;
  @ViewChild('mediaInput') mediaInput: ElementRef;
  @ViewChild('backgroundInput') backgroundInput: ElementRef;

  constructor(public dialogRef: MatDialogRef<LandingsCreateOrEditComponent>,
              @Inject(MAT_DIALOG_DATA) public data: DialogData,
              private landingService: LandingsService,
              private snackBar: MatSnackBar,
              public spinnerService: SpinnerService) {
  }

  landingForm = new FormGroup(
    {
      id: new FormControl(0),
      description: new FormControl(''),
      title: new FormControl(''),
      titleEnabled: new FormControl(true),
      text1: new FormControl(''),
      text1Enabled: new FormControl(true),
      button: new FormControl(''),
      buttonLink: new FormControl(''),
      buttonEnabled: new FormControl(true),
      text2: new FormControl(''),
      text2Enabled: new FormControl(true),
      gift: new FormControl(''),
      giftEnabled: new FormControl(true),
      phoneBoxEnabled: new FormControl(true),
      buttonsColor: new FormControl(''),
      buttonClickCount: new FormControl(0),
      logoEnabled: new FormControl(false),
      mediaEnabled: new FormControl(false),
    }
  );

  ngOnInit(): void {
    if (this.data.landing) {
      this.landingForm.setValue({
        id: this.data.landing.id,
        description: this.data.landing.description,
        title: this.data.landing.title,
        titleEnabled: this.data.landing.titleEnabled,
        text1: this.data.landing.text1,
        text1Enabled: this.data.landing.text1Enabled,
        button: this.data.landing.button,
        buttonLink: this.data.landing.buttonLink,
        buttonEnabled: this.data.landing.buttonEnabled,
        text2: this.data.landing.text2,
        text2Enabled: this.data.landing.text2Enabled,
        gift: this.data.landing.gift,
        giftEnabled: this.data.landing.giftEnabled,
        phoneBoxEnabled: this.data.landing.phoneBoxEnabled,
        buttonsColor: this.data.landing.buttonsColor,
        buttonClickCount: this.data.landing.buttonClickCount,
        logoEnabled: this.data.landing.logoEnabled,
        mediaEnabled: this.data.landing.mediaEnabled,
      });

      this.getImages();
    }
  }


  getImages(): void {
    this.logoImgUrl = this.baseUrl + this.data.landing.id + '/' + this.data.landing.logo?.fileName;
    this.mediaUrl = this.baseUrl + this.data.landing.id + '/' + this.data.landing.media?.fileName;
    this.backgroundImgUrl = this.baseUrl + this.data.landing.id + '/' + this.data.landing.background?.fileName;
  }

  createOrEditLanding(): void {
    if (this.data.landing) {
      const landing: Landing = this.landingForm.value;
      // landing.buttonsColor = this.themeColor.hex;
      this.landingService.updateLanding(landing).subscribe((res) => {
        this.snackBar.open('ویرایش صفحه لندینگ موفقیت آمیز بود', null, {
          duration: 2000,
        });
        this.landingService.onUpdate();
        this.closeDialog();
      });
    } else {
      const landing: Landing = this.landingForm.value;
      // landing.buttonsColor = this.themeColor.hex;
      this.landingService.createLanding(landing).subscribe((res) => {
        this.snackBar.open('لندینگ جدید با موفقیت ایجاد شد', null, {
          duration: 2000,
        });
        this.landingService.onUpdate();
        this.closeDialog();
      });
    }
  }

  closeDialog(): void {
    this.dialogRef.close();
    // this.coursesAndEpisodesService.onReviewsUpdate();
  }

  uploadFile(field: string): any {
    // console.log('upload function called');
    let nativeElement;
    switch (field) {
      case 'logo': {
        nativeElement = this.logoInput.nativeElement;
        break;
      }
      case 'media': {
        nativeElement = this.mediaInput.nativeElement;
        break;
      }
      case 'background': {
        nativeElement = this.backgroundInput.nativeElement;
        break;
      }
      default: {
        break;
      }
    }
    this.uploadSub = this.landingService.uploadFile(this.data.landing.id, field, nativeElement.files[0]).subscribe((res) => {
      // console.log('upload subscribe answered!');
      this.data.landing[field] = res;
      this.getImages();
      this.landingService.onUpdate();
    }, ((e) => {
      this.snackBar.open(e.error, null, {
        duration: 2000,
      });
    }));
  }

  cancelUpload(): void {
    this.uploadSub.unsubscribe();
    this.snackBar.open('اپلود متوقف شد', null, {
      duration: 2000,
    });
  }

  // changeColor(event): void {
  //   this.themeColor = event.color.hex;
  // }
}
