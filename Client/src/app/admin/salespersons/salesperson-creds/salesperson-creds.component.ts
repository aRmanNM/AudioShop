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
  templateUrl: './salesperson-creds.component.html',
  styleUrls: ['./salesperson-creds.component.scss']
})
export class SalespersonCredsComponent implements OnInit {
  baseUrl = environment.apiUrl + 'Credentials/';
  salesperson: Salesperson;

  constructor(public dialogRef: MatDialogRef<SalespersonCredsComponent>,
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

  updateSalesperson(): void {
    this.adminService.updateSalespersonCredential(this.data.salespersonId,
      this.salesperson.credentialAccepted,
      this.salesperson.salespersonCredential.message).subscribe((res) => {

      this.salesperson.credentialAccepted = !this.salesperson.credentialAccepted;
      this.snackBar.open('اطلاعات هویتی ویرایش شد', null, {
        duration: 3000,
      });

      this.adminService.onSalespersonsUpdate();
      this.closeDialog();
    });
  }
}
