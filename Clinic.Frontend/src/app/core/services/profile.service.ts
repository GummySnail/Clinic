import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {map, Observable, Subscription} from "rxjs";
import {Account} from "../models/Account";

@Injectable({
  providedIn: 'root'
})
export class ProfileService {

  constructor(private http: HttpClient) { }

  public createPatientProfile(body: any): Observable<void>{
    return this.http.post<void>("http://localhost:5122/api/profile/create-patient-profile", body);
  }
}
