import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";
import {AuthService} from "../../../services/auth.service";
import {Router} from "@angular/router";
import {MatSnackBar, MatSnackBarConfig} from "@angular/material/snack-bar";
import {catchError, throwError} from "rxjs";
import {SignUpRequest} from "../../../models/requests/sign-up-request";
import Validation from '../../../../shared/validation/confirmPasswordValidation';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent implements OnInit{
  public hidePassword: boolean = true;
  public hideConfirmPassword: boolean = true;
  public errorType: string | null = null;
  public errorMessage: string  | null = null;
  signUpForm: FormGroup = new FormGroup({
    email: new FormControl(''),
    password: new FormControl(''),
    confirmPassword: new FormControl(''),
  });
  constructor(private formBuilder: FormBuilder, private authService: AuthService, private router: Router, private snackBar: MatSnackBar) {}

  ngOnInit(): void {
    this.initializeForm();
  }

  public signUp(): void {
    let signUpRequest: SignUpRequest = {
      email: this.signUpForm.controls['email'].value,
      password: this.signUpForm.controls['password'].value
    };

    this.authService.signUp(signUpRequest).pipe(
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
      this.snackBar.open('You successfully Sign Up', 'Close', config);
      this.router.navigate(['']);
    });
    console.log(signUpRequest.email + " " + signUpRequest.password);
  }

  public isError(): boolean{
    return this.errorType !== null;
  }
  private initializeForm(): void {
    this.signUpForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(32)]],
      confirmPassword: ['', Validators.required]
    },
    {
      validators: [Validation.match('password', 'confirmPassword')]
    });

    this.signUpForm.controls['email'].valueChanges.subscribe(() => {
      if(this.errorType === 'Email') {
        this.errorType = null;
        this.errorMessage = null;
      }
    });

    this.signUpForm.controls['password'].valueChanges.subscribe(() => {
      if(this.errorType === 'Password') {
        this.errorType = null;
        this.errorMessage = null;
      }
    });
  }
}
