import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {SignInComponent} from "./core/components/auth/sign-in/sign-in.component";
import {SignUpComponent} from "./core/components/auth/sign-up/sign-up.component";
import {HomeComponent} from "./core/components/home/home.component";
import {EmailConfirmationComponent} from "./core/components/auth/email-confirmation/email-confirmation.component";
import {
  CreatePatientProfileComponent
} from "./core/components/profile/create-patient-profile/create-patient-profile.component";

const routes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: 'auth',
    children: [
      { path: 'sign-up', component: SignUpComponent },
      { path: 'sign-in', component: SignInComponent },
      { path: 'confirm-email', component: EmailConfirmationComponent }
    ]
  },
  {
    path: 'profiles',
    children: [
      { path: 'create-patient-profile', component: CreatePatientProfileComponent },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
