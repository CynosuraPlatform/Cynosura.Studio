import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { ConfigService } from '../config/config.service';
import { CreatedEntity } from '../core/models/created-entity.model';

import { Register } from './account-request.model';

@Injectable()
export class AccountService {
    private apiUrl = this.configService.config.apiBaseUrl + '/api';

    constructor(private httpClient: HttpClient,
                private configService: ConfigService) {
    }

    register(register: Register): Observable<CreatedEntity<number>> {
        const url = `${this.apiUrl}/Register`;
        return this.httpClient.post<CreatedEntity<number>>(url, register);
    }
}
