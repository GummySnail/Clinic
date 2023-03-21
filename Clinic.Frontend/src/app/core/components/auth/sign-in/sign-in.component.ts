import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";
import {SignInRequest} from "../../../models/requests/auth/sign-in-request";
import {AuthService} from "../../../services/auth.service";
import {MatSnackBar, MatSnackBarConfig} from "@angular/material/snack-bar";
import {catchError, throwError} from "rxjs";
import {Router} from "@angular/router";

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.css']
})
export class SignInComponent implements OnInit{
  public hidePassword: boolean = true;
  public errorType: string | null = null;
  public errorMessage: string  | null = null;
  signInForm: FormGroup = new FormGroup({
    email: new FormControl(''),
    password: new FormControl(''),
  });
  constructor(private formBuilder: FormBuilder, private authService: AuthService, private router: Router, private snackBar: MatSnackBar) {}

  ngOnInit(): void {
    this.initializeForm();
  }

  public signIn(): void {
    let signInRequest: SignInRequest = {
      email: this.signInForm.controls['email'].value,
      password: this.signInForm.controls['password'].value
    };

    this.authService.signIn(signInRequest).pipe(
      catchError(response => {
        if(response.error.message.includes('email') || response.error.message.includes('Email')){
          this.errorType = 'Email';
          this.errorMessage = response.error.message;
        }else if (response.error.message.includes('password')){
          this.errorType = 'Password';
          this.errorMessage = response.error.message;
        }
        return throwError(() => response);
      })
    ).subscribe(() => {
      let config = new MatSnackBarConfig();
      config.duration = 3000;
      this.snackBar.open('You successfully logged in', 'Close', config);
      this.router.navigate(['']);
    });
  }

  public isError(): boolean{
    return this.errorType !== null;
  }
  private initializeForm(): void {
    this.signInForm = this.formBuilder.group({
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(32)]]
      });

    this.signInForm.controls['email'].valueChanges.subscribe(() => {
      if(this.errorType === 'Email') {
        this.errorType = null;
        this.errorMessage = null;
      }
    });

    this.signInForm.controls['password'].valueChanges.subscribe(() => {
      if(this.errorType === 'Password') {
        this.errorType = null;
        this.errorMessage = null;
      }
    });
  }

}
