import {Component, OnInit} from '@angular/core';
import {AuthService} from '../../services/auth.service';
import {ErrorStateMatcher} from '@angular/material/core';
import {FormControl, FormGroup, FormGroupDirective, NgForm, Validators} from '@angular/forms';
import {MatSnackBar} from '@angular/material/snack-bar';
import {SpinnerService} from '../../services/spinner.service';

export class PasswordStateMatcher implements ErrorStateMatcher {
  isErrorState(
    control: FormControl | null,
    form: FormGroupDirective | NgForm | null
  ): boolean {
    return control.touched && (form.hasError('mismatch') || control.invalid);
  }
}

@Component({
  selector: 'app-password',
  templateUrl: './password.component.html',
  styleUrls: ['./password.component.scss']
})
export class PasswordComponent implements OnInit {
  matcher = new PasswordStateMatcher();

  passForm = new FormGroup(
    {
      oldPassword: new FormControl('', [Validators.required]),
      newPassword: new FormControl('', [
        Validators.required,
        // Validators.pattern(this.passwordPattern),
        Validators.minLength(6),
        Validators.maxLength(40)
      ]),
      confirmNewPassword: new FormControl('', [Validators.required, Validators.maxLength(40)]),
    },
    this.passwordMatchValidator
  );

  constructor(private authService: AuthService,
              private snackBar: MatSnackBar,
              public spinnerService: SpinnerService) {
  }

  ngOnInit(): void {
  }

  passwordMatchValidator(g: FormGroup): any {
    return g.get('newPassword').value === g.get('confirmNewPassword').value
      ? null
      : {mismatch: true};
  }

  updatePassword(): void {
    this.authService.updatePassword(this.passForm.value.oldPassword, this.passForm.value.newPassword).subscribe(() => {
      this.passForm.reset();
      this.snackBar.open('رمز با موفقیت تغییر کرد', null, {
        duration: 3000,
      });
    }, error => {
      this.passForm.reset();
      this.snackBar.open('خطایی رخ داد', null, {
        duration: 3000,
      });
    });
  }
}
