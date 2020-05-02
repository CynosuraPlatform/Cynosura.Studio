import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ConfigService } from '../config/config.service';

import { GetTemplateVersions, GetTemplates } from './template-request.model';
import { TemplateModel } from './template.model';

@Injectable()
export class TemplateService {

    private apiUrl = this.configService.config.apiBaseUrl + '/api';

    constructor(
        private configService: ConfigService,
        private httpClient: HttpClient) {
    }

    getTemplates(getTemplates: GetTemplates): Observable<TemplateModel[]> {
        const url = `${this.apiUrl}/GetTemplates`;
        return this.httpClient.post<TemplateModel[]>(url, getTemplates);
    }

    getTemplateVersions(getTemplateVersions: GetTemplateVersions): Observable<string[]> {
        const url = `${this.apiUrl}/GetTemplateVersions`;
        return this.httpClient.post<string[]>(url, getTemplateVersions);
    }
}
