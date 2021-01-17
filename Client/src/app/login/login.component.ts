import { Component, OnInit } from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {Router} from '@angular/router';
import {AuthService} from '../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  error: string;

  loginForm = new FormGroup({
    userName: new FormControl('', [Validators.required, Validators.maxLength(40)]),
    password: new FormControl('', [Validators.required, Validators.maxLength(40)]),
  });

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit(): void {}

  login(): any {
    this.error = '';
    this.authService.login(this.loginForm.value).subscribe(
      () => {
        this.router.navigate(['/dashboard']);
      },
      (er) => {
        this.error = er.error;
        this.loginForm.reset();
        console.log('login failed!');
      }
    );
  }

}
