import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment'
import {HttpClient, HttpParams} from "@angular/common/http";
import {SignInRequest} from "../models/requests/auth/sign-in-request";
import {map, Observable, ReplaySubject} from "rxjs";
import {Account} from "../models/Account";
import {SignUpRequest} from "../models/requests/auth/sign-up-request";
import {CustomEncoder} from "../../shared/custom-encoder";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl: string = environment.apiUrl;
  private currentUserSource: ReplaySubject<Account | null> = new ReplaySubject<Account | null>(1);
  public currentUser$: Observable<Account | null> = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) { }

  public signUp(body: SignUpRequest): Observable<void>{
    return this.http.post<void>(this.baseUrl + 'sign-up', body);
  }
  public signIn(body: SignInRequest): Observable<void>{
    return this.http.post<Account>(this.baseUrl + 'sign-in', body).pipe(
      map((user: Account) => {
        if (user){
          this.setCurrentUser(user);
        }
      })
    )
  }

  public setCurrentUser(user: Account): void {
    user.role = JSON.parse(atob(user.accessToken.split('.')[1]))['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
    user.email = JSON.parse(atob(user.accessToken.split('.')[1]))['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'];
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }

  public getCurrentUser(): Account{
    return JSON.parse(localStorage.getItem('user')!);
  }

  public isAuthenticated() : boolean {
    return !!localStorage.getItem("user");
  }

  public deleteCurrentUser(): void {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }
  public confirmEmail = (token: string, email: string) => {
    let params = new HttpParams({ encoder: new CustomEncoder() });
    params = params.append('token', token);
    params = params.append('email', email);

    return this.http.get(this.baseUrl + 'email-confirmation', {params: params});
  }


}
