import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {Router} from '@angular/router';
import {AuthService} from '../../services/auth.service';
import {SpinnerService} from '../../services/spinner.service';
import {MatSnackBar} from '@angular/material/snack-bar';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  error: string;

  loginForm = new FormGroup({
    userName: new FormControl('admin', [Validators.required, Validators.maxLength(40)]),
    password: new FormControl('12345678910', [Validators.required, Validators.maxLength(40)]),
  });

  constructor(private authService: AuthService,
              private router: Router,
              public spinnerService: SpinnerService,
              private snackBar: MatSnackBar) {
  }

  ngOnInit(): void {
  }

  login(): any {
    this.error = '';
    this.authService.login(this.loginForm.value).subscribe(
      () => {
        if (this.authService.isInRole('Admin')) {
          this.router.navigate(['/admin']);
        } else if (this.authService.isInRole('Salesperson')) {
          this.router.navigate(['/salesperson']);
        } else {
          this.error = 'ورود ناموفق';
          return;
        }
      },
      (er) => {
        this.error = 'ورود ناموفق';
        this.loginForm.reset();
        // console.log('login failed!');
        this.snackBar.open('ورود ناموفق بود', null, {
          duration: 2000,
        });
      }
    );
  }

}
