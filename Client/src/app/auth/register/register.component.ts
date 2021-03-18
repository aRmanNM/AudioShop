import {Component, OnInit} from '@angular/core';
import {ErrorStateMatcher} from '@angular/material/core';
import {AsyncValidatorFn, FormControl, FormGroup, FormGroupDirective, NgForm, Validators} from '@angular/forms';
import {of, timer} from 'rxjs';
import {map, switchMap} from 'rxjs/operators';
import {AuthService} from '../../services/auth.service';
import {Router} from '@angular/router';
import {SpinnerService} from '../../services/spinner.service';
import {MatSnackBar} from '@angular/material/snack-bar';

export class PasswordStateMatcher implements ErrorStateMatcher {
  isErrorState(
    control: FormControl | null,
    form: FormGroupDirective | NgForm | null
  ): boolean {
    return control.touched && (form.hasError('mismatch') || control.invalid);
  }
}

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  showSpinner = false;
  matcher = new PasswordStateMatcher();
  // passwordPattern = '(?=^.{6,20}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;\'?/&gt;.&lt;,])(?!.*\\s).*$';
  isRegister: boolean;

  registerForm = new FormGroup(
    {
      userName: new FormControl(
        '',
        [Validators.required, Validators.maxLength(40)],
        [this.validateUserNameNotTaken()]
      ),
      firstName: new FormControl(''),
      lastName: new FormControl(''),
      city: new FormControl(''),
      country: new FormControl(''),
      age: new FormControl(''),
      gender: new FormControl(''),
      password: new FormControl('', [
        Validators.required,
        // Validators.pattern(this.passwordPattern),
        Validators.minLength(6),
        Validators.maxLength(40)
      ]),
      confirmPassword: new FormControl('', [Validators.required, Validators.maxLength(40)]),
    },
    this.passwordMatchValidator
  );

  constructor(private authService: AuthService,
              private router: Router,
              public spinnerService: SpinnerService,
              private snackBar: MatSnackBar) {
  }

  ngOnInit(): void {
  }

  register(): void {
    this.showSpinner = true;
    this.authService.register(this.registerForm.value).subscribe((res) => {
      this.showSpinner = false;
      this.router.navigate(['/salesperson']);
    }, error => {
      this.snackBar.open('ثبت نام ناموفق', null, {
        duration: 2000,
      });
    });
  }

  passwordMatchValidator(g: FormGroup): any {
    return g.get('password').value === g.get('confirmPassword').value
      ? null
      : {mismatch: true};
  }

  validateUserNameNotTaken(): AsyncValidatorFn {
    return (control) => {
      return timer(500).pipe(
        switchMap(() => {
          if (!control.value) {
            return of(null);
          }
          return this.authService.checkUserNameExists(control.value).pipe(
            map((res) => {
              return res ? {userNameExists: true} : null;
            })
          );
        })
      );
    };
  }

}
