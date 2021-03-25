import {Component, OnInit} from '@angular/core';
import {environment} from '../../../environments/environment';
import {SliderItem} from '../../models/slider-item';
import {MatDialog} from '@angular/material/dialog';
import {SpinnerService} from '../../services/spinner.service';
import {SliderService} from '../../services/slider.service';
import {SliderEditCreateComponent} from './slider-edit-create/slider-edit-create.component';
import {MatSnackBar} from '@angular/material/snack-bar';

@Component({
  selector: 'app-slider',
  templateUrl: './slider.component.html',
  styleUrls: ['./slider.component.scss']
})
export class SliderComponent implements OnInit {
  sliderItems: SliderItem[];
  baseUrl = environment.apiUrl + 'Slider/';
  columnsToDisplay = ['id', 'title', 'description', 'courseId', 'isActive', 'photo', 'actions'];
  dialogActive = false;

  constructor(private sliderService: SliderService,
              public dialog: MatDialog,
              public spinnerService: SpinnerService,
              private snackBar: MatSnackBar) {
  }

  ngOnInit(): void {
    this.sliderService.sliderUpdateEmitter.subscribe(() => {
      this.getSliderItems();
    });

    this.sliderService.onSliderUpdate();
  }

  getSliderItems(): void {
    this.sliderService.getSliderItems().subscribe((res) => {
      this.sliderItems = res;
    });
  }


  openAddOrEditDialog(sliderItem: SliderItem): void {
    this.dialogActive = true;
    const dialogRef = this.dialog.open(SliderEditCreateComponent, {
      width: '400px',
      data: {sliderItem}
    });

    dialogRef.afterClosed().subscribe(res => {
      this.dialogActive = false;
    });
  }

  deleteSliderItem(sliderItemId: number): void {
    const result = confirm('آیا از انجام این عملیات اطمینان دارید؟');
    if (result) {
      this.sliderService.deleteSliderItem(sliderItemId).subscribe(() => {
        this.snackBar.open('اسلایدر حذف شد', null, {
          duration: 2000,
        });

        this.sliderService.onSliderUpdate();
      }, error => {
        this.snackBar.open('عملیات موفق نبود', null, {
          duration: 2000,
        });
      });
    }
  }

}
