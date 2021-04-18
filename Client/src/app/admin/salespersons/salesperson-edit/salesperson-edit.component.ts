import {Component, Inject, OnInit} from '@angular/core';
import {Salesperson} from '../../../models/salesperson';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {AdminService} from '../../../services/admin.service';
import {MatSnackBar} from '@angular/material/snack-bar';
import {SpinnerService} from '../../../services/spinner.service';
import {FormControl, FormGroup, Validators} from '@angular/forms';

interface DialogData {
  salesperson: Salesperson;
}

@Component({
  selector: 'app-salesperson-edit',
  templateUrl: './salesperson-edit.component.html',
  styleUrls: ['./salesperson-edit.component.scss']
})
export class SalespersonEditComponent implements OnInit {

  spForm = new FormGroup(
    {
      id: new FormControl(''),
      discountPercentageOfSalesperson: new FormControl('', [Validators.required, Validators.min(1), Validators.max(100)]),
      salePercentageOfSalesperson: new FormControl('', [Validators.required, Validators.min(1), Validators.max(100)])
    }
  );

  constructor(public dialogRef: MatDialogRef<SalespersonEditComponent>,
              @Inject(MAT_DIALOG_DATA) public data: DialogData,
              private adminService: AdminService,
              private snackBar: MatSnackBar,
              public spinnerService: SpinnerService) {
  }

  ngOnInit(): void {
    if (this.data.salesperson) {
      this.spForm.setValue({
        id: this.data.salesperson.id,
        discountPercentageOfSalesperson: this.data.salesperson.discountPercentageOfSalesperson,
        salePercentageOfSalesperson: this.data.salesperson.salePercentageOfSalesperson
      });
    }
  }

  closeDialog(): void {
    this.dialogRef.close();
  }

  updateSalesperson(): void {
    this.adminService.updateSalesperson(this.spForm.value.id, this.spForm.value).subscribe(() => {
      this.snackBar.open('اطلاعات فروشنده ویرایش شد', null, {
        duration: 3000,
      });

      this.adminService.onSalespersonsUpdate();
      this.closeDialog();
    });
  }
}
