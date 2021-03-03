import {Component, Inject, OnInit} from '@angular/core';
import {environment} from '../../../../environments/environment';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {MatSnackBar} from '@angular/material/snack-bar';
import {SpinnerService} from '../../../services/spinner.service';
import {AdminService} from '../../../services/admin.service';
import {Salesperson} from '../../../models/salesperson';

interface DialogData {
  salespersonId: string;
}

@Component({
  selector: 'app-salesperson-edit',
  templateUrl: './salesperson-edit.component.html',
  styleUrls: ['./salesperson-edit.component.scss']
})
export class SalespersonEditComponent implements OnInit {
  baseUrl = environment.apiUrl + 'Credentials/';
  salesperson: Salesperson;

  constructor(public dialogRef: MatDialogRef<SalespersonEditComponent>,
              @Inject(MAT_DIALOG_DATA) public data: DialogData,
              private adminService: AdminService,
              private snackBar: MatSnackBar,
              public spinnerService: SpinnerService) {
  }

  ngOnInit(): void {
    this.getSalesperson();
  }

  closeDialog(): void {
    this.dialogRef.close();
  }

  getSalesperson(): void {
    this.adminService.getSalesperson(this.data.salespersonId).subscribe((res) => {
      this.salesperson = res;
    });
  }

  toggleCredential(): void {
    this.adminService.updateSalesperson(this.data.salespersonId).subscribe((res) => {
      this.salesperson.credentialAccepted = !this.salesperson.credentialAccepted;
      this.snackBar.open('اطلاعات هویتی تایید شد', null, {
        duration: 2000,
      });

      this.adminService.onSalespersonsUpdate();
      this.closeDialog();
    });
  }
}
