import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { ConfigService } from '../config/config.service';
import { CreatedEntity } from '../core/models/created-entity.model';
import { Page } from '../core/page.model';
import { FileResult } from '../core/file-result.model';

import { Enum } from './enum.model';
import { GetEnums, GetEnum, ExportEnums,
    UpdateEnum, CreateEnum, DeleteEnum, GenerateEnum } from './enum-request.model';

@Injectable({ providedIn: 'root' })
export class EnumService {
    private apiUrl = this.configService.config.apiBaseUrl + '/api';

    constructor(private httpClient: HttpClient, private configService: ConfigService) { }

    getEnums(getEnums: GetEnums): Observable<Page<Enum>> {
        const url = `${this.apiUrl}/GetEnums`;
        return this.httpClient.post<Page<Enum>>(url, getEnums);
    }

    getEnum(getEnum: GetEnum): Observable<Enum> {
        const url = `${this.apiUrl}/GetEnum`;
        return this.httpClient.post<Enum>(url, getEnum);
    }

    exportEnums(exportEnums: ExportEnums): Observable<FileResult> {
        const url = `${this.apiUrl}/ExportEnums`;
        return this.httpClient.post(url, exportEnums, {
            responseType: 'blob' as 'json',
            observe: 'response',
        }).pipe(map((response => new FileResult(response))));
    }

    updateEnum(updateEnum: UpdateEnum): Observable<{}> {
        const url = `${this.apiUrl}/UpdateEnum`;
        return this.httpClient.post(url, updateEnum);
    }


    createEnum(createEnum: CreateEnum): Observable<CreatedEntity<string>> {
        const url = `${this.apiUrl}/CreateEnum`;
        return this.httpClient.post<CreatedEntity<string>>(url, createEnum);
    }

    deleteEnum(deleteEnum: DeleteEnum): Observable<{}> {
        const url = `${this.apiUrl}/DeleteEnum`;
        return this.httpClient.post(url, deleteEnum);
    }

    generateEnum(generateEnum: GenerateEnum): Observable<{}> {
        const url = `${this.apiUrl}/GenerateEnum`;
        return this.httpClient.post(url, generateEnum);
    }
}
