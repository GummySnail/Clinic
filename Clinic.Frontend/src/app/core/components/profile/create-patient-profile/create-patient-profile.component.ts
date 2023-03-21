import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";
import {Router} from "@angular/router";
import {MatSnackBar, MatSnackBarConfig} from "@angular/material/snack-bar";
import {ProfileService} from "../../../services/profile.service";
import {CreatePatientProfileRequest} from "../../../models/requests/profile/create-patient-profile-request";
import {formatDate, Location} from "@angular/common";

@Component({
  selector: 'app-create-patient-profile',
  templateUrl: './create-patient-profile.component.html',
  styleUrls: ['./create-patient-profile.component.css']
})
export class CreatePatientProfileComponent implements OnInit{
  public selectedFile: File | null = null;
  public errorType: string | null = null;
  public errorMessage: string  | null = null;
  public minDate: Date = new Date();
  public maxDate: Date = new Date();

  createPatientProfileForm: FormGroup = new FormGroup({
    firstName: new FormControl(''),
    lastName: new FormControl(''),
    middleName: new FormControl(''),
    dateOfBirth: new FormControl(''),
    phoneNumber: new FormControl(''),
    photo: new FormControl(''),
  });

  constructor(private location: Location, private formBuilder: FormBuilder, private profileService: ProfileService, private router: Router, private snackBar: MatSnackBar)
  {
    const today = new Date();
    const year = today.getFullYear() - 100;
    this.minDate.setFullYear(year);
    this.maxDate = today;
  }
  ngOnInit(): void {
    this.initializeForm();
  }

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];
  }

  public create(): void {
    let createPatientProfileRequest: CreatePatientProfileRequest = {
      firstName: this.createPatientProfileForm.controls['firstName'].value,
      lastName: this.createPatientProfileForm.controls['lastName'].value,
      middleName: this.createPatientProfileForm.controls['middleName'].value,
      dateOfBirth: formatDate(this.createPatientProfileForm.controls['dateOfBirth'].value, 'yyyy-MM-dd', 'en-US'),
      phoneNumber: this.createPatientProfileForm.controls['phoneNumber'].value,
      photo: this.selectedFile,
    };

    const formData = new FormData();
    formData.append("FirstName", createPatientProfileRequest.firstName);
    formData.append("LastName", createPatientProfileRequest.lastName);
    formData.append("MiddleName", createPatientProfileRequest.middleName!);
    formData.append("DateOfBirth", createPatientProfileRequest.dateOfBirth);
    formData.append("PhoneNumber", createPatientProfileRequest.phoneNumber);
    formData.append("ProfilePhoto", this.selectedFile!);

    this.profileService.createPatientProfile(formData).pipe()
      .subscribe(() => {
        let config = new MatSnackBarConfig();
        config.duration = 3000;
        this.snackBar.open("You successfully create patient profile", "Close", config);
        this.location.back();
      })
}

public isError(): boolean{
  return this.errorType !== null;
}

  private initializeForm(): void {
    this.createPatientProfileForm = this.formBuilder.group({
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      middleName: [''],
      dateOfBirth: ['', [Validators.required]],
      phoneNumber: ['', [Validators.required, Validators.pattern('^[0-9]*$')]],
      photo: [''],
    });

    this.createPatientProfileForm.controls['firstName'].valueChanges.subscribe(() => {
      if(this.errorType === 'firstName') {
        this.errorType = null;
        this.errorMessage = null;
      }
    });

    this.createPatientProfileForm.controls['lastName'].valueChanges.subscribe(() => {
      if(this.errorType === 'lastName') {
        this.errorType = null;
        this.errorMessage = null;
      }
    });

    this.createPatientProfileForm.controls['middleName'].valueChanges.subscribe(() => {
      if(this.errorType === 'middleName') {
        this.errorType = null;
        this.errorMessage = null;
      }
    });

    this.createPatientProfileForm.controls['dateOfBirth'].valueChanges.subscribe(() => {
      if(this.errorType === 'dateOfBirth') {
        this.errorType = null;
        this.errorMessage = null;
      }
    });

    this.createPatientProfileForm.controls['phoneNumber'].valueChanges.subscribe(() => {
      if(this.errorType === 'phoneNumber') {
        this.errorType = null;
        this.errorMessage = null;
      }
    });

    this.createPatientProfileForm.controls['photo'].valueChanges.subscribe(() => {
      if(this.errorType === 'photo') {
        this.errorType = null;
        this.errorMessage = null;
      }
    });
  }
}
