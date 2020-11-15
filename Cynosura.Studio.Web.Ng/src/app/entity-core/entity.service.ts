import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { ConfigService } from '../config/config.service';
import { CreatedEntity } from '../core/models/created-entity.model';
import { Page } from '../core/page.model';
import { FileResult } from '../core/file-result.model';

import { Entity } from './entity.model';
import { GetEntities, GetEntity, ExportEntities,
    UpdateEntity, CreateEntity, DeleteEntity, GenerateEntity } from './entity-request.model';

@Injectable({ providedIn: 'root' })
export class EntityService {
    private apiUrl = this.configService.config.apiBaseUrl + '/api';

    constructor(private httpClient: HttpClient, private configService: ConfigService) { }

    getEntities(getEntities: GetEntities): Observable<Page<Entity>> {
        const url = `${this.apiUrl}/GetEntities`;
        return this.httpClient.post<Page<Entity>>(url, getEntities);
    }

    getEntity(getEntity: GetEntity): Observable<Entity> {
        const url = `${this.apiUrl}/GetEntity`;
        return this.httpClient.post<Entity>(url, getEntity);
    }

    exportEntities(exportEntities: ExportEntities): Observable<FileResult> {
        const url = `${this.apiUrl}/ExportEntities`;
        return this.httpClient.post(url, exportEntities, {
            responseType: 'blob' as 'json',
            observe: 'response',
        }).pipe(map((response => new FileResult(response))));
    }

    updateEntity(updateEntity: UpdateEntity): Observable<{}> {
        const url = `${this.apiUrl}/UpdateEntity`;
        return this.httpClient.post(url, updateEntity);
    }

    createEntity(createEntity: CreateEntity): Observable<CreatedEntity<string>> {
        const url = `${this.apiUrl}/CreateEntity`;
        return this.httpClient.post<CreatedEntity<string>>(url, createEntity);
    }

    deleteEntity(deleteEntity: DeleteEntity): Observable<{}> {
        const url = `${this.apiUrl}/DeleteEntity`;
        return this.httpClient.post(url, deleteEntity);
    }

    generateEntity(generateEntity: GenerateEntity): Observable<{}> {
        const url = `${this.apiUrl}/GenerateEntity`;
        return this.httpClient.post(url, generateEntity);
    }
}
