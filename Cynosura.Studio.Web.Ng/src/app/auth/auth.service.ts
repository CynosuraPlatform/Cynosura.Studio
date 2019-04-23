import { Injectable } from "@angular/core";
import { HttpClient, HttpParams, HttpHeaders, HttpResponse } from "@angular/common/http";
import { throwError as observableThrowError, Observable, BehaviorSubject, of } from "rxjs";
import { filter, map, tap, first, flatMap, catchError } from "rxjs/operators";

import { ConfigService } from "../config/config.service";
import { CustomEncoder } from "./custom-encoder";
import { AuthStateModel } from "./auth-state.model";
import { AuthTokenModel } from "./auth-tokens.model";
import { LoginModel } from "./login.model";
import { RefreshGrantModel } from "./refresh-grant.model";
import { User } from "./user.model";
import * as decode from "jwt-decode";

@Injectable()
export class AuthService {
    private initalState: AuthStateModel = { tokens: undefined, authReady: false };
    private state: BehaviorSubject<AuthStateModel>;

    state$: Observable<AuthStateModel>;
    tokens$: Observable<AuthTokenModel>;
    loggedIn$: Observable<boolean>;
    currentUser$: Observable<User>;

    constructor(private httpClient: HttpClient, private configService: ConfigService) {
        this.state = new BehaviorSubject<AuthStateModel>(this.initalState);
        this.state$ = this.state.asObservable();

        this.tokens$ = this.state.pipe(filter((state: AuthStateModel) => state.authReady))
            .pipe(map((state: AuthStateModel) => state.tokens));
        this.loggedIn$ = this.tokens$.pipe(map(tokens => !!tokens));
        this.currentUser$ = this.tokens$.pipe(map(tokens =>  tokens ? this.getUser(tokens.access_token) : null));
    }

    init(): Observable<AuthTokenModel> {
        return this.startupTokenRefresh();
    }

    login(user: LoginModel): Observable<any> {
        return this.getTokens(user, "password");
    }

    logout(): void {
        this.updateState({ tokens: undefined });
        this.removeToken();
    }

    refreshTokens(): Observable<AuthTokenModel> {
        return this.state.pipe(first())
            .pipe(map((state) => state.tokens))
            .pipe(flatMap(tokens => this.getTokens({ refresh_token: (<AuthTokenModel>tokens).refresh_token }, "refresh_token")
                .pipe(catchError(error => observableThrowError("Session Expired")))
            ));
    }

    tokens(): AuthTokenModel {
        return this.state.value.tokens;
    }

    private getTokens(data: RefreshGrantModel | LoginModel, grantType: string): Observable<any> {
        const headers = new HttpHeaders({ "Content-Type": "application/x-www-form-urlencoded" });
        const options = { headers: headers };
        const sendData = {
            grant_type: grantType, scope: "openid offline_access",
            ...data
        };
        const params = Object.keys(sendData)
            .reduce((prev, next) => prev.set(next, sendData[next]), new HttpParams({encoder: new CustomEncoder()}));
        return this.httpClient.post<AuthTokenModel>(`${this.configService.config.apiBaseUrl}/connect/token`, params.toString(), options)
            .pipe(tap((tokens: AuthTokenModel) => {
                this.storeToken(tokens);
                this.updateState({ authReady: true, tokens });
            }));
    }

    private getUser(accessToken: string): User {
        const decodedToken = decode(accessToken);
        const user = new User();
        user.userName = decodedToken.name;
        if (decodedToken.role instanceof Array) {
            user.roles = decodedToken.role;
        } else if (decodedToken.role) {
            user.roles = [decodedToken.role];
        } else {
            user.roles = [];
        }
        user.initPermissions();
        return user;
    }

    private storeToken(tokens: AuthTokenModel): void {
        const previousTokens = this.retrieveTokens();
        if (previousTokens && !tokens.refresh_token) {
            tokens.refresh_token = previousTokens.refresh_token;
        }
        localStorage.setItem("auth-tokens", JSON.stringify(tokens));
    }

    private retrieveTokens(): AuthTokenModel {
        const tokensString = localStorage.getItem("auth-tokens");
        const tokensModel: AuthTokenModel = tokensString === null ? null : JSON.parse(tokensString);
        return tokensModel;
    }

    private removeToken(): void {
        localStorage.removeItem("auth-tokens");
    }

    private updateState(newState: AuthStateModel): void {
        const previousState = this.state.getValue();
        this.state.next(Object.assign({}, previousState, newState));
    }

    private startupTokenRefresh(): Observable<AuthTokenModel> {
        return of(this.retrieveTokens())
            .pipe(flatMap((tokens: AuthTokenModel) => {
                if (!tokens) {
                    this.updateState({ authReady: true });
                    return observableThrowError("No token in Storage");
                }
                this.updateState({ tokens });
                this.updateState({ authReady: true });

                return this.refreshTokens();
            }))
            .pipe(catchError(error => {
                this.logout();
                this.updateState({ authReady: true });
                return observableThrowError(error);
            }));
    }
}
