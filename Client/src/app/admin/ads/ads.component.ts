import {Component, OnInit} from '@angular/core';
import {AdService} from '../../services/ad.service';
import {MatDialog} from '@angular/material/dialog';
import {SpinnerService} from '../../services/spinner.service';
import {Ad} from '../../models/ad';
import {PlacesComponent} from './places/places.component';
import {Course} from '../../models/course';
import {CreateOrEditComponent} from '../courses/create-or-edit/create-or-edit.component';
import {AdsCreateEditComponent} from './ads-create-edit/ads-create-edit.component';

@Component({
  selector: 'app-ads',
  templateUrl: './ads.component.html',
  styleUrls: ['./ads.component.scss']
})
export class AdsComponent implements OnInit {
  dialogActive = false;
  ads: Ad[];
  columnsToDisplay = ['title', 'adType', 'adPlaces', 'isEnabled', 'actions'];

  constructor(private adService: AdService,
              public dialog: MatDialog,
              public spinnerService: SpinnerService) {
  }

  ngOnInit(): void {
    this.adService.updatedEmmiter.subscribe(() => {
      this.getAds();
    });

    this.adService.onUpdate();
  }

  getAds(): void {
    this.adService.getAds().subscribe((res) => {
      this.ads = res;
    });
  }

  openPlacesDialog(): void {
    this.dialogActive = true;
    const dialogRef = this.dialog.open(PlacesComponent, {
      width: '600px',
      data: 'nothing yet'
    });
  }

  openAddOrEditDialog(ad: Ad): void {
    this.dialogActive = true;
    const dialogRef = this.dialog.open(AdsCreateEditComponent, {
      width: '600px',
      data: {ad}
    });

    dialogRef.afterClosed().subscribe(res => {
      this.dialogActive = false;
    });
  }

}
