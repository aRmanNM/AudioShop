import {Component, OnInit} from '@angular/core';
import {LandingsService} from '../../services/landings.service';
import {Landing} from '../../models/landing';
import {MatDialog} from '@angular/material/dialog';
import {SpinnerService} from '../../services/spinner.service';
import {LandingsCreateOrEditComponent} from './landings-create-or-edit/landings-create-or-edit.component';
import {environment} from '../../../environments/environment';

@Component({
  selector: 'app-landings',
  templateUrl: './landings.component.html',
  styleUrls: ['./landings.component.scss']
})
export class LandingsComponent implements OnInit {
  landings: Landing[];
  dialogActive = false;
  columnsToDisplay = ['description', 'title', 'visits', 'phoneNumbersCount', 'actions'];
  baseUrl = environment.apiUrl;

  constructor(private landingService: LandingsService,
              public dialog: MatDialog,
              public spinnerService: SpinnerService) {
  }

  ngOnInit(): void {
    this.landingService.updatedEmmiter.subscribe(() => {
      this.getLandings();
    });

    this.landingService.onUpdate();
  }

  getLandings(): void {
    this.landingService.getLandings().subscribe((res) => {
      this.landings = res;
      // console.log(res);
    });
  }

  openAddOrEditDialog(landing: Landing): void {
    this.dialogActive = true;
    const dialogRef = this.dialog.open(LandingsCreateOrEditComponent, {
      width: '500px',
      data: {landing}
    });

    dialogRef.afterClosed().subscribe(res => {
      this.dialogActive = false;
    });
  }

  refreshLandings(): void {
    this.landingService.onUpdate();
  }

  exportPhoneNumbers(landingId: number): void {
    // console.log(landingId);
    this.landingService.exportLandingPhoneNumbers(landingId).subscribe((res) => {
      this.downloadFile(res, 'application/ms-excel');
    });
  }

  downloadFile(data: any, type: string): void {
    let blob = new Blob([data], {type: type});
    let url = window.URL.createObjectURL(blob);
    let pwa = window.open(url);
    if (!pwa || pwa.closed || typeof pwa.closed === 'undefined') {
      alert('Please disable your Pop-up blocker and try again.');
    }
  }
}
