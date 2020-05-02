import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ConfigService } from '../config/config.service';
import { CreatedEntity } from '../core/models/created-entity.model';
import { Page } from '../core/page.model';

import { View } from './view.model';
import { GetViews, GetView, UpdateView, CreateView, DeleteView } from './view-request.model';

@Injectable()
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
