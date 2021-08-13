import {Component, ElementRef, Inject, OnInit, ViewChild} from '@angular/core';
import {Ad} from '../../../models/ad';
import {environment} from '../../../../environments/environment';
import {COMMA, ENTER} from '@angular/cdk/keycodes';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {Observable} from 'rxjs';
import {MatAutocomplete, MatAutocompleteSelectedEvent} from '@angular/material/autocomplete';
import {Place} from '../../../models/place';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {MatSnackBar} from '@angular/material/snack-bar';
import {SpinnerService} from '../../../services/spinner.service';
import {AdService} from '../../../services/ad.service';
import {MatChipInputEvent} from '@angular/material/chips';
import {map, startWith} from 'rxjs/operators';

interface DialogData {
  ad: Ad;
}

@Component({
  selector: 'app-ads-create-edit',
  templateUrl: './ads-create-edit.component.html',
  styleUrls: ['./ads-create-edit.component.scss']
})
export class AdsCreateEditComponent implements OnInit {
  baseUrl = environment.apiUrl + 'Ads/';
  fileUrl;
  allPlaces: Place[] = [];
  places: Place[] = [];
  separatorKeysCodes: number[] = [ENTER, COMMA];
  placeCtrl = new FormControl();
  filteredPlaces: Observable<Place[]>;
  uploadSub;

  @ViewChild('fileInput') fileInput: ElementRef;
  @ViewChild('placeInput') placeInput: ElementRef<HTMLInputElement>;
  @ViewChild('auto') matAutocomplete: MatAutocomplete;

  constructor(public dialogRef: MatDialogRef<AdsCreateEditComponent>,
              @Inject(MAT_DIALOG_DATA) public data: DialogData,
              private adService: AdService,
              private snackBar: MatSnackBar,
              public spinnerService: SpinnerService) {
  }

  adForm = new FormGroup(
    {
      id: new FormControl(0),
      title: new FormControl('', [Validators.required]),
      description: new FormControl(''),
      link: new FormControl(''),
      adType: new FormControl('', [Validators.required]),
      places: new FormControl(''),
      isEnabled: new FormControl('', [Validators.required])
    }
  );

  ngOnInit(): void {

    if (this.data.ad) {
      this.adForm.setValue({
        id: this.data.ad.id,
        title: this.data.ad.title,
        description: this.data.ad.description,
        link: this.data.ad.link,
        adType: this.data.ad.adType.toString(),
        places: this.data.ad.places,
        isEnabled: this.data.ad.isEnabled
      });

      this.getPlaces(this.data.ad.adType);
      this.places = JSON.parse(JSON.stringify(this.data.ad.places));
      this.getFile();
    }
  }

  getFile(): void {
    this.fileUrl = this.baseUrl + this.data.ad.id + '/' + this.data.ad.file?.fileName;
  }

  add(event: MatChipInputEvent): void {
    this.places.push(this.allPlaces.find(c => c.titleEn === event.value));

    // // Clear the input value
    // event.categoryInput!.clear();

    this.placeCtrl.setValue(null);
  }

  remove(place: Place): void {
    const index = this.places.indexOf(place);

    if (index >= 0) {
      this.places.splice(index, 1);
    }
  }

  selected(event: MatAutocompleteSelectedEvent): void {
    this.places.push(event.option.value);
    this.placeInput.nativeElement.value = '';
    this.placeCtrl.setValue(null);
  }

  private _filter(value: Place): Place[] {

    return this.allPlaces.filter(c => this.allPlaces.indexOf(c) === 0);
  }

  getPlaces(adType: number): void {
    this.places = [];
    this.adService.getPlaces().subscribe((res) => {
      this.allPlaces = res.filter(p => p.adType == adType);
      this.filteredPlaces = this.placeCtrl.valueChanges.pipe(
        startWith(null),
        map((place: Place | null) => place ? this._filter(place) : this.allPlaces.slice()));
    });
  }

  createOrEditAd(): void {
    if (this.data.ad) {
      const ad: Ad = this.adForm.value;
      ad.places = this.places;
      console.log('ad', ad);
      this.adService.editAd(ad).subscribe((res) => {
        this.snackBar.open('ویرایش تبلیغ موفقیت آمیز بود', null, {
          duration: 2000,
        });
        this.adService.onUpdate();
        this.closeDialog();
      }, error => {
        this.snackBar.open('ویرایش با شکست مواجه شد', null, {
          duration: 2000,
        });
      });
    } else {
      const ad: Ad = this.adForm.value;
      ad.places = this.places;
      console.log('ad', ad);
      this.adService.createAd(ad).subscribe((res) => {
        this.snackBar.open('تبلیغ جدید با موفقیت ایجاد شد', null, {
          duration: 2000,
        });
        this.adService.onUpdate();
        this.closeDialog();
      }, error => {
        this.snackBar.open('ساخت تبلیغ جدید با شکست مواجه شد', null, {
          duration: 2000,
        });
      });
    }
  }

  closeDialog(): void {
    this.dialogRef.close();
  }

  uploadFile(): any {
    this.dialogRef.disableClose = true;
    const nativeElement = this.fileInput.nativeElement;
    this.uploadSub = this.adService.uploadFile(this.data.ad.id, nativeElement.files[0]).subscribe((res) => {
      this.dialogRef.disableClose = false;
      this.data.ad.file = res;
      this.getFile();
      this.adService.onUpdate();
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
}
