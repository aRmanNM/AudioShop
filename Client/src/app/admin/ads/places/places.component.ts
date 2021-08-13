import {Component, OnInit} from '@angular/core';
import {SpinnerService} from '../../../services/spinner.service';
import {MatSnackBar} from '@angular/material/snack-bar';
import {AdService} from '../../../services/ad.service';
import {Place} from '../../../models/place';

@Component({
  selector: 'app-places',
  templateUrl: './places.component.html',
  styleUrls: ['./places.component.scss']
})
export class PlacesComponent implements OnInit {
  fullScreens: Place[];
  natives: Place[];
  banners: Place[];

  constructor(private adService: AdService,
              public spinnerService: SpinnerService,
              private snackBar: MatSnackBar) {
  }

  ngOnInit(): void {
    this.getPlaces();
  }

  getPlaces(): void {
    this.adService.getPlaces().subscribe((res) => {
      this.fullScreens = res.filter(p => p.adType === 0);
      this.natives = res.filter(p => p.adType === 1);
      this.banners = res.filter(p => p.adType === 2);
    });
  }

  editPlace(place: Place): void {
    place.isEnabled = !place.isEnabled;
    this.adService.editPlace(place).subscribe((res) => {
      this.snackBar.open('تنظیمات با موفقیت بروزرسانی شد', null, {
        duration: 2000,
      });
    });
  }

}
