import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";

import { Register } from "./account-request.model";
import { ConfigService } from "../config/config.service";
import { CreatedEntity } from "../core/models/created-entity.model";


@Injectable()
export class AccountService {
    private apiUrl = this.configService.config.apiBaseUrl + "/api";

    constructor(private httpClient: HttpClient,
                private configService: ConfigService) {
    }

    register(register: Register): Promise<CreatedEntity<number>> {
        const url = `${this.apiUrl}/Register`;
        return this.httpClient.post<CreatedEntity<number>>(url, register)
            .toPromise();
    }
}
