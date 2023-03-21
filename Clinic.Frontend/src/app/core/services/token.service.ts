import { Injectable } from '@angular/core';
import {environment} from "../../../environments/environment.development";
import {HttpClient} from "@angular/common/http";
import {AuthService} from "./auth.service";
import {map, Observable} from "rxjs";
import {RefreshTokensRequest} from "../models/requests/auth/refresh-tokens-request";
import {RefreshTokensResponse} from "../models/responses/refresh-tokens-response";
import {Account} from "../models/Account";
import {RevokeTokenRequest} from "../models/requests/auth/revoke-token-request";

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  private baseUrl: string = environment.apiUrl;
  constructor(private http: HttpClient, private authService: AuthService) { }

  public refreshToken(): Observable<void> {
    let currentUser = this.authService.getCurrentUser();
    let body: RefreshTokensRequest = {
      accessToken: currentUser.accessToken,
      refreshToken: currentUser.refreshToken
    };
    return this.http.post<RefreshTokensResponse>(this.baseUrl + 'refresh', body).pipe(
      map((response: RefreshTokensResponse) => {
        if (response){
          this.updateTokens(response);
        }
      })
    );
  }

  public revokeToken(): Observable<void> {
    let currentUser = this.authService.getCurrentUser();
    let body: RevokeTokenRequest = {
      refreshToken: currentUser.refreshToken
    };
    return this.http.post<void>(this.baseUrl + 'logout', body);
  }
  private updateTokens(tokens: RefreshTokensResponse): void {
    let user: Account = this.authService.getCurrentUser();
    user.accessToken = tokens.accessToken;
    user.refreshToken = tokens.refreshToken;
    this.authService.setCurrentUser(user);
  }
}
