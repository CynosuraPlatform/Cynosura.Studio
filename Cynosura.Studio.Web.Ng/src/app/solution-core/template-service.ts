import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ConfigService } from '../config/config.service';

@Injectable()
export class TemplateService {

    private apiUrl = this.configService.config.apiBaseUrl + '/api';

    constructor(
        private configService: ConfigService,
        private httpClient: HttpClient) {
    }

    getTemplates(): Observable<TemplateModel> {
        const url = `${this.apiUrl}/GetTemplates`;
        return this.httpClient.post<TemplateModel>(url, null);
    }
}

export interface TemplateModel {
    name: string;
}
