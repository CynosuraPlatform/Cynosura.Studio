import { Injectable } from "@angular/core";
import { HttpClient, HttpParams, HttpHeaders, HttpResponse } from "@angular/common/http";
import { Observable } from "rxjs/Rx";
import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { filter } from "rxjs/operators/filter";
import { map } from "rxjs/operators/map";
import { tap } from "rxjs/operators/tap";

import { AuthStateModel } from "../models/auth-state.model";
import { AuthTokenModel } from "../models/auth-tokens.model";
import { LoginModel } from "../models/login.model";
import { RefreshGrantModel } from "../models/refresh-grant.model";

@Injectable()
export class AuthService {
    private initalState: AuthStateModel = { tokens: undefined, authReady: false };
    private state: BehaviorSubject<AuthStateModel>;

    state$: Observable<AuthStateModel>;
    tokens$: Observable<AuthTokenModel>;
    loggedIn$: Observable<boolean>;

    constructor(private httpClient: HttpClient) {
        this.state = new BehaviorSubject<AuthStateModel>(this.initalState);
        this.state$ = this.state.asObservable();

        this.tokens$ = this.state.pipe(filter((state: AuthStateModel) => state.authReady))
            .pipe(map((state: AuthStateModel) => state.tokens));
        this.loggedIn$ = this.tokens$.pipe(map(tokens => !!tokens));
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
        return this.state.first()
            .map(state => state.tokens)
            .flatMap(tokens => this.getTokens({ refresh_token: (<AuthTokenModel>tokens).refresh_token }, 'refresh_token')
                .catch(error => Observable.throw('Session Expired'))
            );
    }

    tokens(): AuthTokenModel {
        return this.state.value.tokens;
    }

    private getTokens(data: RefreshGrantModel | LoginModel, grantType: string): Observable<any> {
        const headers = new HttpHeaders({ 'Content-Type': "application/x-www-form-urlencoded" });
        const options = { headers: headers };

        Object.assign(data, { grant_type: grantType, scope: "openid offline_access" });

        let params = new HttpParams();
        Object.keys(data)
            .forEach(key => params = params.set(key, (<any>data)[key]));
        return this.httpClient.post<AuthTokenModel>(`/connect/token`, params.toString(), options)
            .pipe(tap((tokens: AuthTokenModel) => {
                this.storeToken(tokens);
                this.updateState({ authReady: true, tokens });
            }));
    }

    private storeToken(tokens: AuthTokenModel): void {
        const previousTokens = this.retrieveTokens();
        if (previousTokens != null && tokens.refresh_token == null) {
            tokens.refresh_token = previousTokens.refresh_token;
        }
        localStorage.setItem("auth-tokens", JSON.stringify(tokens));
    }

    private retrieveTokens(): AuthTokenModel {
        const tokensString = localStorage.getItem("auth-tokens");
        const tokensModel: AuthTokenModel = tokensString == null ? null : JSON.parse(tokensString);
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
        return Observable.of(this.retrieveTokens())
            .flatMap((tokens: AuthTokenModel) => {
                if (!tokens) {
                    this.updateState({ authReady: true });
                    return Observable.throw('No token in Storage');
                }
                this.updateState({ tokens });
                this.updateState({ authReady: true });

                return this.refreshTokens();
            })
            .catch(error => {
                this.logout();
                this.updateState({ authReady: true });
                return Observable.throw(error);
            });
    }
}
