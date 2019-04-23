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
    private headers = new HttpHeaders({ "Content-Type": "application/json" });

    constructor(private httpClient: HttpClient, private configService: ConfigService) { }

    getEnums(getEnums: GetEnums): Promise<Page<Enum>> {
        const url = `${this.apiUrl}/GetEnums`;
        return this.httpClient.post<Page<Enum>>(url, JSON.stringify(getEnums), { headers: this.headers })
            .toPromise();
    }

    getEnum(getEnum: GetEnum): Promise<Enum> {
        const url = `${this.apiUrl}/GetEnum`;
        return this.httpClient.post<Enum>(url, JSON.stringify(getEnum), { headers: this.headers })
            .toPromise();
    }

    updateEnum(updateEnum: UpdateEnum): Promise<{}> {
        const url = `${this.apiUrl}/UpdateEnum`;
        return this.httpClient.post(url, JSON.stringify(updateEnum), { headers: this.headers })
            .toPromise();
    }


    createEnum(createEnum: CreateEnum): Promise<CreatedEntity<string>> {
        const url = `${this.apiUrl}/CreateEnum`;
        return this.httpClient.post<CreatedEntity<string>>(url, JSON.stringify(createEnum), { headers: this.headers })
            .toPromise();
    }

    deleteEnum(deleteEnum: DeleteEnum): Promise<{}> {
        const url = `${this.apiUrl}/DeleteEnum`;
        return this.httpClient.post(url, JSON.stringify(deleteEnum), { headers: this.headers })
            .toPromise();
    }

    generateEnum(generateEnum: GenerateEnum): Promise<{}> {
        const url = `${this.apiUrl}/GenerateEnum`;
        return this.httpClient.post(url, JSON.stringify(generateEnum), { headers: this.headers })
            .toPromise();
    }
}
