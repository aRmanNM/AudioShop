import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {SpinnerService} from '../../services/spinner.service';
import {SalespersonService} from '../../services/salesperson.service';
import {environment} from '../../../environments/environment';
import {Credential} from '../../models/credential';
import {MatSnackBar} from '@angular/material/snack-bar';

@Component({
  selector: 'app-credentials',
  templateUrl: './credentials.component.html',
  styleUrls: ['./credentials.component.scss']
})
export class CredentialsComponent implements OnInit {
  baseUrl = environment.apiUrl + 'Credentials/';
  @ViewChild('fileInputId') fileInputId: ElementRef;
  @ViewChild('fileInputBank') fileInputBank: ElementRef;
  bankCardPhotoUrl;
  idCardPhotoUrl;
  credential: Credential;
  credentialAccepted = false;

  credentialForm = new FormGroup({
    id: new FormControl('0'),
    userId: new FormControl(''),
    userName: new FormControl(''),
    idCardNumber: new FormControl('', [Validators.required]),
    bankAccountNumber: new FormControl('', [Validators.required]),
    bankAccountShebaNumber: new FormControl('', [Validators.required]),
    bankCardNumber: new FormControl('', [Validators.required]),
    bankCardName: new FormControl('', [Validators.required]),
    bankName: new FormControl('', [Validators.required]),
    phone: new FormControl('', [Validators.required])
  });

  constructor(public spinnerService: SpinnerService,
              private salespersonService: SalespersonService,
              private snackBar: MatSnackBar) {
  }

  ngOnInit(): void {
    this.salespersonService.credentialUpdated.subscribe((res) => {
      this.getCredential();
      this.checkSalespersonCredetialAccepted();
    });

    this.salespersonService.updateCredential();
  }

  getCredential(): void {
    this.salespersonService.getCredential().subscribe((res) => {
      if (res) {
        this.credential = res;
        this.getImage();
        this.credentialForm.setValue({
          id: res.id,
          userId: res.userId,
          userName: res.userName,
          idCardNumber: res.idCardNumber,
          bankAccountNumber: res.bankAccountNumber,
          bankAccountShebaNumber: res.bankAccountShebaNumber,
          bankCardNumber: res.bankCardNumber,
          bankCardName: res.bankCardName,
          bankName: res.bankName,
          phone: res.phone
        });
      }
    });
  }

  createOrEditCredential(): void {
    this.salespersonService.updateOrCreateCredential(this.credentialForm.value).subscribe((res) => {
      this.snackBar.open('اطلاعات ثبت شد', null, {
        duration: 2000,
      });

      this.salespersonService.updateCredential();
    });
  }

  uploadPhoto(usedAs: string): any {
    let nativeElement;
    if (usedAs === 'idcard') {
      nativeElement = this.fileInputId.nativeElement;
    } else if (usedAs === 'bankcard') {
      nativeElement = this.fileInputBank.nativeElement;
    }

    this.salespersonService.uploadPhoto(this.credentialForm.value.id, nativeElement.files[0], usedAs).subscribe((res) => {
      if (usedAs === 'idcard') {
        this.credential.idCardPhoto = res;
        this.getImage();
        this.salespersonService.updateCredential();
      } else if (usedAs === 'bankcard') {
        this.credential.bankCardPhoto = res;
        this.getImage();
        this.salespersonService.updateCredential();
      }
    }, ((e) => {
      this.snackBar.open(e.error, null, {
        duration: 2000,
      });
    }));
  }

  getImage(): void {
    if (this.credential.bankCardPhoto) {
      this.bankCardPhotoUrl = this.baseUrl + this.credential.userName + '/' + this.credential.bankCardPhoto.fileName;
    }
    if (this.credential.idCardPhoto) {
      this.idCardPhotoUrl = this.baseUrl + this.credential.userName + '/' + this.credential.idCardPhoto.fileName;
    }
  }

  checkSalespersonCredetialAccepted(): void {
    this.salespersonService.checkSalespersonCredetialAccepted().subscribe((res) => {
      this.credentialAccepted = res;
    });
  }
}
