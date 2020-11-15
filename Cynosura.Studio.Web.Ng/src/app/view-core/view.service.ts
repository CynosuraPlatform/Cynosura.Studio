import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { ConfigService } from '../config/config.service';
import { CreatedEntity } from '../core/models/created-entity.model';
import { Page } from '../core/page.model';
import { FileResult } from '../core/file-result.model';

import { View } from './view.model';
import { GetViews, GetView, ExportViews,
    UpdateView, CreateView, DeleteView } from './view-request.model';

@Injectable({ providedIn: 'root' })
export class ViewService {
    private apiUrl = this.configService.config.apiBaseUrl + '/api';

    constructor(private httpClient: HttpClient, private configService: ConfigService) { }

    getViews(getViews: GetViews): Observable<Page<View>> {
        const url = `${this.apiUrl}/GetViews`;
        return this.httpClient.post<Page<View>>(url, getViews);
    }

    getView(getView: GetView): Observable<View> {
        const url = `${this.apiUrl}/GetView`;
        return this.httpClient.post<View>(url, getView);
    }

    exportViews(exportViews: ExportViews): Observable<FileResult> {
        const url = `${this.apiUrl}/ExportViews`;
        return this.httpClient.post(url, exportViews, {
            responseType: 'blob' as 'json',
            observe: 'response',
        }).pipe(map((response => new FileResult(response))));
    }

    updateView(updateView: UpdateView): Observable<{}> {
        const url = `${this.apiUrl}/UpdateView`;
        return this.httpClient.post(url, updateView);
    }

    createView(createView: CreateView): Observable<CreatedEntity<string>> {
        const url = `${this.apiUrl}/CreateView`;
        return this.httpClient.post<CreatedEntity<string>>(url, createView);
    }

    deleteView(deleteView: DeleteView): Observable<{}> {
        const url = `${this.apiUrl}/DeleteView`;
        return this.httpClient.post(url, deleteView);
    }
}
