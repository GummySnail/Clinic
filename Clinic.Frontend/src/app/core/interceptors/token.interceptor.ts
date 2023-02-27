import {Injectable} from "@angular/core";
import {HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from "@angular/common/http";
import {BehaviorSubject, catchError, filter, Observable, switchMap, take, throwError} from "rxjs";
import {AuthService} from "../services/auth.service";
import {TokenService} from "../services/token.service";

@Injectable()
export class TokenInterceptor implements HttpInterceptor{
  private isRefreshing: boolean = false;
  private refreshTokenSubject: BehaviorSubject<string | null> = new BehaviorSubject<string | null>(null);

  constructor(private authService: AuthService, private tokenService: TokenService) {
  }
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    request = this.addAccessTokenToHeader(request);
    return next.handle(request).pipe(catchError(error => {
      if (error instanceof HttpErrorResponse && error.status === 401 && error.error === null){
        return this.handleRefreshTokenError(request, next);
      }
      return throwError(error);
    }))
  }

  private addAccessTokenToHeader(request: HttpRequest<any>): HttpRequest<any>{
    let currentUser = this.authService.getCurrentUser();
    if (currentUser){
      return request.clone({
        setHeaders: {
          Authorization: `Bearer ${currentUser.accessToken}`
        }
      });
    }
    return request;
  }

  private handleRefreshTokenError(request: HttpRequest<any>, next: HttpHandler): Observable<any>{
    if (!this.isRefreshing){
      this.isRefreshing = true;
      this.refreshTokenSubject.next(null);

      let currentUser = this.authService.getCurrentUser();

      if (currentUser)
      {
        return this.tokenService.refreshToken().pipe(
          switchMap(() => {
            this.isRefreshing = false;
            currentUser = this.authService.getCurrentUser();
            this.refreshTokenSubject.next(currentUser.accessToken);
            return next.handle(this.addAccessTokenToHeader(request));
          }),
          catchError(error => {
            this.isRefreshing = false;
            this.authService.deleteCurrentUser();
            return throwError(error);
          })
        )
      }
    }
    return this.refreshTokenSubject.pipe(
      filter(token => token !== null),
      take(1),
      switchMap(() => next.handle(this.addAccessTokenToHeader(request)))
    );
  }
}
