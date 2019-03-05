import { Injectable } from "@angular/core";
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpErrorResponse } from "@angular/common/http";
import { Router } from "@angular/router";
import { throwError as observableThrowError, Observable, BehaviorSubject } from "rxjs";
import { tap, catchError, switchMap, finalize, filter, take } from "rxjs/operators";

import { AuthService } from "./auth.service";
import { AuthTokenModel } from "./auth-tokens.model";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

    isRefreshingToken = false;
    tokenSubject: BehaviorSubject<string> = new BehaviorSubject<string>(null);

    constructor(private authService: AuthService,
        private router: Router) { }

    addToken(request: HttpRequest<any>, token: string): HttpRequest<any> {
        return request.clone({
            setHeaders: {
                Authorization: `Bearer ${token}`
            }
        });
    }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const tokens = this.authService.tokens();
        if (tokens != null) {
            request = this.addToken(request, tokens.access_token);
        }
        return next.handle(request)
            .pipe(catchError(
                (err: any) => {
                    if (err instanceof HttpErrorResponse) {
                        const httpError = err as HttpErrorResponse;
                        if (httpError.status === 400 && tokens != null) {
                            return this.handle400Error(err);
                        }
                        if (httpError.status === 401) {
                            return this.handle401Error(request, next);
                        }
                    }
                    return observableThrowError(err);
                })
            );
    }

    handle401Error(request: HttpRequest<any>, next: HttpHandler): Observable<any> {
        if (!this.isRefreshingToken) {
            this.isRefreshingToken = true;

            // Reset here so that the following requests wait until the token
            // comes back from the refreshToken call.
            this.tokenSubject.next(null);

            return this.authService.refreshTokens()
                .pipe(switchMap((newTokens: AuthTokenModel) => {
                    if (newTokens) {
                        this.tokenSubject.next(newTokens.access_token);
                        return next.handle(this.addToken(request, newTokens.access_token));
                    }

                    // If we don't get a new token, we are in trouble so logout.
                    return this.logoutUser();
                }))
                .pipe(catchError((error) => {
                    // If there is an exception calling 'refreshToken', bad news so logout.
                    return this.logoutUser();
                }))
                .pipe(finalize(() => {
                    this.isRefreshingToken = false;
                }));
        } else {
            return this.tokenSubject
                .pipe(filter(token => token != null))
                .pipe(take(1))
                .pipe(switchMap((token) => {
                    return next.handle(this.addToken(request, token));
                }));
        }
    }

    logoutUser() {
        this.authService.logout();
        this.router.navigate(["/"]);

        return observableThrowError("");
    }

    handle400Error(error) {
        if (error && error.status === 400 && error.error && error.error.error === "invalid_grant") {
            // If we get a 400 and the error message is 'invalid_grant', the token is no longer valid so logout.
            return this.logoutUser();
        }

        return observableThrowError(error);
    }
}
