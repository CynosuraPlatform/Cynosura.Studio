import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";

import { Profile } from "./profile.model";
import { ConfigService } from "../config/config.service";
import { GetProfile, UpdateProfile } from "./profile-request.model";


@Injectable()
export class ProfileService {
    private apiUrl = this.configService.config.apiBaseUrl + "/api";
    private headers = new HttpHeaders({ "Content-Type": "application/json" });

    constructor(private httpClient: HttpClient,
                private configService: ConfigService) {
    }

    getProfile(getProfile: GetProfile): Promise<Profile> {
        const url = `${this.apiUrl}/GetProfile`;
        return this.httpClient.post<Profile>(url, getProfile)
            .toPromise();
    }

    updateProfile(updateProfile: UpdateProfile): Promise<{}> {
        const url = `${this.apiUrl}/UpdateProfile`;
        return this.httpClient.post(url, updateProfile)
            .toPromise();
    }
}
