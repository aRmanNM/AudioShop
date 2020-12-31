import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";
import {AuthService} from "../../Services/auth.service";
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  error: string;
  returnUrl: string;
  passwordPattern = "(?=^.{6,20}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$";
  private registerForm: FormGroup;

  constructor(private authService: AuthService, private router: Router, private formBuilder: FormBuilder, private activatedRoute: ActivatedRoute,) {
    this.registerForm = this.formBuilder.group({
          displayName: new FormControl("", [Validators.required, Validators.maxLength(20)]),
          email: new FormControl(
              "",
              [Validators.required, Validators.email, Validators.maxLength(30)]
          ),
          password: new FormControl("", [
            Validators.required,
            Validators.pattern(this.passwordPattern),
            Validators.maxLength(20)
          ]),
          confirmPassword: new FormControl("", [Validators.required, Validators.maxLength(20)]),
    }, this.passwordMatchValidator);
  }

  ngOnInit(): void {
    this.returnUrl =
        this.activatedRoute.snapshot.queryParams.returnUrl || "Tabs/Shop";
  }

  register() {
    this.authService.register(this.registerForm.value).subscribe(
        () => {
          this.router.navigateByUrl(this.returnUrl);
        },
        (er) => {
          this.error = er.error;
          this.registerForm.reset();
          console.log("login failed!");
        }
    );
  }

  passwordMatchValidator(g: FormGroup) {
    return g.get('password').value === g.get('confirmPassword').value
        ? null
        : { mismatch: true };
  }
}
