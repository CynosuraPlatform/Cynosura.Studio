import { Injectable } from "@angular/core";
import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";

import { ConfigService } from "../config/config.service";
import { CreatedEntity } from "../core/models/created-entity.model";
import { Enum } from "./enum.model";
import { GetEnums, GetEnum, UpdateEnum, CreateEnum, DeleteEnum, GenerateEnum } from "./enum-request.model";
import { EnumFilter } from "./enum-filter.model";
import { Page } from "../core/page.model";

@Injectable()
export class EnumService {
    private apiUrl = this.configService.config.apiBaseUrl + "/api";

    constructor(private httpClient: HttpClient, private configService: ConfigService) { }

    getEnums(getEnums: GetEnums): Promise<Page<Enum>> {
        const url = `${this.apiUrl}/GetEnums`;
        return this.httpClient.post<Page<Enum>>(url, getEnums)
            .toPromise();
    }

    getEnum(getEnum: GetEnum): Promise<Enum> {
        const url = `${this.apiUrl}/GetEnum`;
        return this.httpClient.post<Enum>(url, getEnum)
            .toPromise();
    }

    updateEnum(updateEnum: UpdateEnum): Promise<{}> {
        const url = `${this.apiUrl}/UpdateEnum`;
        return this.httpClient.post(url, updateEnum)
            .toPromise();
    }


    createEnum(createEnum: CreateEnum): Promise<CreatedEntity<string>> {
        const url = `${this.apiUrl}/CreateEnum`;
        return this.httpClient.post<CreatedEntity<string>>(url, createEnum)
            .toPromise();
    }

    deleteEnum(deleteEnum: DeleteEnum): Promise<{}> {
        const url = `${this.apiUrl}/DeleteEnum`;
        return this.httpClient.post(url, deleteEnum)
            .toPromise();
    }

    generateEnum(generateEnum: GenerateEnum): Promise<{}> {
        const url = `${this.apiUrl}/GenerateEnum`;
        return this.httpClient.post(url, generateEnum)
            .toPromise();
    }
}
